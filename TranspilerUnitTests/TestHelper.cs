using System.Reflection;

public static class TestHelper
{
    public static Task Verify(string jsonStr)
    {
        var sourceText = new JsonAdditionalText($"file{MetaTranspiler.Common.MetaParserFileExtension}", jsonStr);
        // Create references for assemblies we require
        // We could add multiple references if required
        IEnumerable<PortableExecutableReference> references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonElement).Assembly.Location)
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: Array.Empty<SyntaxTree>(),
            references: references); // 👈 pass the references to the compilation

        var generator = new MetaTranspiler.ParserGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { generator.AsSourceGenerator() }, additionalTexts: new[] { sourceText });

        driver = driver.RunGenerators(compilation);

        return Verifier
            .Verify(driver)
            .UseDirectory("Snapshots");
    }

    public static string Get_Input_File(string filePath)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(TranspilerUnitTests)}.Inputs.{filePath}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
