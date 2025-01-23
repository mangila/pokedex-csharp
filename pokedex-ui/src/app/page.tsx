import {getPokemonByName, PokedexUrl} from "@/api";
import {Box, Paper} from "@mui/material";

export default async function Page() {
    const bulba = await getPokemonByName("bulbasaur");
    const img = `${PokedexUrl}/pokemon/file/${bulba.sprite_id}`
    return <Box>
        <img
            src={img}
            width="200"
            height="200"
            alt={bulba.name}
        />
        <Paper>
            hello world
        </Paper>
    </Box>
}
