"use client"

import React from "react";
import {PageContainer} from "@toolpad/core";
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";

export default function Layout({children}: { children: React.ReactNode }) {
    return <>
        <PokemonDashboardLayout>
            <PageContainer>
                {children}
            </PageContainer>
        </PokemonDashboardLayout>
    </>
}