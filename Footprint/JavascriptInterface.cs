using Android.Webkit;
using Java.Interop;
using System.Text.Json;

namespace Footprint
{
    public class JavascriptInterface : Java.Lang.Object
    {
        [JavascriptInterface]
        [Export]
        public string Style() => Settings.Instance.MapTheme;

        [JavascriptInterface]
        [Export]
        public string Points() => JsonSerializer.Serialize(Database.Connection.Table<Point>().ToList());
    }
}
