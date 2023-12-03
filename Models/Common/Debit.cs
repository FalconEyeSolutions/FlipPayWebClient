using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public abstract record Debit { }

public record AccountIDDebit([property: JsonPropertyName("accountId")] string AccountId) : Debit;

public record BankAccountDebit(
    [property: JsonPropertyName("accountName")] string AccountName,
    [property: JsonPropertyName("accountNumber")] string AccountNumber,
    [property: JsonPropertyName("accountBsb")] string AccountBsb
) : Debit;
