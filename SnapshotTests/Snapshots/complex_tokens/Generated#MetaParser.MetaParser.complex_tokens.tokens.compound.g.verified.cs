//HintName: MetaParser.MetaParser.complex_tokens.tokens.compound.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
private static bool consume_compound_token (global::System.ReadOnlySpan<char> source, out byte id, out int length)
{
    switch (source[0])
    {
        case ' ':
        case '\t':
        case '\f':
        {
            id = TokenId.Whitespace;
            length = consume_whitespace (source);
            return true;
            
        }
        case (>= '0' and <= '9'):
        {
            id = TokenId.Digits;
            length = consume_digits (source);
            return true;
            
        }
        case (>= 'a' and <= 'z'):
        case (>= 'A' and <= 'Z'):
        {
            id = TokenId.Letters;
            length = consume_letters (source);
            return true;
            
        }
        case '\n':
        {
            id = TokenId.Newline;
            length = consume_newline (source);
            return true;
            
        }
    }
    
    id = default;
    length = default;
    return false;
    
    static int consume_whitespace (global::System.ReadOnlySpan<char> buffer)
    {
        int consumed = 0;
        while (buffer.Length > consumed)
        {
            switch (buffer[consumed])
            {
                case ' ':
                case '\t':
                case '\f':
                {
                    consumed++;
                    continue;
                }
                default: return consumed;
            }
        }
        return consumed;
    }
    
    static int consume_digits (global::System.ReadOnlySpan<char> buffer)
    {
        int consumed = 0;
        while (buffer.Length > consumed)
        {
            switch (buffer[consumed])
            {
                case (>= '0' and <= '9'):
                {
                    consumed++;
                    continue;
                }
                default: return consumed;
            }
        }
        return consumed;
    }
    
    static int consume_letters (global::System.ReadOnlySpan<char> buffer)
    {
        int consumed = 0;
        while (buffer.Length > consumed)
        {
            switch (buffer[consumed])
            {
                case (>= 'a' and <= 'z'):
                case (>= 'A' and <= 'Z'):
                {
                    consumed++;
                    continue;
                }
                default: return consumed;
            }
        }
        return consumed;
    }
    
    static int consume_newline (global::System.ReadOnlySpan<char> buffer)
    {
        int consumed = 0;
        while (buffer.Length > consumed)
        {
            switch (buffer[consumed])
            {
                case '\n':
                {
                    consumed++;
                    continue;
                }
                default: return consumed;
            }
        }
        return consumed;
    }
    
}
}
