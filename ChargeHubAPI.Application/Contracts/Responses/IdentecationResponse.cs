using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Responses;

public class IdentecationResponse : StandardResponse
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("identecation")]
    public string Identecation { get; set; } = string.Empty;
}


