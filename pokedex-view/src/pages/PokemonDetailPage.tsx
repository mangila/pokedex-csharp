import {useParams} from "react-router";
import {findByName} from "@shared/api";
import {useQueryClient} from "@tanstack/react-query";
import {Typography} from "@mui/material";
import {useErrorBoundary} from "react-error-boundary";
import {useEffect, useState} from "react";
import {PokemonSpeciesDto} from "@shared/types";

export default function PokemonDetailPage() {
    const {name} = useParams();
    const {showBoundary} = useErrorBoundary();
    const [data, setData] = useState<PokemonSpeciesDto | undefined>(undefined);
    const queryClient = useQueryClient();

    useEffect(() => {
        if (name) {
            queryClient.fetchQuery({
                queryKey: ["pokemon-detail", name],
                queryFn: () => findByName(name)
            }).then(result => {
                setData(result)
            }).catch(reason => {
                showBoundary(new Error(reason))
            });
        }
    }, [name]);

    if (!data) {
        return null
    }


    return <>
        <Typography variant="body2" color="textSecondary" component="div">
            {data.name}
        </Typography>
    </>
}