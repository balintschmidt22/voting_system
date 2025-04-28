import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.css';
import '@/index.css';
import { RootLayout } from "@/pages/RootLayout";
import { HomePage } from "@/pages/HomePage";
/*import { UsersPage } from "@/pages/users/UsersPage.tsx";
import { UserPage } from "@/pages/users/UserPage.tsx";*/
import { NotFoundPage } from "@/pages/NotFoundPage";
import { Protected } from './components/Protected';
import { LoginPage } from './pages/user/LoginPage';
import {LogoutPage} from "@/pages/user/LogoutPage.tsx";
import { RegisterPage } from './pages/user/RegisterPage';
import { UserContextProvider } from './contexts/UserContextProvider';


const router = createBrowserRouter([
    {
        element: <RootLayout />,
        children: [
            {
                path: "/",
                element: <Protected><HomePage /></Protected>
            },
            {
                path: "/votes/active",
                element: <Protected><HomePage /></Protected>
            },
            {
                path: "/votes/closed",
                element: <Protected><HomePage /></Protected>
            },
            {
                path: "/user/login",
                element: <LoginPage />,
            },
            {
                path: "/user/logout",
                element: <Protected><LogoutPage /></Protected>,
            },
            {
                path: "/user/register",
                element: <RegisterPage />,
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
    <UserContextProvider>
        <RouterProvider router={router} />
    </UserContextProvider>
);