using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/users")]
[ApiController]
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
    [Authorize(Roles = "Admin")] // 🔒 Solo los admins pueden acceder
    public IActionResult GetUsers()
    {
        return Ok(Users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")] // 🔒 Admin y User pueden acceder
    public IActionResult GetUserById(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return user == null ? NotFound() : Ok(user);
    }
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
