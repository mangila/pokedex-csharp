import {useErrorBoundary} from "react-error-boundary";
import {useState} from "react";
import {PokemonSpecial, PokemonType} from '@shared/types';
import {useInfiniteScroll} from '@shared/hooks';
import {Box, Card, CardContent, CircularProgress, Grid2, Skeleton} from "@mui/material";
import MissingNo from "../assets/missingno.png"
import PokemonCard from "@components/PokemonCard";

export default function PokemonPage() {
    const {showBoundary} = useErrorBoundary();
    const [typesFilter, setTypesFilter] = useState<PokemonType[]>([]);
    const [specialFilter, setSpecialFilter] = useState<PokemonSpecial[]>([]);
    const {
        data,
        loader,
        isLoading,
        error
    } = useInfiniteScroll(["infinite-pokemons", typesFilter, specialFilter], 12, typesFilter, specialFilter);

    if (error) {
        setTypesFilter([])
        setSpecialFilter([])
        showBoundary(error);
    }

    return <>
        <Grid2 container
               direction={{
                   xs: "column",
                   sm: "row"
               }}
               spacing={1}
               textAlign="center"
               alignItems={{
                   xs: "center",
                   lg: "flex-start",
               }}
               justifyContent={{
                   xs: "center",
                   lg: "flex-start"
               }}>
            {
                data && data.pages
                    .flatMap(page => page.documents)
                    .map(species => <PokemonCard key={species.id} species={species}/>)
            }
        </Grid2>
        <Grid2 ref={loader}
               container
               direction={{
                   xs: "column",
                   sm: "row"
               }}
               spacing={1}
               textAlign="center"
               alignItems={{
                   xs: "center",
                   lg: "flex-start",
               }}
               justifyContent={{
                   xs: "center",
                   lg: "flex-start"
               }}
        >
            {isLoading && skeletons()}
            {data?.pages[0].documents.length === 0 &&
                <Box>
                    <img
                        src={MissingNo}
                        alt={"missingno.png"}
                    />
                </Box>
            }
        </Grid2>
    </>
}

const skeletons = () => {
    const s = []
    for (let i = 0; i < 12; i++) {
        s.push(<PokemonCardSkeleton key={i}/>)
    }
    return s;
}

function PokemonCardSkeleton() {
    return <>
        <Card>
            <Box sx={{
                display: 'flex',
                justifyContent: "flex-end",
            }}>
                <CircularProgress/>
            </Box>
            <Skeleton
                width={215}
                height={215}
            />
            <CardContent sx={{
                display: 'flex',
                justifyContent: "center",
                alignItems: "center",
            }}>
                <Skeleton width={50}/>
            </CardContent>
        </Card>
    </>
}
