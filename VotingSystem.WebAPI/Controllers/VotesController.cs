﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Authorize]
[Route("/votes")]
public class VotesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IVotesService _votesService;
    private readonly IAnonymousVoteService _anonymousVotesService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="votesService"></param>
    /// <param name="anonymousVotesService"></param>
    public VotesController(IMapper mapper,
        IVotesService votesService,
        IAnonymousVoteService anonymousVotesService)
    {
        _mapper = mapper;
        _votesService = votesService;
        _anonymousVotesService = anonymousVotesService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<VoteResponseDto>))]
    public async Task<IActionResult> GetVotes()
    {
        var votes = await _votesService.GetVotesAsync();
        var voteResponseDtos = _mapper.Map<List<VoteResponseDto>>(votes);

        return Ok(voteResponseDtos);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="voteRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(VoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateVote([FromBody] VoteRequestDto voteRequestDto)
    {
        var vote = _mapper.Map<Vote>(voteRequestDto);
        var userId = this.User.FindFirstValue("id");

        try
        {
            await _votesService.AddAsync(vote, userId!);

            var voteResponseDto = _mapper.Map<VoteResponseDto>(vote);
            return CreatedAtAction(nameof(CreateVote), new { id = voteResponseDto.Id }, voteResponseDto);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (SaveFailedException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("my")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<VoteResponseDto>))]
    public async Task<IActionResult> GetMyVotes()
    {
        var userId = this.User.FindFirstValue("id");
        var votes = await _votesService.GetMyVotesAsync(userId);
        var voteResponseDtos = _mapper.Map<List<VoteResponseDto>>(votes);

        return Ok(voteResponseDtos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("active")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<VoteResponseDto>))]
    public async Task<IActionResult> GetActiveVotes(int? count = null)
    {
        var votes = await _votesService.GetActiveVotesAsync(count);
        var voteResponseDtos = _mapper.Map<List<VoteResponseDto>>(votes);

        return Ok(voteResponseDtos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("closed")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<VoteResponseDto>))]
    public async Task<IActionResult> GetClosedVotes(int? count = null)
    {
        var votes = await _votesService.GetClosedVotesAsync(count);
        var voteResponseDtos = _mapper.Map<List<VoteResponseDto>>(votes);

        return Ok(voteResponseDtos);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(VoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVoteById([FromRoute] int id)
    {
        try
        {
            var vote = await _votesService.GetByIdAsync(id);
            var voteResponseDto = _mapper.Map<VoteResponseDto>(vote);
            return Ok(voteResponseDto);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message =  ex.Message});
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("voted/{id:int}/{user}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAlreadyVoted([FromRoute] int id, string user)
    {
        try
        {
            var vote = await _votesService.GetByIdAsync(id);
        
            var voted = vote.VoteParticipations.Any(x => x.UserId == user);

            return Ok(voted);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message =  ex.Message});
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(List<VoteResponseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetVoteBySubString(
        [FromBody] SearchRequestDto body)
    {
        if (string.IsNullOrWhiteSpace(body.Sub))
            return BadRequest("Search term cannot be empty.");
        
        var votes = await _votesService.GetBySubString(body.Sub, body.IsActive);

        return Ok(votes);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("search-by-date")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(List<VoteResponseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetVoteByDate(
        [FromBody] SearchDateRequestDto body)
    {
        var votes = await _votesService.GetByDate(body.Start, body.End, body.IsActive);

        return Ok(votes);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}/results")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVoteResults(int id)
    {
        try
        {
            var vote = await _votesService.GetByIdAsync(id);
            
            if (vote.End > DateTime.UtcNow)
            {
                return BadRequest("Voting is still open. Results are not available yet.");
            }

            var results = await _anonymousVotesService.GetVoteResultsAsync(id);
            return Ok(new { results });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message =  ex.Message});
        }
    }

}