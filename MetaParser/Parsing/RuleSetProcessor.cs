using MetaParser.Rules;
using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics.Contracts;

namespace MetaParser
{
    public class RuleSetProcessor<TInput, TOutput>
        where TInput : unmanaged
        where TOutput : unmanaged, IEquatable<TOutput>
    {
        #region Properties
        public RuleSet<TInput, TOutput> Rules { get; init; }
        #endregion

        public RuleSetProcessor(RuleSet<TInput, TOutput> rules)
        {
            Rules = rules;
        }

        #region Parsing
        public ReadOnlyMemory<TokenValue<TInput, TOutput>> Process(ReadOnlySequence<TOutput> Data)
        {
            var Stream = new Tokenizer<TOutput>(Data);
            var arrayBuffer = new ArrayBufferWriter<TokenValue<TInput, TOutput>>(2048);
            TokenValue<TInput, TOutput>? last = null;
            do
            {
                var buffer = arrayBuffer.GetSpan(32);
                for (int i = 0; i < buffer.Length; i++)
                {
                    last = Consume_Next(Stream, last);
                    if (last is null)
                    {
                        break;
                    }
                    buffer[i] = last.Value;
                }

                arrayBuffer.Write(buffer);
            }
            while (!Stream.AtEnd);

            return arrayBuffer.WrittenMemory;
        }
        #endregion

        private TokenValue<TInput, TOutput>? Consume_Next(Tokenizer<TOutput> Stream, TokenValue<TInput, TOutput>? lastToken)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            Contract.EndContractBlock();

            if (Stream.AtEOF || Stream.AtEnd)
            {
                Stream.Consume(1);
                return null;
            }

            if (Try_Consume(Rules, Stream, lastToken, out var consumed))
            {
                return consumed;
            }

            // In the absence of any other matches, consume a default token
            return new TokenValue<TInput, TOutput>(default, Stream.Consume(1));
        }

        /// <summary>
        /// Attempts to consume a token using a specific rule-set
        /// </summary>
        /// <param name="Ruleset"></param>
        /// <param name="Tokenizer"></param>
        /// <param name="prevToken"></param>
        /// <returns></returns>
        private static bool Try_Consume(RuleSet<TInput, TOutput> Ruleset, ITokenizer<TOutput> Tokenizer, TokenValue<TInput, TOutput>? prevToken, out TokenValue<TInput, TOutput>? Result)
        {
            ArgumentNullException.ThrowIfNull(Ruleset);
            ArgumentNullException.ThrowIfNull(Tokenizer);
            Contract.EndContractBlock();

            foreach (ITokenRule<TInput, TOutput> rule in Ruleset.Items)
            {
                if (rule.TryConsume(Tokenizer, prevToken?.Type, out var outTokenType, out var outTokenLength))
                {
                    var consumed = Tokenizer.Consume(Index.FromStart((int)outTokenLength));
                    Result = new TokenValue<TInput, TOutput>(outTokenType, consumed);
                    return true;
                }
            }

            Result = null;
            return false;
        }
    }
}
