import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import App from './App.tsx'
import {createBrowserRouter, RouterProvider} from 'react-router';
import PokemonDashboardLayout from './PokemonDashboardLayout.tsx';
import DashboardPage from '@pages/DashboardPage.tsx';
import PokedexPage from '@pages/PokedexPage.tsx';
import FavoritePage from '@pages/FavoritePage.tsx';
import PokemonPage from '@pages/PokemonPage.tsx';

const router = createBrowserRouter([
    {
        Component: App,
        children: [
            {
                path: '/',
                Component: PokemonDashboardLayout,
                children: [
                    {
                        path: '',
                        Component: DashboardPage,
                    },
                    {
                        path: 'favorite',
                        Component: FavoritePage,
                    },

                    {
                        path: 'pokedex',
                        Component: PokedexPage,
                    },
                    {
                        path: 'pokemon',
                        Component: PokemonPage,
                    },
                ],
            },
        ],
    },
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <RouterProvider router={router}/>
    </StrictMode>,
)
