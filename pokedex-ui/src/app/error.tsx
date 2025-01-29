'use client'

import React from 'react'
import {DashboardLayout, PageContainer} from "@toolpad/core";
import {Button, Typography} from '@mui/material';

export default function Error({
                                  error,
                                  reset,
                              }: {
    error: Error & { digest?: string }
    reset: () => void
}) {
    return <>
        <DashboardLayout>
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
        </DashboardLayout>
    </>
}