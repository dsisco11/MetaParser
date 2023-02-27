using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal class ConsumeAndThen : IMetaCodeBuilder
    {
        public static ConsumeAndThen Instance = new ConsumeAndThen();

        public void WriteTo(MetaParserContext context)
        {
            var consumer = context.Tokens.WorkingSet.Single();
            var writer = context.writer;

            if (consumer.Start is not null)
            {
                writer.WriteLine($"var {MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMajor}.Slice({consumer.Start.Length});");// Skip ahead of the token {VarStart}
            }

            if (consumer.Stop is not null)
            {
                writer.WriteLine($"while ({MetaParserContext.VarNameBufferMinor}.Length > 0)");
                writer.WriteLine("{");
                writer.Indent++;

                // If we have an escape set, then check that
                if (consumer.Escape is not null && consumer.Stop is not null)
                {
                    writer.Write($"if ({MetaParserContext.VarNameBufferMinor}");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Escape);
                    writer.WriteLine(")");
                    writer.WriteLine("{");
                    writer.Indent++;
#if DEBUG
                    writer.WriteLine("/* look past the escape sequence */");
#endif
                    writer.WriteLine($"var {MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Slice({consumer.Escape.Length});");
                    writer.Write($"if ({MetaParserContext.VarNameBufferLocal} ");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                    writer.WriteLine(")");
#if DEBUG
                    writer.WriteLine("/* if the stop sequence immediately follows the escape sequence then they are consumed */");
#endif
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferLocal}.Slice({consumer.Stop.Length});");
                    writer.WriteLine($"continue;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                }

                if (consumer.Stop is not null)
                {
                    writer.Write($"if ({MetaParserContext.VarNameBufferMinor}");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                    writer.WriteLine(")");
#if DEBUG
                    writer.WriteLine("/* If we have a stop sequence, check for it */");
#endif
                    writer.WriteLine("{");
                    writer.Indent++;
#if DEBUG
                    writer.WriteLine("/* end consumption */");
#endif
                    writer.WriteLine("break;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }

            if (consumer.Consume is not null)
            {
                writer.WriteLine();
                writer.WriteLine($"while ({MetaParserContext.VarNameBufferMinor}.Length > 0)");
                writer.WriteLine("{");
                writer.Indent++;
                writer.Write($"if ({MetaParserContext.VarNameBufferMinor}");
                ConsumerSpanSeqBuilder.WriteTo(context, consumer.Consume);
                writer.WriteLine(")");
#if DEBUG
                writer.WriteLine("/* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */");
#endif
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice({consumer.Consume.Length});");
#if DEBUG
                writer.WriteLine("// consumer forces moving on to next loop");
#endif
                writer.WriteLine("continue;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
#if DEBUG
                writer.WriteLine("// otherwise, default behaviour is to stop looping");
#endif
                writer.WriteLine("break;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
            }
            else// If the token doesnt specify a specific set of valid items to consume, then all items are valid
            {
                if (consumer.Stop is not null)// To avoid infinite loops we just do this sanity check here to make sure we dont produce conditionless itteration
                {
#if DEBUG
                    writer.WriteLine("// Token doesn't specify any consumables, thus ALL items are considered valid consumables");
#endif
                    writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice(1);");
                }
            }

            // If we have a token terminator (end sequence) set...
            // The check that it is present, if not then token consumption fails as the end terminator sequence is required when specified
            if (consumer.Stop is not null)
            {
                writer.Indent--;
                writer.WriteLine("}");// end while loop
                writer.WriteLine();

                writer.Write($"if ({MetaParserContext.VarNameBufferMinor} ");
                ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                writer.WriteLine(")");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"length = {consumer.Stop.Length} + ({MetaParserContext.VarNameBufferMajor}.Length - {MetaParserContext.VarNameBufferMinor}.Length);");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                writer.WriteLine("length = default;");
                writer.WriteLine("return false;");
            }
            else
            {
                writer.WriteLine($"length = {MetaParserContext.VarNameBufferMajor}.Length - {MetaParserContext.VarNameBufferMinor}.Length;");
                writer.WriteLine("return true;");
            }

        }


    }
}
