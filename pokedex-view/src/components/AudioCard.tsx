import React from 'react';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardMedia from '@mui/material/CardMedia';

interface AudioCardProps {
    audioSrc: string;
}

export const AudioCard: React.FC<AudioCardProps> = ({audioSrc}) => {
    const audioRef = React.useRef<HTMLAudioElement | null>(null);

    return (
        <Card sx={{maxWidth: 345}}>
            <CardHeader
                subheader="Legacy"
            />
            <CardMedia>
                <audio ref={audioRef} controls style={{width: '100%'}}>
                    <source src={audioSrc} type="audio/ogg"/>
                    Your browser does not support the audio element.
                </audio>
            </CardMedia>
        </Card>
    );
};
