using Api.Data;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
    {
        var users = await _db.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.Email, u.Role, u.CreatedAt })
            .ToListAsync(ct);

        return Ok(users);
    }
}