using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FCms.Auth.Implementations;
using FCmsManagerAngular.ViewModels;

namespace FCmsManagerAngular.Controllers;

[Area("cms")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("cms/api/v1/auth/login")]
    public IActionResult Login([FromBody] UserViewModel login)
    {
        if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
        {
            return BadRequest(new ApiResultModel(ApiResultModel.FAIL) { Description = "Username and password are required." });
        }

        var users = CmsUsers.GetInstance();
        var user = users.GetAllUsers().FirstOrDefault(u =>
            string.Equals(u.Username, login.Username, StringComparison.OrdinalIgnoreCase));

        if (user == null || !user.VerifyPassword(login.Password))
        {
            return Unauthorized(new ApiResultModel(ApiResultModel.FAIL) { Description = "Invalid credentials." });
        }

        var token = GenerateJwtToken(user.Username);
        return Ok(new ApiResultModel(ApiResultModel.SUCCESS)
        {
            Data = new { Token = token }
        });
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "120");

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
