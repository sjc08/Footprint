using Android.Views;
using Android.Webkit;
using System.Text.Json;

namespace Footprint
{
    public class ViewFragment : Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater?.Inflate(Resource.Layout.fragment_view, container, false);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var webView = view.FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            MyClient client = new();
            client.PageFinished += (view, _) =>
            {
                string script = $"data = {JsonSerializer.Serialize(Database.Connection.Table<Point>().ToList())}";
                view?.EvaluateJavascript(script, null);
                // Log.Debug(nameof(ViewFragment), script);
            };
            webView.SetWebViewClient(client);
            webView.LoadUrl("file:///android_asset/www/view.html");
        }
    }
}
