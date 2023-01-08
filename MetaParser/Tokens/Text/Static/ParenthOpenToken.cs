using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record ParenthOpenToken() : StaticToken(ETextToken.ParenthOpen, '(')
    {
        public readonly static ParenthOpenToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
