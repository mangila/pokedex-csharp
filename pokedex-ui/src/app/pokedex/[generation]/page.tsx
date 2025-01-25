import {Box} from "@mui/material";

export default async function Page({params}: {
    params: Promise<{ generation: string; }>
}) {
    const {generation} = await params
    return <Box>
        {generation}
    </Box>
}