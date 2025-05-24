import { get, postAsJson, postAsJsonWithoutResponse } from "@/api/client/http";
import { VoteResponseDto } from "../models/VoteResponseDto";
import { AnonymousVoteRequestDto } from "../models/AnonymousVoteRequestDto";

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

export async function getUserAlreadyVoted(id: number, user: string | undefined): Promise<boolean>{
    return await get<boolean>(`votes/voted/${id}/${user}`)
}

export async function addAnonymousVote(body: AnonymousVoteRequestDto): Promise<void> {
    await postAsJsonWithoutResponse<AnonymousVoteRequestDto>("anonymousvotes", body);
}

export async function getVoteBySubString(sub: string, isActive: boolean): Promise<VoteResponseDto[]> {
    return await postAsJson<{ sub: string; isActive: boolean }, VoteResponseDto[]>("votes/search", {
        sub,
        isActive,
    });
}

export async function getVotesByDateInterval(start: string, end: string, isActive: boolean): Promise<VoteResponseDto[]> {
    return await postAsJson<{ start: string; end: string; isActive: boolean}, VoteResponseDto[]>("/votes/search-by-date", {
        start,
        end,
        isActive,
    });
}


export async function getVoteResults(voteId: number): Promise<{ [option: string]: number }> {
    const data = await get<{ results: { [option: string]: number } }>(`votes/${voteId}/results`);
    return data.results;
}