namespace Api.Domain;

public class RefreshToken
{
    public int Id { get; set; }
    public string TokenHash { get; set; } = null!;
    public Guid UserId { get; set; }       
    public AppUser User { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}