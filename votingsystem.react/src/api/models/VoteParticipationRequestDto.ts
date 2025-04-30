import {UserResponseDto} from "@/api/models/UserResponseDto.ts";
import { VoteResponseDto } from "./VoteResponseDto";

export interface VoteParticipationRequestDto{
    user: UserResponseDto;
    vote: VoteResponseDto;
}