using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Responses;

public class StandardResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}

