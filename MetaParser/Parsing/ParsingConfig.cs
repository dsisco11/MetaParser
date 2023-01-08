using MetaParser.Parsing.Tokens;

namespace MetaParser.Parsing
{
    public sealed record ParsingConfig<TokenData>
        where TokenData : unmanaged, IEquatable<TokenData>
    {
        #region Properties
        public readonly EOFToken<TokenData> EOF_TOKEN = new();
        public readonly TokenRuleSet<TokenData>[] Rulesets;
        #endregion

        #region Constructors
        public ParsingConfig(params TokenRuleSet<TokenData>[] rulesets)
        {
            Rulesets = rulesets;
        }
        #endregion

    }
}
