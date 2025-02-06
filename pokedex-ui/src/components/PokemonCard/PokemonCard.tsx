"use client"
import Image from "next/image";
import {capitalize, Card, CardActionArea, CardContent, Chip, Grid2, IconButton, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {padWithLeadingZeros, SessionStorageKeys} from "@shared/utils";
import {useRouter} from "next/navigation";
import {Favorite} from "@mui/icons-material";

interface Props {
    id: number
    speciesName: string
    baby: boolean
    legendary: boolean
    mythical: boolean
    pokemon: PokemonDto
}

export default function PokemonCard(props: Props) {
    const {id, speciesName, baby, legendary, mythical, pokemon} = props;
    const router = useRouter();
    const officialArtworkFrontDefault = pokemon
        .images
        .find(media => media.file_name === `${pokemon.name}-official-artwork-front-default.png`)

    if (!officialArtworkFrontDefault) {
        throw new Error(`${pokemon.name}-official-artwork-front-default.png`);
    }

    const types = pokemon.types.map((type) => (
        <Grid2 key={type.type}>
            <Chip label={capitalize(type.type)} variant={type.type}/>
        </Grid2>
    ));

    const isBaby = baby ? <Chip label={"Baby"} variant={"outlined"}/> : null
    const isLegendary = legendary ? <Chip label={"Legendary"} variant={"outlined"}/> : null
    const isMythical = mythical ? <Chip label={"Mythical"} variant={"outlined"}/> : null

    return <>
        <Card sx={{
            width: "280px",
        }}>
            <Grid2 container justifyContent="flex-end">
                <IconButton>
                    <Favorite/>
                </IconButton>
            </Grid2>
            <CardActionArea
                onClick={() => {
                    sessionStorage.setItem(
                        SessionStorageKeys.LAST_VISITED_FRAGMENT,
                        `${speciesName}-pokemon`)
                    router.push(`/pokemon/${speciesName}`)
                }}
            >
                <Grid2
                    container
                    justifyContent="center"
                    alignItems="center"
                >
                    <Image
                        src={officialArtworkFrontDefault.src}
                        alt={pokemon.name}
                        width={200}
                        height={200}
                    />
                </Grid2>
                <CardContent>
                    <Grid2 container
                           spacing={1}
                           justifyContent="center"
                           textAlign="center"
                           alignItems="center">
                        <Grid2 size={12}>
                            <Typography gutterBottom variant="h5" component="div">
                                {capitalize(speciesName)}
                            </Typography>
                        </Grid2>
                        <Grid2 container
                               justifyContent="center"
                               textAlign="center"
                               alignItems="center"
                        >
                            {types}
                            <Grid2>{isBaby}</Grid2>
                            <Grid2>{isLegendary}</Grid2>
                            <Grid2>{isMythical}</Grid2>
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