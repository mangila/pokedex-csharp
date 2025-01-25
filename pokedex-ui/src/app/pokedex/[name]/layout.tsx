import {getAllPokemons, getPokemonByName} from "@/api";
import {Box} from "@mui/material";
import React from "react";

export const revalidate = 60

export const dynamicParams = false

export async function generateStaticParams() {
    const response = await getAllPokemons();
    return response.map((pokemon) => ({
        name: pokemon.name,
    }));
}

export async function generateMetadata({
                                           params,
                                       }: {
    params: Promise<{ title: string, name: string }>
}) {
    const {name} = await params
    const pokemon = await getPokemonByName(name)
    return {
        title: `pokedex-ui: ${pokemon.name}`,
    }
}

export default function Layout({children}: { children: React.ReactNode }) {
    return <Box>{children}</Box>
}