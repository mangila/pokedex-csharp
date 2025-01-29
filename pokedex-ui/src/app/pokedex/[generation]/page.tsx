import PokemonGenerationDisplay from "@components/PokemonGenerationDisplay";
import {Grid2} from "@mui/material";
import {findAllPokemonsByGeneration} from "@shared/api";
import {PokemonGeneration} from "@shared/types";
import {notFound} from "next/navigation";

export default async function Page({params}: {
    params: Promise<{ generation: PokemonGeneration; }>
}) {
    const {generation} = await params
    const pokemons = await findAllPokemonsByGeneration(generation)
    if (!pokemons) {
        return notFound();
    }
    return <>
        <Grid2 container>
            {pokemons.map(pokemon => (
                <PokemonGenerationDisplay
                    key={pokemon.pokemon_id}
                    pokemon={pokemon}
                    height={96}
                    width={96}/>
            ))}
        </Grid2>
    </>;
}