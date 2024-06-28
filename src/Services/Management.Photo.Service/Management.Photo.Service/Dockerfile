#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Management.Photo.Service/Management.Photo.Service/Management.Photo.Service.csproj", "Services/Management.Photo.Service/Management.Photo.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Events/Events.csproj", "BuildingBlocks/Events/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["Services/Management.Photo.Service/Management.Photo.Application/Management.Photo.Application.csproj", "Services/Management.Photo.Service/Management.Photo.Application/"]
COPY ["Services/Management.Photo.Service/Management.Photo.Domain/Management.Photo.Domain.csproj", "Services/Management.Photo.Service/Management.Photo.Domain/"]
COPY ["Services/Management.Photo.Service/Management.Photo.Infrastructure/Management.Photo.Infrastructure.csproj", "Services/Management.Photo.Service/Management.Photo.Infrastructure/"]
RUN dotnet restore "./Services/Management.Photo.Service/Management.Photo.Service/Management.Photo.Service.csproj"
COPY . .
WORKDIR "/src/Services/Management.Photo.Service/Management.Photo.Service"
RUN dotnet build "./Management.Photo.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Management.Photo.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.Photo.Service.dll"]