using MetaParser.Core;

using System.Collections.Generic;

namespace MetaParser.Builders.Interfaces;

internal abstract class MetaCodeBuilder : IMetaCodeBuilder, ICodeBuilder<ParserContext>
{
    #region Fields
    protected readonly LinkedList<ICodeBuilder<ParserContext>> preBuilders = new();
    protected readonly LinkedList<ICodeBuilder<ParserContext>> contentBuilders = new();
    protected readonly LinkedList<ICodeBuilder<ParserContext>> postBuilders = new();
    #endregion

    protected abstract void Write(ParserContext context);

    #region Call Chain Builders
    public ICodeBuilder<ParserContext> Before(ICodeBuilder<ParserContext> builder)
    {
        preBuilders.AddLast(builder);
        return this;
    }

    public ICodeBuilder<ParserContext> After(ICodeBuilder<ParserContext> builder)
    {
        postBuilders.AddLast(builder);
        return this;
    }

    public ICodeBuilder<ParserContext> And(ICodeBuilder<ParserContext> builder)
    {
        contentBuilders.AddLast(builder);
        return this;
    }
    #endregion

    protected void WriteContent(ParserContext context)
    {
        foreach (var builder in contentBuilders)
        {
            builder.WriteTo(context);
        }
    }

    public void WriteTo(ParserContext context)
    {
        foreach (var builder in preBuilders)
        {
            builder.WriteTo(context);
        }

        Write(context);

        foreach (var builder in postBuilders)
        {
            builder.WriteTo(context);
        }
    }

}
