import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.css';
import '@/index.css';

import { RootLayout } from "@/pages/RootLayout";
import { HomePage } from "@/pages/HomePage";
import { UsersPage } from "@/pages/movies/UsersPage.tsx";
import { UserPage } from "@/pages/movies/UserPage.tsx";
import { CreateReservationPage } from "@/pages/screenings/CreateReservationPage";
import { NotFoundPage } from "@/pages/NotFoundPage";


const router = createBrowserRouter([
    {
        element: <RootLayout />,
        children: [
            {
                path: "/",
                element: <HomePage />
            },
            {
                path: "/users",
                element: <UsersPage />,
            },
            {
                path: "/users/:userId",
                element: <UserPage />,
            },
/*            {
                path: "/screenings/:screeningId/create-reservation",
                element: <CreateReservationPage />,
            },*/
            {
                path: "*",
                element: <NotFoundPage />
            },
        ]
    },
]);

createRoot(document.getElementById('root')!).render(
    <RouterProvider router={router} />
);
