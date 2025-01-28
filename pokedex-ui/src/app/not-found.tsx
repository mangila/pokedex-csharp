"use client"

import {DashboardLayout, PageContainer} from "@toolpad/core";
import React from "react";
import Image from "next/image";
import {Typography} from "@mui/material";

export default function NotFound() {
    return <>
        <DashboardLayout>
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
        </DashboardLayout>
    </>
}