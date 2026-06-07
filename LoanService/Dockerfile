# ── Etapa 1: Build ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias (capa cacheada)
COPY LoanService.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ── Etapa 2: Runtime (imagen mucho más pequeña) ────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar solo los binarios publicados
COPY --from=build /app/publish .

# Puerto expuesto por la API
EXPOSE 8080

# Variable para que ASP.NET Core escuche en el puerto correcto
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LoanService.dll"]
