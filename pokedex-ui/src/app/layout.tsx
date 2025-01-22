import type {Metadata} from "next";
import {Roboto} from "next/font/google";
import {AppRouterCacheProvider} from '@mui/material-nextjs/v15-appRouter';
import {CssBaseline} from "@mui/material";
import {theme} from "@/theme";
import {CatchingPokemon, Dashboard} from '@mui/icons-material/';
import {NextAppProvider} from "@toolpad/core/nextjs";
import {Navigation} from "@toolpad/core";
import {DashboardLayout} from "@toolpad/core/DashboardLayout";
import {PageContainer} from "@toolpad/core/PageContainer";

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

const NAVIGATION: Navigation = [
    {
        kind: 'header',
        title: 'Main items',
    },
    {
        title: 'Dashboard',
        icon: <Dashboard/>,
    },
    {
        segment: 'pokedex',
        title: 'Pokedex',
        icon: <CatchingPokemon/>,
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
                                 title: 'Pokedex-ui',
                                 logo: ""
                             }}
                             theme={theme}>
                <DashboardLayout>
                    <PageContainer>
                        {children}
                    </PageContainer>
                </DashboardLayout>
            </NextAppProvider>
        </AppRouterCacheProvider>
        </body>
        </html>
    );
}
