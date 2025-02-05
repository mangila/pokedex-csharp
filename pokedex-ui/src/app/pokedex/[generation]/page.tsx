import PokemonGenerationCard from "@components/PokemonGenerationCard";
import {Box, Grid2} from "@mui/material";
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
                    return <Box
                        id={`${speciesDto.name}-generation`}
                        key={speciesDto.id}
                    >
                        <PokemonGenerationCard
                            key={speciesDto.id}
                            id={speciesDto.id}
                            speciesName={speciesDto.name}
                            pokemon={pokemon}
                            height={140}
                            width={140}/>
                    </Box>
                })
        })

    return <>
        <Grid2 container
               spacing={1}
               textAlign="center"
               alignItems="center"
               justifyContent="space-between"
        >
            {cards}
        </Grid2>
    </>
}