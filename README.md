# Pokedex

A pretty Pokédex (yet another)

## pokedex-ui

Next.js app that displays the Pokémon data. Using MUI components.

* http://localhost:3000/

## pokedex-api

Restful API with swagger endpoint. Connected to a Redis docker instance and a MongoDB docker instance
for a cache-a-side pattern

* http://localhost:5144/swagger - Swagger

## pokedex-poller

C# Worker Service project that polls data from PokeApi and persists the fetched data to a MongoDB docker instance.

## pokedex-shared

Shared config and services around the project

## pokedex-unit-test

nUnit3 test project for unit tests with FluentAssertion

## pokedex-integration-test

nUnit3 test project with Testcontainers and FluentAssertion

## datasource

MongoDB as database and Redis as a IDistributedCache.

* Redis Insight - http://localhost:8001
* Mongo Express - http://localhost:8081

## loki

* Grafana with Loki datasource - http://localhost:3000


