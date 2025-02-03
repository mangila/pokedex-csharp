"use client"
import PokemonCard from "@components/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll} from "@shared/hooks";

export default function Page() {
    const {data, loader, isFetchingNextPage, hasNextPage, isLoading, error} = useInfiniteScroll(["pokemons"], 15);

    if (isLoading) {
        return <>Loading...</>
    }
    if (error) {
        throw error;
    }

    return <>
        <Grid2 container spacing={1}>
            {data?.pages.map((page) => (
                page.pokemons.map((pokemon) => (
                    <Grid2 key={pokemon.pokemon_id} container>
                        <PokemonCard pokemon={pokemon}/>
                    </Grid2>
                ))
            ))}
            <Box ref={loader}>
                {isFetchingNextPage ? 'Loading more...' : null}
                {hasNextPage ? 'Loading more...' : "No more pokemons to load"}
            </Box>
        </Grid2>
    </>;
}