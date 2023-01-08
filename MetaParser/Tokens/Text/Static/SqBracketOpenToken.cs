using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record SqBracketOpenToken() : StaticToken(ETextToken.SqBracketOpen, '[')
    {
        public readonly static SqBracketOpenToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
