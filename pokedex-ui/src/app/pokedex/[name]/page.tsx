import {getPokemonByName} from "@/api";
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
        {name}
        <Image src={pokemon.images[0].src}
               width={200}
               height={200}
               priority
               alt={pokemon.name}/>
        <Image src={pokemon.images[1].src}
               width={200}
               height={200}
               priority
               alt={pokemon.name}/>
        <AudioCard audioSrc={pokemon.audios[0].src}/>
        <AudioCard audioSrc={pokemon.audios[1].src}/>
    </Box>
}