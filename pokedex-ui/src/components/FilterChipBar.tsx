"use client"
import {ButtonBase, capitalize, Chip} from '@mui/material';
import React from 'react';

interface Props<T> {
    chips: T[]
    filter: T[];
    setFilterAction: React.Dispatch<React.SetStateAction<T[]>>;
}

export default function FilterChipBar<T>({chips, filter, setFilterAction}: Props<T>) {
    const handleChipClick = (type: T) => {
        setFilterAction(prevActiveTypes => {
            const newActiveTypes = prevActiveTypes.includes(type)
                ? prevActiveTypes.filter(t => t !== type)
                : [...prevActiveTypes, type];
            return newActiveTypes;
        });
    };

    const buttonBasedChips = chips.map((chip) => {
        const isActive = filter.includes(chip);
        return <ButtonBase key={chip as string} onClick={() => handleChipClick(chip)}>
            <Chip
                sx={{
                    color: isActive ? 'white' : 'default',
                    backgroundColor: isActive ? '#EF5350' : 'default',
                }}
                label={capitalize(chip as string)}
            />
        </ButtonBase>
    })

    return (
        <>
            {buttonBasedChips}
        </>
    );
}