using Android.Locations;
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

        public Location ToLocation()
        {
            return new("")
            {
                Time = Time,
                Latitude = Latitude,
                Longitude = Longitude,
                Accuracy = Accuracy,
                Altitude = Altitude,
                Bearing = Bearing,
                Speed = Speed
            };
        }

        public static Point FromLocation(Location location)
        {
            return new()
            {
                Time = location.Time,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Accuracy = location.Accuracy,
                Altitude = location.Altitude,
                Bearing = location.Bearing,
                Speed = location.Speed
            };
        }
    }
}
