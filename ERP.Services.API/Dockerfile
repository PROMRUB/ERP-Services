#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN sed -i 's/^Components: main$/& contrib/' /etc/apt/sources.list.d/debian.sources
RUN apt-get update
RUN apt-get install -y ttf-mscorefonts-installer fontconfig
RUN fc-cache -f -v

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ERP.Services.API/ERP.Services.API.csproj", "ERP.Services.API/"]
RUN dotnet restore "./ERP.Services.API/./ERP.Services.API.csproj"
COPY . .
WORKDIR "/src/ERP.Services.API"
RUN dotnet build "./ERP.Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ERP.Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./ERP.Services.API/logo.jpg /app/pubish/logo.jpg
ENTRYPOINT ["dotnet", "ERP.Services.API.dll"]