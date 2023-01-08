using MetaParser.Parsing;

namespace MetaParser.Tokens.Text
{
    public abstract record StaticToken(ETextToken Type, char Character) : ValueToken(Type, new[] { Character });
}
