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
            var wr = context.writer;

            //wr.WriteLine($"static bool {context.Get_Token_Consumer_Function_Name(token.Name)} ({CodeCommon.FormatReadOnlySpanBuffer(context.IdType)} start, out {CodeCommon.Format(SpecialType.System_Int32)} consumed)");
            //wr.WriteLine("{");
            //wr.Indent++;

            if (consumer.Start is not null)
            {
                wr.WriteLine($"var buffer = start.Slice({consumer.Start.Length});");// Skip ahead of the token start
            }

            wr.WriteLine();

            if (consumer.Stop is not null)
            {
                wr.WriteLine("while (buffer.Length > 0)");
                wr.WriteLine("{");
                wr.Indent++;

                // If we have an escape set, then check that
                if (consumer.Escape is not null && consumer.Stop is not null)
                {
                    wr.Write("if (buffer ");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Escape);
                    wr.WriteLine(")");
                    wr.WriteLine("{");
                    wr.Indent++;
#if DEBUG
                    wr.WriteLine("/* look past the escape sequence */");
#endif
                    wr.WriteLine($"var lookahead = buffer.Slice({consumer.Escape.Length});");
                    wr.Write("if (lookahead ");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                    wr.WriteLine(")");
#if DEBUG
                    wr.WriteLine("/* if the stop sequence immediately follows the escape sequence then they are consumed */");
#endif
                    wr.WriteLine("{");
                    wr.Indent++;
                    wr.WriteLine($"buffer = lookahead.Slice({consumer.Stop.Length});");
                    wr.WriteLine($"continue;");
                    wr.Indent--;
                    wr.WriteLine("}");
                    wr.Indent--;
                    wr.WriteLine("}");
                    wr.WriteLine();
                }

                if (consumer.Stop is not null)
                {
                    wr.Write($"if (buffer ");
                    ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                    wr.WriteLine(")");
#if DEBUG
                    wr.WriteLine("/* If we have a stop sequence, check for it */");
#endif
                    wr.WriteLine("{");
                    wr.Indent++;
#if DEBUG
                    wr.WriteLine("/* end consumption */");
#endif
                    wr.WriteLine("break;");
                    wr.Indent--;
                    wr.WriteLine("}");
                    wr.WriteLine();
                }
            }

            if (consumer.Consume is not null)
            {
                wr.WriteLine();
                wr.WriteLine("while (buffer.Length > 0)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.Write($"if (buffer ");
                ConsumerSpanSeqBuilder.WriteTo(context, consumer.Consume);
                wr.WriteLine(")");
#if DEBUG
                wr.WriteLine("/* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */");
#endif
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"buffer = buffer.Slice({consumer.Consume.Length});");
#if DEBUG
                wr.WriteLine("// consumer forces moving on to next loop");
#endif
                wr.WriteLine("continue;");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
#if DEBUG
                wr.WriteLine("// otherwise, default behaviour is to stop looping");
#endif
                wr.WriteLine("break;");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
            }
            else// If the token doesnt specify a specific set of valid items to consume, then all items are valid
            {
                if (consumer.Stop is not null)// To avoid infinite loops we just do this sanity check here to make sure we dont produce conditionless itteration
                {
#if DEBUG
                    wr.WriteLine("// Token doesn't specify any consumables, thus ALL items are considered valid consumables");
#endif
                    wr.WriteLine("buffer = buffer.Slice(1);");
                }
            }

            // If we have a token terminator (end sequence) set...
            // The check that it is present, if not then token consumption fails as the end terminator sequence is required when specified
            if (consumer.Stop is not null)
            {
                wr.Indent--;
                wr.WriteLine("}");// end while loop
                wr.WriteLine();

                wr.Write("if (buffer ");
                ConsumerSpanSeqBuilder.WriteTo(context, consumer.Stop);
                wr.WriteLine(")");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"consumed = {consumer.Stop.Length} + (start.Length - buffer.Length);");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
                wr.WriteLine("consumed = default;");
            }
            else
            {
                wr.WriteLine("consumed = start.Length - buffer.Length;");
            }
        }


    }
}
