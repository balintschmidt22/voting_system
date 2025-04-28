import { MovieResponseDto } from "@/api/models/VoteResponseDto.ts";
import { RoomResponseDTO } from "@/api/models/RoomResponseDto";

export interface ScreeningResponseDto {
    id: number;
    movie: MovieResponseDto;
    room: RoomResponseDTO;
    startsAt: string;
    price: number;
}