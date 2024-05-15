using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectGetListResponseItem(
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("prId")] string? PrId,
    [property: JsonPropertyName("productId")] string? ProductId,
    [property: JsonPropertyName("merchantId")] string? MerchantId,
    [property: JsonPropertyName("created")] string? Created,
    [property: JsonPropertyName("activated")] string? Activated,
    [property: JsonPropertyName("due")] string? Due,
    [property: JsonPropertyName("closed")] string? Closed,
    [property: JsonPropertyName("amountRequested")] decimal? AmountRequested,
    [property: JsonPropertyName("amountDisbursed")] decimal? AmountDisbursed,
    [property: JsonPropertyName("fees")] decimal? Fees,
    [property: JsonPropertyName("amountOutstanding")] decimal? AmountOutstanding
);