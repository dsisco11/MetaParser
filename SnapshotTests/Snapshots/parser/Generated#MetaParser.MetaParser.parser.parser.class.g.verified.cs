//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> buffer0)
        {
            StageInputChunk<char, byte> input0 = new(buffer0);
            var output0 = Execute_Parsing_Table_0(input0);
            StageInputChunk<byte, byte> input1 = new(input0.Outputs);
            var output1 = Execute_Parsing_Table_1(input1);
            StageInputChunk<byte, byte> input2 = new(input1.Outputs);
            var output2 = Execute_Parsing_Table_2(input2);
            return Array.Empty<Token>();
            
        }
        private static StageOutput Execute_Parsing_Table_0(StageInputChunk<char, byte> buffer0)
        {
                var buffer1 = buffer0.Inputs;
                var buffer2 = buffer1.Span;
                int inIndex = 0;
                int outIndex = 0;
                var outId = buffer0.Outputs;
                var outLength = buffer0.Lengths;
                
                while (buffer2.Length > 0)
                {
                    var processed = process_parser_table_0(buffer2);
                    if (processed.length != default)
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
                        
                        var consumed = buffer1.Slice(0, processed.length);
                        outId.Span[outIndex] = processed.id;
                        outLength.Span[outIndex] = processed.length;
                        inIndex += processed.length;
                        outIndex++;
                        buffer1 = buffer1.Slice(processed.length);
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
                    var buffer1 = buffer0.Inputs;
                    var buffer2 = buffer1.Span;
                    int inIndex = 0;
                    int outIndex = 0;
                    var outId = buffer0.Outputs;
                    var outLength = buffer0.Lengths;
                    
                    while (buffer2.Length > 0)
                    {
                        var processed = process_parser_table_1(buffer2);
                        if (processed.length != default)
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
                            
                            var consumed = buffer1.Slice(0, processed.length);
                            outId.Span[outIndex] = processed.id;
                            outLength.Span[outIndex] = processed.length;
                            inIndex += processed.length;
                            outIndex++;
                            buffer1 = buffer1.Slice(processed.length);
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
                        var buffer1 = buffer0.Inputs;
                        var buffer2 = buffer1.Span;
                        int inIndex = 0;
                        int outIndex = 0;
                        var outId = buffer0.Outputs;
                        var outLength = buffer0.Lengths;
                        
                        while (buffer2.Length > 0)
                        {
                            var processed = process_parser_table_2(buffer2);
                            if (processed.length != default)
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
                                
                                var consumed = buffer1.Slice(0, processed.length);
                                outId.Span[outIndex] = processed.id;
                                outLength.Span[outIndex] = processed.length;
                                inIndex += processed.length;
                                outIndex++;
                                buffer1 = buffer1.Slice(processed.length);
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
            }
