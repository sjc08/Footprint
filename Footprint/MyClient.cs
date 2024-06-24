using Android.Webkit;

namespace Footprint
{
    public class MyClient : WebViewClient
    {
        public delegate void PageFinishedEventHandler(WebView? view, string? url);

        public event PageFinishedEventHandler? PageFinished;

        public override void OnPageFinished(WebView? view, string? url)
        {
            base.OnPageFinished(view, url);
            PageFinished?.Invoke(view, url);
        }
    }
}
