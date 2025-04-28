import {VoteResponseDto} from "@/api/models/VoteResponseDto.ts";

export interface AnonymousVoteResponseDto {
    id: number;
    vote: VoteResponseDto;
    selectedOption: string;
    submittedAt: Date;
}