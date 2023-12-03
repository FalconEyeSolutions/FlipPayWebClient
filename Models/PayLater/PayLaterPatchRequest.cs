using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.PayLater;

public record PayLaterPatchRequest(
    [property: JsonPropertyName("productFields")] List<ProductField> ProductFields
);
