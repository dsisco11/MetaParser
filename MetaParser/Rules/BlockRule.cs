using MetaParser.Tokens;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MetaParser.Rules
{
    [DebuggerDisplay("BlockRule :: {Definition} :: [{BlockStart.ToString()}] -> [{BlockEnd.ToString()}]")]
    public class BlockRule<TEnum, TValue> : ITokenRule<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(false, true);

        #region Properties
        public TValue[] BlockStart { get; init; }
        public TValue[] BlockEnd { get; init; }
        public TValue? EscapeDelimiter { get; init; }
        private TEnum Definition { get; init; }
        private TEnum Malformed { get; init; }
        #endregion

        #region Constructors
        public BlockRule(TEnum definition, TEnum malformed, ReadOnlySpan<TValue> blockStart, ReadOnlySpan<TValue> blockEnd)
        {
            BlockStart = blockStart.ToArray();
            BlockEnd = blockEnd.ToArray();
            Definition = definition;
            Malformed = malformed;
            EscapeDelimiter = null;
        }

        public BlockRule(TEnum definition, TEnum malformed, ReadOnlySpan<TValue> blockStart, TValue blockEnd, TValue? escapeDelimiter)
        {
            BlockStart = blockStart.ToArray();
            BlockEnd = new[] { blockEnd };
            Definition = definition;
            Malformed = malformed;
            EscapeDelimiter = escapeDelimiter;
        }

        public BlockRule(TEnum definition, TEnum malformed, TValue blockStart, TValue blockEnd, TValue? escapeDelimiter)
        {
            BlockStart = new[] { blockStart };
            BlockEnd = new[] { blockEnd };
            Definition = definition;
            Malformed = malformed;
            EscapeDelimiter = escapeDelimiter;
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            if (!rd.IsNext(BlockStart.AsSpan(), advancePast: true))
            {
                TokenType = default;
                TokenLength = default;
                return false;
            }

            if (EscapeDelimiter is not null)
            {// If we have an escaping delimiter set, then we can be sure that the block end consists of only a single value.
                // Skip forward until we find the block end sequence or we reach the stream end
                if (!rd.TryReadTo(out ReadOnlySpan<TValue> _, BlockEnd[0], delimiterEscape: EscapeDelimiter.Value, advancePastDelimiter: true))
                {// Consume everything thats left if we couldnt find it
                    rd.AdvanceToEnd();
                    TokenType = Malformed;
                }
                else
                {
                    TokenType = Definition;
                }
            }
            else
            {
                // Skip forward until we find the block end sequence or we reach the stream end
                if (!rd.TryReadTo(out ReadOnlySpan<TValue> _, BlockEnd.AsSpan(), advancePastDelimiter: true))
                {// Consume everything thats left if we couldnt find it
                    rd.AdvanceToEnd();
                    TokenType = Malformed;
                }
                else
                {
                    TokenType = Definition;
                }
            }

            TokenLength = rd.Consumed;
            return true;
        }
    }
}
