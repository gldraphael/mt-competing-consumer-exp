FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY MessageContracts/MessageContracts.csproj MessageContracts/
COPY AsbWorkerApp/AsbWorkerApp.csproj AsbWorkerApp/
RUN dotnet restore AsbWorkerApp/AsbWorkerApp.csproj
COPY . .
WORKDIR /src/AsbWorkerApp/
RUN dotnet build AsbWorkerApp.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish AsbWorkerApp.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AsbWorkerApp.dll"]
