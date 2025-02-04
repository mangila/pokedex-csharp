"use client"
import React from "react";
import {PageContainer} from "@toolpad/core";
import {Grid2, Skeleton} from "@mui/material";

const skeletons = () => {
    const s = []
    for (let i = 0; i < 20; i++) {
        s.push(
            <Grid2 key={i}>
                <Skeleton width={200} height={200}/>
            </Grid2>
        )
    }
    return s;
}

export default function Loading() {
    return <>
        <PageContainer>
            <Grid2 container spacing={2}>
                {skeletons()}
            </Grid2>
        </PageContainer>
    </>
}