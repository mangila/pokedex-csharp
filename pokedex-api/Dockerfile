FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ./pokedex-api ./pokedex-api
COPY ./pokedex-shared ./pokedex-shared

RUN dotnet restore ./pokedex-api/pokedex-api.csproj
RUN dotnet publish ./pokedex-api/pokedex-api.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5144

ENTRYPOINT ["dotnet", "pokedex-api.dll"]