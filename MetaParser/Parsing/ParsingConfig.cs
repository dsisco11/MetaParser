using MetaParser.Rules;
using MetaParser.Tokens;

namespace MetaParser.Parsing
{
    public readonly struct ParsingConfig<TTokenData>
        where TTokenData : unmanaged, IEquatable<TTokenData>
    {
        #region Fields
        private readonly EOFToken<TTokenData> eof = new();
        private readonly RuleSet<TTokenData>[] rulesets;
        #endregion

        #region Properties
        public readonly EOFToken<TTokenData> EOF => eof;
        public RuleSet<TTokenData>[] Rulesets => rulesets;

        #endregion

        #region Constructors
        public ParsingConfig(params RuleSet<TTokenData>[] rulesets)
        {
            this.rulesets = rulesets;
        }
        #endregion

    }
}
