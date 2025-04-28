import {UserResponseDto} from "./UserResponseDto.ts";
import { AnonymousVoteResponseDto } from "./AnonymousVoteResponseDto";
import { VoteParticipationResponseDto } from "./VoteParticipationResponseDto";

export interface VoteResponseDto {
    id: number;
    user: UserResponseDto;
    question: string;
    options: string[];
    start: Date;
    end: Date;
    anonymousVotes: AnonymousVoteResponseDto[];
    voteParticipation: VoteParticipationResponseDto[];
    createdAt: Date;
    updatedAt: Date;
}
