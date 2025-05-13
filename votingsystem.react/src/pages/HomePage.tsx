import {getActiveVotes, getClosedVotes, getVoteBySubString, getVotesByDateInterval} from "@/api/client/votes-client.ts";
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
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [searchInput, setSearchInput] = useState("");

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
                <div className="input-group mb-4" style={{maxWidth: "450px"}}>
                  <span className="input-group-text bg-white border-end-0">
                    🔍
                  </span>
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search votes..."
                        onChange={(e) => setSearchInput(e.target.value)}
                    />
                    <button
                        className="btn btn-outline-primary"
                        onClick={async () => {
                            if (searchInput === "") {

                                setVotes(await getActiveVotes());
                                return;
                            }
                            try {
                                setVotes(await getVoteBySubString(searchInput, true));
                            } catch (e) {

                            }
                        }}
                    >
                        Search
                    </button>
                </div>
                <div className="input-group mb-4" style={{ maxWidth: "450px" }}>
                    <span className="input-group-text bg-white">📅</span>
                    <input
                        type="date"
                        className="form-control"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                    <input
                        type="date"
                        className="form-control"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                    <button
                        className="btn btn-outline-primary"
                        onClick={async () => {
                            try {
                                if (!startDate || !endDate) return;
                                const votes = await getVotesByDateInterval(startDate, endDate, true);
                                setVotes(votes);
                            } catch (e) {
                                //
                            }
                        }}
                    >
                        Search
                    </button>
                </div>

            </>
            : location.pathname === '/votes/closed' ? 
            <>
                <h1>Anonymous Voting</h1>
                <h2>Closed polls ({votes.length})</h2>
                <div className="input-group mb-4" style={{maxWidth: "450px"}}>
                  <span className="input-group-text bg-white border-end-0">
                    🔍
                  </span>
                    <input 
                        type="text"
                        className="form-control"
                        placeholder="Search votes..."
                        onChange={(e) => setSearchInput(e.target.value)}
                    />
                    <button
                        className="btn btn-outline-primary"
                        onClick={async () => {
                            if (searchInput === "") {

                                setVotes(await getClosedVotes());
                                return;
                            }
                            try {
                                setVotes(await getVoteBySubString(searchInput, false));
                            } catch (e) {

                            }
                        }}
                    >
                        Search
                    </button>
                </div>
                <div className="input-group mb-4" style={{ maxWidth: "450px" }}>
                    <span className="input-group-text bg-white">📅</span>
                    <input
                        type="date"
                        className="form-control"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                    <input
                        type="date"
                        className="form-control"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                    <button
                        className="btn btn-outline-primary"
                        onClick={async () => {
                            try {
                                if (!startDate || !endDate) return;
                                const votes = await getVotesByDateInterval(startDate, endDate, false);
                                setVotes(votes);
                            } catch (e) {
                                //
                            }
                        }}
                    >
                        Search
                    </button>
                </div>
            </>
            : <></>
        }
        <VotesGrid votes={votes} />
    </>);
}