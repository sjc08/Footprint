using Android;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using AndroidX.Core.Content;
using Asjc.Android.ServiceHelper;

namespace Footprint
{
    [Service(ForegroundServiceType = ForegroundService.TypeLocation)]
    public class LocationService : Service, ILocationListener
    {
        public const string CHANNEL_ID = "default_channel";
        public const string CHANNEL_NAME = "Default Channel";
        public const int NOTIFICATION_ID = 10000;

        public event Action<Point, Point>? OnPoint;

        private LocationManager locationManager;

        public override IBinder? OnBind(Intent? intent) => new ServiceBinder<LocationService>(this);

        public override void OnCreate()
        {
            base.OnCreate();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                ((NotificationManager)GetSystemService(NotificationService)).CreateNotificationChannel(new(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default));

            locationManager = (LocationManager)GetSystemService(LocationService);
            Database.Connection.CreateTable<Point>();
        }

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            var builder = new Notification.Builder(this, CHANNEL_ID)
                .SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(Resources.GetString(Resource.String.app_text))
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
            StartForeground(NOTIFICATION_ID, builder);
            return StartCommandResult.Sticky;
        }

        private int mode = 2;
        public int Mode
        {
            get => mode;
            set
            {
                mode = value;
                int interval = new int[] { 1, 10, 60, 600 }[mode];
                Toast.MakeText(this, $"更新间隔：{interval} s", ToastLength.Short).Show();
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
                {
                    locationManager.RemoveUpdates(this);
                    locationManager.RequestLocationUpdates(LocationManager.GpsProvider, interval * 1000, 0, this);
                }
                else
                {
                    Toast.MakeText(this, "请在设置中开启位置权限", ToastLength.Short).Show();
                }
            }
        }

        public void OnLocationChanged(Location location)
        {
            var last = Database.Connection.Table<Point>().LastOrDefault();
            // If it's the first record or the location has changed.
            if (last == null || location.DistanceTo(last.ToLocation()) > 50)
                Database.Connection.Insert(Point.FromLocation(location));
            // Update the duration of stay.
            var point = Database.Connection.Table<Point>().Last();
            point.Duration = location.Time - point.Time;
            Database.Connection.Update(point);
            OnPoint?.Invoke(point, Point.FromLocation(location));
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string? provider, Availability status, Bundle? extras) { }
    }
}
