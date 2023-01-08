using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record BracketOpenToken() : StaticToken(ETextToken.BracketOpen, '{')
    {
        public readonly static BracketOpenToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
