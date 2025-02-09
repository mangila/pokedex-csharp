import {useQueryClient} from "@tanstack/react-query";
import {Grid2} from "@mui/material";
import PokemonGenerationCard from '@components/PokemonGenerationCard';
import {FAVORITE_POKEMON_IDS, FAVORITE_POKEMON_SPECIES, getFavorites} from '@shared/utils';
import {getPokemonByIds} from '@shared/api';
import {useEffect, useState} from "react";
import {PokemonSpeciesDto} from "@shared/types";
import {useErrorBoundary} from "react-error-boundary";

export default function FavoritePage() {
    const {showBoundary} = useErrorBoundary();
    const [data, setData] = useState<PokemonSpeciesDto[] | undefined>(undefined);
    const queryClient = useQueryClient();
    useEffect(() => {
        queryClient.fetchQuery({
            queryKey: [FAVORITE_POKEMON_IDS],
            queryFn: getFavorites,
        }).then(data => {
            queryClient.fetchQuery({
                queryKey: [FAVORITE_POKEMON_SPECIES],
                queryFn: () => getPokemonByIds(data),
            }).then(data => {
                setData(data)
            }).catch(reason => {
                showBoundary(new Error(reason));
            })
        })
    }, [queryClient, showBoundary])

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