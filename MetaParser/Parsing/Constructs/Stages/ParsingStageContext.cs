using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.Parsing.Constructs.Stages;

/// <summary>
/// A processing group is a collection of input values, output values, and consumers from which a single parser stage is constructed.
/// </summary>
internal sealed record ParsingStageContext
{
    #region Fields
    public ParsingStageContext? Head;
    public ParsingStageContext? Tail;
    public readonly int Index;
    public readonly TypeSyntax InputType;
    public readonly TypeSyntax OutputType;
    public readonly ImmutableArray<ConsumerEntity> Consumers;
    #endregion

    #region Accessors
    public ImmutableHashSet<string> Outputs => Consumers.Select(static x => x.OutputTokenAlias).ToImmutableHashSet();
    public bool IsHead => Head is null;
    public bool IsTail => Tail is null;
    #endregion

    #region Constructors
    public ParsingStageContext(int index, TypeSyntax inputType, TypeSyntax outputType, IEnumerable<ConsumerEntity> consumers)
    {
        Consumers = consumers.ToImmutableArray();
        InputType = inputType;
        OutputType = outputType;
        Index = index;
    }
    #endregion
}
