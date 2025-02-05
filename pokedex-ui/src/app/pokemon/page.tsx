"use client"
import PokemonCard from "@components/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll} from "@shared/hooks";

export default function Page() {
    const {data, loader, isFetchingNextPage, hasNextPage, isLoading, error} = useInfiniteScroll(["pokemons"], 16);

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
                        return <Grid2 key={species.id}
                                      container
                                      textAlign={"center"}
                                      justifyContent={"center"}
                                      alignItems={"center"}
                                      size={{xs: 12, sm: 4, lg: 3}}
                        >
                            <PokemonCard id={species.id}
                                         speciesName={species.name}
                                         pokemon={pokemon}/>
                        </Grid2>
                    });
            })
    })

    return <>
        <Grid2 container spacing={1} sx={{
            flexDirection: {
                xs: 'column',
                sm: 'row',
            },
        }}>
            {cards}
            <Box ref={loader}>
                {isFetchingNextPage ? 'Loading more...' : null}
                {hasNextPage && isLoading ? 'Loading more...' : null}
            </Box>
        </Grid2>
    </>;
}