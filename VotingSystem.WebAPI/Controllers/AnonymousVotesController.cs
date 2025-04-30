using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Models;
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
    private readonly IVotesService _votesService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="anonymousVoteService"></param>
    /// <param name="votesService"></param>
    public AnonymousVotesController(IMapper mapper, 
        IAnonymousVoteService anonymousVoteService,
        IVotesService votesService)
    {
        _mapper = mapper;
        _anonymousVoteService = anonymousVoteService;
        _votesService = votesService;
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="anonymousVoteRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(VoteParticipationResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddNewAnonymousVote(
        [FromBody] AnonymousVoteRequestDto anonymousVoteRequestDto)
    {
        //var av = _mapper.Map<AnonymousVote>(anonymousVoteRequestDto);
        //var vote = await _votesService.GetByIdAsync(anonymousVoteRequestDto.VoteId);
        await _anonymousVoteService.AddAnonymousVoteAsync(anonymousVoteRequestDto.VoteId, anonymousVoteRequestDto.SelectedOption);
        return Created();
    }
}