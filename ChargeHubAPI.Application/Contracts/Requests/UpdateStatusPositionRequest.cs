using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChargeHubAPI.Application.Contracts.Requests;

public class UpdateStatusPositionRequest
{
    [Required]
    public string Identecation { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("status_position")]
    public StatusPositionPayload StatusPosition { get; set; } = new();
}

