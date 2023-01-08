using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record PlusToken() : StaticToken(ETextToken.Plus, '+')
    {
        public readonly static PlusToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }
}
