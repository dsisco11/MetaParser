//HintName: file.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public partial class Parser : MetaParser.Parser<char>
{
    private static bool try_consume_constant_token (System.Memory.ReadOnlyMemory<char> source, out int id, out int length)
    {
        var buffer = source.Slice();
        buffer = buffer.Slice(0, 8);
        switch(buffer)
        {
            case "function":
                id = TokenId.KEYWORD_FUNCTION;
                length = 8;
                return true;
        }
        
        buffer = buffer.Slice(0, 3);
        switch(buffer)
        {
            case "var":
                id = TokenId.KEYWORD_VAR;
                length = 3;
                return true;
        }
        
        buffer = buffer.Slice(0, 1);
        switch(buffer)
        {
            case "{":
                id = TokenId.LEFT_CURLY_BRACKET;
                length = 1;
                return true;
            case "}":
                id = TokenId.RIGHT_CURLY_BRACKET;
                length = 1;
                return true;
        }
        
        id = default;
        length = default;
        return false;
    }
}
