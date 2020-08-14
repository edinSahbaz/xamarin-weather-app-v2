using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherApp
{
    public static class GeoCoordinatesProcessor
    {
        public static async Task<Location> LoadCoordinates()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
                return location;

            return new Location
            {
                Latitude = 51.5,
                Longitude = -1,
                VerticalAccuracy = null,
                Accuracy = null,
                Speed = null,
                Altitude = null,
                Course = null,
                IsFromMockProvider = false,
                Timestamp = new DateTime()
            };
        }
    }
}
