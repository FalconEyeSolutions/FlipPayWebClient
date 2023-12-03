using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.PayLater;

public record PayNowGetResponse(
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("prId")] string PrId,
    [property: JsonPropertyName("prUrl")] string PrUrl,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("reference")] string Reference,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("disbursement")] Disbursement Disbursement,
    [property: JsonPropertyName("sender")] Sender Sender,
    [property: JsonPropertyName("receiver")] ReceiverPay Receiver,
    [property: JsonPropertyName("paymentPage")] PaymentPage PaymentPage,
    [property: JsonPropertyName("files")] List<string> Files,
    [property: JsonPropertyName("notifications")] Notifications Notifications
);
