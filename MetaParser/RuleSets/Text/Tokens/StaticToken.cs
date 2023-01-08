using MetaParser.Parsing;

namespace MetaParser.RuleSets.Text.Tokens
{
    public abstract record StaticToken(ETextToken type, char value) : ValueToken(type, new[] { value });
}
