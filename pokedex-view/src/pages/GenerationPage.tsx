import Typography from "@mui/material/Typography";
import {useParams} from "react-router";

export default function GenerationPage() {
    const {generation} = useParams();
    return <Typography>Welcome to GenerationPage! {generation}</Typography>;
}