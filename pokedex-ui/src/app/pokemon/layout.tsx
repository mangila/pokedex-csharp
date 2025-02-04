"use client"
import React from "react";
import {PageContainer} from "@toolpad/core";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";
import SearchBar from "@components/SearchBar";


export default function Layout({children}: { children: React.ReactNode }) {
    return <>
        <QueryClientProvider client={new QueryClient}>
            <PokemonDashboardLayout>
                <PageContainer>
                    <SearchBar/>
                    {children}
                </PageContainer>
            </PokemonDashboardLayout>
        </QueryClientProvider>
    </>
}
