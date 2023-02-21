using System.Reflection;

namespace Tests.Fixtures;

public class CodeGeneratorFixture
{
    private readonly CSharpCompilation compilation;

    public CodeGeneratorFixture()
    {
        // Create references for assemblies we require
        // We could add multiple references if required
        IEnumerable<PortableExecutableReference> references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonElement).Assembly.Location)
        };

        compilation = CSharpCompilation.Create(
            assemblyName: Assembly.GetEntryAssembly()!.GetName().Name,
            syntaxTrees: Array.Empty<SyntaxTree>(),
            references: references); // 👈 pass the references to the compilation
    }

    public GeneratorDriver Generate(string filePath, string fileContent)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var sourceText = new JsonAdditionalText($"{fileName}.metaparser.json", fileContent);
        var generator = new MetaParser.Generator();
        var gen = CSharpGeneratorDriver.Create(new[] { generator.AsSourceGenerator() }, additionalTexts: new[] { sourceText });
        return gen.RunGenerators(compilation);
    }


    public Task Verify(string filePath, string fileContent)
    {
        var driver = Generate(filePath, fileContent);

        return Verifier
            .Verify(driver)
            //.AutoVerify()
            .UseFileName("Generated")
            .UseDirectory(Path.Combine("../", "Snapshots", Path.GetFileNameWithoutExtension(filePath)));
    }

    public static string Get_Input_File_Contents(string filePath)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(SnapshotTests)}.Tests.Inputs.{filePath}");
        if (stream is null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
