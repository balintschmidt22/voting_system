using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// Controller for handling anonymous votes
/// </summary>
[ApiController]
[Route("anonymousvotes")]
public class AnonymousVotesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAnonymousVoteService _anonymousVoteService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="anonymousVoteService"></param>
    public AnonymousVotesController(IMapper mapper, 
        IAnonymousVoteService anonymousVoteService)
    {
        _mapper = mapper;
        _anonymousVoteService = anonymousVoteService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AnonymousVoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnonymousVoteById([FromRoute] int id)
    {
        var anonymousVote = await _anonymousVoteService.GetByIdAsync(id);
        var anonymousVoteResponseDto = _mapper.Map<AnonymousVoteResponseDto>(anonymousVote);

        return Ok(anonymousVoteResponseDto);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("option/{option}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AnonymousVoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnonymousVoteByOption([FromRoute] string option)
    {
        var anonymousVote = await _anonymousVoteService.GetAnonymousVotesByOptionAsync(option);
        var anonymousVoteResponseDto = _mapper.Map<AnonymousVoteResponseDto>(anonymousVote);

        return Ok(anonymousVoteResponseDto);
    }
}