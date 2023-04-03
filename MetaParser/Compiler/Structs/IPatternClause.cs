using MetaParser.Parsing.Constructs;

using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;


/// <summary>
/// Represents an abstract pattern which can be used to create a matching sequence
/// </summary>
internal interface IPatternClause : IEnumerable<IPatternClause>
{
    bool IsReducible { get; }
    EPatternKind Kind { get; }

    /// <summary> Indicates whether the pattern is reducible, meaning it can be represented as a simpler item </summary>
    IPatternClause Reduce();
}