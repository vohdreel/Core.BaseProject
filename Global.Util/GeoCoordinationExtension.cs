using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Global.Util
{

    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(string latitude, string longitude)
        {
            Latitude = Double.Parse(latitude);
            Longitude = Double.Parse(longitude);
        }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude =longitude;
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

        public static Coordinates GetCoordinatesFromApi(string address, string apiKey)
        {
            object parameters = new
            {
                address,
                key = apiKey

            };
            var resultJson = HttpHelper
                .Get<JObject>(
                "https://maps.googleapis.com/maps/api/geocode/json",
                "json",
                parameters);

            var jsonCoordinates = resultJson["results"][0]["geometry"]["location"];
            if (jsonCoordinates != null)
                return new Coordinates(
                    jsonCoordinates["lat"].ToObject<double>(),
                    jsonCoordinates["lng"].ToObject<double>());
            else
                return null;
        }



    }
}
