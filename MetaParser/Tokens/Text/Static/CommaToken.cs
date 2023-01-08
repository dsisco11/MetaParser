using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record CommaToken() : StaticToken(ETextToken.Comma, ',')
    {
        public readonly static CommaToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
