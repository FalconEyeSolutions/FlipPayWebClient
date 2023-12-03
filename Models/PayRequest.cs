using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models;

public abstract record PayRequest(
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("reference")] string Reference,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("disbursement")] Disbursement Disbursement,
    [property: JsonPropertyName("notifications")] Notifications? Notifications
);
