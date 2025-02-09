import {IconButton, Tooltip} from "@mui/material";
import {Favorite, FavoriteBorder} from "@mui/icons-material";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {FAVORITE_POKEMON_IDS, FAVORITE_POKEMON_SPECIES, getFavorites, updateFavorites} from "@shared/utils";
import {useEffect, useState} from "react";

interface Props {
    id: number
}

export default function FavoriteButton({id}: Props) {
    const [favorite, setFavorite] = useState<boolean>(false);
    const [clicked, setClicked] = useState<boolean>(false);
    const queryClient = useQueryClient();

    const handleFavoriteClick = () => {
        mutate();
    };

    useEffect(() => {
        queryClient.fetchQuery({
            queryKey: [FAVORITE_POKEMON_IDS],
            queryFn: getFavorites,
        }).then(result => {
            const includes = result.includes(id);
            setFavorite(includes)
        })
    }, [queryClient, id, clicked]);

    const {mutate} = useMutation<number[]>({
        mutationFn: () => updateFavorites(id),
        mutationKey: [FAVORITE_POKEMON_IDS],
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: [FAVORITE_POKEMON_IDS],
            });
            queryClient.invalidateQueries({
                queryKey: [FAVORITE_POKEMON_SPECIES],
            });
            setClicked(prevClicked => !prevClicked);
        }
    });
    
    return <Tooltip title={favorite ? "Unmark as Favorite" : "Mark as Favorite"}>
        <IconButton aria-label="favorite" onClick={handleFavoriteClick}>
            {favorite ? <Favorite/> : <FavoriteBorder/>}
        </IconButton>
    </Tooltip>
}