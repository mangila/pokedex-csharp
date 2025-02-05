import PokemonGenerationCard from "@components/PokemonGenerationCard";
import {Grid2} from "@mui/material";
import {findByGeneration} from "@shared/api";
import {PokemonGeneration} from "@shared/types";
import {notFound} from "next/navigation";

export default async function Page({params}: {
    params: Promise<{ generation: PokemonGeneration; }>
}) {
    const {generation} = await params
    const species = await findByGeneration(generation)
    if (!species) {
        return notFound();
    }

    const cards = species
        .map(speciesDto => {
            return speciesDto.varieties
                .filter(pokemon => pokemon.default)
                .map(pokemon => {
                    return <PokemonGenerationCard
                        key={speciesDto.id}
                        id={speciesDto.id}
                        speciesName={speciesDto.name}
                        pokemon={pokemon}
                        height={96}
                        width={96}/>
                })
        })

    return <>
        <Grid2 container
               textAlign="center"
               alignItems="center"
               justifyContent="center"
        >
            {cards}
        </Grid2>
    </>;
}