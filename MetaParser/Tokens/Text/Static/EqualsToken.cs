using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record EqualsToken() : StaticToken(ETextToken.Equals, '=')
    {
        public readonly static EqualsToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
