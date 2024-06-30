using Android.Content;
using Android.Views;
using AndroidX.DocumentFile.Provider;
using CsvHelper;
using Humanizer;
using System.Globalization;

namespace Footprint
{
    public class SettingsFragment : Fragment
    {
        public override View? OnCreateView(LayoutInflater? inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater?.Inflate(Resource.Layout.fragment_settings, container, false);
        }

        public override void OnViewCreated(View? view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            view.FindViewById<Button>(Resource.Id.button1).Click += (_, _) =>
            {
                Intent intent = new(Intent.ActionGetContent);
                intent.SetType("*/*");
                StartActivityForResult(intent, 1);
            };
            view.FindViewById<Button>(Resource.Id.button2).Click += (_, _) =>
            {
                Intent intent = new(Intent.ActionOpenDocumentTree);
                StartActivityForResult(intent, 2);
            };
            view.FindViewById<Button>(Resource.Id.button3).Click += (_, _) =>
            {
                var builder = new AlertDialog.Builder(Activity).SetTitle("警告")
                                                               .SetMessage($"您的所有数据将会被永久删除，此操作不可恢复！")
                                                               .SetPositiveButton("是", delegate { Database.Connection.DeleteAll<Point>(); })
                                                               .SetNegativeButton("否", delegate { })
                                                               .Show();
            };
            view.FindViewById<TextView>(Resource.Id.dataSummary).Text = GetString(Resource.String.data_summary,
            [
                Database.Connection.Table<Point>().Count(),
                new FileInfo(Database.Connection.DatabasePath).Length.Bytes().Humanize()
            ]);
            var map = view.FindViewById<EditText>(Resource.Id.map);
            map.Text = Settings.Instance.Map;
            map.TextChanged += (_, e) => Settings.Instance.Map = e.Text.ToString();
            var style = view.FindViewById<EditText>(Resource.Id.style);
            style.Text = Settings.Instance.Style;
            style.TextChanged += (_, e) => Settings.Instance.Style = e.Text.ToString();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Settings.Instance.Save();
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && data != null)
            {
                var uri = data.Data;
                try
                {
                    if (requestCode == 1)
                    {
                        using var inputStream = Context.ContentResolver.OpenInputStream(uri);
                        using var reader = new StreamReader(inputStream);
                        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                        var records = csv.GetRecords<Point>();
                        Database.Connection.InsertAll(records);
                    }
                    if (requestCode == 2)
                    {
                        using var outputStream = Context.ContentResolver.OpenOutputStream(DocumentFile.FromTreeUri(Activity, uri).CreateFile("text/csv", "Footprint.csv").Uri);
                        using var writer = new StreamWriter(outputStream);
                        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                        var records = Database.Connection.Table<Point>().ToList();
                        csv.WriteRecords(records);
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Short).Show();
                }
            }
        }
    }
}
