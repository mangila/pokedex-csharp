'use client';
import {createTheme} from '@mui/material/styles';

export const theme = createTheme({
    palette: {
        background: {
            default: '#EF5350',  // Pokemon red
        },
    },
    typography: {
        fontFamily: 'var(--font-roboto)',
    },
});