using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record SqBracketCloseToken() : StaticToken(ETextToken.SqBracketClose, ']')
    {
        public readonly static SqBracketCloseToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
