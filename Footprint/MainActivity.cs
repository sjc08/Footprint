using Android.Content;
using Android.Content.PM;
using Android.Views;
using Asjc.Android.ServiceHelper;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;

namespace Footprint
{
    [Activity(Label = "@string/app_name", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : Activity, NavigationBarView.IOnItemSelectedListener
    {
        public ServiceConnector<LocationService> Connector { get; } = new();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Intent intent = new(this, typeof(LocationService));
            StartForegroundService(intent);
            BindService(intent, Connector, Bind.AutoCreate);

            Database.Connection.CreateTable<Point>();

            var bottomNav = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav);
            bottomNav.SetOnItemSelectedListener(this);

            FragmentManager.BeginTransaction().Replace(Resource.Id.content, home).Commit();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Connector.Connected)
                UnbindService(Connector);
        }

        private readonly HomeFragment home = new();
        private readonly ViewFragment view = new();
        private readonly SettingsFragment settings = new();

        public bool OnNavigationItemSelected(IMenuItem p0)
        {
            switch (p0.ItemId)
            {
                case Resource.Id.home:
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content, home).Commit();
                    break;
                case Resource.Id.view:
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content, view).Commit();
                    break;
                case Resource.Id.settings:
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content, settings).Commit();
                    break;
            }
            return true;
        }
    }
}