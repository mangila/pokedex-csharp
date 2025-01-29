"use client"
import React from "react";
import {DashboardLayout, PageContainer, useActivePage} from "@toolpad/core";
import {useParams} from "next/navigation";
import {capitalizeFirstLetter} from "@shared/utils";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";

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
            <DashboardLayout>
                <PageContainer title={title} breadcrumbs={breadcrumbs}>
                    {children}
                </PageContainer>
            </DashboardLayout>
        </>
    }
    return <>
        <QueryClientProvider client={new QueryClient}>
            <DashboardLayout>
                <PageContainer>
                    {children}
                </PageContainer>
            </DashboardLayout>
        </QueryClientProvider>
    </>
}
