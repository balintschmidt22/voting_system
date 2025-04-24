using AutoMapper;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Services;
using VotingSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace VotingSystem.WebAPI.Controllers;

/// <summary>
/// usersController
/// </summary>
[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="usersService"></param>
    public UsersController(IMapper mapper, 
        IUsersService usersService)
    {
        _mapper = mapper;
        _usersService = usersService;
    }
    
    /// <summary>
    /// Get all users in descending order by creation date
    /// </summary>
    /// <param name="count">An optional parameter to restrict the number of the returned users</param>
    /// <response code="200">A list of users</response>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<UserResponseDto>))]
    public async Task<IActionResult> GetUsers(int? count = null)
    {
        var users = await _usersService.GetLatestUsersAsync(count);
        var userResponseDtos = _mapper.Map<List<UserResponseDto>>(users);

        return Ok(userResponseDtos);
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">The requested user</response>
    /// <response code="404">Not found</response>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _usersService.GetByIdAsync(id);
        var userResponseDto = _mapper.Map<UserResponseDto>(user);

        return Ok(userResponseDto);
    }
    
}
