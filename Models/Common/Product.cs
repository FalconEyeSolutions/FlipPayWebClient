using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record Product(
    [property: JsonPropertyName("productId")] string ProductId,
    [property: JsonPropertyName("productFields")] List<ProductField> ProductFields
);
