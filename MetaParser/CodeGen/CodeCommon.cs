using Microsoft.CodeAnalysis;

using System;

namespace MetaParser.CodeGen
{
    internal static class CodeCommon
    {
        public const string List = "global::System.Collections.Generic.List";
        public const string Span = "global::System.Span";
        public const string Memory = "global::System.Memory";

        public const string ReadOnlySpan = "global::System.ReadOnlySpan";
        public const string ReadOnlyMemory = "global::System.ReadOnlyMemory";

        public static string FormatMemoryBuffer(SpecialType type) => $"global::System.Memory<{Format(type)}>";
        public static string FormatSpanBuffer(SpecialType type) => $"global::System.Span<{Format(type)}>";

        public static string FormatReadOnlyMemoryBuffer(SpecialType type) => $"global::System.ReadOnlyMemory<{Format(type)}>";
        public static string FormatReadOnlySpanBuffer(SpecialType type) => $"global::System.ReadOnlySpan<{Format(type)}>";

        public static string Format(SpecialType type)
        {
            return type switch
            {
                SpecialType.System_SByte => "sbyte",
                SpecialType.System_Byte => "byte",
                SpecialType.System_Char => "char",
                SpecialType.System_Int16 => "short",
                SpecialType.System_Int32 => "int",
                SpecialType.System_Int64 => "long",
                SpecialType.System_UInt16 => "ushort",
                SpecialType.System_UInt32 => "uint",
                SpecialType.System_UInt64 => "ulong",
                SpecialType.System_Single => "float",
                SpecialType.System_Double => "double",
                SpecialType.System_String => "string",
                SpecialType.System_Void => "void",
                SpecialType.System_Boolean => "bool",
                _ => throw new NotImplementedException()
            };
        }
    }
}
