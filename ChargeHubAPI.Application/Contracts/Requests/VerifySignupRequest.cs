using System.ComponentModel.DataAnnotations;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class VerifySignupRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string VerificationCode { get; set; } = string.Empty;
}

