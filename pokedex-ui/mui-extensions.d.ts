import '@mui/material/Chip';

declare module '@mui/material/Chip' {
    interface ChipPropsVariantOverrides {
        fire: true;
        water: true;
        grass: true;
        electric: true;
    }
}