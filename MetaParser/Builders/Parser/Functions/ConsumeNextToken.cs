using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.Builders.Parser.Functions
{
    internal class ConsumeNextToken : IMetaCodeBuilder
    {
        public const string Name = "TryConsume";

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;

            wr.WriteLine($"private static bool {Name}({CodeCommon.FormatReadOnlySpanBuffer(context.InputType)} source, out {context.IdTypeName} Id, out {CodeCommon.Format(SpecialType.System_Int32)} Length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"if ({context.ConstantTokenConsumerFunctionName}(source, out var constId, out var constLen))");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("Id = constId;");
            wr.WriteLine("Length = constLen;");
            wr.WriteLine("return true;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine($"else if ({context.CompoundTokenConsumerFunctionName}(source, out var compId, out var compLen))");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("Id = compId;");
            wr.WriteLine("Length = compLen;");
            wr.WriteLine("return true;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            wr.WriteLine("Id = default;");
            wr.WriteLine("Length = default;");
            wr.WriteLine("return false;");

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
