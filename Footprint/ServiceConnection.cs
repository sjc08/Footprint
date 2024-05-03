using Android.Content;
using Android.OS;

namespace Footprint
{
    public class ServiceConnection<T> : Java.Lang.Object, IServiceConnection where T : Service
    {
        public IBinder? Binder { get; private set; }

        public event Action<ComponentName?, Binder<T>?>? Connected;
        public event Action<ComponentName?>? Disconnected;

        public void OnServiceConnected(ComponentName? name, IBinder? service)
        {
            Binder = service;
            Connected?.Invoke(name, service as Binder<T>);
        }

        public void OnServiceDisconnected(ComponentName? name)
        {
            Binder = null;
            Disconnected?.Invoke(name);
        }
    }
}
