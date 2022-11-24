using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private async void RegisterProcedure(object sender, EventArgs e)
        {
            var email = EntryEmail.Text;
            var password = EntryPassword.Text;
            if (string.IsNullOrEmpty(email)) return;

            if (string.IsNullOrEmpty(password)) return;



            await Navigation.PushAsync(new LoginPage());
        }
    }
}