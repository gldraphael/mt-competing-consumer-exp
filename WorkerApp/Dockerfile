FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY MessageContracts/MessageContracts.csproj MessageContracts/
COPY WorkerApp/WorkerApp.csproj WorkerApp/
RUN dotnet restore WorkerApp/WorkerApp.csproj
COPY . .
WORKDIR /src/WorkerApp/
RUN dotnet build WorkerApp.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WorkerApp.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerApp.dll"]
