"use client"

import FilterChipBar from "@components/FilterChipBar";
import PokemonCard from "@components/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll, useScrollIntoLastVisitedFragment} from "@shared/hooks";
import {PokemonSpecial, PokemonType} from "@shared/types";
import {useState} from "react";

const specials: PokemonSpecial[] = [
    "baby", "legendary", "mythical"
];

const pokemonTypes: PokemonType[] = [
    "normal", "fighting", "flying", "poison", "ground",
    "rock", "bug", "ghost", "steel", "fire", "water",
    "grass", "electric", "psychic", "ice", "dragon",
    "dark", "unknown", "shadow", "fairy", "stellar"
];


export default function Page() {
    useScrollIntoLastVisitedFragment()
    const [typesFilter, setTypesFilter] = useState<PokemonType[]>([]);
    const [specialFilter, setSpecialFilter] = useState<PokemonSpecial[]>([]);
    const {data, loader, isLoading, error} = useInfiniteScroll(["pokemons"], 12, typesFilter, specialFilter);

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
                        return <Box
                            id={`${species.name}-pokemon`}
                            key={species.id}
                        >
                            <PokemonCard
                                id={species.id}
                                baby={species.baby}
                                legendary={species.legendary}
                                mythical={species.mythical}
                                speciesName={species.name}
                                pokemon={pokemon}/>
                        </Box>
                    });
            })
    })

    return <>
        <Grid2 container
               spacing={1}
               textAlign="center"
               alignItems="center"
               justifyContent="center"
               mb={2}
        >
            <FilterChipBar<PokemonType>
                chips={pokemonTypes}
                filter={typesFilter}
                setFilterAction={setTypesFilter}
            />
            <FilterChipBar<PokemonSpecial>
                chips={specials}
                filter={specialFilter}
                setFilterAction={setSpecialFilter}
            />
        </Grid2>
        <Grid2 container
               direction={{
                   xs: "column",
                   sm: "row"
               }}
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
            {cards}
            <Box ref={loader}>
                {isLoading ? 'Loading more...' : null}
            </Box>
        </Grid2>
    </>;
}