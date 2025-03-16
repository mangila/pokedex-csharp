FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ./pokedex-poller ./pokedex-poller
COPY ./pokedex-shared ./pokedex-shared

RUN dotnet publish ./pokedex-poller/pokedex-poller.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "pokedex-poller.dll"]