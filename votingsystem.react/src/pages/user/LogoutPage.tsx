import { LoadingIndicator } from "@/components/LoadingIndicator";
import { useUserContext } from "@/contexts/UserContext";
import { useEffect } from "react";

export function LogoutPage() {
    const { handleLogout } = useUserContext();

    useEffect(() => {
        handleLogout().then(() => {
            // Force full reload of the app at login page
            window.location.href = "/";
        });
    }, [handleLogout]);

    return <LoadingIndicator />;
}