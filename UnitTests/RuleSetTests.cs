using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.Tokens;

namespace UnitTests
{
    public class RuleSetTests
    {

        [Fact]
        public void TestRuleSetOrdering()
        {
            // Mock rules
            var mockRuleSingle = new SingleRule<TokenType<ETextToken>, char>(ETextToken.None, '*');
            var mockRuleSequence_1 = new SequenceRule<TokenType<ETextToken>, char>(ETextToken.None, "abc");
            var mockRuleSequence_2 = new SequenceRule<TokenType<ETextToken>, char>(ETextToken.None, "12345");
            var mockRuleGroupSingle = new GroupSingleRule<TokenType<ETextToken>, char>(ETextToken.None, '\n');
            var mockRuleGroupSet = new GroupSetRule<TokenType<ETextToken>, char>(ETextToken.None, " \t\f");
            var mockRuleBlock = new BlockRule<TokenType<ETextToken>, char>(ETextToken.None, "{", "}");

            // Rules lists
            var expected = new ITokenRule<TokenType<ETextToken>, char>[] { mockRuleBlock, mockRuleGroupSet, mockRuleGroupSingle, mockRuleSequence_2, mockRuleSequence_1, mockRuleSingle };
            var ruleset = new RuleSet<TokenType<ETextToken>, char>(mockRuleGroupSingle, mockRuleGroupSet, mockRuleBlock, mockRuleSingle, mockRuleSequence_1, mockRuleSequence_2);
            Assert.Equal(expected, ruleset.Items);
        }
    }
}
