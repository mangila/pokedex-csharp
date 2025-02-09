export enum PokemonType {
    Normal = "normal",
    Fighting = "fighting",
    Flying = "flying",
    Poison = "poison",
    Ground = "ground",
    Rock = "rock",
    Bug = "bug",
    Ghost = "ghost",
    Steel = "steel",
    Fire = "fire",
    Water = "water",
    Grass = "grass",
    Electric = "electric",
    Psychic = "psychic",
    Ice = "ice",
    Dragon = "dragon",
    Dark = "dark",
    Fairy = "fairy",
}

export enum PokemonSpecial {
    Baby = "baby",
    Legendary = "legendary",
    Mythical = "mythical",
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

export interface PokemonNameDto {
    language: string;
    name: string;
}

export interface PokemonDescriptionDto {
    language: string;
    description: string;
}

export interface PokemonGeneraDto {
    language: string;
    genera: string;
}

export interface PokemonMediaDto {
    media_id: string;
    src: string;
    file_name: string;
    content_type: string;
}

export interface PokemonPedigreeDto {
    generation: string;
    region: string;
}

export interface PokemonStatDto {
    type: string;
    value: number;
}

export interface PokemonTypeDto {
    type: PokemonType;
}

export interface PokemonEvolutionDto {
    value: number;
    name: string;
}

export interface PokemonDto {
    name: string;
    default: boolean;
    height: string;
    weight: string;
    types: PokemonTypeDto[];
    stats: PokemonStatDto[];
    images: PokemonMediaDto[];
    audios: PokemonMediaDto[];
}

export interface PokemonSpeciesDto {
    id: number;
    name: string;
    names: PokemonNameDto[];
    descriptions: PokemonDescriptionDto[];
    genera: PokemonGeneraDto[];
    pedigree: PokemonPedigreeDto;
    evolutions: PokemonEvolutionDto[];
    varieties: PokemonDto[];
    legendary: boolean;
    mythical: boolean;
    baby: boolean;
}

export interface PaginationResultDto {
    total_count: number;
    total_pages: number;
    current_page: number;
    page_size: number;
    documents: PokemonSpeciesDto[];
}
