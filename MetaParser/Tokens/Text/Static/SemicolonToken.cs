using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record SemicolonToken() : StaticToken(ETextToken.Semicolon, ';')
    {
        public readonly static SemicolonToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
