using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MetaParser.JsonTypeConverters;

namespace MetaParser.Schemas.Structs
{
    [JsonConverter(typeof(CompoundItemConverter))]
    public abstract record CompoundItemValue
    {
        public static CompoundItemValue? From(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Array => new CompoundValueRange(element.Deserialize<string[]>() ?? Array.Empty<string>()),
                JsonValueKind.String => new CompoundValueString(element.GetString()!),
                JsonValueKind.Object => element.TryGetProperty("range", out var outRange) ? new CompoundValueRange(outRange.Deserialize<string[]>() ?? Array.Empty<string>()) : null,
                _ => throw new NotImplementedException()
            };
        }
    }
}
