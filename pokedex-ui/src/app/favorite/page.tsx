"use client"

import {Box, Grid2} from "@mui/material";
import {getPokemonByIds} from "@shared/api";
import {PokemonSpeciesDto} from "@shared/types";
import {getFavorites} from "@shared/utils";
import {notFound} from "next/navigation";
import React, {useEffect, useState} from "react";
import PokemonCardSkeleton from "@components/PokemonCard/PokemonCardSkeleton";
import PokemonCard from "@components/PokemonCard/PokemonCard";

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
    const [pokemons, setPokemons] = useState<PokemonSpeciesDto[] | null>(null);

    useEffect(() => {
        async function fetchData() {
            const pokemonData = await getPokemonByIds(getFavorites());
            if (!pokemonData) {
                notFound();
            }
            setPokemons(pokemonData);
        }

        fetchData();
    }, []);

    if (!pokemons) {
        return <Grid2
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
            }}>{skeletons()}</Grid2>
    }


    return <>
        <Grid2
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
            }}>
            {pokemons.map((species) => {
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
            })}
        </Grid2>
    </>;
}

