import {Box, Button, Card, CardContent, Grid2, Typography} from "@mui/material";
import {useErrorBoundary} from "react-error-boundary";

interface Props {
    error: Error;
}

export default function ErrorPage({error}: Props) {
    const {resetBoundary} = useErrorBoundary();
    return <>
        <Grid2
            container
            mt={5}
            justifyContent="center"
            alignItems="center"
        >
            <Card sx={{
                width: "280px",
            }}>
                <CardContent>
                    <Grid2 container
                           spacing={2}
                           direction="column"
                           justifyContent="center"
                           alignItems="center"
                           textAlign="center">
                        <Typography variant="h4" color="textSecondary">
                            {error.message}
                        </Typography>
                        <Box>
                            <Button onClick={resetBoundary}>
                                Go to Dashboard
                            </Button>
                        </Box>
                    </Grid2>
                </CardContent>
            </Card>
        </Grid2>
    </>
}