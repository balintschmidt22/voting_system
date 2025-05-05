import { useParams } from "react-router-dom"; // for routing and accessing params
import { useEffect, useState } from "react";
import {addAnonymousVote, getUserAlreadyVoted, getVote} from "@/api/client/votes-client";
import { addVoteParticipation } from "@/api/client/votes-client";
import { VoteResponseDto } from "@/api/models/VoteResponseDto";
import { VoteParticipationRequestDto } from "@/api/models/VoteParticipationRequestDto";
import { LoadingIndicator } from "@/components/LoadingIndicator";
import { ErrorAlert } from "@/components/alerts/ErrorAlert";
import { useUserContext } from '@/contexts/UserContext';
import {AnonymousVoteRequestDto} from "@/api/models/AnonymousVoteRequestDto.ts"; // Import the hook to get the user from context
import { getUserById } from "@/api/client/users-client";
import {UserResponseDto} from "@/api/models/UserResponseDto.ts";
import { useNavigate } from "react-router-dom";

export function VotePageActive() {
    const { id } = useParams<{ id: string }>(); // Get the vote ID from the URL params
    const [vote, setVote] = useState<VoteResponseDto | null>(null);
    const [selectedOption, setSelectedOption] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponseDto>();
    const [voted, setVoted] = useState<boolean>(false);
    const userContext = useUserContext();
    const navigate = useNavigate();
    

    // Ensure id is a valid string before attempting to parse it
    const voteId = id ? parseInt(id) : NaN;

    if (isNaN(voteId)) {
        // If the ID is invalid (NaN), return an error or handle the invalid case
        return <ErrorAlert message="Invalid vote ID" />;
    }

    // Fetch vote details by ID
    useEffect(() => {
        async function loadVoteDetails() {
            setIsLoading(true);
            setError(null);
            try {
                const voteDetails = await getVote(voteId);

                const now = new Date();
                const voteEnd = new Date(voteDetails.end);

                if (now > voteEnd) {
                    setError("This vote has already ended.");
                    return;
                }

                setVote(voteDetails);
            } catch (e) {
                setError(e instanceof Error ? e.message : "Unknown error");
            } finally {
                setIsLoading(false);
            }
        }

        loadVoteDetails();

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
                setVoted(
                    await getUserAlreadyVoted(voteId, userContext.userId)
                );
            } catch (e) {
                setError(e instanceof Error ? e.message : "Unknown error.");
            } finally {
                setIsLoading(false);
            }
        }
        loadContent();
    }, [voteId, userContext]);

    // Handle vote submission
    const handleVoteSubmit = async () => {
        if (!selectedOption) {
            setError("Please select an option.");
            return;
        }

        if (!user) {
            setError("You must be logged in to vote.");
            return;
        }

        setIsLoading(true);
        try {
            const voteParticipation: VoteParticipationRequestDto = {
                userId: user.id, // The current logged-in user
                voteId: voteId, // The selected vote (ensured non-null by the `!` operator)
            };
            
            await addVoteParticipation(voteParticipation);

            const anonymousVote: AnonymousVoteRequestDto = {
                voteId: voteId,
                selectedOption,
            };

            await addAnonymousVote(anonymousVote);
            
            navigate("/thank-you");
        } catch (e) {
            setError(e instanceof Error ? e.message : "Unknown error");
        } finally {
            setIsLoading(false);
        }
    };

    if (isLoading) return <LoadingIndicator />;
    if (error) return <ErrorAlert message={error} />;

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

                    <form>
                        <h3>Vote Options:</h3>
                        <div>
                            {vote.options.map((option, index) => (
                                <div key={index} className="form-check">
                                    <input
                                        type="radio"
                                        id={`option${index}`}
                                        name="voteOption"
                                        value={option}
                                        onChange={() => setSelectedOption(option)}
                                        className="form-check-input"
                                    />
                                    <label htmlFor={`option${index}`} className="form-check-label">
                                        {option}
                                    </label>
                                </div>
                            ))}
                        </div>

                        {voted ? 
                        <button
                            type="button"
                            className="btn btn-primary mt-3"
                            onClick={(e) => e.preventDefault()}>
                            Already voted!
                        </button> :
                        <button
                            type="button"
                            className="btn btn-primary mt-3"
                            onClick={handleVoteSubmit}
                        >
                            Submit Vote
                        </button>}
                    </form>
                </>
            )}
        </div>
    );
}
