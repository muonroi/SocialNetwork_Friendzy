#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuildingBlocks/Commons/Directory.Packages.props", "BuildingBlocks/Commons/"]
COPY ["Services/Setting.Service/Setting.Service/Setting.Service.csproj", "Services/Setting.Service/Setting.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/Setting.Service/Setting.Application/Setting.Application.csproj", "Services/Setting.Service/Setting.Application/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["Services/Setting.Service/Setting.Domain/Setting.Domain.csproj", "Services/Setting.Service/Setting.Domain/"]
COPY ["Services/Setting.Service/Setting.Infrastructure/Setting.Infrastructure.csproj", "Services/Setting.Service/Setting.Infrastructure/"]
RUN dotnet restore "./Services/Setting.Service/Setting.Service/Setting.Service.csproj"
COPY . .
WORKDIR "/src/Services/Setting.Service/Setting.Service"
RUN dotnet build "./Setting.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Setting.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Setting.Service.dll"]