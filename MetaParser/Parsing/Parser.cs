using MetaParser.Parsing;
using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;

using System.Buffers;
using System.Diagnostics.Contracts;

namespace MetaParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Ty">Value type of the stuff being parsed</typeparam>
    public class Parser<Ty> : IDisposable
        where Ty : unmanaged, IEquatable<Ty>
    {
        #region Properties
        public readonly ParsingConfig<Ty> Config;
        private bool disposedValue;
        #endregion

        #region Constructors
        public Parser(ParsingConfig<Ty> configuration)
        {
            Config = configuration;
        }
        #endregion

        #region Parsing
        public IToken<Ty>[] Parse(ReadOnlyMemory<Ty> Data)
        {
            var Stream = new Tokenizer<Ty>(Data);
            var tokenList = new LinkedList<IToken<Ty>>();
            IToken<Ty> last = Config.EOF_TOKEN;
            do
            {
                last = Consume_Next(Stream, last);
                tokenList.AddLast(last);
            }
            while (!Stream.AtEnd);

            return tokenList.ToArray();
        }
        #endregion

        private IToken<Ty> Consume_Next(Tokenizer<Ty> Stream, IToken<Ty> lastToken)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            Contract.EndContractBlock();

            if (Stream.AtEOF || Stream.AtEnd)
            {
                Stream.Consume(1);
                return Config.EOF_TOKEN;
            }

            foreach (var ruleset in Config.Rulesets)
            {
                if (Try_Consume(ruleset, Stream, lastToken, out var consumed))
                {
                    return consumed ?? Config.EOF_TOKEN;
                }
            }

            // In the absence of any other matches, consume a default token
            return new Token<Ty>(Stream.Consume(1));
        }

        /// <summary>
        /// Attempts to consume a token using a specific rule-set
        /// </summary>
        /// <param name="Ruleset"></param>
        /// <param name="Tokenizer"></param>
        /// <param name="prevToken"></param>
        /// <returns></returns>
        private static bool Try_Consume(TokenRuleSet<Ty> Ruleset, ITokenizer<Ty> Tokenizer, IToken<Ty> prevToken, out IToken<Ty>? Result)
        {
            ArgumentNullException.ThrowIfNull(Ruleset);
            ArgumentNullException.ThrowIfNull(Tokenizer);
            Contract.EndContractBlock();

            foreach (ITokenRule<Ty> rule in Ruleset.Items)
            {
                if (rule.Check(Tokenizer, prevToken))
                {
                    Result = rule.Consume(Tokenizer, prevToken);
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
