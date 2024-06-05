#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/User.Service/User.Service/User.Service.csproj", "Services/User.Service/User.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/User.Service/User.Aplication/User.Application.csproj", "Services/User.Service/User.Aplication/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["Services/User.Service/User.Domain/User.Domain.csproj", "Services/User.Service/User.Domain/"]
COPY ["Services/User.Service/User.Infrastructure/User.Infrastructure.csproj", "Services/User.Service/User.Infrastructure/"]
RUN dotnet restore "./Services/User.Service/User.Service/User.Service.csproj"
COPY . .
WORKDIR "/src/Services/User.Service/User.Service"
RUN dotnet build "./User.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Service.dll"]