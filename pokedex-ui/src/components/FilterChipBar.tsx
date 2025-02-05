"use client"
import {ButtonBase, Chip} from '@mui/material';
import React from 'react';

interface Props {
    chips: string[]
    filter: string[];
    setFilterAction: React.Dispatch<React.SetStateAction<string[]>>;
}

export default function FilterChipBar({chips, filter, setFilterAction}: Props) {
    const handleChipClick = (type: string) => {
        setFilterAction(prevActiveTypes => {
            const newActiveTypes = prevActiveTypes.includes(type)
                ? prevActiveTypes.filter(t => t !== type)
                : [...prevActiveTypes, type];
            return newActiveTypes;
        });
    };

    return (
        <>
            {chips.map(type => (
                <ButtonBase key={type} onClick={() => handleChipClick(type)}>
                    <Chip
                        label={type}
                        color={filter.includes(type) ? "primary" : "default"}
                    />
                </ButtonBase>
            ))}
        </>
    );
}