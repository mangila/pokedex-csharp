'use client';
import {createTheme} from '@mui/material/styles';

const theme = createTheme({

    typography: {
        fontFamily: 'var(--font-roboto)',
    },
    components: {
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: '#EF5350',
                },
            },
        },
        MuiChip: {
            styleOverrides: {
                root: {
                    variants: [
                        {
                            props: {variant: 'fire'},
                            style: {
                                backgroundColor: '#F08030',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'water'},
                            style: {
                                backgroundColor: '#6890F0',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'grass'},
                            style: {
                                backgroundColor: '#78C850',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'electric'},
                            style: {
                                backgroundColor: '#F8D030',
                                color: '#FFFFFF',
                            },
                        },
                    ],
                },
            },
        },
    }
});

export default theme;