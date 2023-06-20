$ErrorActionPreference = 'Stop'

Push-Location -Path "./docker"

try {
    $excludedTestsBinaries = @("*.Tests.*");

    Copy-Item -Path "$PSScriptRoot\..\binaries" -Destination . -Recurse -Exclude $excludedTestsBinaries -Force

    docker image build --tag "task-manager:latest" --file Dockerfile ./

    if ($LASTEXITCODE) {
        exit $LASTEXITCODE
    }
}
finally {
    Pop-Location
}
