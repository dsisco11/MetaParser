//HintName: MetaParser.MetaParser.constant_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private bool try_consume_token_constant (global::System.ReadOnlyMemory<System.Char> source, out System.Int32 id, out System.Int32 length)
    {
        switch (source.Span)
        {
            case ['b', ..]:
            {
                id = TokenId.Fourth;
                length = 1;
                return true;
            }
            case ['a', ..]:
            {
                id = TokenId.Third;
                length = 1;
                return true;
            }
            case ['a', 'b', 'c', ..]:
            {
                id = TokenId.First;
                length = 3;
                return true;
            }
            case ['a', 'b', ..]:
            {
                id = TokenId.Second;
                length = 2;
                return true;
            }
            case ['x', ..]:
            {
                id = TokenId.Fifth;
                length = 1;
                return true;
            }
            case ['y', ..]:
            {
                id = TokenId.Fifth;
                length = 1;
                return true;
            }
            case ['z', ..]:
            {
                id = TokenId.Fifth;
                length = 1;
                return true;
            }
        }
        id = default;
        length = default;
        return false;
    }
}
