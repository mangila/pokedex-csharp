import { PokemonGeneration } from "@/types";
import {Box, Paper} from "@mui/material";

export default function Page() {
    return <Box>
        <Paper sx={{
            backgroundColor: "#1111",
        }}>
            Kanto
            {PokemonGeneration.GenerationI}
        </Paper>
        <Paper>
            {PokemonGeneration.GenerationII}
        </Paper>
        <Paper>
            {PokemonGeneration.GenerationIII}
        </Paper>
    </Box>
}