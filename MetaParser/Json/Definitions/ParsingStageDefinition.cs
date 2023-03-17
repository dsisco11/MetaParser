using System.Collections.Generic;

namespace MetaParser.Json.Definitions;

internal abstract class ParsingStageDefinition<T> : IParsingStageDefinition
    where T : IConsumerDeclaration
{
    #region Properties
    public abstract EParsingStage Stage { get; }
    public abstract Dictionary<string, IEnumerable<T>>? Consumers { get; }
    #endregion

    Dictionary<string, IEnumerable<IConsumerDeclaration>>? IParsingStageDefinition.Consumers => Consumers as Dictionary<string, IEnumerable<IConsumerDeclaration>>;
}
