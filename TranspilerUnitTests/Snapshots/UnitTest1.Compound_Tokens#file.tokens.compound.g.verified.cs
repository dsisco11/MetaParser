//HintName: file.tokens.compound.g.cs
namespace Foo.Bar.Tokens;
public partial class Parser : MetaParser.Parser<char>
{
    private static bool try_consume_compound_token (System.Memory.ReadOnlyMemory<char> source, out int id, out int length)
    {
        switch (source.span[0])
        {
            case ' ':
            case '\t':
            case '\f':
                id = TokenId.WHITESPACE;
                length = _consume_all_whitespace (source.span);
                return true;
                
            case >= '0' and <= '9':
                id = TokenId.DIGITS;
                length = _consume_all_digits (source.span);
                return true;
                
            case >= 'a' and <= 'z':
            case >= 'A' and <= 'Z':
            case '-':
            case '_':
                id = TokenId.LETTERS;
                length = _consume_all_letters (source.span);
                return true;
                
            case '\n':
                id = TokenId.NEWLINE;
                length = _consume_all_newline (source.span);
                return true;
                
        }
        
        id = default;
        length = default;
        return false;
        
        static int _consume_all_whitespace (System.Memory.ReadOnlySpan<char> buf)
        {
            int consumed = 0;
            while (buf.Length > consumed)
            {
                switch (buf[consumed])
                {
                    case ' ':
                    case '\t':
                    case '\f':
                    {
                        consumed++;
                        break;
                    }
                    default: return consumed;
                }
            }
            return consumed;
        }
        
        static int _consume_all_digits (System.Memory.ReadOnlySpan<char> buf)
        {
            int consumed = 0;
            while (buf.Length > consumed)
            {
                switch (buf[consumed])
                {
                    case >= '0' and <= '9':
                    {
                        consumed++;
                        break;
                    }
                    default: return consumed;
                }
            }
            return consumed;
        }
        
        static int _consume_all_letters (System.Memory.ReadOnlySpan<char> buf)
        {
            int consumed = 0;
            while (buf.Length > consumed)
            {
                switch (buf[consumed])
                {
                    case >= 'a' and <= 'z':
                    case >= 'A' and <= 'Z':
                    case '-':
                    case '_':
                    {
                        consumed++;
                        break;
                    }
                    default: return consumed;
                }
            }
            return consumed;
        }
        
        static int _consume_all_newline (System.Memory.ReadOnlySpan<char> buf)
        {
            int consumed = 0;
            while (buf.Length > consumed)
            {
                switch (buf[consumed])
                {
                    case '\n':
                    {
                        consumed++;
                        break;
                    }
                    default: return consumed;
                }
            }
            return consumed;
        }
        
    }
}
