using Tests.Fixtures;

namespace Tests.SnapshotTests;

[UsesVerify]
public class Generation : IClassFixture<CodeGeneratorFixture>
{
    private readonly CodeGeneratorFixture fixture;

    public Generation (CodeGeneratorFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData("parser.json")]
    public Task Result(string fileName)
    {
        var sourceCode = CodeGeneratorFixture.Get_Input_File_Contents(fileName);
        return fixture.Verify<MetaParser.Generator>(fileName, sourceCode);
    }
}