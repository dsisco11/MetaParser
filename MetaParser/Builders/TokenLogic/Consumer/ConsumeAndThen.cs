using MetaParser.CodeGen.Interfaces;
using MetaParser.Core;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal class ConsumeAndThen : IMetaCodeBuilder
    {
        public static ConsumeAndThen Instance = new ConsumeAndThen();

        public void WriteTo(MetaParserContext context)
        {
            var writer = context.writer;
            var consumer = context.Consumers.WorkingSet.Single();
#if DEBUG
            writer.WriteLine($"/*");
            writer.WriteLine($"* TokenID: {consumer.Token.Name} (#{consumer.Token.Index})");
            writer.WriteLine($"* ==[ CONSUMER_DATA ]==");
            writer.WriteLine($"* {consumer}");
            writer.WriteLine($"*/");
#endif

            if (consumer.Start is not null)
            {
#if DEBUG
                writer.WriteLine("/* Consume the START sequence which got us here in the first place, we already know its part of the token */");
#endif
                if (!consumer.Start.IsConstantLength)
                {
                    writer.WriteLine("/* WARNING: consumer START sequence is of uncertain length, it is possible this could cause token parsing discrepancies */");
                }
                writer.WriteLine($"var {MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMajor}.Slice({Math.Max(1, consumer.Start.MinLength)});");
            }

            // Check for escape sequence
            if (consumer.Stop is not null)
            {
                writer.WriteLine($"while ({MetaParserContext.VarNameBufferMinor}.Length > 0)");
                writer.WriteLine("{");
                writer.Indent++;

                // If we have an escape set, then check that
                if (consumer.Escape is not null)
                {
                    writer.Write("if (");
                    writer.Write($"{MetaParserContext.VarNameBufferMinor}");
                    ConsumerPatternMatcher.WriteTo(context, consumer.Escape, true, true);
                    writer.WriteLine(")");
                    writer.WriteLine("{");
                    writer.Indent++;
#if DEBUG
                    writer.WriteLine("/* look past the ESCAPE sequence */");
#endif
                    writer.WriteLine($"var {MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Slice({consumer.Escape.Length});");
#if DEBUG
                    writer.WriteLine("/* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */");
#endif
                    // Check STOP sequence
                    writer.Write("if (");
                    writer.Write($"{MetaParserContext.VarNameBufferLocal}");
                    ConsumerPatternMatcher.WriteTo(context, consumer.Stop, true, true);
                    writer.WriteLine(")");
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
                    writer.Write("if (");
                    writer.Write($"{MetaParserContext.VarNameBufferMinor}");
                    ConsumerPatternMatcher.WriteTo(context, consumer.Stop, true, true);
                    writer.WriteLine(")");
#if DEBUG
                    writer.WriteLine("/* If we have a STOP sequence, check for it */");
#endif
                    writer.WriteLine("{");
                    writer.Indent++;
#if DEBUG
                    writer.WriteLine("/* end */");
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
                writer.Write("if (");
                writer.Write($"{MetaParserContext.VarNameBufferMinor}");
                ConsumerPatternMatcher.WriteTo(context, consumer.Consume, true, true);
                writer.WriteLine(")");
#if DEBUG
                writer.WriteLine("/* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */");
#endif
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice({consumer.Consume.Length});");
#if DEBUG
                writer.WriteLine("/* consumer forces moving on to next loop */");
#endif
                writer.WriteLine("continue;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
#if DEBUG
                writer.WriteLine("/* otherwise, default behaviour is to exit loop */");
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
                    writer.WriteLine("/* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */");
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

                writer.Write("if (");
                writer.Write($"{MetaParserContext.VarNameBufferMinor}");
                ConsumerPatternMatcher.WriteTo(context, consumer.Stop, true, true);
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
