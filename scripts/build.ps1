# The purpose of this script file is to build whole solution
# and output all binaries to single place to provide docker
# with unified binaries source

dotnet build --configuration Release --output ./binaries 
