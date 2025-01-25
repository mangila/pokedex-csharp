'use client' // Error boundaries must be Client Components

import {postLoki} from '@/api'
import {useEffect} from 'react'

export default function Error({
                                  error,
                                  reset,
                              }: {
    error: Error & { digest?: string }
    reset: () => void
}) {
    useEffect(() => {
        postLoki({
            loglevel: 'error',
            message: error.message,
            data: error
        }).catch((err: Error) => {
            {
                console.error(err)
            }
        });
    }, [error])

    return (
        <div>
            <h2>Something went wrong!</h2>
            <button
                onClick={
                    // Attempt to recover by trying to re-render the segment
                    () => reset()
                }
            >
                Try again
            </button>
        </div>
    )
}