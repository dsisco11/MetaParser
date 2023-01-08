using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record BracketCloseToken() : StaticToken(ETextToken.BracketClose, '}')
    {
        public readonly static BracketCloseToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
