//HintName: MetaParser.MetaParser.complex_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private bool try_consume_token_constant (global::System.ReadOnlyMemory<System.Char> source, out System.Int32 id, out System.Int32 length)
    {
        switch (source.Span)
        {
            case ['v', 'a', 'r', ..]:
            {
                id = TokenId.Keyword_Var;
                length = 3;
                return true;
            }
            case ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
            {
                id = TokenId.Keyword_Function;
                length = 8;
                return true;
            }
            case ['{', ..]:
            {
                id = TokenId.Char_Open_Bracket;
                length = 1;
                return true;
            }
            case ['}', ..]:
            {
                id = TokenId.Char_Close_Bracket;
                length = 1;
                return true;
            }
            case ['*', ..]:
            {
                id = TokenId.Char_Asterisk;
                length = 1;
                return true;
            }
            case ['/', ..]:
            {
                id = TokenId.Char_Solidus;
                length = 1;
                return true;
            }
            case ['\\', ..]:
            {
                id = TokenId.Char_Reverse_Solidus;
                length = 1;
                return true;
            }
        }
        id = default;
        length = default;
        return false;
    }
}
