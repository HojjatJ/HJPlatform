$ErrorActionPreference = "Stop"

Write-Host "Checking git repository..." -ForegroundColor Cyan

git rev-parse --is-inside-work-tree

if ($LASTEXITCODE -ne 0) {
    throw "Current folder is not a git repository."
}


Write-Host ""
Write-Host "Current branch:" -ForegroundColor Yellow
git branch --show-current


Write-Host ""
Write-Host "Git status:" -ForegroundColor Yellow
git status


# ---------------------------------------
# Check .gitignore
# ---------------------------------------

$gitignore = ".gitignore"

$requiredEntries = @(
    "**/bin/",
    "**/obj/",
    ".vs/",
    "*.user",
    "*.suo",
    "TestResults/"
)


if (!(Test-Path $gitignore)) {

    Write-Host ".gitignore not found. Creating..." -ForegroundColor Yellow

    New-Item $gitignore -ItemType File | Out-Null
}


$content = Get-Content $gitignore -ErrorAction SilentlyContinue


foreach ($entry in $requiredEntries) {

    if ($content -notcontains $entry) {

        Write-Host "Adding ignore rule: $entry" -ForegroundColor Green

        Add-Content $gitignore $entry
    }
}


Write-Host ""
Write-Host ".gitignore check completed." -ForegroundColor Green


# ---------------------------------------
# Show changes
# ---------------------------------------

Write-Host ""
Write-Host "Files changed:" -ForegroundColor Yellow

git status --short


Write-Host ""
Write-Host "Ready for commit." -ForegroundColor Cyan


$answer = Read-Host "Create commit now? (y/n)"


if ($answer -eq "y") {

    git add .

    git commit -m "feat: add architecture testing foundation"

    Write-Host ""
    Write-Host "Commit created successfully." -ForegroundColor Green

}
else {

    Write-Host "Commit skipped." -ForegroundColor Yellow
}


Write-Host ""
Write-Host "Next step:"
Write-Host "git push"