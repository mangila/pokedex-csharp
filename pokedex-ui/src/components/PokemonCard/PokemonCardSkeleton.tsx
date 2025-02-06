import {Card, CardContent, Grid2, Skeleton} from "@mui/material";

export default function PokemonCardSkeleton() {
    return <>
        <Card sx={{
            width: "280px",
        }}>
            <Grid2
                container
                justifyContent="center"
                alignItems="center"
            >
                <Skeleton
                    width={200}
                    height={200}
                />
            </Grid2>
            <CardContent>
                <Grid2 container
                       spacing={1}
                       justifyContent="center"
                       alignItems="center"
                >
                    <Skeleton
                        width={200}
                    />
                    <Skeleton
                        width={200}
                    />
                </Grid2>
            </CardContent>
        </Card>
    </>
}