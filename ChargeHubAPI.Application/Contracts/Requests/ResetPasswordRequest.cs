using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class ResetPasswordRequest
{
    [Required]
    [JsonPropertyName("phoneNumber")]
    [RegularExpression("^\\+?[0-9]{10,15}$", ErrorMessage = "Phone number must include country code and digits only.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("resetCode")]
    [StringLength(6, MinimumLength = 6)]
    public string ResetCode { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

