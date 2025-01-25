import {getAllPokemons} from "@/api";
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

export async function generateMetadata() {
    return {
        title: "pokedex-ui | pokedex",
    }
}

export default function Layout({children}: { children: React.ReactNode }) {
    return <Box>{children}</Box>
}