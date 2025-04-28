import Card from "react-bootstrap/Card";
import Col from "react-bootstrap/Col";
import { VoteResponseDto } from "@/api/models/VoteResponseDto.ts"
import { Link } from "react-router-dom";
import { format } from "date-fns";

interface Props {
    vote: VoteResponseDto;
}

export function VoteCard({ vote }: Props) {
    return (
        <Col>
            <Card className="h-100">
                {/*<Base64CardImage data={movie.image} alt={movie.title} variant="top" />*/}
                <Card.Body>
                    <h5 className="card-title">#{vote.id})</h5>
                    <h5 className="card-body">
                        <Link className="stretched-link text-dark text-underline-hover" to={`/votes/${vote.id}`}>{vote.question}</Link>
                    </h5>
                    <h6 className="card-footer">
                        <p>Start: {format(vote.start, "MMMM do, yyyy H:mma")}</p>
                        <p>End: {format(vote.end, "MMMM do, yyyy H:mma")}</p>
                    </h6>
                </Card.Body>
            </Card>
        </Col>
    )
}