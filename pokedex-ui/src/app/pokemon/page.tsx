"use client"
import PokemonCard from "@components/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll} from "@shared/hooks";

export default function Page() {
    const {data, loader, isFetchingNextPage, hasNextPage, isLoading, error} = useInfiniteScroll(["pokemons"], 12);

    if (error) {
        throw error;
    }

    const cards = data?.pages.map((page) => {
        return page.documents
            .map((species) => {
                return species
                    .varieties
                    .filter((pokemon) => pokemon.default)
                    .map((pokemon) => {
                        return <PokemonCard
                            key={species.id}
                            id={species.id}
                            speciesName={species.name}
                            pokemon={pokemon}/>
                    });
            })
    })

    return <>
        <Grid2 container
               spacing={1}
               textAlign={"center"}
               justifyContent={"center"}
               alignItems={"center"}
        >
            {cards}
            <Box ref={loader}>
                {isFetchingNextPage ? 'Loading more...' : null}
                {hasNextPage && isLoading ? 'Loading more...' : null}
            </Box>
        </Grid2>
    </>;
}