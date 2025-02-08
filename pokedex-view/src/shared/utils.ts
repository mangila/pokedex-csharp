const appTitle = import.meta.env.VITE_APP_TITLE;
const mode = import.meta.env.MODE;
const pokedexApiUrl = import.meta.env.VITE_POKEDEX_API_URL;
const lokiApiUrl = import.meta.env.VITE_LOKI_API_URL;

export const ENVIRONMENT_VARS = {
    appTitle,
    mode,
    pokedexApiUrl,
    lokiApiUrl
};