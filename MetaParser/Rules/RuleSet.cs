using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("RuleSet: [{Items.ToString()}]")]
    public record RuleSet<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Properties
        public ITokenRule<TEnum, TValue>[] Items { get; init; }
        #endregion

        #region Constructors
        protected RuleSet() 
        {
            Items = Array.Empty<ITokenRule<TEnum, TValue>>();
        }

        public RuleSet(params ITokenRule<TEnum, TValue>[] items)
        {
            Items = items.OrderByDescending(r => r.Specificity.SpecificityValue).ToArray();
        }

        public RuleSet(params ITokenRule<TEnum, TValue>[][] items) : this(items.SelectMany(x => x).ToArray())
        {
        }
        #endregion
    }

    public record UnsortedRuleSet<TEnum, TValue> : RuleSet<TEnum, TValue>
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Constructors
        public UnsortedRuleSet(params ITokenRule<TEnum, TValue>[] items) : base()
        {
            Items = items;
        }

        public UnsortedRuleSet(params ITokenRule<TEnum, TValue>[][] items) : this(items.SelectMany(x => x).ToArray())
        {
        }
        #endregion
    }
}
