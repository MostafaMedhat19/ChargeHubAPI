namespace ChargeHubAPI.Domain.Entities;

public class User
{
    public string UserId { get; set; } = string.Empty;
    public string Identecation { get; set; } = string.Empty; // spelling per requirements
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int CarCharge { get; set; }
    public Esp32Device? Esp32 { get; set; }
    public StatusPosition? StatusPosition { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? SignupVerificationCode { get; set; }
    public DateTimeOffset? SignupVerificationExpiresAt { get; set; }
    public string? PasswordResetCode { get; set; }
    public DateTimeOffset? PasswordResetCodeExpiresAt { get; set; }
}



