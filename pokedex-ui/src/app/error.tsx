'use client' // Error boundaries must be Client Components

import React, {useEffect} from 'react'
import {DashboardLayout, PageContainer} from "@toolpad/core";
import {Button} from '@mui/material';
import {pushToLoki} from '@shared/api';

export default function Error({
                                  error,
                                  reset,
                              }: {
    error: Error & { digest?: string }
    reset: () => void
}) {
    useEffect(() => {
        console.error(error)
        pushToLoki({
            level: 'error',
            message: error.message,
            data: error
        })
    }, [error])

    return <>
        <DashboardLayout>
            <PageContainer>
                Something went wrong
                <Button
                    onClick={
                        // Attempt to recover by trying to re-render the segment
                        () => reset()
                    }
                >
                    Try again
                </Button>
            </PageContainer>
        </DashboardLayout>
    </>
}