//HintName: MetaParser.MetaParser.compound_tokens.tokens.compound.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
private bool try_consume_token_compound (global::System.ReadOnlyMemory<System.Char> source, out System.Int32 id, out System.Int32 length)
{
    switch (source.Span[0])
    {
        case ' ':
        case '\t':
        case '\f':
        {
            id = TokenId.Whitespace;
            length = consume_all_whitespace (source.Span);
            return true;
            
        }
        case (>= '0' and <= '9'):
        {
            id = TokenId.Digits;
            length = consume_all_digits (source.Span);
            return true;
            
        }
        case (>= 'a' and <= 'z'):
        case (>= 'A' and <= 'Z'):
        case '-':
        case '_':
        {
            id = TokenId.Letters;
            length = consume_all_letters (source.Span);
            return true;
            
        }
        case '\n':
        {
            id = TokenId.Newline;
            length = consume_all_newline (source.Span);
            return true;
            
        }
    }
    
    id = default;
    length = default;
    return false;
    
    static System.Int32 consume_all_whitespace (global::System.ReadOnlySpan<System.Char> buffer)
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
    
    static System.Int32 consume_all_digits (global::System.ReadOnlySpan<System.Char> buffer)
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
    
    static System.Int32 consume_all_letters (global::System.ReadOnlySpan<System.Char> buffer)
    {
        int consumed = 0;
        while (buffer.Length > consumed)
        {
            switch (buffer[consumed])
            {
                case (>= 'a' and <= 'z'):
                case (>= 'A' and <= 'Z'):
                case '-':
                case '_':
                {
                    consumed++;
                    continue;
                }
                default: return consumed;
            }
        }
        return consumed;
    }
    
    static System.Int32 consume_all_newline (global::System.ReadOnlySpan<System.Char> buffer)
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
