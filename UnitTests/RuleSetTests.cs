using MetaParser.Parsing;
using MetaParser.Rules;

namespace UnitTests
{
    public class RuleSetTests
    {

        [Fact]
        public void TestRuleSetOrdering()
        {
            // Mock rules
            var mockRuleSingle = new SingleRule<byte, char>(ETextToken.None, '*');
            var mockRuleSequence_1 = new SequenceRule<byte, char>(ETextToken.None, "abc");
            var mockRuleSequence_2 = new SequenceRule<byte, char>(ETextToken.None, "12345");
            var mockRuleGroupSingle = new GroupSingleRule<byte, char>(ETextToken.None, '\n');
            var mockRuleGroupSet = new GroupSetRule<byte, char>(ETextToken.None, " \t\f");
            var mockRuleBlock = new BlockRule<byte, char>(ETextToken.None, ETextToken.None, '{', '}', null);

            // Rules lists
            var expected = new ITokenRule<byte, char>[] { mockRuleBlock, mockRuleGroupSet, mockRuleGroupSingle, mockRuleSequence_2, mockRuleSequence_1, mockRuleSingle };
            var ruleset = new RuleSet<byte, char>(mockRuleGroupSingle, mockRuleGroupSet, mockRuleBlock, mockRuleSingle, mockRuleSequence_1, mockRuleSequence_2);
            Assert.Equal(expected, ruleset.Items);
        }
    }
}
