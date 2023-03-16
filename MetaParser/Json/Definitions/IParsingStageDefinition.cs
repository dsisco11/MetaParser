using System.Collections.Generic;
using System.Collections.Immutable;

namespace MetaParser.Json.Definitions;

internal interface IParsingStageDefinition<T> where T : IConsumerDeclaration
{
    EParsingStage Stage { get; }
    ImmutableDictionary<string, IEnumerable<T>>? Definitions { get; }
}
