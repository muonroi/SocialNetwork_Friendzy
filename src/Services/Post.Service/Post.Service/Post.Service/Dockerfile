#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Post.Service/Post.Service/Post.Service/Post.Service.csproj", "Services/Post.Service/Post.Service/Post.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Events/Events.csproj", "BuildingBlocks/Events/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["Services/Post.Service/Post.Service/Post.Application/Post.Application.csproj", "Services/Post.Service/Post.Service/Post.Application/"]
COPY ["Services/Post.Service/Post.Service/Post.Domain/Post.Domain.csproj", "Services/Post.Service/Post.Service/Post.Domain/"]
COPY ["Services/Post.Service/Post.Service/Post.Infrastructure/Post.Infrastructure.csproj", "Services/Post.Service/Post.Service/Post.Infrastructure/"]
RUN dotnet restore "./Services/Post.Service/Post.Service/Post.Service/Post.Service.csproj"
COPY . .
WORKDIR "/src/Services/Post.Service/Post.Service/Post.Service"
RUN dotnet build "./Post.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Post.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Post.Service.dll"]