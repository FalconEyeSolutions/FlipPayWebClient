using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectPostRequest(
    string MerchantId,
    string Reference,
    decimal Amount,
    Disbursement Disbursement,
    Notifications? Notifications,
    [property: JsonPropertyName("debit")] Debit? Debit
) : PayRequest(MerchantId, Reference, Amount, Disbursement, Notifications);
