using System;
using System.Diagnostics;
using System.IO;
using Drinks4Us.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFridgeItemPage : ContentPage
    {
        private FridgeItemBuilder _fridgeItemBuilder;

        public EditFridgeItemPage(FridgeItem fridgeItem)
        {
            InitializeComponent();

            this._fridgeItemBuilder = fridgeItem.ToBuilder();
            BindingContext = fridgeItem;
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var productName = ProductNameEntry.Text;
            var productQuantity = ProductQuantityEntry.Text;
            var productPurchaseDate = ProductPurchaseDate.Date;
            var productExpireDate = ProductExpireDate.Date;

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

            var fridgeItem = _fridgeItemBuilder
                .Name(productName)
                .Quantity(int.Parse(productQuantity))
                .PurchaseDate(productPurchaseDate)
                .ExpireDate(productExpireDate)
                .Build();
            Debug.WriteLine(fridgeItem);

            await App.GetInstance().Storage.Dao.FridgeItemsDao.Update(fridgeItem);
            await DisplayAlert("Success", "Successfully updated item!", "Ok!");
            var currentAppUser = App.GetInstance().CurrentAppUser;
            if (currentAppUser != null)
            {
                await App.GetInstance().Storage.Dao.LogDao.Add(new Models.Log()
                {
                    DateTime = DateTime.Now,
                    Message =
                        $"{currentAppUser.Email} heeft {fridgeItem.Quantity}x {fridgeItem.Name} toegevoegd aan {fridgeItem.Fridge.Name}"
                });
            }
            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TakePicture_OnClicked(object sender, EventArgs e)
        {
            // Werkt helaas niet https://github.com/xamarin/Essentials/issues/2041
            var picture = await MediaPicker.CapturePhotoAsync();

            using var stream = await picture.OpenReadAsync();
            if (stream == null) return;
            ResultImage.Source = ImageSource.FromStream(() => stream);
            var newFile =
                Path.Combine(FileSystem.CacheDirectory,
                    picture.FileName); // in CacheDirectory, you could try to save in other folders
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);

            ResultImage.Source = ImageSource.FromStream(() => stream);
            _fridgeItemBuilder = _fridgeItemBuilder.ImageUrl(newFile);
        }

        private async void SelectPicture_OnClicked(object sender, EventArgs e)
        {
            var picture = await MediaPicker.PickPhotoAsync();

            using var stream = await picture.OpenReadAsync();
            if (stream == null) return;
            var newFile =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    picture.FileName); // in CacheDirectory, you could try to save in other folders
            using var newStream = File.OpenWrite(newFile);

            await stream.CopyToAsync(newStream);

            ResultImage.Source = ImageSource.FromStream(() => stream);
            _fridgeItemBuilder = _fridgeItemBuilder.ImageUrl(newFile);
        }
    }
}