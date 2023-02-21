using Tests.Fixtures;

namespace Tests.SnapshotTests;

[UsesVerify]
public class CodeGeneratorOutput : IClassFixture<CodeGeneratorFixture>
{
    private readonly CodeGeneratorFixture fixture;

    public CodeGeneratorOutput (CodeGeneratorFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData("constant_tokens.json")]
    [InlineData("compound_tokens.json")]
    [InlineData("complex_tokens.json")]
    public Task Result(string fileName)
    {
        var sourceCode = CodeGeneratorFixture.Get_Input_File_Contents(fileName);
        return fixture.Verify(fileName, sourceCode);
    }
}