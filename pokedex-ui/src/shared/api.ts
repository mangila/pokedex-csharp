import {LokiLogRequest, PokemonDto, PokemonGeneration, PokemonMediaProjectionDto} from "./types";
import {APP_NAME, ENV, LOKI_PUSH_URL, POKEDEX_API_V1_URL} from "./utils";


export const pushToLoki = async (request: LokiLogRequest): Promise<boolean> => {
    const timestamp = (Date.now() * 1000000).toString() // nanoseconds
    const payload = {
        streams: [
            {
                stream: {app: APP_NAME, env: ENV},
                values: [[timestamp, JSON.stringify(request)]],
            },
        ],
    };
    const response = await fetch(LOKI_PUSH_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
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

export const getPokemonByName = async (pokemonName: string): Promise<PokemonDto | undefined> => {
    const uri = `${POKEDEX_API_V1_URL}/pokemon/${pokemonName}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        return undefined;
    }
    return await response.json() as PokemonDto;
}
