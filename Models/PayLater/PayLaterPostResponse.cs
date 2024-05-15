using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.PayLater;

public record PayLaterPostResponse(
    [property: JsonPropertyName("prId")] string? PrId,
    [property: JsonPropertyName("prUrl")] string? PrUrl,
    [property: JsonPropertyName("status")] string? Status
);