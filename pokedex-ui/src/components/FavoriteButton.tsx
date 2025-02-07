"use client"
import React, {useState} from 'react';
import IconButton from '@mui/material/IconButton';
import Favorite from '@mui/icons-material/Favorite';
import FavoriteBorder from '@mui/icons-material/FavoriteBorder';
import {appendFavorites, hasFavorites, removeFavorites} from '@shared/utils';

interface Props {
    id: number;
}

export default function FavoriteButton({id}: Props) {
    const [favorite, setFavorite] = useState<boolean>(hasFavorites(id));

    const handleClick = () => {
        if (favorite) {
            removeFavorites(id);
            setFavorite(false);
        } else {
            appendFavorites(id)
            setFavorite(true);
        }
    };

    return (
        <IconButton onClick={handleClick}>
            {favorite ? (
                <Favorite sx={{color: "#EF5350"}}/>
            ) : (
                <FavoriteBorder/>
            )}
        </IconButton>
    );
}