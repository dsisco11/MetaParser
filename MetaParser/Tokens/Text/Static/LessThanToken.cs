using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record LessThanToken() : StaticToken(ETextToken.LessThan, '<')
    {
        public readonly static LessThanToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
