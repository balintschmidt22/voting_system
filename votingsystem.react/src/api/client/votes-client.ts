import { get } from "@/api/client/http";
import { VoteResponseDto } from "../models/VoteResponseDto";

export async function getVotes(count?: number): Promise<VoteResponseDto[]> {
    return await get<VoteResponseDto[]>("votes", count ? { count: count.toString() } : undefined);
}

export async function getVote(id: number): Promise<VoteResponseDto> {
    return await get<VoteResponseDto>(`votes/${id}`);
}

export async function getActiveVotes(): Promise<VoteResponseDto[]> {
    return await get<VoteResponseDto[]>(`votes/active`);
}

export async function getClosedVotes(): Promise<VoteResponseDto[]> {
    return await get<VoteResponseDto[]>(`votes/closed`);
}