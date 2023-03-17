using MetaParser.Parsing.Constructs;

using System.Collections.Generic;

namespace MetaParser.Json.Definitions;

internal interface IConsumerDeclaration
{
    EConsumerType Type { get; }
    IEnumerable<IPatternDeclaration> Start { get; }
    IEnumerable<IPatternDeclaration> Consume { get; }
    IEnumerable<IPatternDeclaration> Stop { get; }
    IEnumerable<IPatternDeclaration> Escape { get; }
}