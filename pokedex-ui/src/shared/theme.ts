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
    },
});

export default theme;