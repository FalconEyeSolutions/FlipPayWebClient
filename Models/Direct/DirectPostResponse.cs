using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectPostResponse(
    [property: JsonPropertyName("prId")] string? PrId,
    [property: JsonPropertyName("prUrl")] string? PrUrl,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("message1")] string? MessageOne,
    [property: JsonPropertyName("message2")] string? MessageTwo,
    [property: JsonPropertyName("repayment")] RepaymentDetails? Repayment
);

public record RepaymentDetails(
    [property: JsonPropertyName("reference")] string? Reference,
    [property: JsonPropertyName("contactEmail")] string? ContactEmail,
    [property: JsonPropertyName("contactPhone")] string? ContactPhone,
    [property: JsonPropertyName("options")] List<RepaymentOption>? Options
);

public record RepaymentOption(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("accountName")] string? AccountName,
    [property: JsonPropertyName("accountBsb")] string? AccountBsb,
    [property: JsonPropertyName("accountNumber")] string? AccountNumber,
    [property: JsonPropertyName("payID")] string? PayId
);
