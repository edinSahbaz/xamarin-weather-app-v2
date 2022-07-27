using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public WeatherModel.RootObject currentWeather { get; set; }
        public ObservableCollection<DisplayDailyWeatherModel> dailyWeather { get; set; }

        #region Private Fields
        private string _imageSource;
        private string _temperature;
        private string _city;
        private string _date;
        private string _feelsLike;
        private string _sunset;
        private string _searchInput;
        #endregion

        public MainPageViewModel()
        {
            dailyWeather = new ObservableCollection<DisplayDailyWeatherModel>();

            ApiHelper.InitializeClient();
            LoadByCurrentLocation();
        }

        #region Bindable properties
        public string Title
        {
            get => "Weather Forecast";
        }

        public ICommand LoadSearched => new Command(async () =>
        {
            currentWeather = await WeatherDataProcessor.LoadWeatherByCityName(searchInput);

            var data = await WeatherDataProcessor.LoadDailyByCityName(searchInput);

            PopulateDailyWeatherList(data);
            PopulateElements();
        });

        public ICommand LoadCurrentLocation => new Command(() =>
        {
            LoadByCurrentLocation();
        });

        public string temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;

                var args = new PropertyChangedEventArgs(nameof(temperature));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string imageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;

                var args = new PropertyChangedEventArgs(nameof(imageSource));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string city
        {
            get { return _city; }
            set
            {
                _city = value;

                var args = new PropertyChangedEventArgs(nameof(city));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string date
        {
            get { return _date; }
            set
            {
                _date = value;

                var args = new PropertyChangedEventArgs(nameof(date));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string feelsLike
        {
            get { return _feelsLike; }
            set
            {
                _feelsLike = value;

                var args = new PropertyChangedEventArgs(nameof(feelsLike));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string sunset
        {
            get { return _sunset; }
            set
            {
                _sunset = value;

                var args = new PropertyChangedEventArgs(nameof(sunset));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string searchInput
        {
            get { return _searchInput; }
            set
            {
                _searchInput = value;

                var args = new PropertyChangedEventArgs(nameof(searchInput));
                PropertyChanged?.Invoke(this, args);
            }
        }
        #endregion

        async void LoadByCurrentLocation()
        {
            var coordinates = await GeoCoordinatesProcessor.LoadCoordinates();
            currentWeather = await WeatherDataProcessor.LoadWeatherByCoordinates(coordinates.Longitude, coordinates.Latitude);
            var dailyForecast = await WeatherDataProcessor.LoadDailyByCoordinates(coordinates.Longitude, coordinates.Latitude);

            PopulateElements();
            PopulateDailyWeatherList(dailyForecast);
        }

        void PopulateElements()
        {
            imageSource = "https://openweathermap.org/img/wn/" + currentWeather.weather.First().icon + "@2x.png";
            date = getCorrectDateFormat(DateTime.Now);

            temperature = Math.Round(currentWeather.main.temp).ToString();
            city = $"{currentWeather.name}, {currentWeather.sys.country}";

            feelsLike = "Feels like " + Math.Round(currentWeather.main.feels_like).ToString();
            sunset = "Sunset " + ConvertUnixTimestampToTime(currentWeather.sys.sunset);
        }

        void PopulateDailyWeatherList(DailyWeatherModel.RootObject data)
        {
            dailyWeather.Clear();
            foreach (var hour in data.list)
            {
                var time = ConvertUnixTimestampToTime(hour.dt) + "0";
                var iconSource = "https://openweathermap.org/img/wn/" + hour.weather.First().icon + "@2x.png";
                var temp = Math.Round(hour.main.temp).ToString();
                dailyWeather.Add(new DisplayDailyWeatherModel { time = time, icon = iconSource, temperature = temp });
            }
        }
        
        static string ConvertUnixTimestampToTime(long unixTimeStamp)
        {
            var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp)
        .DateTime.ToLocalTime();

            return $"{localDateTimeOffset.Hour}:{localDateTimeOffset.Minute}";
        }

        static string getCorrectDateFormat(DateTime date)
        {
            var dayName = DateTime.Now.ToString("ddd");
            var dayNumber = date.Day;
            var month = date.ToString("MMM", CultureInfo.InvariantCulture);

            return $"{dayName}, {dayNumber} {month}";
        }
    }
}
