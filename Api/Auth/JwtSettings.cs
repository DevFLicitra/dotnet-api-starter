namespace Api.Auth;

public record JwtSettings(string Issuer, string Audience, string Key);
