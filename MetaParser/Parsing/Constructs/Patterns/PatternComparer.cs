using System;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs.Patterns;

internal class PatternComparer : IComparer<Pattern>
{
    public static readonly PatternComparer Instance = new PatternComparer();

    public int Compare(Pattern left, Pattern right)
    {
        // if any of these factors are different, then use the order of the factors to determine the comparison result
        // if all factors are the same, then compare the next pattern in the left and right enumerables
        // if the left or right enumerables are exhausted, then the longer enumerable is considered to be greater
        // if all factors are the same and both enumerables are exhausted, then the patterns are considered to be equal
        // if all factors are the same, both enumerables are exhausted, and the patterns are not equal, then compare the patterns based on their index
        // if the index is the same, then the patterns are considered to be equal
        // if the index is different, then the pattern with the lower index is considered to be less than the pattern with the higher index

        // First we make sure both patterns either are/are not sequences
        if (left.HasChildren != right.HasChildren)
        {
            return !left.HasChildren ? -1 : 1;// patterns with no children come before patterns with children
        }

        if (left.HasChildren)
        {
            // If both patterns are sequences, then we compare the children
            IEnumerator<Pattern> leftEnumerator = left.GetEnumerator();
            IEnumerator<Pattern> rightEnumerator = right.GetEnumerator();
            bool leftMoveNext = leftEnumerator.MoveNext();
            bool rightMoveNext = rightEnumerator.MoveNext();
            while (leftMoveNext && rightMoveNext)
            {
                Pattern leftPattern = leftEnumerator.Current;
                Pattern rightPattern = rightEnumerator.Current;
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

        // compare left and right patterns based on the factors: IsRawValues, IsInlinable, IsConstantLength, MinLogicalLength, MaxLogicalLength
        if (left.IsRawValues != right.IsRawValues)
        {
            return left.IsRawValues ? -1 : 1; // raw values come before non-raws
        }

        if (left.IsInlinable != right.IsInlinable)
        {
            return left.IsInlinable ? -1 : 1; // inlineable values come before non-inlineables
        }

        if (left.IsLogical != right.IsLogical)
        {
            return left.IsLogical ? 1 : -1; // logical values come after non-logic
        }

        if (left.IsConstantLength != right.IsConstantLength)
        {
            return left.IsConstantLength ? -1 : 1;
        }

        if (left.MaxLogicalLength != right.MaxLogicalLength)
        {
            return left.MaxLogicalLength.CompareTo(right.MaxLogicalLength);
        }

        if (left.MinLogicalLength != right.MinLogicalLength)
        {
            return left.MinLogicalLength.CompareTo(right.MinLogicalLength);
        }

        //return left.DependencyInfo.Order.CompareTo(right.DependencyInfo.Order);
        return 0;
    }
}
