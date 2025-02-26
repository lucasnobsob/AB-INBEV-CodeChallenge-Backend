#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AB-INBEV.Services.API/AB-INBEV.Services.API.csproj", "src/AB-INBEV.Services.API/"]
COPY ["src/AB-INBEV.Application/AB-INBEV.Application.csproj", "src/AB-INBEV.Application/"]
COPY ["src/AB-INBEV.Domain/AB-INBEV.Domain.csproj", "src/AB-INBEV.Domain/"]
COPY ["src/AB-INBEV.Domain.Core/AB-INBEV.Domain.Core.csproj", "src/AB-INBEV.Domain.Core/"]
COPY ["src/AB-INBEV.CrossCutting.Identity/AB-INBEV.CrossCutting.Identity.csproj", "src/AB-INBEV.CrossCutting.Identity/"]
COPY ["src/AB-INBEV.CrossCutting.IoC/AB-INBEV.CrossCutting.IoC.csproj", "src/AB-INBEV.CrossCutting.IoC/"]
COPY ["src/AB-INBEV.CrossCutting.Bus/AB-INBEV.CrossCutting.Bus.csproj", "src/AB-INBEV.CrossCutting.Bus/"]
COPY ["src/AB-INBEV.Infra.Data/AB-INBEV.Infra.Data.csproj", "src/AB-INBEV.Infra.Data/"]
RUN dotnet restore "./src/AB-INBEV.Services.API/./AB-INBEV.Services.API.csproj"
COPY . .
WORKDIR "/src/src/AB-INBEV.Services.API"
RUN dotnet build "./AB-INBEV.Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AB-INBEV.Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AB-INBEV.Services.API.dll"]