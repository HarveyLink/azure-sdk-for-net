param (
    $InputConfig
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 1

[string[]] $errors = @()

function LogError([string]$message) {
    if ($env:TF_BUILD) {
        Write-Host "##vso[task.logissue type=error]$message"
    }
    Write-Host -f Red "error: $message"
    $script:errors += $message
}

function Invoke-Block([scriptblock]$cmd) {
    $cmd | Out-String | Write-Verbose
    & $cmd

    # Need to check both of these cases for errors as they represent different items
    # - $?: did the powershell script block throw an error
    # - $lastexitcode: did a windows command executed by the script block end in error
    if ((-not $?) -or ($lastexitcode -ne 0)) {
        if ($error -ne $null) {
            Write-Warning $error[0]
        }
        throw "Command failed to execute: $cmd"
    }
}

try {
    $input = Get-Content $InputConfig | ConvertFrom-Json
    $metaData = Get-Content $input.metaData

    $commit = ''
    $readme = ''
    [string]$path = Get-Location
    $metaData | % {
        if($_ -match 'Commit'){
            $commit = $_.substring($_.length - 40, 40)
        }
        if ($_ -match 'cmd.exe') {
            $_ -match 'https:[\S]*readme.md'
            $readme = $matches[0]
        }
    }
    $readme = $readme -replace "blob/[\S]*/specification", "blob/$commit/specification"
    $path = ($path -replace "\\", "/") + "/sdk"

    Invoke-Block {
        & autorest --input-file=$readme --csharp --version=v2 --reflect-api-versions --csharp-sdks-folder=$path
    }

    Write-Host "git diff only sdk folder"
    # prevent warning related to EOL differences which triggers an exception for some reason
    & git -c core.safecrlf=false diff --ignore-space-at-eol --exit-code sdk/
    if ($LastExitCode -ne 0) {
        $status = git status -s | Out-String
        $status = $status -replace "`n", "`n    "
        LogError "Generated code is not up to date. You may need to run eng\scripts\Update-Snippets.ps1 or sdk\storage\generate.ps1 or eng\scripts\Export-API.ps1"
    }
}
finally {
    Write-Host ""
    Write-Host "Summary:"
    Write-Host ""
    Write-Host "   $($errors.Length) error(s)"
    Write-Host ""

    foreach ($err in $errors) {
        Write-Host -f Red "error : $err"
    }

    if ($errors) {
        exit 1
    }
}
