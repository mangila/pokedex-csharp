"use client"
import Image from "next/image";
import {capitalize, Card, CardActionArea, Grid2, Tooltip, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {BLUR_IMAGE, padWithLeadingZeros, SessionStorageKeys} from "@shared/utils";
import {useRouter} from "next/navigation";
import {useScrollIntoLastVisitedFragment} from "@shared/hooks";

interface Props {
    id: number
    speciesName: string
    pokemon: PokemonDto
    width: number;
    height: number;
}

export default function PokemonGenerationCard(props: Props) {
    useScrollIntoLastVisitedFragment()
    const router = useRouter();
    const {id, speciesName, pokemon, width, height} = props;
    const frontDefault = pokemon.images
        .find(media => media.file_name === `${pokemon.name}-front-default.png`)

    if (!frontDefault) {
        throw new Error(`${pokemon.name}-front-default.png`);
    }

    return <>
        <Card>
            <Tooltip title={capitalize(speciesName)} placement="bottom" arrow>
                <CardActionArea
                    onClick={() => {
                        sessionStorage.setItem(
                            SessionStorageKeys.LAST_VISITED_FRAGMENT,
                            `${speciesName}-generation`)
                        router.push(`/pokemon/${speciesName}`)
                    }}
                >
                    <Grid2
                        container
                        direction="column"
                        justifyContent="center"
                        alignItems="center">
                        <Grid2>
                            <Image
                                src={frontDefault.src}
                                alt={speciesName}
                                width={width}
                                height={height}
                                placeholder="blur"
                                blurDataURL={BLUR_IMAGE}
                            />
                        </Grid2>
                        <Grid2>
                            <Typography
                                color={"textSecondary"}
                                fontSize={12}>
                                #{padWithLeadingZeros(id, 4)}
                            </Typography>
                        </Grid2>
                    </Grid2>
                </CardActionArea>
            </Tooltip>
        </Card>
    </>
}