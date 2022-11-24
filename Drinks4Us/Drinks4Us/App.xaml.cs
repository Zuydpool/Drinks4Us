using Drinks4Us.Storage;
using Drinks4Us.Views.Main;
using Xamarin.Forms;

namespace Drinks4Us
{
    public partial class App : Application
    {

        private static App _instance;

        public IStorage Storage { get; } = new StorageFactory().GetInstance();

        public static string PasswordHash = BCrypt.Net.BCrypt.GenerateSalt(12);

        public App()
        {
            _instance = this;
            InitializeComponent();

            MainPage = new NavigationPage(new MainFlyoutPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static App GetInstance()
        {
            return _instance;
        }
    }
}
