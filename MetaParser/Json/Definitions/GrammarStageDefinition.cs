using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed class GrammarStageDefinition : ParsingStageDefinition<TokenConsumerDeclaration>
{
    [JsonIgnore]
    public override EParsingStage Stage => EParsingStage.Parsing;

    #region Properties
    [JsonPropertyName("consumers")]
    public override Dictionary<string, IEnumerable<TokenConsumerDeclaration>>? Consumers { get; }
    #endregion
}
