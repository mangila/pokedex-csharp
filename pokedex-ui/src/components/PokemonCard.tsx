import Image from "next/image";
import {Card, CardActionArea, CardContent, Chip, Grid2, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {capitalizeFirstLetter, padWithLeadingZeros} from "@shared/utils";

interface PokemonCardProps {
    id: number
    speciesName: string
    pokemon: PokemonDto
}

export default function PokemonCard({id, speciesName, pokemon}: PokemonCardProps) {
    const officialArtworkFrontDefault = pokemon
        .images
        .find(media => media.file_name === `${pokemon.name}-official-artwork-front-default.png`)

    if (!officialArtworkFrontDefault) {
        throw new Error(`${pokemon.name}-official-artwork-front-default.png`);
    }
    
    return <>
        <Card sx={{
            width: "280px",
        }}>
            <CardActionArea>
                <Image
                    src={officialArtworkFrontDefault.src}
                    alt={pokemon.name}
                    width={200}
                    height={200}
                />
                <CardContent>
                    <Grid2 container
                           spacing={2}>
                        <Grid2 size={12}>
                            <Typography gutterBottom variant="h5" component="div">
                                {capitalizeFirstLetter(speciesName)}
                            </Typography>
                        </Grid2>
                        <Grid2 size={6}>
                            <Chip label={pokemon.types[0].type} variant="filled"/>
                        </Grid2>
                        <Grid2 size={6}>
                            <Chip label={pokemon.types[0].type} variant="electric"/>
                        </Grid2>
                        <Grid2 size={12}>
                            <Typography fontSize={12} color="text.secondary">
                                #{padWithLeadingZeros(id, 4)}
                            </Typography>
                        </Grid2>
                    </Grid2>
                </CardContent>
            </CardActionArea>
        </Card>
    </>
}