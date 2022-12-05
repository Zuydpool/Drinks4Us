using System;
using System.Linq;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFridgePage : ContentPage
    {
        public AddFridgePage()
        {
            InitializeComponent();
        }

        private async void SaveFridgeButton_OnClicked(object sender, EventArgs e)
        {
            var fridgeName = FridgeNameEntry.Text;
            var fridgeImageUrl = FridgeImageUrlEntry.Text;

            // Check Validators
            if (FridgeNameValidator.IsNotValid) return;

            if (fridgeImageUrl == null)
            {
                await DisplayAlert("Error", "Product Image Url cannot be empty!", "Ok");
                return;
            }

            var allFridges = await App.GetInstance().Storage.Dao.FridgeDao.GetAll();
            var lastFridgeId = allFridges.LastOrDefault()?.Id + 1 ?? 1;

            var fridge = new Fridge
            {
                Id = lastFridgeId,
                Name = fridgeName,
                ImageUrl = fridgeImageUrl,
            };


            await App.GetInstance().Storage.Dao.FridgeDao.Add(fridge);
            await DisplayAlert("Success", "Successfully added fridge!", "Ok!");
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}