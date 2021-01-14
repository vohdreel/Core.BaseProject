using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using System.Text;

namespace Global.Util
{

    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public static class GeoCoordinationExtension
    {

        public static double GetDistanceBetweenLocations(Coordinates origin, Coordinates destiny)
        {

            GeoCoordinate _origin = new GeoCoordinate(origin.Latitude, origin.Longitude);
            GeoCoordinate _destiny = new GeoCoordinate(destiny.Latitude, destiny.Longitude);
            return _origin.GetDistanceTo(_destiny);

            //var cristo = new GeoCoordinate(-22.9519, -43.2105);
            //var liberty = new GeoCoordinate(40.689247, -74.044502);
            //return cristo.GetDistanceTo(liberty);

        }



    }
}
