using System;
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
        #region Private Fields
        private string _imageSource;
        private string _temperature;
        private string _city;
        private string _date;
        private string _feelsLike;
        private string _sunset;
        private string _searchInput;
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            ApiHelper.InitializeClient();

            LoadInitialData();

            LoadSearched = new Command(async () =>
            {
                weather = await WeatherDataProcessor.LoadCurrentWeatherByCityName(searchInput);
                PopulateElements();
            });
        }

        public WeatherModel.RootObject weather { get; set; }

        public string Title
        {
            get => "Weather Forecast";
        }

        public ICommand LoadSearched { get; set; }

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

        async void LoadInitialData()
        {
            var coordinates = await GeoCoordinatesProcessor.LoadCoordinates();
            weather = await WeatherDataProcessor.LoadCurrentWeatherByCoordinates(coordinates.Longitude, coordinates.Latitude);

            PopulateElements();
        }

        void PopulateElements()
        {
            imageSource = "https://openweathermap.org/img/wn/" + weather.weather.First().icon + "@2x.png";
            date = getCorrectDateFormat(DateTime.Now);


            temperature = Math.Round(weather.main.temp).ToString();
            city = $"{weather.name}, {weather.sys.country}";

            feelsLike = "Feels like " + Math.Round(weather.main.feels_like).ToString();
            sunset = "Sunset " + ConvertUnixTimestampToTime(weather.sys.sunset);
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
