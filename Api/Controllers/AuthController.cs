using Api.Auth;
using Api.Data;
using Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Asp.Versioning;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtSettings _jwt;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public AuthController(AppDbContext db, JwtSettings jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string AccessToken, string RefreshToken);
    public record RefreshRequest(string RefreshToken);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var exists = await _db.Users.AnyAsync(u => u.Email == email, ct);
        if (exists) return Conflict(new { message = "Email already registered" });

        var user = new AppUser { Email = email };
        user.PasswordHash = _hasher.HashPassword(user, req.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return Created("", new { user.Id, user.Email });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email, ct);
        if (user is null) return Unauthorized();

        if (user.LockoutUntil.HasValue && user.LockoutUntil > DateTime.UtcNow)
            return Unauthorized(new { message = $"Account bloccato fino alle {user.LockoutUntil:HH:mm} UTC" });

        var ok = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
        if (ok == PasswordVerificationResult.Failed)
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutUntil = DateTime.UtcNow.AddMinutes(15);
                user.FailedLoginAttempts = 0;
            }
            await _db.SaveChangesAsync(ct);
            return Unauthorized();
        }

        user.FailedLoginAttempts = 0;
        user.LockoutUntil = null;

        var accessToken = CreateToken(user);
        var rawRefresh = GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            TokenHash = HashToken(rawRefresh),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };
        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync(ct);

        return Ok(new AuthResponse(accessToken, rawRefresh));
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return Ok(new { userId, email, role });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest req, CancellationToken ct)
    {
        var tokenHash = HashToken(req.RefreshToken);
        var stored = await _db.RefreshTokens
            .Include(r => r.User)
            .SingleOrDefaultAsync(r => r.TokenHash == tokenHash, ct);

        if (stored is null || stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        stored.IsRevoked = true;

        var newRaw = GenerateRefreshToken();
        var newToken = new RefreshToken
        {
            TokenHash = HashToken(newRaw),
            UserId = stored.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };
        _db.RefreshTokens.Add(newToken);
        await _db.SaveChangesAsync(ct);

        return Ok(new AuthResponse(CreateToken(stored.User), newRaw));
    }

    [HttpPost("revoke")]
    [AllowAnonymous]
    public async Task<IActionResult> Revoke(RefreshRequest req, CancellationToken ct)
    {
        var tokenHash = HashToken(req.RefreshToken);
        var stored = await _db.RefreshTokens
            .SingleOrDefaultAsync(r => r.TokenHash == tokenHash, ct);

        if (stored is null) return NotFound();

        stored.IsRevoked = true;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    private string CreateToken(AppUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}