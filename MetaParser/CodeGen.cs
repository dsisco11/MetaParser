using System;

namespace MetaParser
{
    internal static class CodeGen
    {
        public static string FormatMemoryBuffer(Type t) => $"global::System.Memory<{t.FullName}>";
        public static string FormatSpanBuffer(Type t) => $"global::System.Span<{t.FullName}>";

        public static string FormatReadOnlyMemoryBuffer(Type t) => $"global::System.ReadOnlyMemory<{t.FullName}>";
        public static string FormatReadOnlySpanBuffer(Type t) => $"global::System.ReadOnlySpan<{t.FullName}>";
    }
}
