using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.PayNow;

public record PayNowPostResponse(
    [property: JsonPropertyName("prID")] string PrId,
    [property: JsonPropertyName("prURL")] string PrUrl,
    [property: JsonPropertyName("status")] string Status
);