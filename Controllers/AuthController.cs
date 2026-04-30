using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Simulated login: validates credentials against the database (no JWT).</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.LoginAsync(request);
        if (user is null)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid email or password." });
        }

        return Ok(user);
    }
}
