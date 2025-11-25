using System.ComponentModel.DataAnnotations;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class RegisterEsp32Request
{
    [Required]
    public string Identecation { get; set; } = string.Empty;

    [Required]
    public string BtName { get; set; } = string.Empty;

    [Required]
    public string BtAddress { get; set; } = string.Empty;
}




