using WeatherApp.Models;
using WeatherApp.ClassLibrary.Models;
using WeatherApp.ClassLibrary.Api;

namespace WeatherApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static CurrentWeatherModel.RootObject CurrentWeather { get; set; }
        public ObservableCollection<WeatherDisplayModel> HourlyWeather { get; set; }

        #region Private Fields
        private string _imageSource;
        private string _temperature;
        private string _city;
        private string _date;
        private string _feelsLike;
        private string _sunset;
        private string _searchInput;
        private bool _loaded;
        #endregion

        public MainPageViewModel()
        {
            loaded = false;
            HourlyWeather = new ObservableCollection<WeatherDisplayModel>();

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
            try
            {
                CurrentWeather = await WeatherEndpoints.LoadWeatherByCityName(searchInput);

                var data = await WeatherEndpoints.LoadDailyByCityName(searchInput);

                PopulateDailyWeatherList(data);
                PopulateElements();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            }
        });

        public ICommand LoadCurrentLocation => new Command(async () =>
        {
            try
            {
                LoadByCurrentLocation();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("An error has occured!", e.Message, "OK");
            }
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

        public bool loaded
        {
            get { return _loaded; }
            set
            {
                _loaded = value;

                var args = new PropertyChangedEventArgs(nameof(loaded));
                PropertyChanged?.Invoke(this, args);
            }
        }
        #endregion

        async void LoadByCurrentLocation()
        {
            loaded = false;

            var coordinates = await GeoCoordinatesProcessor.LoadCoordinates();

            var lat = coordinates.Latitude;
            var lon = coordinates.Longitude;

            CurrentWeather = await WeatherEndpoints.LoadWeatherByCoordinates(lon, lat);
            var dailyForecast = await WeatherEndpoints.LoadDailyByCoordinates(lon, lat);

            PopulateElements();
            PopulateDailyWeatherList(dailyForecast);
            
            loaded = true;
        }

        void PopulateElements()
        {
            imageSource = "https://openweathermap.org/img/wn/" + CurrentWeather.weather.First().icon + "@2x.png";
            date = getCorrectDateFormat(DateTime.Now);

            temperature = Math.Round(CurrentWeather.main.temp).ToString();
            city = $"{CurrentWeather.name}, {CurrentWeather.sys.country}";

            feelsLike = "Feels like " + Math.Round(CurrentWeather.main.feels_like).ToString();
            sunset = "Sunset " + ConvertUnixTimestampToTime(CurrentWeather.sys.sunset);
        }

        void PopulateDailyWeatherList(HourlyWeatherModel.RootObject data)
        {
            HourlyWeather.Clear();
            foreach (var hour in data.list)
            {
                var time = ConvertUnixTimestampToTime(hour.dt) + "0";
                var dayAndDate = getCorrectDateFormat(DateTime.Parse(hour.dt_txt));
                var iconSource = "https://openweathermap.org/img/wn/" + hour.weather.First().icon + "@2x.png";
                var temp = Math.Round(hour.main.temp).ToString();

                HourlyWeather.Add(new WeatherDisplayModel { time = time, date = dayAndDate, icon = iconSource, temperature = temp });
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
            var dayName = date.ToString("ddd");
            var dayNumber = date.Day;
            var month = date.ToString("MMM", CultureInfo.InvariantCulture);

            return $"{dayName}, {dayNumber} {month}";
        }
    }
}
