"use client"
import React from "react";
import {Grid2} from "@mui/material";
import PokemonGenerationCardSkeleton from "@components/PokemonGenerationCard/PokemonGenerationCardSkeleton";

const skeletons = () => {
    const s = []
    for (let i = 0; i < 25; i++) {
        s.push(
            <PokemonGenerationCardSkeleton
                key={i}
            />
        )
    }
    return s;
}

export default function Loading() {
    return <>
        <Grid2 container
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
            {skeletons()}
        </Grid2>
    </>
}