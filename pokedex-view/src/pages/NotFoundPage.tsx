import {Box, Card, CardContent, Grid2, Typography} from "@mui/material"
import MissingNo from "../assets/missingno.png"
import {Link} from "react-router"

export default function NotFoundPage() {
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
                            404 | Page Not Found
                        </Typography>
                        <Typography variant="body1" color="textSecondary">
                            The page you are looking for does not exist.
                        </Typography>
                        <Box>
                            <img
                                src={MissingNo}
                                alt={"missingno.png"}
                            />
                        </Box>
                        <Link to={"/dashboard"}>
                            Go to Dashboard
                        </Link>
                    </Grid2>
                </CardContent>
            </Card>
        </Grid2>
    </>
}