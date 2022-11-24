using System;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFridgeItemModal : ContentPage
    {
        public AddFridgeItemModal()
        {
            InitializeComponent();
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var productName = ProductNameEntry.Text;
            var productQuantity = ProductQuantityEntry.Text;
            var productPurchaseDate = ProductPurchaseDate.Date;
            var productExpireDate = ProductExpireDate.Date;
            var productImageUrl = ProductImageUrlEntry.Text;

            // Check Validators
            if (ProductNameValidator.IsNotValid) return;
            if (ProductQuantityValidator.IsNotValid) return;

            if (productImageUrl == null)
            {
                await DisplayAlert("Error", "Product Image Url cannot be empty!", "Ok");
                return;
            }

            var fridgeItem = new FridgeItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = productName,
                Quantity = int.Parse(productQuantity),
                PurchaseDate = productPurchaseDate,
                ExpireDate = productExpireDate,
                ImageUrl = productImageUrl,
            };


            await App.GetInstance().Storage.Dao.FridgeItemsDao.Add(fridgeItem);
            await DisplayAlert("Success", "Successfully added item!", "Ok!");
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}