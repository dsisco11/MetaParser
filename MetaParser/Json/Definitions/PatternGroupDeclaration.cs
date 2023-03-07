using MetaParser.Core;
using MetaParser.Patternization;

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
    public static readonly PatternGroupDeclaration<T> Empty = new PatternGroupDeclaration<T>(EPatternCondition.Only, Array.Empty<T>());

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

    public Pattern Resolve(MetaParserContext context)
    {
        if (items.Any())
        {
            if (items.Count() == 1)
            {
                return items.Single().Resolve(context);
            }

            return new PatternGroup(condition, items.Select(o => o.Resolve(context)).ToArray());
        }

        return Pattern.Empty;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)items).GetEnumerator();
    }
}
