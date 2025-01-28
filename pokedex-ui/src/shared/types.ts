export type Loglevel = 'debug' | 'info' | 'warn' | 'error';

export interface LokiLogRequest {
    level: Loglevel;
    message: string;
    data: unknown
}

export interface PokemonDto {
    pokemon_id: string;
    name: string;
    region: string;
    height: string;
    weight: string;
    description: string;
    generation: string;
    types: PokemonTypeDto[];
    evolutions: PokemonEvolutionDto[];
    stats: PokemonStatDto[];
    images: PokemonMediaDto[];
    audios: PokemonMediaDto[];
    legendary: boolean;
    mythical: boolean;
    baby: boolean;
}

export interface PokemonEvolutionDto {
    value: number;
    name: string;
}

export interface PokemonStatDto {
    type: string;
    value: number;
}

export interface PokemonTypeDto {
    type: string;
}

export interface PokemonMediaDto {
    media_id: string;
    src: string;
    file_name: string;
    content_type: string;
}

export interface PokemonMediaProjectionDto {
    pokemon_id: string;
    name: string;
    images: PokemonMediaDto[];
}

export enum PokemonGeneration {
    GenerationI = "generation-i",
    GenerationII = "generation-ii",
    GenerationIII = "generation-iii",
    GenerationIV = "generation-iv",
    GenerationV = "generation-v",
    GenerationVI = "generation-vi",
    GenerationVII = "generation-vii",
    GenerationVIII = "generation-viii",
    GenerationIX = "generation-ix"
}

export enum PokemonRegion {
    Kanto = "Kanto",
    Johto = "Johto",
    Hoenn = "Hoenn",
    Sinnoh = "Sinnoh",
    Unova = "Unova",
    Kalos = "Kalos",
    Alola = "Alola",
    Galar = "Galar",
    Paldea = "Paldea",
}
