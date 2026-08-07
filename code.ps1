# HJPlatform Documentation Sync Script
# Safe mode: existing docs only

$docs = Join-Path (Get-Location) "docs"

if (!(Test-Path $docs)) {
    throw "docs folder not found."
}

$statusFile = Join-Path $docs "IMPLEMENTATION-STATUS.md"

if (Test-Path $statusFile) {

    $content = Get-Content $statusFile -Raw

    $content = $content -replace "Last updated:\s*\d{4}-\d{2}-\d{2}",
        "Last updated: 2026-08-07"

    if ($content -notmatch "## API Layer") {

        $section = @'

---

## API Layer — وضعیت تفصیلی

**Status: Implemented and Verified**

Completed:

- FastEndpoints configured.
- Health endpoint implemented.
- Product create endpoint implemented.
- Installation endpoints implemented.
- Global exception handling implemented.
- Swagger configuration completed.
- API layer contains no business rules.

---

## Application Layer — وضعیت تفصیلی

**Status: Implemented and Verified**

Completed:

- ProductService implemented.
- InstallationService implemented.
- FluentValidation integrated.
- Mapperly mapping registered through DI.
- ProductAlreadyExistsException introduced.
- CancellationToken flow verified.

---

## Infrastructure Layer — وضعیت تفصیلی

**Status: Implemented and Verified**

Completed:

- EF Core persistence configured.
- ApplyConfigurationsFromAssembly verified.
- Product persistence completed.
- Installation persistence configuration completed.
- Repository implementations reviewed.

---

'@

        Add-Content -Path $statusFile -Value $section -Encoding UTF8

        Write-Host "IMPLEMENTATION-STATUS.md updated"
    }
    else {
        Write-Host "IMPLEMENTATION-STATUS already contains API section"
    }
}


$progressFile = Join-Path $docs "PROGRESS-LOG.md"

if (Test-Path $progressFile) {

    $progress = Get-Content $progressFile -Raw

    if ($progress -notmatch "2026-08-07 — Application / Infrastructure / API Completion Review") {

        $entry = @'

---

## 2026-08-07 — Application / Infrastructure / API Completion Review

Completed:

- Application layer review completed.
- API endpoints reviewed.
- Infrastructure persistence reviewed.
- Exception handling flow verified.
- Documentation synchronized.

Validation:

- dotnet build PASS
- dotnet test PASS

'@

        Add-Content -Path $progressFile -Value $entry -Encoding UTF8

        Write-Host "PROGRESS-LOG.md updated"
    }
    else {
        Write-Host "Progress entry already exists"
    }
}

Write-Host "Documentation synchronization completed." -ForegroundColor Green