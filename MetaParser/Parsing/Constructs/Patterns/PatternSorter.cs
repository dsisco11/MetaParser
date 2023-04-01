using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs.Patterns;

internal class PatternSorter : IComparer<PatternEntity>
{
    public static readonly PatternSorter Instance = new PatternSorter();

    public int Compare(PatternEntity left, PatternEntity right)
    {
        // if any of these factors are different, then use the order of the factors to determine the comparison result
        // if all factors are the same, then compare the next pattern in the left and right enumerables
        // if the left or right enumerables are exhausted, then the longer enumerable is considered to be greater
        // if all factors are the same and both enumerables are exhausted, then the patterns are considered to be equal
        // if all factors are the same, both enumerables are exhausted, and the patterns are not equal, then compare the patterns based on their index
        // if the index is the same, then the patterns are considered to be equal
        // if the index is different, then the pattern with the lower index is considered to be less than the pattern with the higher index

        // First we make sure both patterns either are/are not sequences
        if (left.IsSequence != right.IsSequence)
        {
            return !left.IsSequence ? -1 : 1;// patterns with no children come before patterns with children
        }

        if (left.IsSequence)
        {
            // If both patterns are sequences, then we compare the children
            IEnumerator<PatternEntity> leftEnumerator = left.GetEnumerator();
            IEnumerator<PatternEntity> rightEnumerator = right.GetEnumerator();
            bool leftMoveNext = leftEnumerator.MoveNext();
            bool rightMoveNext = rightEnumerator.MoveNext();
            while (leftMoveNext && rightMoveNext)
            {
                PatternEntity leftPattern = leftEnumerator.Current;
                PatternEntity rightPattern = rightEnumerator.Current;
                int compareResult = Compare(leftPattern, rightPattern);
                if (compareResult != 0)
                {
                    return compareResult;
                }
                leftMoveNext = leftEnumerator.MoveNext();
                rightMoveNext = rightEnumerator.MoveNext();
            }

            if (leftMoveNext)
            {
                return -1;
            }
            else if (rightMoveNext)
            {
                return 1;
            }
        }

        if (left.IsDeterministic != right.IsDeterministic)
        {
            return left.IsDeterministic ? -1 : 1; // deterministic values come before non-deterministic
        }

        if (left.IsInlinable != right.IsInlinable)
        {
            return left.IsInlinable ? -1 : 1; // inlineable values come before non-inlineables
        }

        if (left.IsConditional != right.IsConditional)
        {
            return left.IsConditional ? 1 : -1; // logical values come after non-logic
        }

        if (left.IsConstantLength != right.IsConstantLength)
        {
            return left.IsConstantLength ? -1 : 1;
        }

        if (left.MaxConditions != right.MaxConditions)
        {
            return left.MaxConditions.CompareTo(right.MaxConditions);
        }

        if (left.MinConditions != right.MinConditions)
        {
            return left.MinConditions.CompareTo(right.MinConditions);
        }

        return 0;
    }
}
