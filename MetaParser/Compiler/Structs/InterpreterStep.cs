using MetaParser.Json.Definitions;

using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal sealed record InterpreterStep
{
    public readonly Dictionary<EParsingStage, StageData> Stages = new();
}
