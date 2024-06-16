#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Management.Friends.Action.Service/Management.Friends.Action.Service/Management.Friends.Action.Service.csproj", "Services/Management.Friends.Action.Service/Management.Friends.Action.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/Management.Friends.Action.Service/Management.Friends.Action.Application/Management.Friends.Action.Application.csproj", "Services/Management.Friends.Action.Service/Management.Friends.Action.Application/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["Services/Management.Friends.Action.Service/Management.Friends.Action.Domain/Management.Friends.Action.Domain.csproj", "Services/Management.Friends.Action.Service/Management.Friends.Action.Domain/"]
COPY ["Services/Management.Friends.Action.Service/Management.Friends.Action.Infrastructure/Management.Friends.Action.Infrastructure.csproj", "Services/Management.Friends.Action.Service/Management.Friends.Action.Infrastructure/"]
RUN dotnet restore "./Services/Management.Friends.Action.Service/Management.Friends.Action.Service/Management.Friends.Action.Service.csproj"
COPY . .
WORKDIR "/src/Services/Management.Friends.Action.Service/Management.Friends.Action.Service"
RUN dotnet build "./Management.Friends.Action.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Management.Friends.Action.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.Friends.Action.Service.dll"]