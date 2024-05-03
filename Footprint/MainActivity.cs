using Android.Content;

namespace Footprint
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private readonly ServiceConnection<LocationService> connection = new();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Intent intent = new(this, typeof(LocationService));
            StartForegroundService(intent);
            BindService(intent, connection, Bind.AutoCreate);
        }
    }
}