import Typography from "@mui/material/Typography";
import {useParams} from "react-router";

export default function PokemonDetailPage() {
    const {name} = useParams();
    return <Typography>Welcome to PokemonDetailPage!{name}</Typography>;
}