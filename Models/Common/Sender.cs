using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record Sender(
    [property: JsonPropertyName("emailMask")] string? EmailMask,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("email")] string? Email
);

public record SenderPay(
    string EmailMask,
    string Name,
    string Phone,
    string Email,
    [property: JsonPropertyName("sendComms")] bool? SendComms) : Sender(EmailMask, Name, Phone, Email);
