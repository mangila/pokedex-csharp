import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import App from './App.tsx'
import {createBrowserRouter, Navigate, RouterProvider} from 'react-router';
import PokemonDashboardLayout from '@layouts/PokemonDashboardLayout.tsx';
import DashboardPage from '@pages/DashboardPage.tsx';
import FavoritePage from '@pages/FavoritePage.tsx';
import PokedexPage from '@pages/PokedexPage.tsx';
import PokemonPage from '@pages/PokemonPage.tsx';
import PokemonDetailPage from '@pages/PokemonDetailPage.tsx';
import NotFoundPage from '@pages/NotFoundPage.tsx';

const router = createBrowserRouter([
    {
        Component: App,
        children: [
            {
                path: '/',
                Component: PokemonDashboardLayout,
                children: [
                    {
                        path: 'dashboard',
                        Component: DashboardPage,
                    },
                    {
                        path: 'favorite',
                        Component: FavoritePage,
                    },
                    {
                        path: 'pokedex/:generation',
                        Component: PokedexPage,
                    },
                    {
                        path: 'pokemon',
                        Component: PokemonPage,
                    },
                    {
                        path: 'pokemon/:name',
                        Component: PokemonDetailPage,
                    },
                    {
                        path: '/',
                        element: <Navigate to="/dashboard" replace/>, // Redirect to /dashboard
                    },
                    {
                        path: '/pokedex',
                        element: <Navigate to="/dashboard" replace/>, // Redirect to /dashboard
                    },
                    {
                        path: '*',
                        Component: NotFoundPage,
                    },
                ],
            },
        ],
    }
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <RouterProvider router={router}/>
    </StrictMode>,
)
