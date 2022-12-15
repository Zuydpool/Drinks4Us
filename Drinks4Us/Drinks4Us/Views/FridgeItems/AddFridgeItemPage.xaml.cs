using System;
using System.IO;
using System.Linq;
using Drinks4Us.Exceptions;
using Drinks4Us.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFridgeItemPage : ContentPage
    {
        private readonly Fridge _fridge;

        private FridgeItemBuilder _fridgeItemBuilder = new FridgeItem().ToBuilder();

        public AddFridgeItemPage(Fridge fridge)
        {
            InitializeComponent();
            this._fridge = fridge;

            var currentTheme = Application.Current.RequestedTheme;

            //TakePicture.ImageSource = ImageSource.FromResource(currentTheme == OSAppTheme.Dark ? "outline_photo_camera_black_24dp.png" : "outline_photo_camera_white_24dp.png");
            //SelectPicture.ImageSource = ImageSource.FromResource(currentTheme == OSAppTheme.Dark ? "outline_add_a_photo_white_24dp.png" : "outline_add_a_photo_black_24dp.png");
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var productName = ProductNameEntry.Text;
            var productQuantity = ProductQuantityEntry.Text;
            var productPurchaseDate = ProductPurchaseDate.Date;
            var productExpireDate = ProductExpireDate.Date;

            // Check Validators
            if (ProductNameValidator.IsNotValid) return;
            if (ProductQuantityValidator.IsNotValid) return;

            var allFridgeItems = await App.GetInstance().Storage.Dao.FridgeItemsDao.GetAll();
            var lastFridgeItemId = allFridgeItems.LastOrDefault()?.Id + 1 ?? 1;

            try
            {
                var fridgeItem = _fridgeItemBuilder
                    .Id(lastFridgeItemId)
                    .Name(productName)
                    .Quantity(int.Parse(productQuantity))
                    .PurchaseDate(productPurchaseDate)
                    .ExpireDate(productExpireDate)
                    .Fridge(_fridge)
                    .Build();

                var currentAppUser = App.GetInstance().CurrentAppUser;
                await App.GetInstance().Storage.Dao.FridgeItemsDao.Add(fridgeItem);
                _fridge.FridgeItems.Add(fridgeItem); // Add only after it has been added to the database.
                _fridge.LastFilledBy = currentAppUser;
                await App.GetInstance().Storage.Dao.FridgeDao.Update(_fridge);

                if (currentAppUser != null)
                {
                    await App.GetInstance().Storage.Dao.LogDao.Add(new Models.Log()
                    {
                        DateTime = DateTime.Now,
                        Message =
                            $"{currentAppUser.Email} heeft {fridgeItem.Quantity}x {fridgeItem.Name} toegevoegd aan {_fridge.Name}"
                    });
                }

                await DisplayAlert("Success", "Successfully added item!", "Ok!");
                await Navigation.PopAsync();
            }
            catch (IllegalArgumentException ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
                return;
            }

        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TakePicture_OnClicked(object sender, EventArgs e)
        {
            // Werkt helaas niet https://github.com/xamarin/Essentials/issues/2041
            var picture = await MediaPicker.CapturePhotoAsync();
            if (picture == null) return;

            using var stream = await picture.OpenReadAsync();
            if (stream == null) return;
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
            if (picture == null) return;

            using var stream = await picture.OpenReadAsync();
            if (stream == null) return;
            var newFile =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    picture.FileName); // in CacheDirectory, you could try to save in other folders
            using var newStream = File.OpenWrite(newFile);

            await stream.CopyToAsync(newStream);

            ResultImage.Source = ImageSource.FromFile(newFile);
            _fridgeItemBuilder = _fridgeItemBuilder.ImageUrl(newFile);
        }
    }
}