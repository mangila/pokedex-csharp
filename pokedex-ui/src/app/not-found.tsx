"use client"

import {PageContainer} from "@toolpad/core";
import React from "react";
import Image from "next/image";
import {Typography} from "@mui/material";
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";

export default function NotFound() {
    return <>
        <PokemonDashboardLayout>
            <PageContainer>
                <Typography>
                    404 | Not found
                </Typography>
                <Image src={"/missingno.png"}
                       width={200}
                       height={200}
                       priority
                       alt={"missingno"}/>
            </PageContainer>
        </PokemonDashboardLayout>
    </>
}