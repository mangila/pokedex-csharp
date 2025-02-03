"use client"
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";
import React from "react";
import {PageContainer} from "@toolpad/core";

export default function Loading() {
    return <>
        <PokemonDashboardLayout>
            <PageContainer>
                Loading...
            </PageContainer>
        </PokemonDashboardLayout>
    </>
}