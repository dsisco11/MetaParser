//HintName: MetaParser.MetaParser.complex_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private static bool consume_constant_token(global::System.ReadOnlySpan<char> source, out byte id, out int length)
    {
        switch (source)
        {
            case [ "var", ..]:
            {
                return consume_1();
            }
            case [ "function", ..]:
            {
                return consume_2();
            }
            case [ "{", ..]:
            {
                return consume_3();
            }
            case [ "}", ..]:
            {
                return consume_4();
            }
            case [ "*", ..]:
            {
                return consume_5();
            }
            case [ "/", ..]:
            {
                return consume_6();
            }
            case [ "\\", ..]:
            {
                return consume_7();
            }
            case [ " " or "\t" or "\f", ..]:
            {
                return consume_8();
            }
            case [ (>='0' and <='9'), ..]:
            {
                return consume_9();
            }
            case [ (>='a' and <='z') or (>='A' and <='Z'), ..]:
            {
                return consume_10();
            }
            case [ "\n", ..]:
            {
                return consume_11();
            }
        }
        id = default;
        length = default;
        return false;
        
        bool consume_1()
        {
            var start = source;
            id = TokenId.Keyword_Var;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_2()
        {
            var start = source;
            id = TokenId.Keyword_Function;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_3()
        {
            var start = source;
            id = TokenId.Char_Open_Bracket;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_4()
        {
            var start = source;
            id = TokenId.Char_Close_Bracket;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_5()
        {
            var start = source;
            id = TokenId.Char_Asterisk;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_6()
        {
            var start = source;
            id = TokenId.Char_Solidus;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_7()
        {
            var start = source;
            id = TokenId.Char_Reverse_Solidus;
            buffer = start.Slice(1);
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_8()
        {
            var start = source;
            id = TokenId.Whitespace;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ " " or "\t" or "\f"}))
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_9()
        {
            var start = source;
            id = TokenId.Digits;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer is [ (>='0' and <='9'), ..])
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_10()
        {
            var start = source;
            id = TokenId.Letters;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer is [ (>='a' and <='z') or (>='A' and <='Z'), ..])
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_11()
        {
            var start = source;
            id = TokenId.Newline;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ "\n"}))
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
            length = start.Length - buffer.Length;
            return true;
        }
    }
}
