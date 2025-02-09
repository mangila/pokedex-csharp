import {Box, capitalize, Card, CardActionArea, CardContent, CardMedia, Typography} from "@mui/material"
import {PokemonSpeciesDto} from "@shared/types"
import {padWithLeadingZeros} from "@shared/utils"
import {useNavigate} from "react-router";
import FavoriteButton from "./FavoriteButton";

interface Props {
    species: PokemonSpeciesDto
}

export default function PokemonGenerationCard({species}: Props) {
    const navigate = useNavigate();
    return <>
        <Card>
            <Box sx={{
                display: 'flex',
                justifyContent: "flex-end",
            }}>
                <FavoriteButton id={species.id}/>
            </Box>
            <CardActionArea onClick={() => {
                navigate(`/pokemon/${species.name}`);
            }}>
                <CardMedia
                    component="img"
                    height="140"
                    image={species.varieties[0].images[0].src}
                    alt="Beautiful Landscape"
                />
                <CardContent>
                    <Typography>
                        {capitalize(species.name)}
                    </Typography>
                    <Typography color="textSecondary" fontSize="small">
                        {padWithLeadingZeros(species.id)}
                    </Typography>
                </CardContent>
            </CardActionArea>
        </Card>
    </>
}