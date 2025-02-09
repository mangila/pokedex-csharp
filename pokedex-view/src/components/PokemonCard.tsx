import {Box, capitalize, Card, CardActionArea, CardContent, CardMedia, Chip, Grid2, Typography} from "@mui/material";
import {useScrollIntoLastVisitedFragment} from "@shared/hooks";
import {PokemonSpeciesDto, PokemonType} from "@shared/types";
import {useNavigate} from "react-router";
import {useErrorBoundary} from "react-error-boundary";
import FavoriteButton from "./FavoriteButton";
import {LAST_VISITED_FRAGMENT, padWithLeadingZeros} from "@shared/utils";

interface Props {
    species: PokemonSpeciesDto
}

export default function PokemonCard({species}: Props) {
    useScrollIntoLastVisitedFragment()
    const fragmentId = `${species.name}-pokemon`
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

    const isSpecial = species.special.special;

    const types = species
        .varieties
        .filter(pokemon => pokemon.default)
        .flatMap(defaultPokemon => defaultPokemon.types)
        .map(type => <PokemonChip label={capitalize(type.type)} variant={type.type}/>)
    console.log(isSpecial);
    return <>
        <Card id={fragmentId}>
            <Box sx={{
                ml: 1,
                display: 'flex',
                justifyContent: isSpecial ? "space-between" : "flex-end",
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
                    height="215"
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
                    <Grid2 container
                           spacing={1}
                           direction="row"
                           textAlign="center"
                           justifyContent="center"
                           alignItems="center">
                        {types}
                    </Grid2>
                </CardContent>
            </CardActionArea>
        </Card>
    </>
}

interface PokemonChipProps {
    label: string;
    variant?: PokemonType
}

function PokemonChip({label, variant}: PokemonChipProps) {
    return <Chip label={label} variant={variant ?? "outlined"}/>;
}