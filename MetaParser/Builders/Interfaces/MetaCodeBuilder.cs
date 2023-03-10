using MetaParser.Parsing.Constructs.Core;

using System.Collections.Generic;

namespace MetaParser.Builders.Interfaces;

internal abstract class MetaCodeBuilder : IMetaCodeBuilder, ICodeBuilder<MetaParserContext>
{
    #region Fields
    protected readonly LinkedList<ICodeBuilder<MetaParserContext>> preBuilders = new();
    protected readonly LinkedList<ICodeBuilder<MetaParserContext>> contentBuilders = new();
    protected readonly LinkedList<ICodeBuilder<MetaParserContext>> postBuilders = new();
    #endregion

    protected abstract void Write(MetaParserContext context);

    #region Call Chain Builders
    public ICodeBuilder<MetaParserContext> Before(ICodeBuilder<MetaParserContext> builder)
    {
        preBuilders.AddLast(builder);
        return this;
    }

    public ICodeBuilder<MetaParserContext> After(ICodeBuilder<MetaParserContext> builder)
    {
        postBuilders.AddLast(builder);
        return this;
    }

    public ICodeBuilder<MetaParserContext> And(ICodeBuilder<MetaParserContext> builder)
    {
        contentBuilders.AddLast(builder);
        return this;
    }
    #endregion

    protected void WriteContent(MetaParserContext context)
    {
        foreach (var builder in contentBuilders)
        {
            builder.WriteTo(context);
        }
    }

    public void WriteTo(MetaParserContext context)
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
