import Image from 'next/image';
import {Box} from "@mui/material";
import {getPokemonByName} from '@shared/api';
import {notFound} from "next/navigation";


export default async function Page({params}: {
    params: Promise<{ name: string; }>
}) {
    const {name} = await params;
    const pokemon = await getPokemonByName(name)
    if (!pokemon) {
        notFound();
    }
    return <Box>
        {pokemon.description}
        <Image src={pokemon.images[1].src}
               width={200}
               height={200}
               priority
               alt={pokemon.name}/>
    </Box>
}