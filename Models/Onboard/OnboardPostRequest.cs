using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Onboard;

public record OnboardPostRequest(
    [property: JsonPropertyName("sendComms")] bool SendComms,
    [property: JsonPropertyName("sender")] Sender? Sender,
    [property: JsonPropertyName("receiver")] Receiver Receiver,
    [property: JsonPropertyName("notifications")] Notifications? Notifications
);
