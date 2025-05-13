using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// <param name="anonymousVoteRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(AnonymousVoteResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddNewAnonymousVote(
        [FromBody] AnonymousVoteRequestDto anonymousVoteRequestDto)
    {
        var userId = this.User.FindFirstValue("id");
        if (userId == null)
        {
            return Unauthorized();
        }

        if (!string.Equals(userId, anonymousVoteRequestDto.UserId))
        {
            return Forbid();
        }
        
        try
        {
            await _anonymousVoteService.AddAnonymousVoteAsync(anonymousVoteRequestDto.UserId, anonymousVoteRequestDto.VoteId, anonymousVoteRequestDto.SelectedOption);
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to add vote. " + ex.Message });
        }
    }
}