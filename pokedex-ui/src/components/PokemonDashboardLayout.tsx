"use client"

import {IconButton, Tooltip, Typography} from "@mui/material";
import {DashboardLayout, SidebarFooterProps} from "@toolpad/core";
import Link from "next/link";
import {GitHub} from "@mui/icons-material";
import Image from "next/image";

interface LayoutProps {
    children: React.ReactNode;
}

export default function PokemonDashboardLayout({children}: LayoutProps) {
    return <>
        <DashboardLayout
            sx={{
                '& .MuiIconButton-root': {
                    color: 'white',
                },
            }}
            branding={{
                logo: <Image
                    priority
                    width={80}
                    height={50}
                    src={"/logo-pokemon.png"}
                    alt="pokemon logo"/>,
                title: '',
            }}
            slots={{
                toolbarActions: ToolbarActions,
                sidebarFooter: SidebarFooter
            }}
        >
            {children}
        </DashboardLayout>
    </>
}

function SidebarFooter({mini}: SidebarFooterProps) {
    return (
        <Typography
            variant="caption"
            sx={{m: 1, whiteSpace: 'nowrap', overflow: 'hidden'}}
        >

            {mini ? '© Mangila' : `© ${new Date().getFullYear()} Made with by love Mangila`}
        </Typography>
    );
}

function ToolbarActions() {
    return (
        <Link href="https://github.com/mangila/pokedex-DOTNET"
              target="_blank"
        >
            <Tooltip title={"GitHub"}>
                <IconButton>
                    <GitHub/>
                </IconButton>
            </Tooltip>
        </Link>
    );
}