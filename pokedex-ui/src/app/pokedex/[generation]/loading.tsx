"use client"
import React from "react";
import {Grid2, Skeleton} from "@mui/material";

const skeletons = () => {
    const s = []
    for (let i = 0; i < 25; i++) {
        s.push(
            <Grid2 key={i}>
                <Skeleton width={140} height={140}/>
            </Grid2>
        )
    }
    return s;
}

export default function Loading() {
    return <>
        <Grid2 container
               spacing={1}
               textAlign="center"
               alignItems="center"
               justifyContent="space-between"
        >
            {skeletons()}
        </Grid2>
    </>
}