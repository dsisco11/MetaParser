namespace TranspilerUnitTests;

[UsesVerify]
public class UnitTest1
{
    [Fact]
    public Task Constant_Tokens()
    {
        var input = TestHelper.Get_Input_File("constant_tokens.json");
        return TestHelper.Verify(input);
    }

    [Fact]
    public Task Compound_Tokens()
    {
        var input = TestHelper.Get_Input_File("compound_tokens.json");
        return TestHelper.Verify(input);
    }

    [Fact]
    public Task Complex_Tokens()
    {
        var input = TestHelper.Get_Input_File("complex_tokens.json");
        return TestHelper.Verify(input);
    }
}