using MetaParser.Rules;
using MetaParser.Tokens;

using System.Buffers;

namespace MetaParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnum">Value type of the output tokens identifier struct</typeparam>
    /// <typeparam name="TValue">Value type of the data being parsed</typeparam>
    public class Parser<TEnum, TValue> : IDisposable
        where TEnum : unmanaged, IEquatable<TEnum>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Properties
        private readonly RuleSetProcessor<TEnum, TValue> preprocessor;
        private readonly RuleSetProcessor<TEnum, TEnum>[] processors;
        private bool disposedValue;
        #endregion

        #region Constructors

        public Parser(RuleSet<TEnum, TValue> preprocessor, params RuleSet<TEnum, TEnum>[] processors)
        {
            this.preprocessor = new (preprocessor);
            this.processors = processors.Select(x => new RuleSetProcessor<TEnum, TEnum>(x)).ToArray();
        }

        public Parser(RuleSetProcessor<TEnum, TValue> preprocessor, params RuleSetProcessor<TEnum, TEnum>[] processors)
        {
            this.preprocessor = preprocessor;
            this.processors = processors;
        }
        #endregion

        #region Parsing
        public ReadOnlyMemory<TokenValue<TEnum, TEnum>> Parse(ReadOnlyMemory<TValue> Data)
        {
            return Parse(new ReadOnlySequence<TValue>(Data));
        }

        public ReadOnlyMemory<TokenValue<TEnum, TEnum>> Parse(ReadOnlySequence<TValue> Data)
        {
            var rootSeq = preprocessor.Process(Data);
            var layers = new LinkedList<ReadOnlyMemory<TokenValue<TEnum, TEnum>>>();
            ReadOnlyMemory<TEnum> readLayer = CompileSequence(rootSeq.Span);
            foreach (RuleSetProcessor<TEnum, TEnum> processor in processors)
            {
                using (readLayer.Pin())
                {
                    var nextLayer = processor.Process(new(readLayer));
                    layers.AddLast(nextLayer);
                    using (nextLayer.Pin())
                    {
                        readLayer = CompileSequence(nextLayer.Span);
                    }
                }
            }

            if (layers.Count <= 0 || layers.Last is null)
            {
                return default;
            }

            return layers.Last!.Value;
        }

        private Memory<Tin> CompileSequence<Tin, Tout>(ReadOnlySpan<TokenValue<Tin, Tout>> buffer)
            where Tin: unmanaged//, IEquatable<Tin>
            where Tout: unmanaged, IEquatable<Tout>
        {
            var memory = new Memory<Tin>(new Tin[buffer.Length]);
            var result = memory.Span;
            for (int i = 0; i < buffer.Length; i++)
            {
                result[i] = buffer[i].Type;
            }

            return memory;
        }
        #endregion

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
