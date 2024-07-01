using Android.Views;
using Android.Webkit;
using Asjc.Android.ServiceHelper;
using Asjc.Android.SimpleWebViewClient;
using System.Text.Json;

namespace Footprint
{
    public class ViewFragment : Fragment
    {
        private ServiceConnector<LocationService>? connector;
        private WebView? view;

        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater?.Inflate(Resource.Layout.fragment_view, container, false);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.view = view.FindViewById<WebView>(Resource.Id.webView);
            this.view.Settings.AllowUniversalAccessFromFileURLs = true;
            this.view.Settings.JavaScriptEnabled = true;
            this.view.AddJavascriptInterface(new JavascriptInterface(), "CS");
            this.view.SetWebViewClient(new SimpleWebViewClient(pageFinishedCallback: (_, _) =>
            {
                connector?.WhenConnected(s => Update(s.CurrentPoint));
            }));
            this.view.LoadUrl("file:///android_asset/www/view.html");
        }

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            connector = ((MainActivity?)Activity)?.Connector;
        }

        public override void OnStart()
        {
            base.OnStart();

            connector?.WhenConnected(s => s.LocationChanged += PointHandler);
        }

        public override void OnStop()
        {
            base.OnStop();

            connector?.WhenConnected(s => s.LocationChanged -= PointHandler);
        }

        public void PointHandler(Point? recPt, Point? curPt) => Update(curPt);

        public void Update(Point? pt) => view?.EvaluateJavascript($"update({JsonSerializer.Serialize(pt)})", null);
    }
}
