import type {Metadata} from "next";
import {Roboto} from "next/font/google";
import {AppRouterCacheProvider} from '@mui/material-nextjs/v15-appRouter';
import {CssBaseline} from "@mui/material";
import {CatchingPokemon, Dashboard, DeveloperBoard, Favorite} from '@mui/icons-material';
import {NextAppProvider} from "@toolpad/core/nextjs";
import theme from "@shared/theme";
import {APP_NAME} from "@shared/utils";
import {Suspense} from "react";

const roboto = Roboto({
    weight: ['300', '400', '500', '700'],
    subsets: ['latin'],
    display: 'swap',
    variable: '--font-roboto',
});

export const metadata: Metadata = {
    title: APP_NAME,
    description: "Mangila@Github",
};

const NAVIGATION = [
    {
        title: 'Dashboard',
        icon: <Dashboard/>,
        segment: 'dashboard',
    },
    {
        title: 'Pokemon',
        icon: <CatchingPokemon/>,
        segment: 'pokemon',
        pattern: 'pokemon{/:name}*',
    },
    {
        title: 'Pokedex',
        icon: <DeveloperBoard/>,
        segment: 'pokedex',
        children: [
            {title: 'Generation I', segment: 'generation-i'},
            {title: 'Generation II', segment: 'generation-ii'},
            {title: 'Generation III', segment: 'generation-iii'},
            {title: 'Generation IV', segment: 'generation-iv'},
            {title: 'Generation V', segment: 'generation-v'},
            {title: 'Generation VI', segment: 'generation-vi'},
            {title: 'Generation VII', segment: 'generation-vii'},
            {title: 'Generation VIII', segment: 'generation-viii'},
            {title: 'Generation IX', segment: 'generation-ix'},
        ],
    },
    {
        title: 'Favorites',
        icon: <Favorite/>,
        segment: 'favorite',
    },
];

export default function RootLayout({children}: Readonly<{ children: React.ReactNode }>) {
    return (
        <html lang="en">
        <body className={roboto.variable}>
        <AppRouterCacheProvider>
            <CssBaseline/>
            <Suspense fallback={<div>loading..</div>}>
                <NextAppProvider navigation={NAVIGATION}
                                 theme={theme}>
                    {children}
                </NextAppProvider>
            </Suspense>
        </AppRouterCacheProvider>
        </body>
        </html>
    );
}
