#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Message.Service/Message.Service/Message.Service.csproj", "Services/Message.Service/Message.Service/"]
COPY ["BuildingBlocks/Commons/Commons.csproj", "BuildingBlocks/Commons/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["Services/Message.Service/Message.Infrastructure/Message.Infrastructure.csproj", "Services/Message.Service/Message.Infrastructure/"]
COPY ["Services/Message.Service/Message.Application/Message.Application.csproj", "Services/Message.Service/Message.Application/"]
COPY ["BuildingBlocks/ExternalAPI/ExternalAPI.csproj", "BuildingBlocks/ExternalAPI/"]
COPY ["Services/Message.Service/Message.Domain/Message.Domain.csproj", "Services/Message.Service/Message.Domain/"]
RUN dotnet restore "./Services/Message.Service/Message.Service/Message.Service.csproj"
COPY . .
WORKDIR "/src/Services/Message.Service/Message.Service"
RUN dotnet build "./Message.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Message.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Message.Service.dll"]