"use client"
import {Grid2} from "@mui/material";
import {getAllPokemons} from "@shared/api";
import {PaginationResultDto} from "@shared/types";
import {useInfiniteQuery} from "@tanstack/react-query";
import type {InfiniteData} from "@tanstack/query-core";

export default function Page() {
    const pageSize = 5;
    const {data, isFetchingNextPage, fetchNextPage, isLoading, error} = useInfiniteQuery<PaginationResultDto, Error>({
        queryKey: ["pokemons", pageSize],
        initialPageParam: 1,
        refetchOnMount: true,
        retry: 3,
        queryFn: ({pageParam}) =>
            getAllPokemons(pageParam as number, pageSize),
        getNextPageParam: (lastPage: PaginationResultDto) => {
            return lastPage.current_page + 1;
        },
    });

    if (isLoading) {
        return <>Loading...</>
    }
    if (error) {
        throw error;
    }

    const infiniteData = data as InfiniteData<PaginationResultDto>;

    return <>
        <Grid2 container direction="row" spacing={1}>
            <div>
                {infiniteData.pages.map((page) => (
                    <div key={page.current_page}>
                        {page.pokemons.map((pokemon) => (
                            <div key={pokemon.pokemon_id}>{pokemon.pokemon_id} . {pokemon.name}</div>
                        ))}
                    </div>
                ))}
                <button onClick={() => fetchNextPage()} disabled={isFetchingNextPage}>
                    {isFetchingNextPage ? 'Loading more...' : 'Load More'}
                </button>
            </div>
        </Grid2>
    </>;
}