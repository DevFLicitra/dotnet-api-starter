# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copio solo il csproj per sfruttare la cache del restore
COPY Api/Api.csproj Api/
RUN dotnet restore Api/Api.csproj

# Copio tutto il progetto API
COPY Api/ Api/
WORKDIR /src/Api

RUN dotnet publish Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]