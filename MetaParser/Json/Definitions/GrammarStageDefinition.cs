using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record class GrammarStageDefinition : IParsingStageDefinition<TokenConsumerDeclaration>
{
    [JsonIgnore]
    public EParsingStage Stage => EParsingStage.Lexing;

    #region Properties
    [JsonPropertyName("consumers")]
    public ImmutableDictionary<string, IEnumerable<TokenConsumerDeclaration>>? Consumers { get; set; }
    #endregion
}
