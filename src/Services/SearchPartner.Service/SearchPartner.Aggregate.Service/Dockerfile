#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/SearchPartner.Service/SearchPartner.Aggregate.Service/SearchPartners.Aggregate.Service.csproj", "Services/SearchPartner.Service/SearchPartner.Aggregate.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
RUN dotnet restore "Services/SearchPartner.Service/SearchPartner.Aggregate.Service/SearchPartners.Aggregate.Service.csproj"
COPY . .
WORKDIR "/src/Services/SearchPartner.Service/SearchPartner.Aggregate.Service"
RUN dotnet build "./SearchPartners.Aggregate.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SearchPartners.Aggregate.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchPartners.Aggregate.Service.dll"]