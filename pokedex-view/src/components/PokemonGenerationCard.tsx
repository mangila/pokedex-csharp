import {Box, capitalize, Card, CardActionArea, CardContent, CardMedia, Typography} from "@mui/material"
import {PokemonSpeciesDto} from "@shared/types"
import {LAST_VISITED_FRAGMENT, padWithLeadingZeros} from "@shared/utils"
import {useNavigate} from "react-router";
import FavoriteButton from "./FavoriteButton";
import {useErrorBoundary} from "react-error-boundary";
import {useScrollIntoLastVisitedFragment} from "@shared/hooks";

interface Props {
    species: PokemonSpeciesDto
}

export default function PokemonGenerationCard({species}: Props) {
    useScrollIntoLastVisitedFragment()
    const fragmentId = `${species.name}-generation`
    const navigate = useNavigate();
    const {showBoundary} = useErrorBoundary();

    const frontDefault = species
        .varieties
        .filter(pokemon => pokemon.default)
        .flatMap(pokemon => pokemon.images)
        .find(media => media.file_name.includes("front-default.webp"))

    if (!frontDefault) {
        showBoundary(new Error("not found front-default.webp"));
    }

    return <>
        <Card id={fragmentId}>
            <Box sx={{
                display: 'flex',
                justifyContent: "flex-end",
            }}>
                <FavoriteButton id={species.id}/>
            </Box>
            <CardActionArea onClick={() => {
                sessionStorage.setItem(LAST_VISITED_FRAGMENT, fragmentId)
                navigate(`/pokemon/${species.name}`);
            }}>
                <CardMedia
                    component="img"
                    height="140"
                    image={frontDefault!.src}
                    alt={species.name}
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