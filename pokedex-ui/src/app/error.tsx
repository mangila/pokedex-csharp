'use client'

import React from 'react'
import {PageContainer} from "@toolpad/core";
import {Button, Typography} from '@mui/material';
import PokemonDashboardLayout from '@components/PokemonDashboardLayout';

export default function Error({
                                  error,
                                  reset,
                              }: {
    error: Error & { digest?: string }
    reset: () => void
}) {
    return <>
        <PokemonDashboardLayout>
            <PageContainer>
                <Typography gutterBottom>
                    Something went wrong.
                </Typography>
                <Typography>
                    {error.digest}
                </Typography>
                <Button
                    onClick={
                        () => reset()
                    }
                >
                    Try again
                </Button>
            </PageContainer>
        </PokemonDashboardLayout>
    </>
}