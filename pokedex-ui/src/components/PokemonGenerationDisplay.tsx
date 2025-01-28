import Image from "next/image";
import {Grid2, Tooltip, Typography} from "@mui/material";
import {PokemonMediaProjectionDto} from "@shared/types";
import {BLUR_IMAGE, padWithLeadingZeros} from "@shared/utils";
import Link from "next/link";

interface PokemonImageProps {
    pokemon: PokemonMediaProjectionDto
    width: number;
    height: number;
}

export default function PokemonGenerationDisplay(props: PokemonImageProps) {
    const {pokemon, width, height} = props;
    return <Link
        href={`/pokemon/${pokemon.name}`}>
        <Tooltip title={pokemon.name} placement="bottom" arrow>
            <Grid2
                container
                alignItems="center"
                direction="column"
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
                    src={pokemon.images[0].src}
                    alt={pokemon.name}
                    width={width}
                    height={height}
                    placeholder="blur"
                    blurDataURL={BLUR_IMAGE}
                />
                <Typography
                    color={"textSecondary"}
                    fontSize={12}>
                    #{padWithLeadingZeros(pokemon.pokemon_id, 4)}
                </Typography>
            </Grid2>
        </Tooltip>
    </Link>
}