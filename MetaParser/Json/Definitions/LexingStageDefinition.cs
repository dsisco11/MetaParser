using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record class LexingStageDefinition : IParsingStageDefinition<ValueConsumerDeclaration>
{
    [JsonIgnore]
    public EParsingStage Stage => EParsingStage.Lexing;

    #region Properties
    [JsonPropertyName("definitions")]
    public ImmutableDictionary<string, IEnumerable<ValueConsumerDeclaration>>? Definitions { get; set; }
    #endregion
}
