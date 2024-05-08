using Android.Views;

namespace Footprint
{
    public class ViewFragment : Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_view, container, false);
        }
    }
}
