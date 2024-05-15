using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.PayLater;

public record PayLaterPostRequest(
    string MerchantId,
    string Reference,
    decimal Amount,
    Disbursement Disbursement,
    Notifications? Notifications,
    [property: JsonPropertyName("product")] List<Product> Product,
    [property: JsonPropertyName("sender")] Sender? Sender,
    [property: JsonPropertyName("receiver")] ReceiverPay? Receiver,
    [property: JsonPropertyName("paymentPage")] PaymentPage? PaymentPage,
    [property: JsonPropertyName("files")] List<string>? Files
) : PayRequest(MerchantId, Reference, Amount, Disbursement, Notifications);