FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/TaskManager.WorkerService/TaskManager.WorkerService.csproj", "src/TaskManager.WorkerService/"]
RUN dotnet restore "src/TaskManager.WorkerService/TaskManager.WorkerService.csproj"
COPY . .
WORKDIR "/src/src/TaskManager.WorkerService"
RUN dotnet build "TaskManager.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.WorkerService.dll"]
