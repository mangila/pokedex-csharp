"use client"
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";
import {PageContainer} from "@toolpad/core";
import React from "react";

export default function Loading() {
    return <>
        <PokemonDashboardLayout>
            <PageContainer>
                Loading...
            </PageContainer>
        </PokemonDashboardLayout>
    </>
}