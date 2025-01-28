'use client' // Error boundaries must be Client Components

import React, {useEffect} from 'react'
import {DashboardLayout, PageContainer} from "@toolpad/core";
import {Button, Typography} from '@mui/material';
import {LokiLogRequest} from '@shared/types';

const postLoki = async (request: LokiLogRequest): Promise<boolean> => {
    const uri = "/api/loki";
    const response = await fetch(uri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
    });
    return response.ok;
};

export default function Error({
                                  error,
                                  reset,
                              }: {
    error: Error & { digest?: string }
    reset: () => void
}) {
    useEffect(() => {
        const sendLog = async () => {
            const success = await postLoki({
                level: 'error',
                message: error.message,
                data: error
            });
            if (success) {
                console.log('Log sent successfully');
            } else {
                console.error('Failed to send log');
            }
        };
        sendLog();
    });

    return <>
        <DashboardLayout>
            <PageContainer>
                <Typography>
                    {error.name}
                </Typography>
                <Typography>
                    {error.message}
                </Typography>
                <Typography>
                    {error.digest}
                </Typography>
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