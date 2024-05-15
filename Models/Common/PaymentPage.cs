using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record PaymentPage(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("abn")] string Abn,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("website")] string Website,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("logos")] List<string>? Logos //Max 2 logos urls
);