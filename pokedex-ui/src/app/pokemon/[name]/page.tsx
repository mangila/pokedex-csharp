import Image from 'next/image';
import {Box} from "@mui/material";
import {findByName} from '@shared/api';
import {notFound} from "next/navigation";

export default async function Page({params}: {
    params: Promise<{ name: string; }>
}) {
    const {name} = await params;
    const pokemon = await findByName(name)
    if (!pokemon) {
        notFound();
    }
    const officialArtworkFrontDefault = pokemon.varieties
        .filter(vareity => vareity.default)
        .flatMap(vareity => vareity.images)
        .find(media => media.file_name.includes("official-artwork-front-default.png"))

    if (!officialArtworkFrontDefault) {
        throw new Error("official-artwork-front-default.png");
    }

    return <Box>
        {pokemon.descriptions[0].description}
        <Image src={officialArtworkFrontDefault.src}
               width={200}
               height={200}
               priority
               alt={pokemon.name}/>
    </Box>
}