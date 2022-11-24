using Drinks4Us.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsUserPage : ContentPage
    {
        private readonly AppUser _appUser;

        public DetailsUserPage(AppUser appUser)
        {
            InitializeComponent();

            this._appUser = appUser;

            BindingContext = appUser;
        }

        private async void BackButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void EditUserButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditUserPage(_appUser));
        }

        private async void DeleteUserButton_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Delete", $"Are you sure you want to delete {_appUser.Email}?", "Yes",
                "No");
            if (result)
            {
                var deleteResult = await App.GetInstance().Storage.Dao.AppUsersDao.Delete(_appUser.Id);
                if (deleteResult)
                {
                    await DisplayAlert("Success", "Successfully deleted user!", "Ok!");
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                await Navigation.PopModalAsync();
            }
        }
    }
}