FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Копируем ТОЛЬКО файлы проектов (это важно для ускорения сборки)
COPY ["src/API/API.csproj", "src/API/"]
RUN dotnet restore "src/API/API.csproj"

# Копируем ВСЁ остальное (уже после restore)
COPY . .

WORKDIR "/src/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]