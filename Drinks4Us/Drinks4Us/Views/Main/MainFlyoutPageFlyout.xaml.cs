using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Drinks4Us.Views.Account;
using Drinks4Us.Views.Fridges;
using Drinks4Us.Views.Log;
using Drinks4Us.Views.Pages;
using Drinks4Us.Views.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainFlyoutPageFlyout : ContentPage
    {
        public ListView ListView;

        public MainFlyoutPageFlyout()
        {
            InitializeComponent();

            BindingContext = new MainFlyoutPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class MainFlyoutPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainFlyoutPageFlyoutMenuItem> MenuItems { get; set; }

            public MainFlyoutPageFlyoutViewModel()
            {
                var currentTheme = Application.Current.RequestedTheme;

                MenuItems = new ObservableCollection<MainFlyoutPageFlyoutMenuItem>(new[]
                {
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 0, Title = "Home", TargetType = typeof(HomePage), IconSource =
                            (currentTheme == OSAppTheme.Dark
                                ? "outline_dashboard_white_24dp"
                                : "outline_dashboard_black_24dp")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 1, Title = "Users", TargetType = typeof(UsersPage), IconSource =
                            (currentTheme == OSAppTheme.Dark
                                ? "outline_manage_accounts_white_24dp.png"
                                : "outline_manage_accounts_black_24dp.png")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 2, Title = "Fridges", TargetType = typeof(FridgesPage), IconSource =
                            (currentTheme == OSAppTheme.Dark
                                ? "outline_fastfood_white_24dp.png"
                                : "outline_fastfood_black_24dp.png")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 3, Title = "Log", TargetType = typeof(LogPage),
                        IconSource = (currentTheme == OSAppTheme.Dark
                            ? "outline_article_white_24dp.png"
                            : "outline_article_black_24dp.png")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 4, Title = "Login", TargetType = typeof(LoginPage), IconSource =
                            (currentTheme == OSAppTheme.Dark
                                ? "outline_account_circle_white_24dp.png"
                                : "outline_account_circle_black_24dp.png")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 5, Title = "Registeren", TargetType = typeof(RegisterPage), IconSource =
                            (currentTheme == OSAppTheme.Dark
                                ? "outline_edit_white_24dp.png"
                                : "outline_edit_black_24dp.png")
                    },
                    new MainFlyoutPageFlyoutMenuItem
                    {
                        Id = 6, Title = "Uitloggen", TargetType = typeof(LoginPage), StyleClass = "MenuItemLayoutStyle",
                        CustomAction = Logout,
                        IconSource = (currentTheme == OSAppTheme.Dark
                            ? "outline_logout_white_24dp.png"
                            : "outline_logout_black_24dp.png")
                    }
                });
            }

            private void Logout()
            {
                Application.Current.Properties["Token"] = "";
                //await Navigation.PushAsync(new LoginPage());
            }

            #region INotifyPropertyChanged Implementation

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }
    }
}