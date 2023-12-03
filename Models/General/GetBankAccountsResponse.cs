using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.General;

public record GetBankAccountsResponse([property: JsonPropertyName("accounts")] List<Account> Accounts);

public record Account(
    [property: JsonPropertyName("accountId")] string AccountId,
    [property: JsonPropertyName("accountName")] string AccountName,
    [property: JsonPropertyName("accountNumber")] string AccountNumber,
    [property: JsonPropertyName("bsb")] string Bsb
);
