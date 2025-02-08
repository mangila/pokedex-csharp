import {CatchingPokemon, Dashboard, DeveloperBoard, Favorite} from '@mui/icons-material';
import {ReactRouterAppProvider} from '@toolpad/core/react-router';
import {Outlet} from 'react-router';

const NAVIGATION = [
    {
        title: 'Dashboard',
        icon: <Dashboard/>,
        segment: '',
    },
    {
        title: 'Pokemon',
        icon: <CatchingPokemon/>,
        segment: 'pokemon',
        pattern: 'pokemon{/:name}*',
    },
    {
        title: 'Pokedex',
        icon: <DeveloperBoard/>,
        segment: 'pokedex',
        children: [
            {title: 'Generation I', segment: 'generation-i'},
            {title: 'Generation II', segment: 'generation-ii'},
            {title: 'Generation III', segment: 'generation-iii'},
            {title: 'Generation IV', segment: 'generation-iv'},
            {title: 'Generation V', segment: 'generation-v'},
            {title: 'Generation VI', segment: 'generation-vi'},
            {title: 'Generation VII', segment: 'generation-vii'},
            {title: 'Generation VIII', segment: 'generation-viii'},
            {title: 'Generation IX', segment: 'generation-ix'},
        ],
    },
    {
        title: 'Favorites',
        icon: <Favorite/>,
        segment: 'favorite',
    },
];


export default function App() {
    return (
        <ReactRouterAppProvider navigation={NAVIGATION}>
            <Outlet/>
        </ReactRouterAppProvider>
    );
}