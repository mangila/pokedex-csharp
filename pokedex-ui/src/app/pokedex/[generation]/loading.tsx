"use client"
import React from "react";
import {Grid2, Skeleton} from "@mui/material";

const skeletons = () => {
    const s = []
    for (let i = 0; i < 20; i++) {
        s.push(
            <Grid2 key={i}>
                <Skeleton width={96} height={96}/>
            </Grid2>
        )
    }
    return s;
}

export default function Loading() {
    return <>
        <Grid2 container spacing={2}>
            {skeletons()}
        </Grid2>
    </>
}