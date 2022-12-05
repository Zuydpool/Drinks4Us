using System;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFridgePage : ContentPage
    {
        private readonly Fridge _fridge;

        public EditFridgePage(Fridge fridge)
        {
            InitializeComponent();

            this._fridge = fridge;
            BindingContext = fridge;
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var fridgeName = FridgeNameEntry.Text;
            var fridgeImageUrl = FridgeImageUrlEntry.Text;

            if (fridgeName == null)
            {
                await DisplayAlert("Error", "Product Name cannot be empty!", "Ok");
                return;
            }

            if (fridgeImageUrl == null)
            {
                await DisplayAlert("Error", "Product Image Url cannot be empty!", "Ok");
                return;
            }

            _fridge.Name = fridgeName;
            _fridge.ImageUrl = fridgeImageUrl;


            await App.GetInstance().Storage.Dao.FridgeDao.Update(_fridge);
            await DisplayAlert("Success", "Successfully updated fridge!", "Ok!");
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}