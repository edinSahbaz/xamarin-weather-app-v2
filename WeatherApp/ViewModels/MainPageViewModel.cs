namespace WeatherApp.ViewModels
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            ApiHelper.InitializeClient();
        }

        public string Title
        {
            get => "Weather Forecast App";
        }
    }
}
