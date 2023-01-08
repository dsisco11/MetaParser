using MetaParser.Parsing.Tokens;

namespace MetaParser.Parsing
{
    public readonly struct ParsingConfig<TTokenData>
        where TTokenData : unmanaged, IEquatable<TTokenData>
    {
        #region Fields
        private readonly EOFToken<TTokenData> eof = new();
        private readonly TokenRuleSet<TTokenData>[] rulesets;
        #endregion

        #region Properties
        public readonly EOFToken<TTokenData> EOF => eof;
        public TokenRuleSet<TTokenData>[] Rulesets => rulesets;

        #endregion

        #region Constructors
        public ParsingConfig(params TokenRuleSet<TTokenData>[] rulesets)
        {
            this.rulesets = rulesets;
        }
        #endregion

    }
}
