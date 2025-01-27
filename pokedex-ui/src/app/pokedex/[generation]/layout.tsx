import {PokemonGeneration} from "@/types";
import {Box} from "@mui/material";
import React from "react";


export const dynamicParams = false

export async function generateStaticParams() {
    return Object.values(PokemonGeneration).map((generation) => ({
        generation: generation.toString(),
    }));
}

export async function generateMetadata({params}: {
    params: Promise<{ generation: string; }>
}) {
    const {generation} = await params;
    return {
        title: `pokedex-ui | pokedex | ${generation}`,
    }
}

export default function Layout({children}: { children: React.ReactNode }) {
    return <Box>{children}</Box>
}