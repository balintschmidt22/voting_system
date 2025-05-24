using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// Controller for handling anonymous votes
/// </summary>
[ApiController]
[Authorize]
[Route("anonymousvotes")]
public class AnonymousVotesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAnonymousVoteService _anonymousVoteService;
    private readonly IVoteParticipationService _voteParticipationService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="anonymousVoteService"></param>
    /// <param name="voteParticipationService"></param>
    public AnonymousVotesController(IMapper mapper, 
        IAnonymousVoteService anonymousVoteService,
        IVoteParticipationService voteParticipationService)
    {
        _mapper = mapper;
        _anonymousVoteService = anonymousVoteService;
        _voteParticipationService = voteParticipationService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AnonymousVoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnonymousVoteById([FromRoute] int id)
    {
        try
        {
            var anonymousVote = await _anonymousVoteService.GetByIdAsync(id);
            var anonymousVoteResponseDto = _mapper.Map<AnonymousVoteResponseDto>(anonymousVote);
            return Ok(anonymousVoteResponseDto);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message =  ex.Message});
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="anonymousVoteRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(AnonymousVoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddNewAnonymousVote(
        [FromBody] AnonymousVoteRequestDto anonymousVoteRequestDto)
    {
        try
        {
            var userId = this.User.FindFirstValue("id");

            await _anonymousVoteService.AddAnonymousVoteAsync(userId!, anonymousVoteRequestDto.VoteId, anonymousVoteRequestDto.SelectedOption);
            return Created();
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new { message =  ex.Message});
        }
        catch (SaveFailedException ex)
        {
            return StatusCode(500, new { message =  ex.Message});
        }
    }
}