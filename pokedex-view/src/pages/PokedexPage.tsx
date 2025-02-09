import {useParams} from "react-router";
import {useQueryClient} from "@tanstack/react-query";
import {findByGeneration} from "@shared/api";
import {PokemonGeneration, PokemonSpeciesDto} from "@shared/types";
import {useErrorBoundary} from "react-error-boundary";
import PokemonGenerationCard from "@components/PokemonGenerationCard";
import {Grid2} from "@mui/material";
import {useEffect, useState} from "react";

export default function PokedexPage() {
    const {generation} = useParams();
    const {showBoundary} = useErrorBoundary();
    const [data, setData] = useState<PokemonSpeciesDto[] | undefined>(undefined);
    const queryClient = useQueryClient();

    useEffect(() => {
        if (generation) {
            queryClient.fetchQuery({
                queryKey: ["generation", generation],
                queryFn: () => findByGeneration(generation as PokemonGeneration)
            }).then(result => {
                setData(result)
            }).catch(reason => {
                showBoundary(new Error(reason))
            });
        }
    }, [generation]);

    if (!data) {
        return null
    }

    return <Grid2 container
                  mt={2}
                  spacing={1}
                  textAlign="center"
                  alignItems={{
                      xs: "center",
                      sm: "flex-start",
                  }}
                  justifyContent={{
                      xs: "center",
                      sm: "flex-start"
                  }}
    >
        {data.map(species => <Grid2 key={species.id}>
            <PokemonGenerationCard species={species}/>
        </Grid2>)}
    </Grid2>;
}