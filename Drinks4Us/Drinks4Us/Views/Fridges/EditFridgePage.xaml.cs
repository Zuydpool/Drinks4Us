using System;
using System.IO;
using Drinks4Us.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFridgePage : ContentPage
    {
        private FridgeBuilder _fridgeBuilder;
        public EditFridgePage(Fridge fridge)
        {
            InitializeComponent();

            this._fridgeBuilder = fridge.ToBuilder();
            BindingContext = fridge;
        }

        private async void SaveItemButton_OnClicked(object sender, EventArgs e)
        {
            var fridgeName = FridgeNameEntry.Text;

            if (fridgeName == null)
            {
                await DisplayAlert("Error", "Product Name cannot be empty!", "Ok");
                return;
            }

            var fridge = _fridgeBuilder
                .Name(fridgeName)
                .Build();

            await App.GetInstance().Storage.Dao.FridgeDao.Update(fridge);
            await DisplayAlert("Success", "Successfully updated fridge!", "Ok!");
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
            if (picture == null) return;

            using var stream = await picture.OpenReadAsync();
            if (stream == null) return;
            var newFile =
                Path.Combine(FileSystem.CacheDirectory,
                    picture.FileName); // in CacheDirectory, you could try to save in other folders
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);
            ResultImage.Source = ImageSource.FromStream(() => stream);

            _fridgeBuilder = _fridgeBuilder.ImageUrl(newFile);
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
            _fridgeBuilder = _fridgeBuilder.ImageUrl(newFile);
        }
    }
}