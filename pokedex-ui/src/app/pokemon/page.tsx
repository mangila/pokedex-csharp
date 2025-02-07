"use client"

import FilterChipBar from "@components/FilterChipBar";
import PokemonCard from "@components/PokemonCard/PokemonCard";
import {Box, Grid2} from "@mui/material";
import {useInfiniteScroll, useScrollIntoLastVisitedFragment} from "@shared/hooks";
import {PokemonSpecial, PokemonType} from "@shared/types";
import React, {useState} from "react";
import Image from "next/image";
import PokemonCardSkeleton from "@components/PokemonCard/PokemonCardSkeleton";

const specials: PokemonSpecial[] = [
    "baby", "legendary", "mythical"
];

const pokemonTypes: PokemonType[] = [
    "normal", "fighting", "flying", "poison", "ground",
    "rock", "bug", "ghost", "steel", "fire", "water",
    "grass", "electric", "psychic", "ice", "dragon",
    "dark", "fairy"
];

const skeletons = () => {
    const s = []
    for (let i = 0; i < 12; i++) {
        s.push(<Grid2 key={i}>
                <PokemonCardSkeleton/>
            </Grid2>
        )
    }
    return s;
}


export default function Page() {
    useScrollIntoLastVisitedFragment()
    const [typesFilter, setTypesFilter] = useState<PokemonType[]>([]);
    const [specialFilter, setSpecialFilter] = useState<PokemonSpecial[]>([]);
    const {
        data,
        loader,
        isLoading,
        error
    } = useInfiniteScroll(["pokemons", typesFilter, specialFilter], 12, typesFilter, specialFilter);

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
               }}>
            {cards}
        </Grid2>

        <Grid2 ref={loader}
               container
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
            {isLoading && skeletons()}
            {data?.pages[0].documents.length === 0 &&
                <Image src={"/missingno.png"}
                       width={200}
                       height={200}
                       priority
                       alt={"missingno"}/>
            }
        </Grid2>
    </>;
}