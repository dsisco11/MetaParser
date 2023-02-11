//HintName: file.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public partial class Parser : MetaParser.Parser<char>
{
    private static bool try_consume_constant_token (System.Memory.ReadOnlyMemory<char> source, out int id, out int length)
    {
        var buffer = source.Slice();
        buffer = buffer.Slice(0, 3);
        switch(buffer)
        {
            case "abc":
                id = TokenId.FIRST;
                length = 3;
                return true;
        }
        
        buffer = buffer.Slice(0, 2);
        switch(buffer)
        {
            case "ab":
                id = TokenId.SECOND;
                length = 2;
                return true;
        }
        
        buffer = buffer.Slice(0, 1);
        switch(buffer)
        {
            case "b":
                id = TokenId.FOURTH;
                length = 1;
                return true;
            case "a":
                id = TokenId.THIRD;
                length = 1;
                return true;
            case "x":
            case "y":
            case "z":
                id = TokenId.FIFTH;
                length = 1;
                return true;
        }
        
        id = default;
        length = default;
        return false;
    }
}
