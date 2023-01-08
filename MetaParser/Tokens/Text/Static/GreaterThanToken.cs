using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record GreaterThanToken() : StaticToken(ETextToken.GreaterThan, '>')
    {
        public readonly static GreaterThanToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
