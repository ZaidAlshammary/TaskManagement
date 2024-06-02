using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Dtos.Auth;
using TaskManagement.Exceptions;
using TaskManagement.Services;

namespace TaskManagement.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        try
        {
            var user = await _authService.CreateUser(createUserDto);
            return Ok(new AppResponse
            {
                Code = 201,
                Message = "User created successfully",
                Data = user
            });
        }
        catch (UserNotCreatedException e)
        {
            return BadRequest(new AppResponse
            {
                Code = 400,
                Message = e.Message
            });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var token = await _authService.Login(loginDto);
            return Ok(token);
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized(new AppResponse
            {
                Code = 401,
                Message = "Invalid credentials"
            });
        }
    }
    
    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _authService.GetUsers();
            return Ok(new AppResponse
            {
                Code = 200,
                Message = "Users fetched successfully",
                Data = users
            });
        }
        catch (NotAdminException)
        {
            return Unauthorized(new AppResponse
            {
                Code = 401,
                Message = "You are not an admin"
            });
        }
    }
}