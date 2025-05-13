import { UserResponseDto } from "./UserResponseDto";
import { VoteResponseDto } from "./VoteResponseDto";

export interface VoteParticipationResponseDto {
    id: number;
    user: UserResponseDto;
    vote: VoteResponseDto;
    votedAt: Date;
}