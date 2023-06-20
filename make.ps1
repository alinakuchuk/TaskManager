# Just an aggregator

$ErrorActionPreference = 'Stop'

pwsh ./scripts/build.ps1
if ($LASTEXITCODE) {
	exit $LASTEXITCODE
}

pwsh ./scripts/docker.build.ps1
if ($LASTEXITCODE) {
	exit $LASTEXITCODE
}
