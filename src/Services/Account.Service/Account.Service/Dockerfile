#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Account.Service/Account.Service/Account.Service.csproj", "Services/Account.Service/Account.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/Account.Service/Account.Infrastructure/Account.Infrastructure.csproj", "Services/Account.Service/Account.Infrastructure/"]
COPY ["Services/Account.Service/Account.Application/Account.Application.csproj", "Services/Account.Service/Account.Application/"]
COPY ["Services/Account.Service/Account.Domain/Account.Domain.csproj", "Services/Account.Service/Account.Domain/"]
RUN dotnet restore "./Services/Account.Service/Account.Service/Account.Service.csproj"
COPY . .
WORKDIR "/src/Services/Account.Service/Account.Service"
RUN dotnet build "./Account.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Account.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Account.Service.dll"]