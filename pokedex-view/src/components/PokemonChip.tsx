import {Chip} from "@mui/material";
import {PokemonType} from "@shared/types";

interface PokemonChipProps {
    label: string;
    variant?: PokemonType
}

export default function PokemonChip({label, variant}: PokemonChipProps) {
    return <Chip label={label} variant={variant ?? "outlined"}/>;
}