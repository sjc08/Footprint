using SQLite;

namespace Footprint
{
    public class Database
    {
        private const string file = "Footprint.db";

        public static SQLiteConnection Connection { get; }

        static Database()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file);
            Connection = new(path);
        }
    }
}
