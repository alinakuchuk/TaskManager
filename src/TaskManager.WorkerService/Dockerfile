FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app

COPY ./bin/Debug/net5.0 .
ENTRYPOINT ["dotnet", "TaskManager.WorkerService.dll"]
