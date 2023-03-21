using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Json.Definitions;

internal interface IPatternGroupDeclaration : IEnumerable<IPatternDeclaration>, IPatternDeclaration { }

internal sealed record PatternGroupDeclaration<T> : IPatternGroupDeclaration
    where T : IPatternDeclaration
{

    #region Fields
    private EPatternCondition condition;
    private IEnumerable<T> items = Array.Empty<T>();
    #endregion

    public PatternGroupDeclaration(EPatternCondition condition, IEnumerable<T> items)
    {
        this.items = items;
        this.condition = condition;
    }

    public IEnumerator<IPatternDeclaration> GetEnumerator()
    {
        return items.GetEnumerator() as IEnumerator<IPatternDeclaration>;
    }

    public Pattern? Resolve(ParserContext context)
    {
        if (items.Any())
        {
            if (items.Count() == 1)
            {
                return items.Single().Resolve(context);
            }

            return new PatternGroup(condition, context, items.Select(o => o.Resolve(context)!).ToArray());
        }

        return null;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)items).GetEnumerator();
    }
}
