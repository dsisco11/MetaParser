using System.Linq;

namespace MetaParser.Rules
{
    public sealed record RuleSet<T> where T : unmanaged, IEquatable<T>
    {
        public ITokenRule<T>[] Items { get; init; }
        public RuleSet(params ITokenRule<T>[] items)
        {
            Items = items;
        }
        public RuleSet(params ITokenRule<T>[][] items)
        {
            Items = items.SelectMany(x => x).ToArray();
        }
    }
}
