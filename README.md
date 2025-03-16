# Pokedex

A pretty Pokédex (yet another)

## How to run

run the docker-compose file and it should start everything!

- `docker compose up --force-recreate -d `
- Give the poller some time to fetch pokemon data from PokeAPI
- goto `http://localhost:4020`
- voila!

## pokedex-view

Vite (React) app that displays the Pokémon data. Using MUI components.

## pokedex-api

- Serve the pokemon data

## pokedex-poller

C# Worker Service project that polls data from PokeApi and persists the fetched data to a MongoDB docker instance.

* Caches API request responses
* Caches byte images
* Converts images to .webp if possible

## pokedex-shared

Shared config, models and services around the project

## pokedex-unit-test

nUnit3 test project for unit tests with FluentAssertion

## pokedex-integration-test

nUnit3 test project with Testcontainers and FluentAssertion

* Docker is required

## datasource

MongoDB as database and Redis as a IDistributedCache.

* Redis Insight - http://localhost:8001
* Mongo Compass ConnectionString - `mongodb://admin:password@localhost:27017`

## loki

* Grafana with Loki datasource - http://localhost:3000

## scripts

* HTTP request files