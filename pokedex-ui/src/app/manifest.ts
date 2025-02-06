import {Environment} from '@shared/utils'
import type {MetadataRoute} from 'next'

export default function manifest(): MetadataRoute.Manifest {
    return {
        name: Environment.APP_NAME,
        description: 'Mangila@Github',
        start_url: '/',
        icons: [
            {
                src: '/favicon.ico',
                sizes: 'any',
                type: 'image/x-icon',
            },
        ],
    }
}