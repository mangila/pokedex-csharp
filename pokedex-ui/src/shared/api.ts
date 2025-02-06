import {LokiLogRequest, PaginationResultDto, PokemonGeneration, PokemonSpeciesDto} from "./types";
import {Environment} from "./utils";

export const pushToLoki =
    async (request: LokiLogRequest): Promise<boolean> => {
        const timestamp = (Date.now() * 1000000).toString() // nanoseconds
        const payload = {
            streams: [
                {
                    stream: {app: Environment.APP_NAME, env: Environment.ENV},
                    values: [[timestamp, JSON.stringify(request)]],
                },
            ],
        };
        const response = await fetch(Environment.LOKI_PUSH_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(payload),
        });
        return response.ok;
    };
export const findByPagination = async (page: number, pageSize: number, typesFilter: string[], specialFilter: string[]): Promise<PaginationResultDto> => {
    const host = Environment.POKEDEX_API_V1_URL;
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
        pushToLoki({
            level: "error",
            message: response.statusText,
            data: response
        })
        throw new Error(response.statusText);
    }
    return await response.json() as PaginationResultDto;
};

export const findByGeneration =
    async (generation: PokemonGeneration): Promise<PokemonSpeciesDto[] | undefined> => {
        const uri = `${Environment.POKEDEX_API_V1_URL}/pokemon/search/generation?generation=${generation}`
        const response = await fetch(uri, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        if (!response.ok) {
            pushToLoki({
                level: "error",
                message: response.statusText,
                data: generation
            })
            throw new Error(response.statusText);
        }
        return await response.json() as PokemonSpeciesDto[];
    }

export const findByName =
    async (pokemonName: string): Promise<PokemonSpeciesDto | undefined> => {
        const uri = `${Environment.POKEDEX_API_V1_URL}/pokemon/${pokemonName}`;
        const response = await fetch(uri, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        if (response.status === 404) {
            return undefined;
        }
        if (!response.ok) {
            pushToLoki({
                level: "error",
                message: response.statusText,
                data: pokemonName
            })
            throw new Error(response.statusText);
        }
        return await response.json() as PokemonSpeciesDto;
    }

export const searchByName =
    async (name: string): Promise<PokemonSpeciesDto[] | undefined> => {
        const uri = `${Environment.POKEDEX_API_V1_URL}/pokemon/search/name?search=${name}`
        const response = await fetch(uri, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        if (!response.ok) {
            pushToLoki({
                level: "error",
                message: response.statusText,
                data: name
            })
            throw new Error(response.statusText);
        }
        return await response.json() as PokemonSpeciesDto[];
    }
    

