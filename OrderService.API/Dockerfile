# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY OrderService.API.csproj . 
RUN dotnet restore OrderService.API.csproj

# Копируем остальной код
COPY . .
RUN dotnet publish OrderService.API.csproj -c Release -o /app/publish

# Запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet", "OrderService.API.dll"]