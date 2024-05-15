using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectGetResponse(
    [property: JsonPropertyName("merchantId")] string? MerchantId,
    [property: JsonPropertyName("prId")] string? PrId,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("reference")] string? Reference,
    [property: JsonPropertyName("amount")] decimal? Amount,
    [property: JsonPropertyName("product")] List<Product>? Product,
    [property: JsonPropertyName("disbursement")] Disbursement? Disbursement,
    [property: JsonPropertyName("debit")] Debit? Debit,
    [property: JsonPropertyName("notifications")] Notifications? Notifications
);