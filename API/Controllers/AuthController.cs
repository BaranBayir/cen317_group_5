using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AuthController(IAuthService authService, IAntiforgery antiforgery) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("csrf")]
    public IActionResult GetCsrfToken()
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { requestToken = tokens.RequestToken });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request);
        return result.Succeeded
            ? StatusCode(StatusCodes.Status201Created, result.User)
            : AuthError(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return result.Succeeded ? Ok(result.User) : AuthError(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) || userId <= 0)
        {
            return Unauthorized();
        }

        var user = await authService.GetUserAsync(userId);
        if (user is null)
        {
            await authService.LogoutAsync();
            return Unauthorized();
        }

        return Ok(user);
    }

    private ObjectResult AuthError(AuthResult result)
    {
        var statusCode = result.Failure switch
        {
            AuthFailure.Conflict => StatusCodes.Status409Conflict,
            AuthFailure.InvalidCredentials => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        return Problem(statusCode: statusCode, title: "Authentication request failed.",
            detail: string.Join(" ", result.Errors));
    }
}
