$root = "D:\Projects\Visual Studio\HJPlatform"

$gitignore = @"
# Build results
bin/
obj/

# Visual Studio
.vs/
*.user
*.suo
*.userosscache
*.sln.docstates

# Rider
.idea/

# Logs
*.log

# Test results
TestResults/
*.trx

# NuGet
*.nupkg
packages/

# OS
Thumbs.db
.DS_Store

# Local settings
appsettings.Development.json

"@

Set-Content "$root\.gitignore" $gitignore

Write-Host ".gitignore created."