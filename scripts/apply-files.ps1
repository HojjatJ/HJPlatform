<#
.SYNOPSIS
    HJPlatform — Manifest-based file writer.
    این اسکریپت یک‌بار نوشته می‌شود و برای همه‌ی فیچرهای آینده استفاده می‌شود.
    مدل کدنویس دیگر نیازی به نوشتن PowerShell ندارد — فقط باید خروجی را در فرمت Manifest زیر بدهد.

.USAGE
    1) خروجی مدل کدنویس (فرمت Manifest) را در یک فایل مثلاً manifest.txt کپی کن.
    2) از ریشه‌ی ریپازیتوری اجرا کن:
         ./tools/apply-files.ps1 -ManifestPath .\manifest.txt
    3) اگر خواستی فایل‌های موجود را هم بازنویسی کند:
         ./tools/apply-files.ps1 -ManifestPath .\manifest.txt -Force

.MANIFEST FORMAT (این را دقیقاً به مدل کدنویس بده)
    ===FILE: relative/path/to/File1.cs
    <کل محتوای فایل، دقیقاً همانطور که باید ذخیره شود>
    ===ENDFILE===
    ===FILE: relative/path/to/File2.cs
    <محتوای فایل دوم>
    ===ENDFILE===

    نکات مهم برای مدل کدنویس:
    - مسیر باید نسبت به ریشه‌ی ریپازیتوری باشد (نه مطلق).
    - هیچ توضیح یا متنی خارج از بلوک‌های FILE/ENDFILE قرار نگیرد.
    - محتوای هر فایل عیناً کد نهایی است — بدون Markdown fence اضافه (بدون ```).
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

$currentPath = $null
$buffer = New-Object System.Collections.Generic.List[string]
$filesWritten = @()
$filesSkipped = @()
$filesOverwritten = @()

foreach ($line in $lines) {

    if ($line -match '^===FILE:\s*(.+?)\s*$') {
        if ($null -ne $currentPath) {
            Write-Warning "New FILE marker found before ENDFILE for '$currentPath'. Previous block discarded."
        }
        $currentPath = $matches[1]
        $buffer.Clear()
        continue
    }

    if ($line -match '^===ENDFILE===\s*$') {
        if ($null -eq $currentPath) {
            Write-Warning "ENDFILE marker found without a matching FILE marker. Skipping block."
            continue
        }

        $content = [string]::Join([Environment]::NewLine, $buffer)
        $dir = Split-Path -Parent $currentPath

        if ($dir -and -not (Test-Path $dir)) {
            New-Item -ItemType Directory -Force -Path $dir | Out-Null
        }

        $exists = Test-Path $currentPath

        if ($exists -and -not $Force) {
            Write-Host "SKIPPED (already exists, use -Force to overwrite): $currentPath" -ForegroundColor Yellow
            $filesSkipped += $currentPath
        }
        else {
            Set-Content -Path $currentPath -Value $content -Encoding utf8 -NoNewline
            if ($exists) {
                Write-Host "OVERWRITTEN: $currentPath" -ForegroundColor Magenta
                $filesOverwritten += $currentPath
            }
            else {
                Write-Host "CREATED: $currentPath" -ForegroundColor Green
                $filesWritten += $currentPath
            }
        }

        $currentPath = $null
        continue
    }

    if ($null -ne $currentPath) {
        $buffer.Add($line)
    }
}

if ($null -ne $currentPath) {
    Write-Warning "Manifest ended while still inside file block '$currentPath' (missing ENDFILE?). Content NOT written."
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Created:     $($filesWritten.Count)"
foreach ($f in $filesWritten) { Write-Host "  + $f" }
Write-Host "Overwritten: $($filesOverwritten.Count)"
foreach ($f in $filesOverwritten) { Write-Host "  ~ $f" }
Write-Host "Skipped:     $($filesSkipped.Count)"
foreach ($f in $filesSkipped) { Write-Host "  - $f" }
