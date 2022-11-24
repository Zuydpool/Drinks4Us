using Drinks4Us.Services;
using System;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainFlyoutPage : FlyoutPage
    {
        private readonly WeatherApiService _weatherApiService = new();

        private readonly Timer _oneMinuteTimer = new(60 * 1000);

        public MainFlyoutPage()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshWeather();

            _oneMinuteTimer.Elapsed += UpdateWeather;
            _oneMinuteTimer.Start();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is not MainFlyoutPageFlyoutMenuItem item)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }

        private void UpdateWeather(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(RefreshWeather);
            
        }

        private async void RefreshWeather()
        {
            var currentWeatherCondition = await _weatherApiService.GetCurrentWeatherCondition();

            ToolbarWeatherIcon.IconImageSource =
                ImageSource.FromUri(new Uri(currentWeatherCondition.IconUrl, UriKind.Absolute));
            ToolbarWeatherTemperature.Text = "°" + currentWeatherCondition.Temperature;
        }
    }
}