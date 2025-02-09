import {CatchingPokemon, Dashboard, DeveloperBoard, Favorite} from '@mui/icons-material';
import {findByGeneration} from '@shared/api';
import {theme} from '@shared/theme';
import {PokemonGeneration} from '@shared/types';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {ReactQueryDevtools} from '@tanstack/react-query-devtools'
import {ReactRouterAppProvider} from '@toolpad/core/react-router';
import {Outlet} from 'react-router';

const NAVIGATION = [
    {
        title: 'Dashboard',
        icon: <Dashboard/>,
        segment: 'dashboard',
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

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 1000 * 60 * 10, // 10 minutes
            refetchOnWindowFocus: false,
            retry: 3,
        }
    },
});


export default function App() {
    Object.values(PokemonGeneration).forEach((generation: PokemonGeneration) => {
        queryClient.prefetchQuery({
            queryKey: ["generation", generation],
            queryFn: () => findByGeneration(generation)
        })
    })

    return (
        <QueryClientProvider client={queryClient}>
            <ReactRouterAppProvider navigation={NAVIGATION}
                                    theme={theme}>
                <Outlet/>
            </ReactRouterAppProvider>
            <ReactQueryDevtools initialIsOpen={false}/>
        </QueryClientProvider>
    );
}