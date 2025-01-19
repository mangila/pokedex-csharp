# Pokedex

A pretty Pokédex (yet another)

## pokedex-ui

Next.js app that displays the Pokemon data. Using MUI components.
* http://localhost:3000/

## pokedex-api

Restful API with swagger endpoint. Connected to a Redis docker instance and a MongoDB docker instance 
for a cache-a-side pattern
* http://localhost:5144/swagger - Swagger

## pokedex-poller

C# Worker Service project that polls data from PokeApi and persists the fetched data to a MongoDB docker instance.

## pokedex-shared

Shared config, services around the project

## datasource
MongoDB as database and Redis as a IDistributedCache.
* Redis Insight - http://localhost:8001
* Mongo Express - http://localhost:8081


