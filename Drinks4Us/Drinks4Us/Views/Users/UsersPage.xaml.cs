using System;
using System.Linq;
using Drinks4Us.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        public ObservableRangeCollection<AppUser> Items { get; set; }

        public UsersPage()
        {
            InitializeComponent();

            Items = new ObservableRangeCollection<AppUser>();

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var allAppUsers = await App.GetInstance().Storage.Dao.AppUsersDao.GetAll();
            Items.Clear();


            Items.AddRange(allAppUsers);
        }

        private async void UsersCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not AppUser appUser) return;
            // Deselect selected item
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushAsync(new DetailsUserPage(appUser));
        }

        private async void AddUserButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage());
        }
    }
}