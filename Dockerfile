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

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

# PORT es inyectado por Railway en runtime; se evalúa al iniciar el contenedor
CMD ASPNETCORE_URLS="http://+:${PORT:-8080}" dotnet MantenimientoTempoApi.dll
