import {getAllPokemons} from "@/api";
import {Box, Paper} from "@mui/material";

export default async function Page() {
    const bulba = await getAllPokemons();
    return <Box>
        <Paper>
            {bulba[0].name}
            dashboard
        </Paper>
    </Box>
}
