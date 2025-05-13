import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getVote, getVoteResults } from "@/api/client/votes-client";
import { VoteResponseDto } from "@/api/models/VoteResponseDto";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";

export function VotePageClosed() {
    const { id } = useParams<{ id: string }>();
    const [vote, setVote] = useState<VoteResponseDto | null>(null);
    const [results, setResults] = useState<{ [option: string]: number }>({});
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const voteId = id ? parseInt(id) : NaN;

    useEffect(() => {
        async function loadVoteData() {
            if (isNaN(voteId)) {
                setError("Invalid vote ID");
                setIsLoading(false);
                return;
            }

            try {
                const voteDetails = await getVote(voteId);
                const voteResults = await getVoteResults(voteId);

                setVote(voteDetails);
                setResults(voteResults);
            } catch (e) {
                setError(e instanceof Error ? e.message : "Failed to load data");
            } finally {
                setIsLoading(false);
            }
        }

        loadVoteData();
    }, [voteId]);

    if (isLoading) return <LoadingIndicator />;
    if (error) return <ErrorAlert message={error} />;

    const totalVotes = Object.values(results).reduce((sum, count) => sum + count, 0);

    return (
        <div className="container">
            {vote && (
                <>
                    <h1>{vote.question}</h1>
                    <p>
                        <strong>Start Date:</strong> {new Date(vote.start).toLocaleString()}
                    </p>
                    <p>
                        <strong>End Date:</strong> {new Date(vote.end).toLocaleString()}
                    </p>

                    <h3>Results:</h3>
                    <ul className="list-group">
                        {vote.options.map((option, index) => {
                            const count = results[option] ?? 0;
                            const percentage = totalVotes > 0 ? (count / totalVotes) * 100 : 0;

                            return (
                                <li key={index} className="list-group-item">
                                    <div className="d-flex justify-content-between">
                                        <strong>{option}</strong>
                                        <span>{count} vote(s) • {percentage.toFixed(1)}%</span>
                                    </div>
                                    <div className="progress mt-2">
                                        <div
                                            className="progress-bar"
                                            role="progressbar"
                                            style={{ width: `${percentage}%` }}
                                            aria-valuenow={percentage}
                                            aria-valuemin={0}
                                            aria-valuemax={100}
                                        />
                                    </div>
                                </li>
                            );
                        })}
                    </ul>
                </>
            )}
        </div>
    );
}
