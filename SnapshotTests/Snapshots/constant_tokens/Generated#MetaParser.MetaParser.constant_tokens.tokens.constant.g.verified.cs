//HintName: MetaParser.MetaParser.constant_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private static bool consume_constant_token(global::System.ReadOnlySpan<char> source, out byte id, out int length)
    {
        switch (source)
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
