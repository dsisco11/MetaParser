using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record HypenMinusToken() : StaticToken(ETextToken.HypenMinus, '-')
    {
        public readonly static HypenMinusToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
