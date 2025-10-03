# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["IDV.API/IDV.API.csproj", "IDV.API/"]
COPY ["IDV.Application/IDV.Application.csproj", "IDV.Application/"]
COPY ["IDV.Core/IDV.Core.csproj", "IDV.Core/"]
COPY ["IDV.Infrastructure/IDV.Infrastructure.csproj", "IDV.Infrastructure/"]

RUN dotnet restore "IDV.API/IDV.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/IDV.API"
RUN dotnet build "IDV.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "IDV.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDV.API.dll"]