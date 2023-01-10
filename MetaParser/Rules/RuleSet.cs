using System.Collections;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("RuleSet: [{Items.ToString()}]")]
    public record RuleSet<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Properties
        public ITokenRule<TData, TValue>[] Items { get; init; }
        #endregion

        #region Constructors
        protected RuleSet() 
        {
            Items = Array.Empty<ITokenRule<TData, TValue>>();
        }

        public RuleSet(params ITokenRule<TData, TValue>[] items)
        {
            Items = items.OrderByDescending(r => r.Specificity.SpecificityValue).ToArray();
        }

        public RuleSet(params ITokenRule<TData, TValue>[][] items) : this(items.SelectMany(x => x).ToArray())
        {
        }
        #endregion
    }

    public record UnsortedRuleSet<TData, TValue> : RuleSet<TData, TValue>
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Constructors
        public UnsortedRuleSet(params ITokenRule<TData, TValue>[] items) : base()
        {
            Items = items;
        }

        public UnsortedRuleSet(params ITokenRule<TData, TValue>[][] items) : this(items.SelectMany(x => x).ToArray())
        {
        }
        #endregion
    }
}
