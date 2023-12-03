using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.General;

public record GetProductsResponse(
    [property: JsonPropertyName("products")] List<ProductDetails>? Products
);

public record ProductDetails(
    [property: JsonPropertyName("productId")] string ProductId,
    [property: JsonPropertyName("minAmount")] decimal MinAmount,
    [property: JsonPropertyName("maxAmount")] decimal MaxAmount,
    [property: JsonPropertyName("merchantFacility")] bool? MerchantFacility
);
