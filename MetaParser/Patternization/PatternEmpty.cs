using System.Collections.Generic;

namespace MetaParser.Patternization;

internal record PatternEmpty : Pattern
{
    public override int Length => 0;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override bool ContainsItems => false;

    public override Pattern Combine(Pattern other)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<Pattern> GetSubPatterns()
    {
        yield break;
    }
}
