using System;
using System.Linq;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFridgeItemPage : ContentPage
    {
        private readonly Fridge _fridge;

        public AddFridgeItemPage(Fridge fridge)
        {
            InitializeComponent();
            this._fridge = fridge;
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

            var allFridgeItems = await App.GetInstance().Storage.Dao.FridgeItemsDao.GetAll();
            var lastFridgeItemId = allFridgeItems.LastOrDefault()?.Id + 1 ?? 1;

            var fridgeItem = new FridgeItem
            {
                Id = lastFridgeItemId,
                Name = productName,
                Quantity = int.Parse(productQuantity),
                PurchaseDate = productPurchaseDate,
                ExpireDate = productExpireDate,
                ImageUrl = productImageUrl,
                Fridge = _fridge,
            };


            var currentAppUser = App.GetInstance().CurrentAppUser;
            await App.GetInstance().Storage.Dao.FridgeItemsDao.Add(fridgeItem);
            _fridge.FridgeItems.Add(fridgeItem); // Add only after it has been added to the database.
            _fridge.LastFilledBy = currentAppUser;
            await App.GetInstance().Storage.Dao.FridgeDao.Update(_fridge);

            if (currentAppUser != null)
            {
                await App.GetInstance().Storage.Dao.LogDao.Add(new Models.Log()
                {
                    Id = new Random().Next(),
                    DateTime = DateTime.Today,
                    Message =
                        $"{currentAppUser.Email} heeft {fridgeItem.Quantity}x {fridgeItem.Name} toegevoegd aan {_fridge.Name}"
                });
            }

            await DisplayAlert("Success", "Successfully added item!", "Ok!");
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}