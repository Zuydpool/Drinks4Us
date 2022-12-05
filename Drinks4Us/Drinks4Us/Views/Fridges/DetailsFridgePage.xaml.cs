using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsFridgePage : ContentPage
    {
        private readonly Fridge _fridge;

        public DetailsFridgePage(Fridge fridge)
        {
            InitializeComponent();

            this._fridge = fridge;
            BindingContext = fridge;
        }

        private async void EditItemButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditFridgePage(_fridge));
        }

        private async void DeleteItemButton_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Delete", $"Are you sure you want to delete {_fridge.Name}?", "Yes",
                "No");
            if (result)
            {
                var deleteResult = await App.GetInstance().Storage.Dao.FridgeDao.Delete(_fridge.Id);
                if (deleteResult)
                {
                    await DisplayAlert("Success", "Successfully deleted fridge!", "Ok!");
                    await Navigation.PopAsync();
                }

            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        private async void BackButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}