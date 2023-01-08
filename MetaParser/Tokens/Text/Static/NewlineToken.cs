using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record NewlineToken() : StaticToken(ETextToken.Newline, '\n')
    {
        public readonly static NewlineToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
