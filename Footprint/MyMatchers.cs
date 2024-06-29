using Asjc.Extensions;
using Asjc.Natex;
using Asjc.Natex.Matchers;

namespace Footprint
{
    // A temporary solution to the date problem (but at least it works).

    public class MyComparisonMatcher : ComparisonMatcher
    {
        protected override bool? CompareLessThan(string input, IComparable value)
        {
            var c = MyClass.CompareDate(input, value);
            if (c != null) return c < 0;
            return base.CompareLessThan(input, value);
        }

        protected override bool? CompareGreaterThan(string input, IComparable value)
        {
            var c = MyClass.CompareDate(input, value);
            if (c != null) return c > 0;
            return base.CompareGreaterThan(input, value);
        }

        protected override bool? CompareLessThanOrEqual(string input, IComparable value)
        {
            var c = MyClass.CompareDate(input, value);
            if (c != null) return c <= 0;
            return base.CompareLessThanOrEqual(input, value);
        }

        protected override bool? CompareGreaterThanOrEqual(string input, IComparable value)
        {
            var c = MyClass.CompareDate(input, value);
            if (c != null) return c >= 0;
            return base.CompareGreaterThanOrEqual(input, value);
        }

        protected override bool? CompareEquality(string input, IComparable value)
        {
            var c = MyClass.CompareDate(input, value);
            if (c != null) return c == 0;
            return base.CompareEquality(input, value);
        }
    }

    public class MyRangeMatcher : RangeMatcher
    {
        public override bool? Match(IComparable value, Data data, Natex natex)
        {
            var c1 = MyClass.CompareDate(data.Min, value);
            var c2 = MyClass.CompareDate(data.Max, value);
            if (c1 != null && c2 != null)
                return c1 >= 0 && c2 <= 0;
            return base.Match(value, data, natex);
        }
    }

    public class MyClass
    {
        public static int? CompareDate(string input, IComparable value)
        {
            if (value is DateTime dt1
                && input.ConvertTo(typeof(DateTime), out var result)
                && result is DateTime dt2)
            {
                if (dt2 is { Hour: 0, Minute: 0, Second: 0 })
                    return dt1.Date.CompareTo(dt2);
            }
            return null;
        }
    }
}
