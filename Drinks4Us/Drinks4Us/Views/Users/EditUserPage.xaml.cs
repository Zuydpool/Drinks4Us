using System;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Users
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditUserPage : ContentPage
    {
        private readonly AppUser _appUser;

		public EditUserPage(AppUser appUser)
		{
			InitializeComponent();

            this._appUser = appUser;
            BindingContext = appUser;
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SaveUserButton_OnClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            _appUser.Email = email;
            _appUser.Password = BCrypt.Net.BCrypt.HashPassword(password, App.PasswordHash);

            await App.GetInstance().Storage.Dao.AppUsersDao.Update(_appUser);
            await DisplayAlert("Success", "Successfully updated user!", "Ok!");
            await Navigation.PopModalAsync();
        }
    }
}