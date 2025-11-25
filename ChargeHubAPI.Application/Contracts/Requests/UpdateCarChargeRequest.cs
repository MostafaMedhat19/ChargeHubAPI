using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class UpdateCarChargeRequest
{
    [Required]
    public string Identecation { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    [JsonPropertyName("car_charge")]
    public int CarCharge { get; set; }
}

