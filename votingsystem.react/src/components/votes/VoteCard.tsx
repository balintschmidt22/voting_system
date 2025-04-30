import Card from "react-bootstrap/Card";
import Col from "react-bootstrap/Col";
import { VoteResponseDto } from "@/api/models/VoteResponseDto.ts"
import { Link } from "react-router-dom";
import { format } from "date-fns";
import {useUserContext} from "@/contexts/UserContext.ts";
import {useEffect, useState} from "react";
import {LoadingIndicator} from "@/components/LoadingIndicator.tsx";
import {ErrorAlert} from "@/components/alerts/ErrorAlert.tsx";
import {getUserAlreadyVoted} from "@/api/client/votes-client.ts";

interface Props {
    vote: VoteResponseDto;
}


export function VoteCard({ vote }: Props) {
    const userContext = useUserContext();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [voted, setVoted] = useState<boolean>(false);
    const [isActive, setIsActive] = useState<boolean>(false);

    useEffect(() => {
        async function loadContent() {
            if (!userContext.userId) {
                return;
            }

            setError(null);
            setIsLoading(true);
            try {
                setVoted(
                    await getUserAlreadyVoted(vote.id, userContext.userId)
                );
                setIsActive(
                    new Date(vote.start) <= new Date() && new Date(vote.end) >= new Date()
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
            <Col>
                <Card className="h-100 bg-light-subtle border-dark-subtle">
                    {/*<Base64CardImage data={movie.image} alt={movie.title} variant="top" />*/}
                    <Card.Body>
                        <h5 className="card-title">#{vote.id})</h5>
                        <h5 className="card-body">
                            {
                                voted
                                    ?
                                    isActive 
                                        ?
                                        <Link className="stretched-link text-success text-underline-hover"
                                              to={`/votes/active/${vote.id}`}>{vote.question}</Link>
                                        :
                                        <Link className="stretched-link text-success text-underline-hover"
                                              to={`/votes/closed/${vote.id}`}>{vote.question}</Link>
                                    :
                                    isActive
                                        ?
                                        <Link className="stretched-link text-danger text-underline-hover"
                                              to={`/votes/active/${vote.id}`}>{vote.question}</Link>
                                        :
                                        <Link className="stretched-link text-danger text-underline-hover"
                                              to={`/votes/closed/${vote.id}`}>{vote.question}</Link>
                            }
                        </h5>
                        <h6 className="card-footer bg-light-subtle">
                            <p>Start: {format(vote.start, "MMMM do, yyyy H:mma")}</p>
                            <p>End: {format(vote.end, "MMMM do, yyyy H:mma")}</p>
                        </h6>
                    </Card.Body>
                </Card>
            </Col>
        </>    
    )
}