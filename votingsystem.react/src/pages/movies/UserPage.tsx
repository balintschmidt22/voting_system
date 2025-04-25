import { getuser } from "@/api/client/users-client";
import { getFutureScreenings } from "@/api/client/screenings-client";
import { userResponseDto } from "@/api/models/userResponseDto";
import { ScreeningResponseDto } from "@/api/models/ScreeningResponseDto";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";
import { Base64Image } from "@/components/Base64Image";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { ScreeningCard } from "@/components/screenings/ScreeningCard";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

/**
 * Shows the details of a user and its future screenings
 * @constructor
 */
export function UserPage() {
    const params = useParams();
    const userId = params.userId ? parseInt(params.userId) : null;
    const [user, setuser] = useState<userResponseDto | null>(null);
    const [screenings, setScreenings] = useState<ScreeningResponseDto[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    
    useEffect(() => {
        async function loadContent() {
            if (!userId) {
                return;
            }
            
            setError(null);
            setIsLoading(true);
            try {
                const [loadedUser, loadedScreenings] = await Promise.all([
                    getuser(userId),
                    getFutureScreenings(userId)
                ]);
                setuser(loadedUser);
                setScreenings(loadedScreenings);
            } catch (e) {
                setError(e instanceof Error ? e.message : "Unknown error.");
            } finally {
                setIsLoading(false);
            }
        }

        loadContent();
    }, [userId]);

    // Render
    if (isLoading) {
        return <LoadingIndicator />;
    }

    return (
        <>
            {error ? <ErrorAlert message={error} /> : null}
            {user ? (
                <div className="row">
                    <div className="col-md-3">
                        <Base64Image className="img-fluid" data={user.image} alt={user.title}/>
                    </div>
                    <div className="col-md-9">
                        <h1>{user?.title}</h1>
                        <ul className="list-unstyled">
                            <li><strong>Released:</strong> {user.year}</li>
                            <li><strong>Directed by:</strong> {user.director}</li>
                            <li><strong>Length:</strong> {user.length} minutes</li>
                        </ul>
                        <p>{user?.synopsis}</p>
                    </div>
                    <hr className="mt-4"/>
                    <div>
                        <h2 className="mt-0">Screenings</h2>
                        {screenings.map(screening =>
                            <ScreeningCard
                                key={screening.id}
                                screening={screening}
                                showReservationLink
                            />
                        )}
                    </div>
                </div>
            ) : null}
        </>
    );
}