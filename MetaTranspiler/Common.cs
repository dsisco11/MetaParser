using Json.Schema;

using Microsoft.CodeAnalysis;

using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace MetaTranspiler
{
    public static class Common
    {
        public const string PartialParserClassDeclaration = @"public partial class Parser : MetaParser.Parser<char>";
        public const string MetaParserFileExtension = ".parser-meta.json";
        public static readonly ValidationOptions SchemaOptions = new()
        {
            OutputFormat = OutputFormat.Detailed,
            //Log = new JsonDebugLogger()
        };
        public static JsonSchema ParsingSchema => JsonSchema.FromStream(Get_Parser_Schema_Stream(), JsonSerializerOptions.Default).Result;

        internal static string Get_FileName(ReadOnlyMemory<char> str) => str[..^MetaParserFileExtension.Length].ToString();
        internal static string Get_TokenId_Constant(string name) => $"TokenId.{Get_TokenId_Constant_Name(name)}";
        internal static string Get_TokenId_Constant_Name(string name) => name.ToUpperInvariant();
        internal static string Get_TokenId_Enum_Name(string name) => System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());

        internal static string IndentLines(int indentLevels, string content)
        {
            var indentStr = new string('\t', indentLevels);
            return string.Join($"\n{indentStr}", content.Split('\n'));
        }

        internal static string FormatPartialParserClassFile(string strNamespace, string content)
        {
            return 
$@"using System;
using System.Memory;
namespace {strNamespace};
public partial class Parser : MetaParser.Parser<char>
{{
{IndentLines(1, content)}
}}
";
        }

        internal static Type Get_Integer_Type(long maxValue)
        {
            if (maxValue < byte.MaxValue)
            {
                return typeof(byte);
            }
            else if (maxValue < short.MaxValue)
            {
                return typeof(short);
            }
            else if (maxValue < ushort.MaxValue)
            {
                return typeof(ushort);
            }
            else if (maxValue < int.MaxValue)
            {
                return typeof(int);
            }
            else if (maxValue < uint.MaxValue)
            {
                return typeof(uint);
            }

            return typeof(long);
        }

        internal static string Get_Integer_TypeName(long maxValue)
        {
            if (maxValue < byte.MaxValue)
            {
                return "byte";
            }
            else if (maxValue < short.MaxValue)
            {
                return "short";
            }
            else if (maxValue < ushort.MaxValue)
            {
                return "ushort";
            }
            else if (maxValue < int.MaxValue)
            {
                return "int";
            }
            else if (maxValue < uint.MaxValue)
            {
                return "uint";
            }

            return "long";
        }

        static Stream Get_Parser_Schema_Stream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "MetaTranspiler.parser-schema.json";
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}