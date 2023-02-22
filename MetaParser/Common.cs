using Json.Schema;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace MetaParser
{
    public static class Common
    {

        private static AssemblyName _assemblyName = typeof(Common).Assembly.GetName();
        internal static readonly string s_generatedCodeAttributeSource = $@"
[global::System.CodeDom.Compiler.GeneratedCodeAttribute(""{_assemblyName.Name}"", ""{_assemblyName.Version}"")]
";
        public const string MetaParserFileExtension = ".metaparser.json";

        #region Schema
        const string Schema_File_Resource = "MetaParser.Resources.schema-01.json";
        public static ValidationOptions SchemaOptions
        {
            get
            {
                return new()
                {
                    OutputFormat = OutputFormat.Detailed,
                    //Log = new JsonDebugLogger()
                };
            }
        }

        public static JsonSchema Get_Parser_Schema()
        {
            var jsonStr = Get_Embedded_File_Contents(Schema_File_Resource);
            return JsonSchema.FromText(jsonStr, JsonSerializerOptions.Default);
        }
        #endregion

        internal static string Get_FileName(string str) => Remove_From_End(Path.GetFileName(str).AsSpan(), MetaParserFileExtension.AsSpan());
        internal static string Remove_From_End(ReadOnlySpan<char> source, ReadOnlySpan<char> remove)
        {
            return source.Slice(0, source.Length - remove.Length).ToString();
        }

        internal static SpecialType Get_Integer_Type(long maxValue)
        {
            if (maxValue < byte.MaxValue)
            {
                return SpecialType.System_Byte;
            }
            else if (maxValue < short.MaxValue)
            {
                return SpecialType.System_Int16;
            }
            else if (maxValue < ushort.MaxValue)
            {
                return SpecialType.System_UInt16;
            }
            else if (maxValue < int.MaxValue)
            {
                return SpecialType.System_Int32;
            }
            else if (maxValue < uint.MaxValue)
            {
                return SpecialType.System_UInt32;
            }

            return SpecialType.System_Int64;
        }

        #region Resource Helpers
        internal static Stream Get_Embedded_Resource_Stream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                throw new Exception($"Unable to load embedded schema definition resource from assembly!");
            }

            return stream;
        }

        /// <summary>
        /// Returns the contents of an embedded assembly file as a string
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        internal static string Get_Embedded_File_Contents(string resourceName)
        {
            using var stream = Get_Embedded_Resource_Stream(resourceName);
            using StreamReader rd = new StreamReader(stream);
            return rd.ReadToEnd();
        }

        internal static string Get_Embedded_File_With_Generated_Header(string resourceName, bool includeGeneratedCodeAttribute)
        {
            var contents = Get_Embedded_File_Contents(resourceName);
            using StringWriter baseWriter = new();
            using IndentedTextWriter writer = new(baseWriter);

            writer.WriteLine($"namespace {nameof(MetaParser)};");
            if (includeGeneratedCodeAttribute)
            {
                writer.WriteLine(s_generatedCodeAttributeSource);
            }
            writer.Write(contents);

            return writer.InnerWriter.ToString();
        }
        #endregion
    }
}