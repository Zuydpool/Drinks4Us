using System;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFridgeItemPage : ContentPage
    {
        private readonly FridgeItem _fridgeItem;

        public EditFridgeItemPage(FridgeItem fridgeItem)
        {
            InitializeComponent();

            this._fridgeItem = fridgeItem;
            BindingContext = fridgeItem;
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var productName = ProductNameEntry.Text;
            var productQuantity = ProductQuantityEntry.Text;
            var productPurchaseDate = ProductPurchaseDate.Date;
            var productExpireDate = ProductExpireDate.Date;
            var productImageUrl = ProductImageUrlEntry.Text;

            if (productName == null)
            {
                await DisplayAlert("Error", "Product Name cannot be empty!", "Ok");
                return;
            }

            if (productQuantity == null)
            {
                await DisplayAlert("Error", "Product Quantity cannot be empty!", "Ok");
                return;
            }

            if (productImageUrl == null)
            {
                await DisplayAlert("Error", "Product Image Url cannot be empty!", "Ok");
                return;
            }

            _fridgeItem.Name = productName;
            _fridgeItem.Quantity = int.Parse(productQuantity);
            _fridgeItem.PurchaseDate = productPurchaseDate;
            _fridgeItem.ExpireDate = productExpireDate;
            _fridgeItem.ImageUrl = productImageUrl;


            await App.GetInstance().Storage.Dao.FridgeItemsDao.Update(_fridgeItem);
            await DisplayAlert("Success", "Successfully updated item!", "Ok!");
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}