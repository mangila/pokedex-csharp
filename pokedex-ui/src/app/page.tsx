import {Box, Paper} from "@mui/material";
import loki from "../loki"

export default function Page() {
    loki.info("Page loaded");
    return <Box>
        Home page
        <Paper>
            hello world
        </Paper>
    </Box>
}
