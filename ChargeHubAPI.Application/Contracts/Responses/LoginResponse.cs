using System.Text.Json.Serialization;
using ChargeHubAPI.Application.Dtos;

namespace ChargeHubAPI.Application.Contracts.Responses;

public class LoginResponse : StandardResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public UserDto? User { get; set; }
}

