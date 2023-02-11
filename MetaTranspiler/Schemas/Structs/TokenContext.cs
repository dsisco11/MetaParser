using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    [JsonSerializable(typeof(ParserDefinitionSchema))]
    [JsonSerializable(typeof(TokenDef))]
    [JsonSerializable(typeof(TokenDefConst))]
    [JsonSerializable(typeof(TokenDefCompound))]
    [JsonSerializable(typeof(TokenDefComplex))]
    [JsonSerializable(typeof(TokenReference))]
    //[JsonSerializable(typeof(TokenReference[]))]
    [JsonSerializable(typeof(ETokenType))]
    [JsonSerializable(typeof(Dictionary<string, TokenDef>))]
    public partial class TokenContext : JsonSerializerContext { }
}
