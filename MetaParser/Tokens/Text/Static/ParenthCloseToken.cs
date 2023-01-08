using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record ParenthCloseToken() : StaticToken(ETextToken.ParenthClose, ')')
    {
        public readonly static ParenthCloseToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
