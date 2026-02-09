using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("signup")]
    public ActionResult<AuthResponseDto> Signup(SignupDto dto)
    {
        try
        {
            return Ok(_service.Signup(dto));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login(LoginDto dto)
    {
        try
        {
            return Ok(_service.Login(dto));
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
