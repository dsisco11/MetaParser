using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record class GrammarStageDefinition : IParsingStageDefinition<TokenConsumerDeclaration>
{
    [JsonIgnore]
    public EParsingStage Stage => EParsingStage.Lexing;

    #region Properties
    [JsonPropertyName("definitions")]
    public ImmutableDictionary<string, IEnumerable<TokenConsumerDeclaration>>? Definitions { get; set; }
    #endregion
}
