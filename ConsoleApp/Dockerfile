FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY MessageContracts/MessageContracts.csproj MessageContracts/
COPY ConsoleApp/ConsoleApp.csproj ConsoleApp/
RUN dotnet restore ConsoleApp/ConsoleApp.csproj
COPY . .
WORKDIR /src/ConsoleApp/
RUN dotnet build ConsoleApp.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ConsoleApp.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]
