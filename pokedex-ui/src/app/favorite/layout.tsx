"use client"

import React from "react";
import {DashboardLayout, PageContainer} from "@toolpad/core";

export default function Layout({children}: { children: React.ReactNode }) {
    return <>
        <DashboardLayout>
            <PageContainer>
                {children}
            </PageContainer>
        </DashboardLayout>
    </>
}