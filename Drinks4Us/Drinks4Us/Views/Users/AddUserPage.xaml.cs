using Drinks4Us.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveUserButton_OnClicked(object sender, EventArgs e)
        {
            var emailAddress = EmailEntry.Text;
            var password = PasswordEntry.Text;

            var appUser = new AppUser
            {
                Id = 1,
                Email = emailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(password, App.PasswordHash)
            };


            await App.GetInstance().Storage.Dao.AppUsersDao.Add(appUser);
            await DisplayAlert("Success", "Successfully added user!", "Ok!");

            await Navigation.PopAsync();
        }
    }
}