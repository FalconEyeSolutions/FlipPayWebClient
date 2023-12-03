using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Onboard;

public record OnboardGetResponse(
    [property: JsonPropertyName("onboardingUrl")] string? OnboardingUrl,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("merchantId")] string? MerchantId
);
