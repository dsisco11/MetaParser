using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record ColonToken() : StaticToken(ETextToken.Colon, ':')
    {
        public readonly static ColonToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
