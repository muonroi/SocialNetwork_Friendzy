# Define base image for runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Switch to root to set permissions
USER root

# Create images folder and set permissions
RUN mkdir -p /app/wwwroot/images && \
    chown -R app:app /app/wwwroot/images && \
    chmod -R 777 /app/wwwroot/images

# Switch back to non-root user
USER app

# Define build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore dependencies
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

# Copy source code and build application
COPY . .
WORKDIR "/src/Services/Management.Photo.Service/Management.Photo.Service"
RUN dotnet build "./Management.Photo.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Create publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Management.Photo.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Define final stage for the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.Photo.Service.dll"]
