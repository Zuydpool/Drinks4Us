using System;
using System.Diagnostics;
using Drinks4Us.Views.Main;
using Drinks4Us.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            EntryEmail.Completed += (s, e) => EntryPassword.Focus();
            EntryPassword.Completed += (s, e) => EntryEmail.Focus();
        }

        private async void SignInProcedure(object sender, EventArgs e)
        {
            var email = EntryEmail.Text;
            var password = EntryPassword.Text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Login", "Invalid Credentials", "Ok");
                return;
            }

            var appUser = await App.GetInstance().Storage.Dao.AppUsersDao.GetByEmail(email);
            if (appUser == null)
            {
                await DisplayAlert("Login", "Unknown email address", "Ok");
                return;
            }

            Debug.WriteLine(appUser.Password);

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, appUser.Password);
            if (!isPasswordValid)
            {
                await DisplayAlert("Login", "Email address or password is invalid", "Ok");
                return;
            }

            ActivityIndicator.IsRunning = true;
            App.GetInstance().CurrentAppUser = appUser;

            appUser.LastLogin = DateTime.Now;
            await App.GetInstance().Storage.Dao.AppUsersDao.Update(appUser);

            Navigation.InsertPageBefore(new MainFlyoutPage(), this);
            await Navigation.PopAsync();
            ActivityIndicator.IsRunning = false;

            // Clear entry fields
            EntryEmail.Text = "";
            EntryPassword.Text = "";
        }

        private void ImageButtonShowHidePassword_OnClicked(object sender, EventArgs e)
        {
            var currentTheme = Application.Current.RequestedTheme;

            if (EntryPassword.IsPassword)
            {
                EntryPassword.IsPassword = false;
                ImageButtonShowHidePassword.Source = ImageSource.FromFile(currentTheme == OSAppTheme.Dark ? "outline_visibility_white_24dp.png" : "outline_visibility_black_24dp.png");
            }
            else
            {
                EntryPassword.IsPassword = true;
                ImageButtonShowHidePassword.Source = ImageSource.FromFile(currentTheme == OSAppTheme.Dark ? "outline_visibility_off_white_24dp.png" : "outline_visibility_off_black_24dp.png");
            }
        }

        private async void RegisterButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}