using FlipPayApiLibrary.Models.Common;
using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Direct;

public record DirectPatchRequest(
    [property: JsonPropertyName("productFields")] List<ProductField> ProductFields
);
