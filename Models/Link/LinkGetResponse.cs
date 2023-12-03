using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Link;

public record LinkGetResponse(
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("status")] string Status
);
