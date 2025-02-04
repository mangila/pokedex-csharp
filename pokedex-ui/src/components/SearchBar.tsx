"use client"
import {Autocomplete, Box, Grid2, TextField, Typography} from "@mui/material";
import {useEffect, useRef, useState} from "react";
import {useRouter} from "next/navigation";
import {PokemonSpeciesDto} from "@shared/types";
import {BLUR_IMAGE, capitalizeFirstLetter, padWithLeadingZeros} from "@shared/utils";
import Image from "next/image";
import {searchByName} from "@shared/api";

export default function SearchBar() {
    const router = useRouter();
    const [options, setOptions] = useState<PokemonSpeciesDto[]>([]);
    const [inputValue, setInputValue] = useState('');
    const inputRef = useRef<HTMLInputElement>(null);
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
            <Grid2 size={8}>
                <Autocomplete
                    onChange={(_event, newValue: PokemonSpeciesDto | null) => {
                        if (newValue) {
                            if (inputRef.current) {
                                inputRef.current.blur()
                            }
                            router.push(`/pokemon/${newValue.name}`);
                        }
                    }}
                    onInputChange={(event, newInputValue) => {
                        setInputValue(newInputValue);
                    }}
                    noOptionsText="No Pokemons found"
                    loading={loading}
                    options={options}
                    filterOptions={(x) => x}
                    disableCloseOnSelect
                    getOptionLabel={(option) => option.name}
                    renderOption={(props, option) => {
                        const {key, ...optionProps} = props;
                        const frontDefault = option.varieties
                            .filter(vareity => vareity.default)
                            .flatMap(vareity => vareity.images)
                            .find(media => media.file_name.includes("FrontDefault.png"))

                        if (!frontDefault) {
                            throw new Error("FrontDefault.png");
                        }

                        return (
                            <Box
                                key={key}
                                component="li"
                                {...optionProps}
                            >
                                <Box>
                                    <Typography color={"textSecondary"}>
                                        #{padWithLeadingZeros(option.id, 4)}
                                    </Typography>
                                </Box>
                                <Box>
                                    <Typography>
                                        {capitalizeFirstLetter(option.name)}
                                    </Typography>
                                </Box>
                                <Box>
                                    <Image
                                        src={frontDefault.src}
                                        alt={option.name}
                                        width={120}
                                        height={120}
                                        placeholder="blur"
                                        blurDataURL={BLUR_IMAGE}
                                    />
                                </Box>
                            </Box>
                        )
                    }}
                    renderInput={(params) => (
                        <TextField
                            inputRef={inputRef}
                            {...params}
                            label="Choose a country"
                            slotProps={{
                                htmlInput: {
                                    ...params.inputProps,
                                    autoComplete: 'new-password', // disable autocomplete and autofill
                                },
                            }}
                        />
                    )}
                />
            </Grid2>
        </Grid2>
    )
}