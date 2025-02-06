'use client';
import {createTheme} from '@mui/material/styles';

const theme = createTheme({
    palette: {
        background: {
            default: '#FAF3E0', // Your custom background color
        },
    },
    typography: {
        fontFamily: 'var(--font-roboto)',
    },
    components: {
        MuiChip: {
            styleOverrides: {
                root: {
                    variants: [
                        {
                            props: {variant: 'normal'},
                            style: {
                                backgroundColor: '#A8A878',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'fighting'},
                            style: {
                                backgroundColor: '#C03028',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'flying'},
                            style: {
                                backgroundColor: '#A890F0',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'poison'},
                            style: {
                                backgroundColor: '#A040A0',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'ground'},
                            style: {
                                backgroundColor: '#E0C068',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'rock'},
                            style: {
                                backgroundColor: '#B8A038',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'bug'},
                            style: {
                                backgroundColor: '#A8B820',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'ghost'},
                            style: {
                                backgroundColor: '#705898',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'steel'},
                            style: {
                                backgroundColor: '#B8B8D0',
                                color: '#FFFFFF',
                            },
                        },
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
                        {
                            props: {variant: 'psychic'},
                            style: {
                                backgroundColor: '#F85888',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'ice'},
                            style: {
                                backgroundColor: '#98D8D8',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'dragon'},
                            style: {
                                backgroundColor: '#7038F8',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'dark'},
                            style: {
                                backgroundColor: '#705848',
                                color: '#FFFFFF',
                            },
                        },
                        {
                            props: {variant: 'fairy'},
                            style: {
                                backgroundColor: '#EE99AC',
                                color: '#FFFFFF',
                            },
                        }
                    ],
                },
            },
        },
    }
});

export default theme;