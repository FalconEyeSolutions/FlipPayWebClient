using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectPostResponse(
    [property: JsonPropertyName("prId")] string? PrId,
    [property: JsonPropertyName("prUrl")] string? PrUrl,
    [property: JsonPropertyName("status")] string? Status
);
