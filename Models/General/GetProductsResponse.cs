using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.General;

public interface IProductInfo{ }

public record GetProductsResponse(
    [property: JsonPropertyName("products")] List<IProductInfo>? Products
);

public record ProductDetails(
    [property: JsonPropertyName("productId")] string ProductId,
    [property: JsonPropertyName("minAmount")] decimal MinAmount,
    [property: JsonPropertyName("maxAmount")] decimal MaxAmount
) : IProductInfo;

public record MerchantFacilityInfo(
    [property: JsonPropertyName("merchantFacility")] bool MerchantFacility
) : IProductInfo;