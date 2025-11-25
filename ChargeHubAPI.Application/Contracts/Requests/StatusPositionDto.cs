using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class StatusPositionPayload
{
    [JsonPropertyName("north")]
    public double North { get; set; }
    [JsonPropertyName("east")]
    public double East { get; set; }
    [JsonPropertyName("south")]
    public double South { get; set; }
    [JsonPropertyName("west")]
    public double West { get; set; }
}

