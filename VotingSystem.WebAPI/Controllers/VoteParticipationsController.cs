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
}