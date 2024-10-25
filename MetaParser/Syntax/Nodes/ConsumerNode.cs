using System.Collections.Generic;

using JetBrains.Annotations;

using MetaParser.Syntax;

namespace MetaParser.Trees;

internal abstract record ConsumerNode : GreenNode
{
    public ConsumerNode([NotNull] PatternNode start, PatternNode? consume = null, PatternNode? stop = null, PatternNode? escape = null)
    {
        var list = new List<GreenNode> { start };
        if (consume is not null)
        {
            list.Add(consume);
        }

        if (stop is not null)
        {
            list.Add(stop);
        }

        if (escape is not null)
        {
            list.Add(escape);
        }

        Children = list.ToArray();
        Width = list.Count;
    }

    #region Accessors
    public PatternNode Start => (PatternNode) Children[0];
    public PatternNode? Consume => (PatternNode?) Children[1];
    public PatternNode? Stop => (PatternNode?) (Children.Length > 2 && !(Children[2] is null) ? Children[2] : null);
    public PatternNode? Escape => (PatternNode?) (Children.Length > 3 ? Children[3] : null);
    #endregion
}
