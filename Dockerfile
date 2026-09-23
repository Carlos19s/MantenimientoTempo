# ── Stage 1: Build ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto para restaurar dependencias primero (cache layer)
COPY MantenimientoTempoModels/MantenimientoTempoModels.csproj MantenimientoTempoModels/
COPY MantenimientoTempoApi/MantenimientoTempoApi.csproj       MantenimientoTempoApi/

RUN dotnet restore MantenimientoTempoApi/MantenimientoTempoApi.csproj

# Copiar el resto del código fuente
COPY MantenimientoTempoModels/ MantenimientoTempoModels/
COPY MantenimientoTempoApi/    MantenimientoTempoApi/

# Publicar en modo Release
RUN dotnet publish MantenimientoTempoApi/MantenimientoTempoApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Railway inyecta la variable PORT; ASP.NET la recoge con ASPNETCORE_URLS
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MantenimientoTempoApi.dll"]
