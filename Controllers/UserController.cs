using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/users")]
[ApiController]
[Authorize] // 🔒 Protegido con JWT
public class UsersController : ControllerBase
{
    private static readonly List<User> Users =
        new()
        {
            new User
            {
                Id = 1,
                Name = "Juan Pérez",
                Email = "juan@example.com"
            },
            new User
            {
                Id = 2,
                Name = "Ana López",
                Email = "ana@example.com"
            }
        };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(Users);
    }
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
