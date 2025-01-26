import {LokiLogRequest, PokemonDto, PokemonGeneration, PokemonMediaProjectionDto} from "./types";
import * as process from "node:process";

export const POKEDEX_API_V1_URL = process.env.POKEDEX_API_V1_URL;

export const postLoki = async (request: LokiLogRequest): Promise<boolean> => {
    const uri = "/api/loki";
    const response = await fetch(uri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
    });
    return response.ok;
};

export const getAllPokemons = async (): Promise<PokemonMediaProjectionDto[]> => {
    const uri = `${POKEDEX_API_V1_URL}/pokemon`
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    const json = await response.json();
    return json.pokemons as PokemonMediaProjectionDto[];
};

export const findAllPokemonsByGeneration = async (generation: PokemonGeneration): Promise<PokemonMediaProjectionDto[]> => {
    const uri = `${POKEDEX_API_V1_URL}/pokemon/search/generation?generation=${generation}`
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    const json = await response.json();
    return json.pokemons as PokemonMediaProjectionDto[];
}

export const getPokemonByName = async (pokemonName: string): Promise<PokemonDto> => {
    const uri = `${POKEDEX_API_V1_URL}/pokemon/${pokemonName}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PokemonDto;
}
