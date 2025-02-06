import '@mui/material/Chip';

declare module '@mui/material/Chip' {
    interface ChipPropsVariantOverrides {
        normal: true;
        fighting: true;
        flying: true;
        poison: true;
        ground: true;
        rock: true;
        bug: true;
        ghost: true;
        steel: true;
        fire: true;
        water: true;
        grass: true;
        electric: true;
        psychic: true;
        ice: true;
        dragon: true;
        dark: true;
        fairy: true;
    }
}