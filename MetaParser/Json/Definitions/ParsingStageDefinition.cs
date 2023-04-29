using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal abstract record ParsingStageDefinition<T> : IParsingStageDefinition
    where T : ConsumerDeclaration
{
    #region Properties
    [JsonPropertyName("$type")]
    public abstract EParsingStage Type { get; }

    [JsonPropertyName("ignored")]
    public abstract string[]? Ignored { get; set; }

    [JsonPropertyName("dropped")]
    public abstract T[]? Dropped { get; set; }

    [JsonPropertyName("consumers")]
    public abstract Dictionary<string, IEnumerable<T>>? Consumers { get; set; }
    #endregion

    IEnumerable<KeyValuePair<string, IEnumerable<ConsumerDeclaration>>> IParsingStageDefinition.Consumers
    {
        get
        {
            // yield return all consumers from dictionary cast to the base type
            if (Consumers is null)
            {
                yield break;
            }

            foreach (var pair in Consumers)
            {
                yield return new KeyValuePair<string, IEnumerable<ConsumerDeclaration>>(pair.Key, pair.Value.Cast<ConsumerDeclaration>());
            }
        }
    }

    ConsumerDeclaration[] IParsingStageDefinition.Dropped => Dropped;
}
