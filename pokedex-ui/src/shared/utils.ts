export const padWithLeadingZeros = (id: number, totalLength: number): string => {
    let pokemonId = id.toString()
    while (pokemonId.length < totalLength) {
        pokemonId = '0' + pokemonId;
    }
    return pokemonId;
}

export class SessionStorageKeys {
    static LAST_VISITED_FRAGMENT = "last-visited-fragment"
}

export class LocalStorageKeys {
    static FAVORITES = "favorites";
}

export class Environment {
    static POKEDEX_API_V1_URL: string = process.env.NEXT_PUBLIC_POKEDEX_API_V1_URL;
    static LOKI_PUSH_URL: string = process.env.LOKI_PUSH_URL;
    static ENV = process.env.NODE_ENV;
    static APP_NAME = process.env.APP_NAME;
}

// https://next-placeholder.com/
export const BLUR_IMAGE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIUlEQVR4nGNgwAYO/H/WtOrTwjy4QM+VM2WVxzZGY1UNAHg6CsDqG+GAAAAAAElFTkSuQmCC"