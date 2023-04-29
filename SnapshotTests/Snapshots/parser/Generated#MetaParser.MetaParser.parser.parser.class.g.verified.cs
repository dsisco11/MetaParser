//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser;
public sealed partial class Parser
{
    public RedGreenTree Parse(global::System.ReadOnlyMemory<char> buffer0)
    {
        StageInputChunk<char, byte> input0 = new(buffer0);
        var output0 = Execute_Parsing_Table_0(input0);
        var stage_0_length = output0.TokenCount;
        var stage_0_nodes = new GreenNode[stage_0_length];
        int stage_0_node_tracker = 0;
        for (int i=0; i<stage_0_length; i++)
        {
            GreenNode token;
            var tokenId = input0.Output.Span[i];
            var tokenWidth = input0.Length.Span[i];
            var tokenData = input0.Input.Slice(stage_0_node_tracker, tokenWidth);
            token = new LexerNode(tokenId, tokenData);
            stage_0_nodes[i] = token;
            stage_0_node_tracker += tokenWidth;
        }
        
        var buffer1 = input0.Output.Slice(0, output0.TokenCount);
        StageInputChunk<byte, byte> input2 = new(buffer1);
        var output2 = Execute_Parsing_Table_1(input2);
        var stage_1_length = output2.TokenCount;
        var stage_1_nodes = new GreenNode[stage_1_length];
        int stage_1_node_tracker = 0;
        for (int i=0; i<stage_1_length; i++)
        {
            GreenNode token;
            var tokenId = input2.Output.Span[i];
            var tokenWidth = input2.Length.Span[i];
            if (tokenId != 0)
            {
                var children = new global::System.ArraySegment<GreenNode>(stage_0_nodes, stage_1_node_tracker, tokenWidth);
                token = new SyntaxNode(tokenId, children.Array);
            }
            else
            {
                token = stage_0_nodes[stage_1_node_tracker];
            }
            stage_1_nodes[i] = token;
            stage_1_node_tracker += tokenWidth;
        }
        
        var buffer3 = input2.Output.Slice(0, output2.TokenCount);
        StageInputChunk<byte, byte> input4 = new(buffer3);
        var output4 = Execute_Parsing_Table_2(input4);
        var stage_2_length = output4.TokenCount;
        var stage_2_nodes = new GreenNode[stage_2_length];
        int stage_2_node_tracker = 0;
        for (int i=0; i<stage_2_length; i++)
        {
            GreenNode token;
            var tokenId = input4.Output.Span[i];
            var tokenWidth = input4.Length.Span[i];
            if (tokenId != 0)
            {
                var children = new global::System.ArraySegment<GreenNode>(stage_1_nodes, stage_2_node_tracker, tokenWidth);
                token = new SyntaxNode(tokenId, children.Array);
            }
            else
            {
                token = stage_1_nodes[stage_2_node_tracker];
            }
            stage_2_nodes[i] = token;
            stage_2_node_tracker += tokenWidth;
        }
        
        var buffer5 = input4.Output.Slice(0, output4.TokenCount);
        var rootNode = new SyntaxNode(ETokenType.Unknown, stage_2_nodes);
        return new RedGreenTree(rootNode);
        
    }
    private static StageOutput Execute_Parsing_Table_0(StageInputChunk<char, byte> buffer0)
    {
        var buffer1 = buffer0.Input;
        var buffer2 = buffer1.Span;
        int inIndex = 0;
        int outIndex = 0;
        var outId = buffer0.Output;
        var outLength = buffer0.Length;
        
        while (buffer2.Length > 0)
        {
            var processed = process_parser_table_0(buffer2);
            var consumed = buffer1.Slice(0, processed.advance);
            if (processed.success)
            {
                outId.Span[outIndex] = processed.tokenid;
                outLength.Span[outIndex] = processed.advance;
                outIndex++;
            }
            if (processed.tokenid != default)
            {
                if (buffer1.Length != buffer2.Length)
                {
                    var unk_content_size = buffer1.Length - buffer2.Length;
                    outId.Span[outIndex] = TokenId.Unknown;
                    outLength.Span[outIndex] = unk_content_size;
                    inIndex += unk_content_size;
                    outIndex++;
                    buffer1 = buffer1.Slice(unk_content_size);
                    buffer2 = buffer1.Span;
                }
                
                inIndex += processed.advance;
                buffer1 = buffer1.Slice(processed.advance);
                buffer2 = buffer1.Span;
            }
            else
            {
                buffer2 = buffer2.Slice(1);
            }
        }
        
        if (buffer1.Length != buffer2.Length)
        {
            var unk_content_size = buffer1.Length - buffer2.Length;
            outId.Span[outIndex] = TokenId.Unknown;
            outLength.Span[outIndex] = unk_content_size;
            inIndex += unk_content_size;
            outIndex++;
            buffer1 = buffer1.Slice(unk_content_size);
            buffer2 = buffer1.Span;
        }
        
        return new (outIndex, inIndex);
    }
    private static StageOutput Execute_Parsing_Table_1(StageInputChunk<byte, byte> buffer0)
    {
        var buffer1 = buffer0.Input;
        var buffer2 = buffer1.Span;
        int inIndex = 0;
        int outIndex = 0;
        var outId = buffer0.Output;
        var outLength = buffer0.Length;
        
        while (buffer2.Length > 0)
        {
            var processed = process_parser_table_1(buffer2);
            var consumed = buffer1.Slice(0, processed.advance);
            if (processed.success)
            {
                outId.Span[outIndex] = processed.tokenid;
                outLength.Span[outIndex] = processed.advance;
                outIndex++;
            }
            if (processed.tokenid != default)
            {
                if (buffer1.Length != buffer2.Length)
                {
                    var unk_content_size = buffer1.Length - buffer2.Length;
                    outId.Span[outIndex] = TokenId.Unknown;
                    outLength.Span[outIndex] = unk_content_size;
                    inIndex += unk_content_size;
                    outIndex++;
                    buffer1 = buffer1.Slice(unk_content_size);
                    buffer2 = buffer1.Span;
                }
                
                inIndex += processed.advance;
                buffer1 = buffer1.Slice(processed.advance);
                buffer2 = buffer1.Span;
            }
            else
            {
                buffer2 = buffer2.Slice(1);
            }
        }
        
        if (buffer1.Length != buffer2.Length)
        {
            var unk_content_size = buffer1.Length - buffer2.Length;
            outId.Span[outIndex] = TokenId.Unknown;
            outLength.Span[outIndex] = unk_content_size;
            inIndex += unk_content_size;
            outIndex++;
            buffer1 = buffer1.Slice(unk_content_size);
            buffer2 = buffer1.Span;
        }
        
        return new (outIndex, inIndex);
    }
    private static StageOutput Execute_Parsing_Table_2(StageInputChunk<byte, byte> buffer0)
    {
        var buffer1 = buffer0.Input;
        var buffer2 = buffer1.Span;
        int inIndex = 0;
        int outIndex = 0;
        var outId = buffer0.Output;
        var outLength = buffer0.Length;
        
        while (buffer2.Length > 0)
        {
            var processed = process_parser_table_2(buffer2);
            var consumed = buffer1.Slice(0, processed.advance);
            if (processed.success)
            {
                outId.Span[outIndex] = processed.tokenid;
                outLength.Span[outIndex] = processed.advance;
                outIndex++;
            }
            if (processed.tokenid != default)
            {
                if (buffer1.Length != buffer2.Length)
                {
                    var unk_content_size = buffer1.Length - buffer2.Length;
                    outId.Span[outIndex] = TokenId.Unknown;
                    outLength.Span[outIndex] = unk_content_size;
                    inIndex += unk_content_size;
                    outIndex++;
                    buffer1 = buffer1.Slice(unk_content_size);
                    buffer2 = buffer1.Span;
                }
                
                inIndex += processed.advance;
                buffer1 = buffer1.Slice(processed.advance);
                buffer2 = buffer1.Span;
            }
            else
            {
                buffer2 = buffer2.Slice(1);
            }
        }
        
        if (buffer1.Length != buffer2.Length)
        {
            var unk_content_size = buffer1.Length - buffer2.Length;
            outId.Span[outIndex] = TokenId.Unknown;
            outLength.Span[outIndex] = unk_content_size;
            inIndex += unk_content_size;
            outIndex++;
            buffer1 = buffer1.Slice(unk_content_size);
            buffer2 = buffer1.Span;
        }
        
        return new (outIndex, inIndex);
    }
}
