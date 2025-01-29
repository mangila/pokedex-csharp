"use client"
import {Grid2} from "@mui/material";
import {useInfiniteScroll} from "@shared/hooks";

export default function Page() {
    const {data, loader, isFetchingNextPage, isLoading, error} = useInfiniteScroll(["pokemons"], 15);

    if (isLoading) {
        return <>Loading...</>
    }
    if (error) {
        throw error;
    }

    return <>
        <Grid2 container direction="row" spacing={1}>
            <div>
                {data?.pages.map((page) => (
                    <div key={page.current_page}>
                        {page.pokemons.map((pokemon) => (
                            <div key={pokemon.pokemon_id}>{pokemon.pokemon_id} . {pokemon.name}</div>
                        ))}
                    </div>
                ))}
                <div ref={loader}>
                    {isFetchingNextPage ? 'Loading more...' : 'No more items to load'}
                </div>
            </div>
        </Grid2>
    </>;
}