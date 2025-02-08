import {DashboardLayout, DialogsProvider, SidebarFooterProps} from "@toolpad/core";
import {Link, Outlet} from "react-router";
import {Button, Chip, IconButton, Stack, Tooltip, Typography} from "@mui/material";
import {CheckCircle, GitHub} from "@mui/icons-material";
import {ENVIRONMENT_VARS} from "@shared/utils";
import PokemonLogo from "../assets/logo-pokemon.png"

export default function PokemonDashboardLayout() {
    return <>
        <DialogsProvider>
            <DashboardLayout
                sx={{
                    '& .MuiIconButton-root': {
                        color: '#DCDCDC',
                    },
                    '& .MuiToolbar-root': {
                        backgroundColor: '#EF5350',
                    },
                }}
                slots={{
                    appTitle: AppTitle,
                    toolbarActions: ToolbarActions,
                    sidebarFooter: SidebarFooter
                }}
            >
                <Outlet/>
            </DashboardLayout>
        </DialogsProvider>
    </>
}

function AppTitle() {
    return (
        <Stack direction="row" alignItems="center" spacing={2}>
            <img
                src={PokemonLogo}
                alt="pokemon-logo.png"
            />
            <Typography variant="h6">{ENVIRONMENT_VARS.appTitle}</Typography>
            <Chip size="small" label={ENVIRONMENT_VARS.mode} color="info"/>
            <Tooltip title={`Connected to ${ENVIRONMENT_VARS.pokedexApiUrl}`}>
                <CheckCircle color="success" fontSize="small"/>
            </Tooltip>
        </Stack>
    );
}

function SidebarFooter({mini}: SidebarFooterProps) {
    console.log(__APP_VERSION__);
    console.log(__BUILD_ID__);
    return (
        <Stack>
            <Typography>
                {__APP_VERSION__}
                {__BUILD_ID__}
            </Typography>
            <Typography
                variant="caption"
                sx={{m: 1, whiteSpace: 'nowrap', overflow: 'hidden'}}
            >
                {mini ? '© Mangila' : `© ${new Date().getFullYear()} Made with love by Mangila`}
            </Typography>
            <Button color={"secondary"}>
                About
            </Button>
        </Stack>
    );
}

function ToolbarActions() {
    return (
        <Link to="https://github.com/mangila/pokedex-DOTNET" target="_blank">
            <Tooltip title={"GitHub"}>
                <IconButton>
                    <GitHub/>
                </IconButton>
            </Tooltip>
        </Link>
    );
}