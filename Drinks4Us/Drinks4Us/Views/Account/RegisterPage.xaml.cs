using System;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
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

        private async void RegisterProcedure(object sender, EventArgs e)
        {
            var email = EntryEmail.Text;
            var password = EntryPassword.Text;
            if (string.IsNullOrEmpty(email)) return;

            if (string.IsNullOrEmpty(password)) return;


            if (await App.GetInstance().Storage.Dao.AppUsersDao.CheckIfAccountExists(email))
            {
                await DisplayAlert("Email address already in use", "This email address is already being used.", "Ok");
                return;
            }
            ActivityIndicator.IsRunning = true;

            var appUser = new AppUser
            {
                Email = email,
                Password = password,
                RegisterDate = DateTime.Now,
                LastLogin = DateTime.MinValue
            };
            await App.GetInstance().Storage.Dao.AppUsersDao.Add(appUser);
            await DisplayAlert("Success", "You've successfully registered your account.", "Ok");

            await Navigation.PushAsync(new LoginPage());
            ActivityIndicator.IsRunning = false;

            // Clear entry fields
            EntryEmail.Text = "";
            EntryPassword.Text = "";
        }

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}