using Android.OS;

namespace Footprint
{
    public class Binder<T>(T service) : Binder where T : Service
    {
        public T Service { get; private set; } = service;
    }
}
