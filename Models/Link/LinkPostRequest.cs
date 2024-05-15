using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Link;

public record LinkPostRequest(
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("notifications")] Notifications? Notifications
);