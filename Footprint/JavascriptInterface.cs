using Android.Webkit;
using Asjc.Natex;
using Asjc.Natex.Matchers;
using Java.Interop;
using System.Text.Json;

namespace Footprint
{
    public class JavascriptInterface : Java.Lang.Object
    {
        [JavascriptInterface]
        [Export]
        public string Map() => Settings.Instance.Map.ToLower();

        [JavascriptInterface]
        [Export]
        public string Style() => Settings.Instance.Style.ToLower();

        [JavascriptInterface]
        [Export]
        public string Points(string pattern)
        {
            Natex natex = new(pattern);
            natex.Matchers.Get<PropertyMatcher>().DefaultPaths.Add(["TimeDT"]);
            natex.Matchers.Set<ComparisonMatcher>(new MyComparisonMatcher());
            natex.Matchers.Set<RangeMatcher>(new MyRangeMatcher());
            var s = Database.Connection.Table<Point>().AsParallel().AsOrdered().Where(natex.Match);
            return JsonSerializer.Serialize(s);
        }
    }
}
