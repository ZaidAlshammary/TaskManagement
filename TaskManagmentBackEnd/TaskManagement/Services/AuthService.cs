using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Configuration;
using TaskManagement.Dtos.Auth;
using TaskManagement.Exceptions;
using TaskManagement.Models;
using TaskManagement.Resolvers;

namespace TaskManagement.Services;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;
    private readonly IUserContextResolver _userContextResolver;

    public AuthService(IOptions<JwtConfig> jwtConfigOptions, UserManager<User> userManager, IUserContextResolver userContextResolver)
    {
        _jwtConfig = jwtConfigOptions.Value;
        _userManager = userManager;
        _userContextResolver = userContextResolver;
    }
    
    public async Task<UserDto?> CreateUser(CreateUserDto createUserDto)
    {
        var user = new User
        {
            FullName = createUserDto.Name,
            UserName = createUserDto.Name
        };
        
        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!result.Succeeded)
        {
            throw new UserNotCreatedException(string.Join("\n", result.Errors.Select(e => e.Description).ToList()));
        }
        
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName
        };
    }
    
    public async Task<LoginResponse> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Name);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result)
        {
            throw new InvalidCredentialsException();
        }
        
        return new LoginResponse
        {
            Token = GenerateJwtToken(user)
        };
    }
    
    public async Task<List<UserDto>> GetUsers()
    {
        if (_userContextResolver.IsAdmin == false)
        {
            throw new NotAdminException();
        }
        
        return await _userManager.Users.Select(u => new UserDto
        {
            Id = u.Id,
            FullName = u.FullName
        }).ToListAsync();
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim("IsAdmin", user.IsAdmin.ToString()), 
                new Claim(JwtRegisteredClaimNames.Sub, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}