import type {NextConfig} from "next";

const nextConfig: NextConfig = {
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
    },
};

export default nextConfig;
