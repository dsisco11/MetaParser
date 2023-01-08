using MetaParser.Parsing;

namespace MetaParser.Tokens.Text.Static
{
    public sealed record ColumnToken() : StaticToken(ETextToken.Column, '|')
    {
        public readonly static ColumnToken Instance = new();
        public override IStaticToken<char> static_instance => Instance;
    }

}
