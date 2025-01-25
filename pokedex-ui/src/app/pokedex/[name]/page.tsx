import {getPokemonByName, POKEDEX_API_V1_FILE_URL} from "@/api";
import {PokemonDto} from "@/types";
import Image from 'next/image';
import {Box} from "@mui/material";
import {AudioCard} from "@/components/AudioCard";

export default async function Page({params}: {
    params: Promise<{ name: string; }>
}) {
    const {name} = await params
    const pokemon: PokemonDto = await getPokemonByName(name)
    return <Box>
        <Image src={`${POKEDEX_API_V1_FILE_URL}/${pokemon.medias[0].media_id}`}
               width={200}
               height={200}
               priority
               alt={pokemon.name}/>
        {name}
        <AudioCard audioSrc={`${POKEDEX_API_V1_FILE_URL}/${pokemon.medias[2].media_id}`}/>
        <AudioCard audioSrc={`${POKEDEX_API_V1_FILE_URL}/${pokemon.medias[3].media_id}`}/>
    </Box>
}