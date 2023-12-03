using System.Text.Json.Serialization;

namespace FlipPayApiLibrary.Models.Common;

public record ProductField(
    [property: JsonPropertyName("fieldId")] string FieldId,
    [property: JsonPropertyName("value")] string Value
);
