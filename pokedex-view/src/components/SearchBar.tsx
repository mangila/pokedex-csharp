import {Autocomplete, Box, capitalize, CircularProgress, Grid2, TextField, Typography} from "@mui/material";
import {useEffect, useRef, useState} from "react";
import {PokemonSpeciesDto} from "@shared/types";
import {padWithLeadingZeros} from "@shared/utils";
import {searchByName} from "@shared/api";
import {useNavigate} from "react-router";

export default function SearchBar() {
    const navigate = useNavigate();
    const inputRef = useRef<HTMLInputElement>(null);
    const [options, setOptions] = useState<PokemonSpeciesDto[]>([]);
    const [inputValue, setInputValue] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                if (inputValue.length > 1) {
                    const pokemons = await searchByName(inputValue);
                    if (pokemons) {
                        setOptions(pokemons);
                    }
                }
            } catch (error) {
                console.error('Error fetching data:', error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [inputValue]);

    return (
        <Grid2 container
               direction="column"
               alignItems="center"
               mb={2}>
            <Grid2 size={{
                xs: 12,
                sm: 8
            }}>
                <Autocomplete
                    sx={{
                        backgroundColor: "white",
                        '& .MuiButtonBase-root': {
                            color: 'black',
                        },
                    }}
                    onChange={(_event, newValue: PokemonSpeciesDto | null) => {
                        if (newValue) {
                            if (inputRef.current) {
                                inputRef.current.blur()
                            }
                            navigate(`/pokemon/${newValue.name}`)
                        }
                    }}
                    onInputChange={(_event, newInputValue) => {
                        setInputValue(newInputValue);
                    }}
                    groupBy={(option) => option.pedigree.generation}
                    noOptionsText="No Pokemons found"
                    options={options}
                    loading={loading}
                    getOptionLabel={option => capitalize(option.name)}
                    renderOption={(props, option) => {
                        const {key, ...optionProps} = props;
                        return (
                            <Box
                                key={key}
                                component="li"
                                {...optionProps}
                            >
                                <Grid2 container
                                       spacing={2}>
                                    <Grid2>
                                        <Typography color={"textSecondary"}>
                                            #{padWithLeadingZeros(option.id)}
                                        </Typography>
                                    </Grid2>
                                    <Grid2>
                                        <Typography>
                                            {capitalize(option.name)}
                                        </Typography>
                                    </Grid2>
                                </Grid2>
                            </Box>
                        )
                    }}
                    renderInput={(params) => (
                        <TextField
                            inputRef={inputRef}
                            {...params}
                            label="Search for Pokemons"
                            slotProps={{
                                input: {
                                    ...params.InputProps,
                                    endAdornment: (
                                        <>
                                            {loading ? <CircularProgress color="inherit" size={20}/> : null}
                                            {params.InputProps.endAdornment}
                                        </>
                                    ),
                                },
                            }}
                        />
                    )}
                />
            </Grid2>
        </Grid2>
    )
}