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
        .map(s => {
            return s.varieties
                .filter(p => p.default)
                .map(p => {
                    return <PokemonGenerationCard
                        key={s.id}
                        id={s.id}
                        speciesName={s.name}
                        pokemon={p}
                        height={96}
                        width={96}/>
                })
        })

    return <>
        <Grid2 container>
            {cards}
        </Grid2>
    </>;
}