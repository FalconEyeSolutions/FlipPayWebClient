using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record Receiver(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("email")] string Email
);

public record ReceiverPay(
    string Name,
    string Email,
    [property: JsonPropertyName("mobile")] string Mobile
) : Receiver(Name, Email);
