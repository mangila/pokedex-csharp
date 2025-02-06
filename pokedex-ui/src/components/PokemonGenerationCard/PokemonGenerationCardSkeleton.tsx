import {Card, Grid2, Skeleton} from "@mui/material";

export default function PokemonGenerationCardSkeleton() {
    return <>
        <Card>
            <Grid2 container
                   direction="column"
                   justifyContent="center"
                   alignItems="center">
                <Grid2>
                    <Skeleton width={140}
                              height={140}/>
                </Grid2>
                <Grid2>
                    <Skeleton width={60}/>
                </Grid2>
            </Grid2>
        </Card>
    </>
}