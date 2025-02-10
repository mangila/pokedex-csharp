import {Box, capitalize, Card, CardActionArea, CardContent, CardMedia, Typography} from "@mui/material"
import {PokemonSpeciesDto} from "@shared/types"
import {LAST_VISITED_FRAGMENT, padWithLeadingZeros} from "@shared/utils"
import {useNavigate} from "react-router";
import FavoriteButton from "./FavoriteButton";
import {useErrorBoundary} from "react-error-boundary";
import {useScrollIntoLastVisitedFragment} from "@shared/hooks";
import PokemonChip from "./PokemonChip";

interface Props {
    species: PokemonSpeciesDto
}

export default function PokemonGenerationCard({species}: Props) {
    useScrollIntoLastVisitedFragment()
    const fragmentId = `${species.name}-generation`
    const navigate = useNavigate();
    const {showBoundary} = useErrorBoundary();

    const officalArtwork = species
        .varieties
        .filter(pokemon => pokemon.default)
        .flatMap(pokemon => pokemon.images)
        .find(media => media.file_name.includes("official-artwork-front-default.webp"))

    if (!officalArtwork) {
        showBoundary(new Error("not found official-artwork-front-default.webp"));
    }

    return <>
        <Card id={fragmentId}>
            <Box sx={{
                ml: 1,
                display: 'flex',
                justifyContent: species.special.special ? "space-between" : "flex-end",
                alignItems: "center",
            }}>
                {species.special.baby && <PokemonChip label={"Baby"}/>}
                {species.special.legendary && <PokemonChip label={"Legendary"}/>}
                {species.special.mythical && <PokemonChip label={"Mythical"}/>}
                <FavoriteButton id={species.id}/>
            </Box>
            <CardActionArea onClick={() => {
                sessionStorage.setItem(LAST_VISITED_FRAGMENT, fragmentId)
                navigate(`/pokemon/${species.name}`);
            }}>
                <CardMedia
                    component="img"
                    height="140"
                    image={officalArtwork!.src}
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