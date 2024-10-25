using MetaParser.Core;

using System;
using System.Collections.Generic;

namespace MetaParser.Builders.Core;
internal class ForEachCodeBuilder<TItem> : CodeBuilder
{
    private readonly IEnumerable<TItem> _items;
    private readonly Func<TItem, CodeBuilder> _codeBuilderFunc;

    public ForEachCodeBuilder(IEnumerable<TItem> items, Func<TItem, CodeBuilder> codeBuilderFunc)
    {
        _items = items;
        _codeBuilderFunc = codeBuilderFunc;
    }

    protected override void WriteTo(CodeGenContext context)
    {
        foreach (var item in _items)
        {
            var builder = _codeBuilderFunc(item);
            builder.GenerateCode(context);
        }
    }
}
