using System;
using System.IO;
using System.Linq;
using Drinks4Us.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFridgePage : ContentPage
    {

        private FridgeBuilder _fridgeBuilder = new Fridge().ToBuilder();
        public AddFridgePage()
        {
            InitializeComponent();
        }

        private async void SaveFridgeButton_OnClicked(object sender, EventArgs e)
        {
            var fridgeName = FridgeNameEntry.Text;

            // Check Validators
            if (FridgeNameValidator.IsNotValid) return;

            var allFridges = await App.GetInstance().Storage.Dao.FridgeDao.GetAll();
            var lastFridgeId = allFridges.LastOrDefault()?.Id ?? 0;

            var fridge = _fridgeBuilder
                .Id(lastFridgeId + 1)
                .Name(fridgeName)
                .Build();

            await App.GetInstance().Storage.Dao.FridgeDao.Add(fridge);
            await DisplayAlert("Success", "Successfully added fridge!", "Ok!");
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