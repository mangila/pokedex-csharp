const APP_TITLE = import.meta.env.VITE_APP_TITLE;
const MODE = import.meta.env.MODE;
const POKEDEX_API_URL = import.meta.env.VITE_POKEDEX_API_URL;
export const FAVORITE_POKEMON_IDS = "favorite-pokemon-ids"
export const FAVORITE_POKEMON_SPECIES = "favorite-pokemons-species";
export const Environment = {
    APP_TITLE,
    MODE,
    POKEDEX_API_URL,
};

// Function to update the favorites in localstorage
export const updateFavorites = async (id: number): Promise<number[]> => {
    const favorites = await getFavorites();
    const newFavorites = favorites.includes(id)
        ? favorites.filter((favoriteId: number) => favoriteId !== id)
        : [...favorites, id];
    localStorage.setItem(FAVORITE_POKEMON_IDS, JSON.stringify(newFavorites));
    return newFavorites;
};

// Function to get the favorites in localstorage
export const getFavorites = (): Promise<number[]> => {
    const storedValue = localStorage.getItem(FAVORITE_POKEMON_IDS);
    const favorites = storedValue ? JSON.parse(storedValue) : [];
    return favorites;
}

// Function to pad the PokemonId with #0001, #0455
export const padWithLeadingZeros = (id: number): string => {
    const totalLength = 4
    let pokemonId = id.toString()
    while (pokemonId.length < totalLength) {
        pokemonId = '0' + pokemonId;
    }
    return "#" + pokemonId;
}