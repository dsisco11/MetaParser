using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics.Contracts;

namespace MetaParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData">Value type of the output tokens identifier struct</typeparam>
    /// <typeparam name="TValue">Value type of the data being parsed</typeparam>
    public class Parser<TData, TValue> : IDisposable
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Properties
        private readonly ParsingConfig<TData, TValue> config;
        private bool disposedValue;
        #endregion

        #region Properties
        public ParsingConfig<TData, TValue> Config => config;
        #endregion

        #region Constructors
        public Parser(ParsingConfig<TData, TValue> configuration)
        {
            config = configuration;
        }
        #endregion

        #region Parsing
        public Token<TData, TValue>[] Parse(ReadOnlyMemory<TValue> Data)
        {
            var Stream = new Tokenizer<TValue>(Data);
            var tokenList = new LinkedList<Token<TData, TValue>>();
            Token<TData, TValue>? last = null;
            do
            {
                last = Consume_Next(Stream, last);
                tokenList.AddLast(last);
            }
            while (!Stream.AtEnd);

            return tokenList.ToArray();
        }
        #endregion

        private Token<TData, TValue>? Consume_Next(Tokenizer<TValue> Stream, Token<TData, TValue>? lastToken)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            Contract.EndContractBlock();

            if (Stream.AtEOF || Stream.AtEnd)
            {
                Stream.Consume(1);
                return null;
            }

            foreach (var ruleset in Config.Rulesets)
            {
                if (Try_Consume(ruleset, Stream, lastToken, out var consumed))
                {
                    return consumed;
                }
            }

            // In the absence of any other matches, consume a default token
            return new Token<TData, TValue>(default(TData), Stream.Consume(1));
        }

        /// <summary>
        /// Attempts to consume a token using a specific rule-set
        /// </summary>
        /// <param name="Ruleset"></param>
        /// <param name="Tokenizer"></param>
        /// <param name="prevToken"></param>
        /// <returns></returns>
        private static bool Try_Consume(RuleSet<TData, TValue> Ruleset, ITokenizer<TValue> Tokenizer, Token<TData, TValue>? prevToken, out Token<TData, TValue>? Result)
        {
            ArgumentNullException.ThrowIfNull(Ruleset);
            ArgumentNullException.ThrowIfNull(Tokenizer);
            Contract.EndContractBlock();

            foreach (ITokenRule<TData, TValue> rule in Ruleset.Items)
            {
                if (rule.TryConsume(Tokenizer, prevToken, out var outConsumed))
                {
                    Result = outConsumed;
                    return true;
                }
            }

            Result = null;
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Parser()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
