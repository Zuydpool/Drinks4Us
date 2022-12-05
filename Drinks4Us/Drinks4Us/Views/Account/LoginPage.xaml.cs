using System;
using System.Diagnostics;
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
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

            App.GetInstance().CurrentAppUser = appUser;
            await DisplayAlert("Login", "Login Success", "Ok");

            await Navigation.PushAsync(new HomePage());
        }

        private void ImageButtonShowHidePassword_OnClicked(object sender, EventArgs e)
        {
            if (EntryPassword.IsPassword)
            {
                EntryPassword.IsPassword = false;
                ImageButtonShowHidePassword.Source = ImageSource.FromFile("visibility.png");
            }
            else
            {
                EntryPassword.IsPassword = true;
                ImageButtonShowHidePassword.Source = ImageSource.FromFile("visibility_off.png");
            }
        }
    }
}