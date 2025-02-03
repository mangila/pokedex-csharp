import Image from "next/image";
import {Card, CardActionArea, CardContent, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {capitalizeFirstLetter, padWithLeadingZeros} from "@shared/utils";

interface PokemonCardProps {
    pokemon: PokemonDto
}

export default function PokemonCard({pokemon}: PokemonCardProps) {
    return <>
        <Card
            sx={{
                textAlign: "center",
            }}
            elevation={4}
        >
            <CardActionArea>
                <Image
                    src={pokemon.images[1].src}
                    alt={pokemon.name}
                    width={200}
                    height={200}
                />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                        {capitalizeFirstLetter(pokemon.name)}
                    </Typography>
                    <Typography fontSize={12} color="text.secondary">
                        #{padWithLeadingZeros(pokemon.pokemon_id, 4)}
                    </Typography>
                </CardContent>
            </CardActionArea>
        </Card>
    </>
}