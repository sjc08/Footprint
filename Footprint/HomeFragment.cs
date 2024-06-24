using Android.Util;
using Android.Views;
using Android.Webkit;
using Asjc.Android.ServiceHelper;
using Google.Android.Material.Button;
using System.Text.Json;

namespace Footprint
{
    public class HomeFragment : Fragment
    {
        private ServiceConnector<LocationService>? connector;
        private WebView? live;

        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_home, container, false);
        }

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            connector = ((MainActivity)Activity).Connector;
            connector.WhenConnected(s => s.OnPoint += PointHandler);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            connector.WhenConnected(s => s.OnPoint -= PointHandler);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var group = view.FindViewById<MaterialButtonToggleGroup>(Resource.Id.toggleGroup);
            for (int i = 0; i < group.ChildCount; i++)
            {
                int index = i; // Pay attention to closures!
                var radioButton = (MaterialButton)group.GetChildAt(i);
                radioButton.Click += (_, _) => connector.WhenConnected(s => s.Mode = index);
            }
            view.FindViewById<TextView>(Resource.Id.textView).Text = $"共有 {Database.Connection.Table<Point>().Count()} 条数据";
            connector.WhenConnected(s => group.Check(group.GetChildAt(s.Mode).Id));
            live = view.FindViewById<WebView>(Resource.Id.liveView);
            live.Settings.JavaScriptEnabled = true;
            live.LoadUrl("file:///android_asset/www/live.html");
        }

        public void PointHandler(Point p1, Point p2)
        {
            Log.Debug(nameof(HomeFragment), "Got a point.");
            live?.EvaluateJavascript($"mark({JsonSerializer.Serialize(p1)}, {JsonSerializer.Serialize(p2)})", null);
        }
    }
}
