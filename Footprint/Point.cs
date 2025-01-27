﻿using Android.Locations;
using SQLite;

namespace Footprint
{
    public class Point
    {
        public Point() { }

        public Point(Location location)
        {
            Time = location.Time;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Accuracy = location.Accuracy;
            Altitude = location.Altitude;
            Bearing = location.Bearing;
            Speed = location.Speed;
        }

        [PrimaryKey]
        public long Time { get; set; }

        [Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public DateTime TimeDT
        {
            get
            {
                timeDT ??= new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Time).ToLocalTime();
                return timeDT.Value;
            }
        }

        private DateTime? timeDT;

        public long Duration { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public float Accuracy { get; set; }

        public double Altitude { get; set; }

        public float Bearing { get; set; }

        public float Speed { get; set; }

        public static implicit operator Location(Point p)
        {
            return new("")
            {
                Time = p.Time,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Accuracy = p.Accuracy,
                Altitude = p.Altitude,
                Bearing = p.Bearing,
                Speed = p.Speed
            };
        }
    }
}
