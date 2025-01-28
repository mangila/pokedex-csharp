export function padWithLeadingZeros(pokemonId: string, totalLength: number): string {
    while (pokemonId.length < totalLength) {
        pokemonId = '0' + pokemonId;
    }
    return pokemonId;
}

export const POKEDEX_API_V1_URL: string = process.env.POKEDEX_API_V1_URL;
export const LOKI_PUSH_URL: string = process.env.LOKI_PUSH_URL;
export const ENV = process.env.NODE_ENV;
export const APP_NAME = process.env.APP_NAME;

// https://next-placeholder.com/
export const BLUR_IMAGE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIUlEQVR4nGNgwAYO/H/WtOrTwjy4QM+VM2WVxzZGY1UNAHg6CsDqG+GAAAAAAElFTkSuQmCC"