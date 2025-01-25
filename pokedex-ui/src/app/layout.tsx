import type {Metadata} from "next";
import {Roboto} from "next/font/google";
import {AppRouterCacheProvider} from '@mui/material-nextjs/v15-appRouter';
import {CssBaseline} from "@mui/material";
import {theme} from "@/theme";
import {CatchingPokemon, Dashboard, DeveloperBoard, Favorite} from '@mui/icons-material/';
import {NextAppProvider} from "@toolpad/core/nextjs";
import {DashboardLayout} from "@toolpad/core/DashboardLayout";
import {PageContainer} from "@toolpad/core/PageContainer";
import GitHubLinkIcon from "@/components/GitHubLinkIcon";

const roboto = Roboto({
    weight: ['300', '400', '500', '700'],
    subsets: ['latin'],
    display: 'swap',
    variable: '--font-roboto',
});

export const metadata: Metadata = {
    title: "pokedex-ui",
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
            <NextAppProvider navigation={NAVIGATION}
                             branding={{
                                 logo: <img src={"/logo-pokemon.png"} alt="pokemon logo"/>,
                                 title: '',
                             }}
                             theme={theme}>
                <DashboardLayout
                    slots={{
                        toolbarActions: GitHubLinkIcon
                    }}
                >
                    <PageContainer
                        title={""}
                    >
                        {children}
                    </PageContainer>
                </DashboardLayout>
            </NextAppProvider>
        </AppRouterCacheProvider>
        </body>
        </html>
    );
}
