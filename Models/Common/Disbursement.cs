using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public abstract record Disbursement { }

public record AccountDisbursement([property: JsonPropertyName("accountId")] string AccountId)
    : Disbursement;

public record BpayDisbursement(
    [property: JsonPropertyName("billerCode")] string BillerCode,
    [property: JsonPropertyName("referenceNumber")] string ReferenceNumber
) : Disbursement;

public record BankAccountDisbursement(
    [property: JsonPropertyName("accountName")] string AccountName,
    [property: JsonPropertyName("accountNumber")] string AccountNumber,
    [property: JsonPropertyName("accountBsb")] string AccountBsb
) : Disbursement;