using MetaParser.Parsing;

namespace MetaParser.RuleSets.Text.Tokens
{
    public abstract record StaticToken(ETextToken Type, char Character) : ValueToken(Type, new[] { Character });
}
