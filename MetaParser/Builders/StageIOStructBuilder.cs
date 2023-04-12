using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders;
using static CodeCommon;

internal class StageIOStructBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new System.InvalidOperationException("Writer is null");

        writer.WriteLine($"namespace {context.Config.Namespace};");
        writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]");
        writer.WriteLine($"public readonly record struct {ConsumerResult}({context.Config.IdType} id, int length);");
        // Stage Input
        writer.WriteLine($"public readonly record struct {StageInput}<InputType, OutputType>({ReadOnlyMemory}<InputType> Inputs, {Memory}<OutputType> Outputs, {Memory}<int> Lengths);");
        // Stage Output
        writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]");
        writer.WriteLine($"public readonly record struct {StageOutput}(int TokenCount, int ConsumedCount);");
        // Stage Input Chunk
        writer.WriteLine($"public sealed class {StageInputChunk}<InputType, OutputType>");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("const int CHUNK_SIZE = 128;");
        writer.WriteLine($"private readonly {IMemoryOwner}<OutputType> outPtr;");
        writer.WriteLine($"private readonly {IMemoryOwner}<int> lenPtr;");
        writer.WriteLine();
        writer.WriteLine($"public readonly {ReadOnlyMemory}<InputType> Inputs;");
        writer.WriteLine($"public {Memory}<OutputType> Outputs => outPtr.Memory;");
        writer.WriteLine($"public {Memory}<int> Lengths => lenPtr.Memory;");
        writer.WriteLine();
        writer.WriteLine($"public {StageInputChunk} ({ReadOnlyMemory}<InputType> Input)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"Inputs = Input;");
        writer.WriteLine($"outPtr = {MemoryPool}<OutputType>.Shared.Rent(CHUNK_SIZE);");
        writer.WriteLine($"lenPtr = {MemoryPool}<int>.Shared.Rent(CHUNK_SIZE);");
        writer.WriteLine($"");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
        writer.WriteLine($"~{StageInputChunk}()");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"outPtr.Dispose();");
        writer.WriteLine($"lenPtr.Dispose();");
        writer.Indent--;
        writer.WriteLine("}");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
