using System;

using Json.Schema;

namespace MetaTranspiler
{
    class JsonDebugLogger : ILog
    {
        public void Write(Func<string> message, int indent = 0)
        {
            var msg = message();
            System.Diagnostics.Debug.IndentLevel = indent;
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
