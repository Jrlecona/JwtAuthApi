using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var userRole = ValidateUser(request.Username, request.Password);
        if (userRole == null)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(request.Username, userRole);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string? username, string role)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role) // 👉 Agregamos el rol al token
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string? ValidateUser(string? username, string? password)
    {
        // Aquí puedes validar contra una base de datos. Por ahora, usamos valores fijos
        if (username == "admin" && password == "password123")
            return "Admin";
        if (username == "user" && password == "password123")
            return "User";
        return null;
    }
}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
