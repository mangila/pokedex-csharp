export type Loglevel = 'debug' | 'info' | 'warn' | 'error';

export interface LokiLogRequest {
    loglevel: Loglevel;
    message: string;
    data: unknown
}

export interface PokemonDto {
    pokemon_id: string;
    name: string;
    height: string;
    weight: string;
    description: string;
    generation: string;
    types: PokemonTypeDto[];
    evolutions: PokemonEvolutionDto[];
    stats: PokemonStatDto[];
    audio_id: string;
    sprite_id: string;
    legendary: boolean;
    mythical: boolean;
    baby: boolean;
}

export interface PokemonTypeDto {
    type: string;
}

export interface PokemonEvolutionDto {
    value: number;
    name: string;
}

export interface PokemonStatDto {
    type: string;
    value: number;
}