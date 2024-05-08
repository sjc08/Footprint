using SQLite;

namespace Footprint
{
    public class Point
    {
        [PrimaryKey]
        public long Time { get; set; }

        public long Duration { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public float Accuracy { get; set; }

        public double Altitude { get; set; }

        public float Bearing { get; set; }

        public float Speed { get; set; }
    }
}
