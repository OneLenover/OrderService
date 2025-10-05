# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем глобальные props
COPY Directory.Build.props .

# Копируем csproj всех проектов
COPY OrderService/OrderService.API/*.csproj OrderService.API/
COPY OrderService/OrderService.DataAccess.Postgres/*.csproj OrderService.DataAccess.Postgres/

# Восстанавливаем зависимости
RUN dotnet restore OrderService.API/OrderService.API.csproj

# Копируем весь код
COPY OrderService/. .

WORKDIR /src/OrderService.API
RUN dotnet publish -c Release -o /app

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]