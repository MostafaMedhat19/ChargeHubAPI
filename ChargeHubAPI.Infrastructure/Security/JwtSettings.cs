namespace ChargeHubAPI.Infrastructure.Security;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "ChargeHubAPI";
    public string Audience { get; set; } = "ChargeHubAPI";
    public int ExpiryMinutes { get; set; } = 60;
}


