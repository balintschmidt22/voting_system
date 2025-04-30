import {getActiveVotes, getClosedVotes, getVoteBySubString} from "@/api/client/votes-client.ts";
import { VoteResponseDto } from "@/api/models/VoteResponseDto";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { VotesGrid } from "@/components/votes/VotesGrid";
import { SetStateAction, useEffect, useState} from "react";
import { useLocation } from "react-router-dom";

//const NUMBER_OF_ACTIVE_VOTES = 5;

/**
 * Home page that shows the active votes
 * @constructor
 */
export function HomePage() {
    const location = useLocation();
    const [votes, setVotes] = useState<VoteResponseDto[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function loadContent() {
            setError(null);
            setIsLoading(true);
            try {
                let loadedVotes: SetStateAction<VoteResponseDto[]> = [];
                
                if (location.pathname === '/') {
                    [loadedVotes] = await Promise.all([
                        getActiveVotes(),
                    ]);
                } else if (location.pathname === '/votes/closed') {
                    [loadedVotes] = await Promise.all([
                        getClosedVotes(),
                    ]);
                } else {
                    loadedVotes = [];
                }
                setVotes(loadedVotes);
            } catch (e) {
                setError(e instanceof Error ? e.message : "Unknown error.");
            } finally {
                setIsLoading(false);
            }
        }
        
        loadContent();
    }, [location.pathname]);

    // Render
    if (isLoading) {
        return <LoadingIndicator />;
    }

    return (
    <>
        {error ? <ErrorAlert message={error} /> : null}
        
        {location.pathname === '/' ?
            <>
                <h1>Welcome to the Anonymous Voting!</h1>
                <h2>Active polls ({votes.length})</h2>
                <div className="input-group mb-4" style={{maxWidth: "400px"}}>
                  <span className="input-group-text bg-white border-end-0">
                    🔍
                  </span>
                    <input
                        type="text"
                        className="form-control border-start-0"
                        placeholder="Search votes..."
                        onChange={async (e) => {
                            const value = e.target.value;
                            if (value === "") {
                                // optionally reset to default state (e.g., reload active votes)
                                setVotes(await getActiveVotes());
                                return;
                            }
                            try {
                                setVotes(await getVoteBySubString(value, true));
                            } catch (e) {
                                // optional error handling
                            }
                        }}
                    />
                </div>
            </>
            : location.pathname === '/votes/closed' ? 
            <>
                <h1>Anonymous Voting</h1>
                <h2>Closed polls ({votes.length})</h2>
                <div className="input-group mb-4" style={{maxWidth: "400px"}}>
                  <span className="input-group-text bg-white border-end-0">
                    🔍
                  </span>
                    <input 
                        type="text"
                        className="form-control border-start-0"
                        placeholder="Search votes..."
                        onChange={async (e) => {
                            const value = e.target.value;
                            if (value === "") {
                                
                                setVotes(await getClosedVotes());
                                return;
                            }
                            try {
                                setVotes(await getVoteBySubString(value, false));
                            } catch (e) {
                                
                            }
                        }}
                    />
                </div>  
            </>
            : <></>
        }
        <VotesGrid votes={votes} />
    </>);
}