using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.ClassLibrary.Models;

namespace WeatherApp.ClassLibrary.Api
{
    public class WeatherEndpoints
    {
        static string units = "metric"; //TEMP
        static string apiKey = Credentials.apiKey;

        static async Task<T> LoadWeather<T>(string url) where T : class, new()
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<CurrentWeatherModel.RootObject> LoadWeatherByCityName(string city)
        {
            string url = $"weather?q={city}&units={units}&appid={apiKey}";

            var data = await LoadWeather<CurrentWeatherModel.RootObject>(url);
            return data;
        }

        public static async Task<CurrentWeatherModel.RootObject> LoadWeatherByCoordinates(double lon, double lat)
        {
            string url = $"weather?lat={lat}&lon={lon}&units={units}&appid={apiKey}";

            var data = await LoadWeather<CurrentWeatherModel.RootObject>(url);
            return data;
        }

        public static async Task<HourlyWeatherModel.RootObject> LoadDailyByCityName(string city)
        {
            string url = $"forecast?q={city}&units={units}&appid={apiKey}";

            var data = await LoadWeather<HourlyWeatherModel.RootObject>(url);
            return data;
        }

        public static async Task<HourlyWeatherModel.RootObject> LoadDailyByCoordinates(double lon, double lat)
        {
            string url = $"forecast?lat={lat}&lon={lon}&units={units}&appid={apiKey}";

            var data = await LoadWeather<HourlyWeatherModel.RootObject>(url);
            return data;
        }
    }
}
