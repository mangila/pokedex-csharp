import {PaginationResultDto, PokemonGeneration, PokemonSpeciesDto} from "./types";
import {Environment} from "./utils";

export async function findByPagination(page: number,
                                       pageSize: number,
                                       typesFilter: string[],
                                       specialFilter: string[]): Promise<PaginationResultDto> {
    const host = Environment.POKEDEX_API_URL
    const path = "/pokemon";
    const pageQueryParam = `page=${page}`;
    const pageSizeQueryParam = `pageSize=${pageSize}`;
    const typesQueryParams = typesFilter.map(value => `&types=${value}`).join('');
    const specialQueryParams = specialFilter.map(value => `&special=${value}`).join('');
    const uri = `${host}${path}?${pageQueryParam}&${pageSizeQueryParam}${typesQueryParams}${specialQueryParams}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PaginationResultDto;
}

export async function getPokemonByIds(ids: number[]): Promise<PokemonSpeciesDto[]> {
    const host = Environment.POKEDEX_API_URL
    const path = "/pokemon/search/id?";
    const queryParam = ids.map(value => `&ids=${value}`).join('');
    const uri = `${host}${path}${queryParam}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PokemonSpeciesDto[];
}


export async function findByGeneration(generation: PokemonGeneration): Promise<PokemonSpeciesDto[] | undefined> {
    const host = Environment.POKEDEX_API_URL
    const path = "/pokemon/search/generation?";
    const queryParam = `generation=${generation}`;
    const uri = `${host}${path}${queryParam}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PokemonSpeciesDto[];
}

export async function findByName(pokemonName: string): Promise<PokemonSpeciesDto> {
    const host = Environment.POKEDEX_API_URL
    const path = `/pokemon/${pokemonName}`;
    const uri = `${host}${path}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PokemonSpeciesDto;
}

export async function searchByName(name: string): Promise<PokemonSpeciesDto[] | undefined> {
    const host = Environment.POKEDEX_API_URL
    const path = "/pokemon/search/name?";
    const queryParam = `search=${name}`;
    const uri = `${host}${path}${queryParam}`;
    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return await response.json() as PokemonSpeciesDto[];
}
    

