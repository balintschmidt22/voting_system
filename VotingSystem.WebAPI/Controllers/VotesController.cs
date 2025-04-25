using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("/votes")]
public class VotesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IVotesService _votesService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="votesService"></param>
    public VotesController(IMapper mapper, 
        IVotesService votesService)
    {
        _mapper = mapper;
        _votesService = votesService;
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
        var vote = await _votesService.GetByIdAsync(id);
        var voteResponseDto = _mapper.Map<VoteResponseDto>(vote);

        return Ok(voteResponseDto);
    }
}