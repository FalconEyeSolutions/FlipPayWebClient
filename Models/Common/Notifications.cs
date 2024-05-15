using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record Notifications(
    [property: JsonPropertyName("webhook")] string Webhook,
    [property: JsonPropertyName("token")] string? Token,
    [property: JsonPropertyName("emails")] List<string> Emails
);