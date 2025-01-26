import {findAllPokemonsByGeneration} from "@/api";
import {PokemonGeneration} from "@/types";
import {Box} from "@mui/material";

export default async function Page({params}: {
    params: Promise<{ generation: string; }>
}) {
    const {generation} = await params
    const pokemons = await findAllPokemonsByGeneration(generation as PokemonGeneration)
    return <Box sx={{
        display: "flex",
    }}>
        {pokemons.map(pokemon => (
            <Box key={pokemon.pokemon_id}>
                <img src={pokemon.images[0].src} alt={pokemon.name}/>
                {pokemon.name}
            </Box>
        ))}
    </Box>
}