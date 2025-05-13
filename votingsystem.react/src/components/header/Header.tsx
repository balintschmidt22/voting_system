import { HeaderLink } from "@/components/header/HeaderLink";
import { useUserContext } from "@/contexts/UserContext";
import {useEffect, useState } from "react";
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import {getUserById} from "@/api/client/users-client.ts";
import { LoadingIndicator } from "../LoadingIndicator";
import { ErrorAlert } from "../alerts/ErrorAlert";
import {UserResponseDto} from "@/api/models/UserResponseDto.ts";

export function Header() {
    const { loggedIn } = useUserContext();
    const userContext = useUserContext();
    
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponseDto>();

    useEffect(() => {
        async function loadContent() {
            if (!userContext.userId) {
                return;
            }

            setError(null);
            setIsLoading(true);
            try {
                setUser(
                    await getUserById(userContext.userId)
                );
            } catch (e) {
                setError(e instanceof Error ? e.message : "Unknown error.");
            } finally {
                setIsLoading(false);
            }
        }
        loadContent();
    }, [userContext]);

    if (isLoading) {
        return <LoadingIndicator />
    }
   
    return (
        <>
        {error ? <ErrorAlert message={error} /> : null}
        <Navbar variant="light" bg="light" expand="md">
            <Container>
                <Navbar.Brand>Anonymous Voting</Navbar.Brand>
                <Navbar.Toggle aria-controls="header-navbar-nav" />
                <Navbar.Collapse id="header-navbar-nav">
                    <Nav className="me-auto">
                        <HeaderLink to="/" text="Active Polls"/>
                        <HeaderLink to="/votes/closed" text="Closed Polls"></HeaderLink>
                    </Nav>
                    <Nav className="ms-auto">
                        {loggedIn
                            ? (<><Navbar.Text>Logged in as {user?.userName}</Navbar.Text> 
                                <b><HeaderLink to="/user/logout" text="Logout"/></b> 
                            </>)
                            : (<>
                                <HeaderLink to="/user/login" text="Login"/>
                                <HeaderLink to="/user/register" text="Register"/>
                            </>)  
                        }
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
        </>
    );
}