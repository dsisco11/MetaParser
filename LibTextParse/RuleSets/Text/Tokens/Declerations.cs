using MetaParser.Parsing;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record NewlineToken() : StaticToken(ETextToken.Newline, '\n');
    public sealed record ColonToken() : StaticToken(ETextToken.Colon, ':');
    public sealed record ColumnToken() : StaticToken(ETextToken.Column, '|');
    public sealed record SemicolonToken() : StaticToken(ETextToken.Semicolon, ';');
    public sealed record CommaToken() : StaticToken(ETextToken.Comma, ',');
    public sealed record SqBracketOpenToken() : StaticToken(ETextToken.SqBracketOpen, '[');
    public sealed record SqBracketCloseToken() : StaticToken(ETextToken.SqBracketClose, ']');
    public sealed record ParenthOpenToken() : StaticToken(ETextToken.ParenthOpen, '(');
    public sealed record ParenthCloseToken() : StaticToken(ETextToken.ParenthClose, ')');
    public sealed record BracketOpenToken() : StaticToken(ETextToken.BracketOpen, '{');
    public sealed record BracketCloseToken() : StaticToken(ETextToken.BracketClose, '}');
    public sealed record LessThanToken() : StaticToken(ETextToken.LessThan, '<');
    public sealed record GreaterThanToken() : StaticToken(ETextToken.GreaterThan, '>');
    public sealed record HypenMinusToken() : StaticToken(ETextToken.HypenMinus, '-');
    public sealed record EqualsToken() : StaticToken(ETextToken.Equals, '=');
    public sealed record PlusToken() : StaticToken(ETextToken.Plus, '+');
}
