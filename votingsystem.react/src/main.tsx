import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.css';
import '@/index.css';
import { RootLayout } from "@/pages/RootLayout";
import { HomePage } from "@/pages/HomePage";
/*import { UsersPage } from "@/pages/users/UsersPage.tsx";
import { UserPage } from "@/pages/users/UserPage.tsx";*/
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
                path: "/votes/closed",
                element: <HomePage />
            },
/*            {
                path: "/users",
                element: <UsersPage />,
            },
            {
                path: "/users/:userId",
                element: <UserPage />,
            },*/
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
