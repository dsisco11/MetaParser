using MetaParser.Parsing.Constructs.Core;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal record PatternEmpty : Pattern
{
    #region Accessors
    public override int Length => 0;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override bool HasChildren => false;
    #endregion

    public PatternEmpty()
    {
    }

    public override Pattern Combine(Pattern other, MetaParserContext context)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        yield break;
    }
}
