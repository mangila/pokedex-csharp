import type {Metadata} from "next";
import {Roboto} from "next/font/google";
import {AppRouterCacheProvider} from '@mui/material-nextjs/v15-appRouter';
import {CssBaseline, ThemeProvider} from "@mui/material";
import {theme} from "@/theme";

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

export default function RootLayout({children}: Readonly<{ children: React.ReactNode }>) {
    return (
        <html lang="en">
        <body className={roboto.variable}>
        <AppRouterCacheProvider>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                {children}
            </ThemeProvider>
        </AppRouterCacheProvider>
        </body>
        </html>
    );
}
