using Drinks4Us.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await Navigation.PopModalAsync();
        }

        private async void SaveUserButton_OnClicked(object sender, EventArgs e)
        {
            var emailAddress = EmailEntry.Text;
            var password = PasswordEntry.Text;

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = emailAddress,
                Password = password
            };


            await App.GetInstance().Storage.Dao.AppUsersDao.Add(appUser);
            await DisplayAlert("Success", "Successfully added user!", "Ok!");

            await Navigation.PopModalAsync();
        }
    }
}