using System.Diagnostics;
using System.IO;
using Drinks4Us.Models;
using Drinks4Us.Storage;
using Drinks4Us.Views.Main;
using Xamarin.Forms;

namespace Drinks4Us
{
    public partial class App : Application
    {

        private static App _instance;

        public IStorage Storage { get; }

        public static string PasswordHash = BCrypt.Net.BCrypt.GenerateSalt(12);

        public AppUser? CurrentAppUser;

        public string SQLiteDatabaseLocation;

        public App(string sqliteDatabasePath)
        {
            _instance = this;
            InitializeComponent();
            this.SQLiteDatabaseLocation = sqliteDatabasePath;
            var deleteDb = false;
            if (deleteDb)
            {
                File.Delete(sqliteDatabasePath);
            }


            Storage = new StorageFactory().GetInstance();

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
