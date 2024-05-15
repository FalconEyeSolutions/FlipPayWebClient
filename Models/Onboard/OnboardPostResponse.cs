using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Onboard;

public record OnboardPostResponse(
    [property: JsonPropertyName("onboardingId")] string? OnboardingId,
    [property: JsonPropertyName("onboardingUrl")] string? OnboardingUrl
);