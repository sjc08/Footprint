﻿using Android.Util;
using Android.Views;
using Android.Webkit;
using Asjc.Android.ServiceHelper;
using Google.Android.Material.Button;
using Humanizer;
using System.Text.Json;

namespace Footprint
{
    public class HomeFragment : Fragment
    {
        private ServiceConnector<LocationService>? connector;
        private WebView? live;

        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater?.Inflate(Resource.Layout.fragment_home, container, false);
        }

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            connector = ((MainActivity)Activity).Connector;
        }

        public override void OnStart()
        {
            base.OnStart();

            connector?.WhenConnected(s => s.OnPoint += PointHandler);
        }

        public override void OnStop()
        {
            base.OnStop();

            connector?.WhenConnected(s => s.OnPoint -= PointHandler);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var group = view.FindViewById<MaterialButtonToggleGroup>(Resource.Id.toggleGroup);
            for (int i = 0; i < group.ChildCount; i++)
            {
                int index = i; // Pay attention to closures!
                var radioButton = (MaterialButton)group.GetChildAt(i);
                radioButton.Click += (_, _) => connector?.WhenConnected(s => s.Mode = index);
            }
            connector?.WhenConnected(s => group.Check(group.GetChildAt(s.Mode).Id));
            live = view.FindViewById<WebView>(Resource.Id.liveView);
            live.Settings.JavaScriptEnabled = true;
            var myClient = new MyClient();
            var last = Database.Connection.Table<Point>().LastOrDefault();
            myClient.PageFinished += (view, url) => PointHandler(last, last);
            live.SetWebViewClient(myClient);
            live.LoadUrl("file:///android_asset/www/live.html");
        }

        public void PointHandler(Point? recPt, Point? curPt)
        {
            Log.Debug(nameof(HomeFragment), "Got a point.");
            live?.EvaluateJavascript($"mark({JsonSerializer.Serialize(recPt)}, {JsonSerializer.Serialize(curPt)})", null);
            if (recPt != null && curPt != null)
            {
                Activity.FindViewById<TextView>(Resource.Id.textView).Text = GetString(Resource.String.info,
                [
                    recPt.Duration.Milliseconds().Humanize(),
                    new DateTime(1970,1,1,0,0,0).AddMilliseconds(curPt.Time).ToLocalTime().ToString(),
                    curPt.Latitude,
                    curPt.Longitude,
                    curPt.Accuracy,
                    curPt.Altitude,
                    curPt.Bearing,
                    curPt.Speed
                ]);
            }
        }
    }
}
