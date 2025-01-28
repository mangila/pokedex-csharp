import type {NextConfig} from "next";

const nextConfig: NextConfig = {
    async redirects() {
        return [
            {
                source: '/',
                destination: '/dashboard',
                permanent: true,
            },
            {
                source: '/pokedex',
                destination: '/dashboard',
                permanent: true,
            },
        ]
    },
    generateBuildId: async () => {
        return Date.now().toString()
    },
    images: {
        remotePatterns: [
            {
                protocol: 'http',
                hostname: 'localhost',
                port: '5144',
                pathname: '/api/v1/pokemon/file/**',
            }
        ]
    },
    logging: {
        fetches: {
            fullUrl: true,
        },
    }
};

export default nextConfig;
