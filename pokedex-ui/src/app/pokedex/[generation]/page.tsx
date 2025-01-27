import {findAllPokemonsByGeneration} from "@/api";
import {PokemonGeneration} from "@/types";
import {padWithLeadingZeros} from "@/utils";
import {ButtonBase, Grid2, Tooltip, Typography} from "@mui/material";
import Image from "next/image";
import Link from 'next/link';

export default async function Page({params}: {
    params: Promise<{ generation: string; }>
}) {
    const {generation} = await params
    const pokemons = await findAllPokemonsByGeneration(generation as PokemonGeneration)
    return <Grid2 container>
        {pokemons.map(pokemon => (
            <Link
                key={pokemon.pokemon_id}
                href={`/pokemon/${pokemon.name}`}>
                <Grid2>
                    <Tooltip title={pokemon.name} placement="bottom" arrow>
                        <ButtonBase
                            sx={{
                                borderRadius: 4,
                                '&:hover': {
                                    backgroundColor: '#EF5350',
                                },
                            }}
                        >
                            <Grid2 container direction="column">
                                <Image
                                    priority
                                    src={pokemon.images[0].src}
                                    alt={pokemon.name}
                                    width={96}
                                    height={96}
                                />
                                <Typography
                                    fontSize={12}
                                    color={"textSecondary"}>
                                    #{padWithLeadingZeros(pokemon.pokemon_id, 4)}
                                </Typography>
                            </Grid2>
                        </ButtonBase>
                    </Tooltip>
                </Grid2>
            </Link>
        ))}
    </Grid2>
}