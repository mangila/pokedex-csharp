"use client"
import Link from 'next/link';
import {GitHub} from '@mui/icons-material/';
import {IconButton} from '@mui/material';

export default function GitHubLinkIcon() {

    return (
        <Link href="https://github.com/mangila/pokedex-DOTNET"
              target="_blank"
        >
            <IconButton sx={{
                color: 'white',
            }}>
                <GitHub/>
            </IconButton>
        </Link>
    );
}