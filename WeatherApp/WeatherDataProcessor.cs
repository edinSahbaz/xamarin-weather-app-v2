using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    public class WeatherDataProcessor
    {
        public static async Task<WeatherModel.RootObject> LoadCurrentWeather(string city)
        {
            string url = $"weather?q={city}&appid={Credentials.apiKey}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherModel.RootObject>(content);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
