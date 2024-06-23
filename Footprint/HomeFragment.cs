using Android.Views;
using Google.Android.Material.Button;

namespace Footprint
{
    public class HomeFragment : Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_home, container, false);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            var group = Activity.FindViewById<MaterialButtonToggleGroup>(Resource.Id.toggleGroup);
            var c = ((MainActivity)Activity).Connector;
            for (int i = 0; i < group.ChildCount; i++)
            {
                int index = i; // Pay attention to closures!
                var radioButton = (MaterialButton)group.GetChildAt(i);
                radioButton.Click += (_, _) => c.Service.Mode = index;
            }
            Activity.FindViewById<TextView>(Resource.Id.textView).Text = $"共有 {Database.Connection.Table<Point>().Count()} 条数据";
            c.WhenConnected(s => group.Check(group.GetChildAt(s.Mode).Id));
        }
    }
}
