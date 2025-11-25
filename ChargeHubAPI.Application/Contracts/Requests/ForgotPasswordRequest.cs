using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [JsonPropertyName("phoneNumber")]
    [RegularExpression("^\\+?[0-9]{10,15}$", ErrorMessage = "Phone number must include country code and digits only.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

