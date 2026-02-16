FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["RuanganKita.Api/RuanganKita.Api.csproj", "RuanganKita.Api/"]
RUN dotnet restore "RuanganKita.Api/RuanganKita.Api.csproj"

COPY . .
WORKDIR "/src/RuanganKita.Api"
RUN dotnet build "RuanganKita.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RuanganKita.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "RuanganKita.Api.dll"]
