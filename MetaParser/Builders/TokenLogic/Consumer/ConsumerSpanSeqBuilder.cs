using MetaParser.Contexts;
using MetaParser.Patternization;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal static class ConsumerSpanSeqBuilder
    {
        public static void WriteTo(MetaParserContext context, Pattern pattern)
        {
            var wr = context.writer;
            if (pattern.IsConstant)
            {
                wr.Write(".StartsWith(");
                wr.Write("stackalloc []{ ");
                wr.Write(Translate(context, pattern));
                wr.Write("})");
            }
            else
            {
                wr.Write("is [ ");
                wr.Write(Translate(context, pattern));
                wr.Write(", ..]");
            }
        }

        private static string Translate(MetaParserContext context, Pattern pattern)
        {
            return pattern switch
            {
                PatternConst c => c.value,
                PatternRange r => $"(>={r.begin} and <={r.end})",
                PatternGroup g => string.Join(g.ConditionJoiner, g.items.Select(o => Translate(context, o))),
                PatternEmpty _ => string.Empty,
                _ => throw new NotImplementedException()
            };
        }
    }
}
