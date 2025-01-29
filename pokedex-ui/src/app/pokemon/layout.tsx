"use client"
import React from "react";
import {PageContainer, useActivePage} from "@toolpad/core";
import {useParams} from "next/navigation";
import {capitalizeFirstLetter} from "@shared/utils";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import PokemonDashboardLayout from "@components/PokemonDashboardLayout";

// Set active breadcrumbs for the active Pokemon or nothing if in /pokemon
export default function Layout({children}: { children: React.ReactNode }) {
    const {name} = useParams<{ name: string }>();
    const activePage = useActivePage();
    if (name && activePage) {
        const title = capitalizeFirstLetter(name);
        const path = `${activePage.path}/${title}`;
        const breadcrumbs = [...activePage.breadcrumbs, {
            title,
            path,
        }];
        return <>
            <PokemonDashboardLayout>
                <PageContainer title={title} breadcrumbs={breadcrumbs}>
                    {children}
                </PageContainer>
            </PokemonDashboardLayout>
        </>
    }
    return <>
        <QueryClientProvider client={new QueryClient}>
            <PokemonDashboardLayout>
                <PageContainer>
                    {children}
                </PageContainer>
            </PokemonDashboardLayout>
        </QueryClientProvider>
    </>
}
