using System.Text.Json.Serialization;
using ChargeHubAPI.Application.Dtos;

namespace ChargeHubAPI.Application.Contracts.Responses;

public class UserInfoResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Identecation { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("car_charge")]
    public int CarCharge { get; set; }
    public Esp32Dto? Esp32 { get; set; }
    [JsonPropertyName("status_position")]
    public StatusPositionDto? StatusPosition { get; set; }
}

