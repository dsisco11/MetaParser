using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    [JsonSerializable(typeof(ParserDefinitionSchema))]
    [JsonSerializable(typeof(TokenDef))]
    [JsonSerializable(typeof(TokenDefConstant))]
    [JsonSerializable(typeof(TokenDefCompound))]
    [JsonSerializable(typeof(TokenDefComplex))]
    [JsonSerializable(typeof(ETokenType))]
    [JsonSerializable(typeof(Dictionary<string, TokenDef>))]
    internal partial class TokenJsonContext : JsonSerializerContext { }
}
