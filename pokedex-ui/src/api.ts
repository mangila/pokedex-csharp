import {LokiLogRequest, PokemonDto} from "./types";
import * as process from "node:process";

export const PokedexUrl = process.env.POKEDEX_API_URL;

export const postLoki = async (request: LokiLogRequest): Promise<boolean> => {
    const response = await fetch('/api/loki', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
    });
    return response.ok;
};

export const getPokemonByName = async (pokemonName: string): Promise<PokemonDto> => {
    const response = await fetch(`${PokedexUrl}/pokemon/${pokemonName}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    const data = await response.json();
    return data as PokemonDto;
}