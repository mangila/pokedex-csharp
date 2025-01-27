export function padWithLeadingZeros(pokemonId: string, totalLength: number): string {
    while (pokemonId.length < totalLength) {
        pokemonId = '0' + pokemonId;
    }
    return pokemonId;
}