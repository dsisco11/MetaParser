//HintName: MetaParser.MetaParser.parser.parser.structs.g.cs
namespace UnitTestParser;
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
public readonly record struct ConsumerResult(byte tokenid, int advance, bool success = true);
public readonly record struct StageInput<InputType, OutputType>(global::System.ReadOnlyMemory<InputType> Input, global::System.Memory<OutputType> Output, global::System.Memory<int> Length);
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
public readonly record struct StageOutput(int TokenCount, int ConsumedCount);
public sealed class StageInputChunk<InputType, OutputType>
{
    const int CHUNK_SIZE = 128;
    private readonly global::System.Buffers.IMemoryOwner<OutputType> outPtr;
    private readonly global::System.Buffers.IMemoryOwner<int> lenPtr;
    
    public readonly global::System.ReadOnlyMemory<InputType> Input;
    public global::System.Memory<OutputType> Output => outPtr.Memory;
    public global::System.Memory<int> Length => lenPtr.Memory;
    
    public StageInputChunk (global::System.ReadOnlyMemory<InputType> input)
    {
        Input = input;
        outPtr = global::System.Buffers.MemoryPool<OutputType>.Shared.Rent(CHUNK_SIZE);
        lenPtr = global::System.Buffers.MemoryPool<int>.Shared.Rent(CHUNK_SIZE);
        
    }
    
    ~StageInputChunk()
    {
        outPtr.Dispose();
        lenPtr.Dispose();
    }
}
