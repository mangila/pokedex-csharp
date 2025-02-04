import Image from "next/image";
import {Card, CardActionArea, CardContent, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {capitalizeFirstLetter, padWithLeadingZeros} from "@shared/utils";
import Link from "next/link";

interface PokemonCardProps {
    id: number
    speciesName: string
    pokemon: PokemonDto
}

export default function PokemonCard({id, speciesName, pokemon}: PokemonCardProps) {
    const officialArtworkFrontDefault = pokemon
        .images
        .find(media => media.file_name === `${pokemon.name}-OfficialArtworkFrontDefault.png`)

    if (!officialArtworkFrontDefault) {
        throw new Error(`${pokemon.name}-OfficialArtworkFrontDefault.png`);
    }

    return <>
        <Link
            href={`/pokemon/${speciesName}`}
            style={{textDecoration: "none"}}
        >
            <Card
                sx={{
                    textAlign: "center",
                }}
                elevation={4}
            >
                <CardActionArea>
                    <Image
                        src={officialArtworkFrontDefault.src}
                        alt={pokemon.name}
                        width={200}
                        height={200}
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {capitalizeFirstLetter(speciesName)}
                        </Typography>
                        <Typography fontSize={12} color="text.secondary">
                            #{padWithLeadingZeros(id, 4)}
                        </Typography>
                    </CardContent>
                </CardActionArea>
            </Card>
        </Link>
    </>
}