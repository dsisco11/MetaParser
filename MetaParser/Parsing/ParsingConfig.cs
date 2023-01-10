using MetaParser.Rules;

namespace MetaParser.Parsing
{
    public readonly struct ParsingConfig<TData, TValue>
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Fields
        private readonly RuleSet<TData, TValue>[] rulesets;
        #endregion

        #region Properties
        public RuleSet<TData, TValue>[] Rulesets => rulesets;

        #endregion

        #region Constructors
        public ParsingConfig(RuleSet<TData, TValue> ruleset)
        {
            this.rulesets = new[] { ruleset };
        }
        #endregion

    }
}
