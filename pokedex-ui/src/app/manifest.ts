import type {MetadataRoute} from 'next'

export default function manifest(): MetadataRoute.Manifest {
    return {
        name: 'pokedx-ui',
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