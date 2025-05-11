using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.DataAccess.Models;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// Controller for vote participations
/// </summary>
[ApiController]
[Authorize]
[Route("voteparticipations")]
public class VoteParticipationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IVoteParticipationService _voteParticipationService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="voteParticipationService"></param>
    public VoteParticipationsController(IMapper mapper, 
        IVoteParticipationService voteParticipationService)
    {
        _mapper = mapper;
        _voteParticipationService = voteParticipationService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(VoteParticipationResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVoteParticipationById([FromRoute] int id)
    {
        var voteParticipation = await _voteParticipationService.GetByIdAsync(id);
        var voteParticipationResponseDto = _mapper.Map<VoteParticipationResponseDto>(voteParticipation);

        return Ok(voteParticipationResponseDto);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="voteParticipationRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(VoteParticipationResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddNewVoteParticipation(
        [FromBody] VoteParticipationRequestDto voteParticipationRequestDto)
    {
        await _voteParticipationService.AddVoteParticipationAsync(voteParticipationRequestDto.UserId, voteParticipationRequestDto.VoteId);

        return Created();
    }
}