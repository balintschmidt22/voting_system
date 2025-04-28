import Row from "react-bootstrap/Row"
import { VoteResponseDto } from "@/api/models/VoteResponseDto.ts";
import { VoteCard } from "@/components/votes/VoteCard";

interface Props {
    votes: VoteResponseDto[];
}

export function VotesGrid({ votes } : Props) {
    return (
        <Row xs={1} md={3} className="g-4">
            {votes.map(vote => <VoteCard key={vote.id} vote={vote} />)}
        </Row>
    )
}