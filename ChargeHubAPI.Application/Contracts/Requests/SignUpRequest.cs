using System.ComponentModel.DataAnnotations;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class SignUpRequest
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9_]{5,20}$", ErrorMessage = "Username must be 5-20 characters and contain only letters, numbers, or underscores.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Za-z ]{2,100}$", ErrorMessage = "Name must contain only letters and spaces.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^\\+?[0-9]{10,15}$", ErrorMessage = "Phone number must include country code and digits only.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}



