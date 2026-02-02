using Assessment.Application.Abstractions;
using Assessment.Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var res = await _tokenService.LoginAsync(dto.Email, dto.Password, ct);
            return Ok(res);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
