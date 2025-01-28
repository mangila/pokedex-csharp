import type {MetadataRoute} from 'next'
import {APP_NAME} from '@shared/utils'

export default function manifest(): MetadataRoute.Manifest {
    return {
        name: APP_NAME,
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