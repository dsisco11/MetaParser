using System.Collections.Generic;

namespace MetaParser.Json.Definitions;

internal interface IParsingStageDefinition
{
    EParsingStage Stage { get; }
    Dictionary<string, IEnumerable<IConsumerDeclaration>>? Consumers { get; }
}
