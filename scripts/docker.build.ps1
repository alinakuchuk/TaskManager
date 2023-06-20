# The purpose if this script file is to copy binaries
# excluding tests binaries (we do not want to have them in docker image)
# and build docker image

$ErrorActionPreference = 'Stop'

Push-Location -Path "./docker"

try {
    $excludedTestsBinaries = @("*.Tests.*");

    Copy-Item -Path "$PSScriptRoot\..\binaries" -Destination . -Recurse -Exclude $excludedTestsBinaries -Force

    docker image build --tag "taskmanageracr.azurecr.io/task-manager:latest" --file Dockerfile ./

    if ($LASTEXITCODE) {
        exit $LASTEXITCODE
    }
}
finally {
    Pop-Location
}
