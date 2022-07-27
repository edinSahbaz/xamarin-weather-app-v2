﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    public class WeatherDataProcessor
    {
        static string units = "metric"; //TEMP

        static async Task<WeatherModel.RootObject> LoadCurrentWeather(string url)
        {
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

        public static async Task<WeatherModel.RootObject> LoadWeatherByCityName(string city)
        {
            string url = $"weather?q={city}&units={units}&appid={Credentials.apiKey}";

            var data = await LoadCurrentWeather(url);
            return data;
        }

        public static async Task<WeatherModel.RootObject> LoadWeatherByCoordinates(double lon, double lat)
        {
            string url = $"weather?lat={lat}&lon={lon}&units={units}&appid={Credentials.apiKey}";

            var data = await LoadCurrentWeather(url);
            return data;
        }

        public static async Task<DailyWeatherModel.RootObject> LoadDailyByCityName(string city)
        {
            string url = $"forecast?q={city}&units={units}&appid={Credentials.apiKey}";
            

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DailyWeatherModel.RootObject>(content);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<DailyWeatherModel.RootObject> LoadDailyByCoordinates(double lon, double lat)
        {
            string url = $"forecast?lat={lat}&lon={lon}&units={units}&appid={Credentials.apiKey}";


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DailyWeatherModel.RootObject>(content);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
