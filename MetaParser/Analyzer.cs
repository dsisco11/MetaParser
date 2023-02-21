using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Json.Schema;
using System.Collections.Immutable;

namespace MetaParser;


[Generator(LanguageNames.CSharp)]
public partial class Analyzer : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext Context)
    {
        // define the execution pipeline here via a series of transformations:

        // find all additional files that end with .parser-meta.json
        IncrementalValuesProvider<AdditionalText> ctxFileNames = Context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(Common.MetaParserFileExtension));

        // read their contents and save their name
        IncrementalValuesProvider<FileData> ctxFiles = ctxFileNames.Select(static (text, cancellationToken) => new FileData(Common.Get_FileName(text.Path), text.Path, text.GetText(cancellationToken)!.ToString()));

        IncrementalValuesProvider<(FileData, JsonDocument)> ctxParserJson = ctxFiles.Select((Func<FileData, CancellationToken, (FileData, JsonDocument)>)(static (FileData file, CancellationToken cancellationToken) =>
        {
            var doc = JsonDocument.Parse(file.Content, new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            return (file, doc);
        }));

        Context.RegisterSourceOutput(ctxParserJson, static (SourceProductionContext spc, (FileData file, JsonDocument jsonDoc) data) =>
        {
            FileData file = data.file;
            JsonDocument jsonDoc = data.jsonDoc;
            var schema = Common.Get_Parser_Schema();
            var result = schema.Validate(jsonDoc, Common.SchemaOptions);
            if (!result.IsValid)
            {
                Convert_Schema_Results_To_Diagnostics(spc, file, result);
            }
        });
    }

    private static void Convert_Schema_Results_To_Diagnostics(SourceProductionContext spc, FileData file, ValidationResults Results)
    {
        if (Results.IsValid)
        {
            return;
        }

        if (Results.HasNestedResults)
        {
            foreach (var nestedResults in Results.NestedResults)
            {
                Convert_Schema_Results_To_Diagnostics(spc, file, nestedResults);
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(Results.Message))
            {
                var data = new Dictionary<string, string?>()
                {
                    { "Schema", Results?.AbsoluteSchemaLocation?.Fragment },
                    { "Location", Results?.InstanceLocation.Source }
                }.ToImmutableDictionary();
                var diag = Diagnostic.Create(DIAGNOSTIC_DEFS.SchemaException, Location.None, properties: data, Results!.InstanceLocation.Source, Results.Message);
                spc.ReportDiagnostic(diag);
            }
        }
    }
}
