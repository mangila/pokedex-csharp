import {DashboardLayout, DialogProps, PageContainer, SidebarFooterProps, useDialogs} from "@toolpad/core";
import {Link, Outlet, useNavigate} from "react-router";
import {
    Dialog,
    DialogContent,
    DialogTitle,
    Divider,
    Grid2,
    IconButton,
    Stack,
    Tooltip,
    Typography
} from "@mui/material";
import {GitHub, Info} from "@mui/icons-material";
import {Environment} from "@shared/utils";
import PokemonLogo from "../assets/logo-pokemon.png"
import {ErrorBoundary} from "react-error-boundary";
import ErrorPage from "@pages/ErrorPage";

export default function PokemonDashboardLayout() {
    const navigate = useNavigate();
    return <>
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
            <ErrorBoundary FallbackComponent={ErrorPage}
                           onReset={() => {
                               navigate("/dashboard")
                           }}>
                <PageContainer>
                    <Outlet/>
                </PageContainer>
            </ErrorBoundary>
        </DashboardLayout>
    </>
}

function AppTitle() {
    return (
        <Stack
            direction="row"
            alignItems="center"
            spacing={2}>
            <img
                src={PokemonLogo}
                alt="pokemon-logo.png"
            />
        </Stack>
    );
}

function SidebarFooter({mini}: SidebarFooterProps) {
    return (
        <Stack>
            {mini ? null : <Typography variant="h6">{Environment.APP_TITLE}</Typography>}
            {mini ? null : <Typography>© {new Date().getFullYear()} Made with love by Mangila</Typography>}
        </Stack>
    );
}

function ToolbarActions() {
    const dialogs = useDialogs();
    return (
        <>
            <Grid2 container>
                <Link to="https://github.com/mangila/pokedex-DOTNET" target="_blank">
                    <Tooltip title={"GitHub"}>
                        <IconButton>
                            <GitHub/>
                        </IconButton>
                    </Tooltip>
                </Link>
                <Tooltip title={"About"}>
                    <IconButton onClick={() => dialogs.open(AboutDialog)}>
                        <Info/>
                    </IconButton>
                </Tooltip>
            </Grid2>
        </>
    );
}

function AboutDialog({open, onClose}: DialogProps) {
    return (
        <Dialog fullWidth open={open} onClose={() => onClose()}>
            <DialogTitle>About</DialogTitle>
            <Divider/>
            <DialogContent>
                <Typography>
                    Version: {__APP_VERSION__}
                </Typography>
                <Typography>
                    Build: {__BUILD_ID__}
                </Typography>
                <Typography>
                    Mode: {Environment.MODE}
                </Typography>
            </DialogContent>
        </Dialog>
    );
}