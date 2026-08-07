<#
.SYNOPSIS
    HJPlatform — Manifest-based file writer, updater, and deleter with Safe Write, Unique Backup, and Two-Pass Validation.
.USAGE
    .\scripts\apply-files.ps1 -ManifestPath .\scripts\manifest.txt [-Force]
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$ManifestPath,

    [switch]$Force
)

if (-not (Test-Path $ManifestPath)) {
    Write-Error "Manifest file not found: $ManifestPath"
    exit 1
}

$lines = Get-Content -Path $ManifestPath -Encoding UTF8

# ==========================================
# PASS 1: Manifest Parse & Validation
# ==========================================
$validationMode = 'None'
$validationPath = $null

foreach ($line in $lines) {
    if ($line -match '^===FILE:\s*(.+?)\s*$') {
        if ($validationMode -ne 'None') {
            Write-Error "Validation Error: New marker 'FILE' found before closing '$validationPath' (Mode: $validationMode)."
            exit 1
        }
        $validationMode = 'Create'
        $validationPath = $matches[1]
        continue
    }
    if ($line -match '^===ENDFILE===\s*$') {
        if ($validationMode -ne 'Create') {
            Write-Error "Validation Error: Unmatched ===ENDFILE=== marker found."
            exit 1
        }
        $validationMode = 'None'; $validationPath = $null
        continue
    }

    if ($line -match '^===UPDATE:\s*(.+?)\s*$') {
        if ($validationMode -ne 'None') {
            Write-Error "Validation Error: New marker 'UPDATE' found before closing '$validationPath' (Mode: $validationMode)."
            exit 1
        }
        $validationMode = 'WaitOld'
        $validationPath = $matches[1]
        continue
    }
    if ($line -match '^===OLD===\s*$') {
        if ($validationMode -in @('WaitOld', 'Update_New')) { $validationMode = 'Update_Old'; continue }
    }
    if ($line -match '^===NEW===\s*$') {
        if ($validationMode -eq 'Update_Old') { $validationMode = 'Update_New'; continue }
    }
    if ($line -match '^===ENDUPDATE===\s*$') {
        if ($validationMode -ne 'Update_New') {
            Write-Error "Validation Error: UPDATE block for '$validationPath' missing ===NEW=== section or incorrectly structured."
            exit 1
        }
        $validationMode = 'None'; $validationPath = $null
        continue
    }

    if ($line -match '^===DELETE:\s*(.+?)\s*$') {
        if ($validationMode -ne 'None') {
            Write-Error "Validation Error: New marker 'DELETE' found before closing '$validationPath' (Mode: $validationMode)."
            exit 1
        }
        $validationMode = 'Delete'
        $validationPath = $matches[1]
        continue
    }
    if ($line -match '^===ENDDELETE===\s*$') {
        if ($validationMode -ne 'Delete') {
            Write-Error "Validation Error: Unmatched ===ENDDELETE=== marker found."
            exit 1
        }
        $validationMode = 'None'; $validationPath = $null
        continue
    }
}

if ($validationMode -ne 'None') {
    Write-Error "Manifest incomplete. Unclosed block detected (Mode: '$validationMode', Target: '$validationPath'). Aborting execution before making any changes."
    exit 1
}

# ==========================================
# PASS 2: Execution & File Operations
# ==========================================
$currentMode = 'None'
$currentPath = $null
$buffer = New-Object System.Collections.Generic.List[string]
$bufferOld = New-Object System.Collections.Generic.List[string]
$bufferNew = New-Object System.Collections.Generic.List[string]

$stats = @{
    Created = @()
    Overwritten = @()
    Skipped = @()
    Updated = @()
    Deleted = @()
    Backups = @()
    FailedUpdates = @()
    FailedWrites = @()
}

$utf8NoBom = New-Object System.Text.UTF8Encoding $false

function Write-SafeUtf8NoBom([string]$path, [string]$text) {
    $fullPath = [System.IO.Path]::GetFullPath($path)
    $tempPath = "$fullPath.tmp"

    try {
        if (-not $text.EndsWith([Environment]::NewLine)) {
            $text += [Environment]::NewLine
        }

        [System.IO.File]::WriteAllText($tempPath, $text, $utf8NoBom)

        Move-Item -Path $tempPath -Destination $fullPath -Force

        return $true
    }
    catch {
        Write-Error "Safe Write failed for file '$path': $_"

        if (Test-Path $tempPath) {
            Remove-Item $tempPath -Force -ErrorAction SilentlyContinue
        }

        return $false
    }
}

foreach ($line in $lines) {

    # 1. FILE COMMAND
    if ($line -match '^===FILE:\s*(.+?)\s*$') {
        $currentMode = 'Create'
        $currentPath = $matches[1]
        $buffer.Clear()
        continue
    }
    
    if ($line -match '^===ENDFILE===\s*$') {
        if ($currentMode -ne 'Create') { continue }

        $content = [string]::Join([Environment]::NewLine, $buffer)
        $dir = Split-Path -Parent $currentPath
        if ($dir -and -not (Test-Path $dir)) { New-Item -ItemType Directory -Force -Path $dir | Out-Null }

        $exists = Test-Path $currentPath
        if ($exists -and -not $Force) {
            Write-Host "SKIPPED (already exists, use -Force): $currentPath" -ForegroundColor Yellow
            $stats.Skipped += $currentPath
        } else {
            if ($exists -and $Force) {
                $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
                $bakPath = "$currentPath.bak.$timestamp"
                try {
                    Copy-Item -Path $currentPath -Destination $bakPath -Force -ErrorAction Stop
                    $stats.Backups += $bakPath
                    Write-Host "BACKUP CREATED: $bakPath" -ForegroundColor DarkGray
                } catch {
                    Write-Error "BACKUP FAILED: Unable to backup '$currentPath' before overwrite. Aborting write."
                    $stats.FailedWrites += $currentPath
                    $currentMode = 'None'; $currentPath = $null
                    continue
                }
            }

            $success = Write-SafeUtf8NoBom -path $currentPath -text $content
            if ($success) {
                if ($exists) {
                    Write-Host "OVERWRITTEN: $currentPath" -ForegroundColor Magenta
                    $stats.Overwritten += $currentPath
                } else {
                    Write-Host "CREATED: $currentPath" -ForegroundColor Green
                    $stats.Created += $currentPath
                }
            } else {
                $stats.FailedWrites += $currentPath
            }
        }
        $currentMode = 'None'; $currentPath = $null
        continue
    }

    # 2. UPDATE COMMAND
    if ($line -match '^===UPDATE:\s*(.+?)\s*$') {
        $currentMode = 'WaitOld'
        $currentPath = $matches[1]
        $bufferOld.Clear()
        $bufferNew.Clear()
        continue
    }

    if ($line -match '^===OLD===\s*$') {
        if ($currentMode -in @('WaitOld', 'Update_New')) { $currentMode = 'Update_Old'; continue }
    }

    if ($line -match '^===NEW===\s*$') {
        if ($currentMode -eq 'Update_Old') { $currentMode = 'Update_New'; continue }
    }

    if ($line -match '^===ENDUPDATE===\s*$') {
        if ($currentMode -ne 'Update_New') { continue }

        if (-not (Test-Path $currentPath)) {
            Write-Warning "UPDATE FAILED (File not found): $currentPath"
            $stats.FailedUpdates += $currentPath
        } else {
            $fileRaw = Get-Content -Path $currentPath -Raw
            $fileNorm = $fileRaw -replace "`r`n?", "`n"
            $oldStr = [string]::Join("`n", $bufferOld)
            $newStr = [string]::Join("`n", $bufferNew)

            if ($fileNorm.Contains($oldStr)) {
                $updatedStr = $fileNorm.Replace($oldStr, $newStr)
                $updatedStr = $updatedStr -replace "`n", [Environment]::NewLine
                
                $success = Write-SafeUtf8NoBom -path $currentPath -text $updatedStr
                if ($success) {
                    Write-Host "UPDATED: $currentPath" -ForegroundColor Cyan
                    $stats.Updated += $currentPath
                } else {
                    $stats.FailedWrites += $currentPath
                }
            } else {
                Write-Warning "UPDATE FAILED (OLD text mismatch): $currentPath"
                $stats.FailedUpdates += $currentPath
            }
        }
        $currentMode = 'None'; $currentPath = $null
        continue
    }

    # 3. DELETE COMMAND
    if ($line -match '^===DELETE:\s*(.+?)\s*$') {
        $currentMode = 'Delete'
        $currentPath = $matches[1]
        continue
    }

    if ($line -match '^===ENDDELETE===\s*$') {
        if ($currentMode -ne 'Delete') { continue }

        if (Test-Path $currentPath -PathType Leaf) {
            Remove-Item -Path $currentPath -Force
            Write-Host "DELETED: $currentPath" -ForegroundColor Red
            $stats.Deleted += $currentPath
        } else {
            Write-Host "SKIPPED (File not found): $currentPath" -ForegroundColor Yellow
            $stats.Skipped += $currentPath
        }

        $currentMode = 'None'; $currentPath = $null
        continue
    }

    if ($currentMode -eq 'Create') { $buffer.Add($line) }
    elseif ($currentMode -eq 'Update_Old') { $bufferOld.Add($line) }
    elseif ($currentMode -eq 'Update_New') { $bufferNew.Add($line) }
}

Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "Created:       $($stats.Created.Count)"
foreach ($f in $stats.Created) { Write-Host "  + $f" -ForegroundColor Green }
Write-Host "Overwritten:   $($stats.Overwritten.Count)"
foreach ($f in $stats.Overwritten) { Write-Host "  ~ $f" -ForegroundColor Magenta }
Write-Host "Updated:       $($stats.Updated.Count)"
foreach ($f in $stats.Updated) { Write-Host "  ^ $f" -ForegroundColor Cyan }
Write-Host "Deleted:       $($stats.Deleted.Count)"
foreach ($f in $stats.Deleted) { Write-Host "  x $f" -ForegroundColor Red }
Write-Host "Backups:       $($stats.Backups.Count)"
foreach ($f in $stats.Backups) { Write-Host "  # $f" -ForegroundColor DarkGray }
Write-Host "Skipped:       $($stats.Skipped.Count)"
foreach ($f in $stats.Skipped) { Write-Host "  - $f" -ForegroundColor Yellow }

if ($stats.FailedUpdates.Count -gt 0) {
    Write-Host "Failed Upds:   $($stats.FailedUpdates.Count)" -ForegroundColor Red
    $stats.FailedUpdates | ForEach-Object { Write-Host "  ! $_" -ForegroundColor Red }
}

if ($stats.FailedWrites.Count -gt 0) {
    Write-Host "Failed Writes: $($stats.FailedWrites.Count)" -ForegroundColor Red
    $stats.FailedWrites | ForEach-Object { Write-Host "  ! $_" -ForegroundColor Red }
}

$exitCode = if (($stats.FailedUpdates.Count -gt 0) -or ($stats.FailedWrites.Count -gt 0)) { 1 } else { 0 }
exit $exitCode