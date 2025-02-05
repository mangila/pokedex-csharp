"use client"
import Image from "next/image";
import {Box, ButtonBase, Tooltip, Typography} from "@mui/material";
import {PokemonDto} from "@shared/types";
import {BLUR_IMAGE, capitalizeFirstLetter, padWithLeadingZeros} from "@shared/utils";
import {useRouter} from "next/navigation";

interface PokemonGenerationCardProps {
    id: number
    speciesName: string
    pokemon: PokemonDto
    width: number;
    height: number;
}

export default function PokemonGenerationCard(props: PokemonGenerationCardProps) {
    const router = useRouter();
    const {id, speciesName, pokemon, width, height} = props;
    const frontDefault = pokemon.images
        .find(media => media.file_name === `${pokemon.name}-front-default.png`)

    if (!frontDefault) {
        throw new Error(`${pokemon.name}-front-default.png`);
    }

    return <>
        <Tooltip title={capitalizeFirstLetter(speciesName)} placement="bottom" arrow>
            <ButtonBase
                onClick={() => {
                    router.push(`/pokemon/${speciesName}`)
                }}
            >
                <Box
                    sx={{
                        borderRadius: 7,
                        transition: 'background-color 0.3s, color 0.3s',
                        '&:hover': {
                            backgroundColor: '#EF5350',
                            '& .MuiTypography-root': {
                                color: 'white',
                            },
                        },
                    }}
                >
                    <Image
                        src={frontDefault.src}
                        alt={speciesName}
                        width={width}
                        height={height}
                        placeholder="blur"
                        blurDataURL={BLUR_IMAGE}
                    />
                    <Typography
                        color={"textSecondary"}
                        fontSize={12}>
                        #{padWithLeadingZeros(id, 4)}
                    </Typography>
                </Box>
            </ButtonBase>
        </Tooltip>
    </>
}