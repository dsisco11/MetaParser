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
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_2()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_3()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_4()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_5()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_6()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_7()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_8()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
            
            while (buffer.Length > 0)
            {
                if (buffer is [ " " or "\t" or "\f", ..])
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_9()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
            
            while (buffer.Length > 0)
            {
                if (buffer .StartsWith(stackalloc []{ (>='0' and <='9')}))
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_10()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
            
            while (buffer.Length > 0)
            {
                if (buffer .StartsWith(stackalloc []{ (>='a' and <='z') or (>='A' and <='Z')}))
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
        }
        
        consumed = start.Length - buffer.Length;
        }
        bool consume_11()
        {
        var buffer = start.Slice(1);
        
        while (buffer.Length > 0)
        {
            
            while (buffer.Length > 0)
            {
                if (buffer is [ "\n", ..])
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
        }
        
        consumed = start.Length - buffer.Length;
        }
    }
}
