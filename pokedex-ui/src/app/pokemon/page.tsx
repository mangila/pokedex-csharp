"use client"

import FilterChipBar from "@components/FilterChipBar";
import PokemonCard from "@components/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll} from "@shared/hooks";
import {useState} from "react";

const specialChips = [
    "Baby", "Legendary", "Mythical"
];

const typesChips = [
    "Normal", "Fighting", "Flying", "Poison", "Ground",
    "Rock", "Bug", "Ghost", "Steel", "Fire",
    "Water", "Grass", "Electric", "Psychic", "Ice",
    "Dragon", "Dark", "Unknown", "Shadow", "Fairy",
    "Stellar"
];

export default function Page() {
    const [typesFilter, setTypesFilter] = useState<string[]>([]);
    const [specialFilter, setSpecialFilter] = useState<string[]>([]);
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
                        return <PokemonCard
                            key={species.id}
                            id={species.id}
                            baby={species.baby}
                            legendary={species.legendary}
                            mythical={species.mythical}
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
               mb={2}
        >
            <FilterChipBar
                chips={typesChips}
                filter={typesFilter}
                setFilterAction={setTypesFilter}
            />
            <FilterChipBar
                chips={specialChips}
                filter={specialFilter}
                setFilterAction={setSpecialFilter}
            />
        </Grid2>
        <Grid2 container
               spacing={1}
               textAlign={"center"}
               justifyContent={"center"}
               alignItems={"center"}
        >
            {cards}
            <Box ref={loader}>
                {isLoading ? 'Loading more...' : null}
            </Box>
        </Grid2>
    </>;
}