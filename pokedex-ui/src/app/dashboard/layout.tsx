import {Box} from "@mui/material";
import React from "react";

export async function generateMetadata() {
    return {
        title: "pokedex-ui | dashboard",
    }
}

export default function Layout({children}: { children: React.ReactNode }) {
    return <Box>{children}</Box>
}