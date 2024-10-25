using MetaParser.Core;

using System;

namespace MetaParser.Builders.Core;
internal class ConditionalCodeBuilder : CodeBuilder
{
    private readonly CodeBuilder _innerBuilder;
    private readonly Predicate<CodeGenContext> _predicate;

    public ConditionalCodeBuilder(Predicate<CodeGenContext> predicate, CodeBuilder innerBuilder)
    {
        _predicate = predicate;
        _innerBuilder = innerBuilder;
    }

    protected override void WriteTo(CodeGenContext context)
    {
        if (_predicate(context))
        {
            _innerBuilder.GenerateCode(context);
        }
    }
}
