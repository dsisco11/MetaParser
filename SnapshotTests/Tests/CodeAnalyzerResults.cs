using Tests.Fixtures;

namespace Tests.SnapshotTests;

[UsesVerify]
public class Analyzer : IClassFixture<CodeGeneratorFixture>
{
    private readonly CodeGeneratorFixture fixture;

    public Analyzer (CodeGeneratorFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData("bad_tokens.json")]
    public Task Result(string fileName)
    {
        var sourceCode = CodeGeneratorFixture.Get_Input_File_Contents(fileName);
        return fixture.Verify<MetaParser.Analyzer>(fileName, sourceCode);
    }
}