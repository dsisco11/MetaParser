namespace MetaParser.Rules
{
    public interface IParsingRule : IComparable<IParsingRule>
    {
        RuleSpecificty Specificity { get; }

        int IComparable<IParsingRule>.CompareTo(IParsingRule? other)
        {
            var left = Specificity.SpecificityValue;
            var right = other?.Specificity.SpecificityValue ?? 0;
            return right - left;
        }
    }
}
