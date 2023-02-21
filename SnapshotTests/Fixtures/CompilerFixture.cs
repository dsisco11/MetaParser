using System.Reflection;

namespace Tests.Fixtures
{
    internal class CompilerFixture : CodeGeneratorFixture
    {
        public readonly CSharpCompilation compilation;

        public CompilerFixture()
        {
            // Create references for assemblies we require
            IEnumerable<PortableExecutableReference> references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            };

            compilation = CSharpCompilation.Create(
                assemblyName: Assembly.GetEntryAssembly()!.GetName().Name,
                syntaxTrees: Array.Empty<SyntaxTree>(),
                references: references); // 👈 pass the references to the compilation
        }

        public CSharpCompilation Compile(string[] files)
        {
            return CSharpCompilation.Create(
                assemblyName: "Script",
                syntaxTrees: files.Select(o => CSharpSyntaxTree.ParseText(o)).ToArray(),
                references: new[] { MetadataReference.CreateFromFile(typeof(Binder).Assembly.Location) },
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );
        }
    }
}
