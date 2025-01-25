import {getPokemonByName} from "@/api";
import {Box, Paper} from "@mui/material";

export default async function Page() {
    const bulba = await getPokemonByName("bulbasaur");
    return <Box>
        <Paper>
            {bulba.name}
            hello world
        </Paper>
    </Box>
}
