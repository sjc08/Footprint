using Asjc.Natex.Matchers;

namespace Footprint
{
    // A temporary solution to the date problem (but at least it works).

    public class MyComparisonMatcher : ComparisonMatcher
    {
        protected override int Compare(IComparable value, object obj)
        {
            return MyClass.CompareDate(value, obj) ?? base.Compare(value, obj);
        }
    }

    public class MyRangeMatcher : RangeMatcher
    {
        protected override int Compare(IComparable value, object obj)
        {
            return MyClass.CompareDate(value, obj) ?? base.Compare(value, obj);
        }
    }

    public class MyClass
    {
        public static int? CompareDate(IComparable value, object obj)
        {
            if (value is DateTime dt1 && obj is DateTime dt2
                && dt2 is { Hour: 0, Minute: 0, Second: 0 })
                return dt1.Date.CompareTo(dt2);
            return null;
        }
    }
}
