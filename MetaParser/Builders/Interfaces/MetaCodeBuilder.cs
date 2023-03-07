using MetaParser.Core;

using System.Collections.Generic;

namespace MetaParser.Builders.Interfaces;

internal abstract class MetaCodeBuilder : IMetaCodeBuilder
{
    protected readonly List<IMetaCodeBuilder> _attachments = new(2);

    public IMetaCodeBuilder Then(IMetaCodeBuilder attachment)
    {
        _attachments.Add(attachment);
        return this;
    }

    public void WriteTo(MetaParserContext context)
    {
        foreach (var attachment in _attachments)
        {
            attachment.WriteTo(context);
        }
    }
}
