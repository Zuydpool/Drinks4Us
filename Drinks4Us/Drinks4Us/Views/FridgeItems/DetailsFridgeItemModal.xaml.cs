using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsFridgeItemModal : ContentPage
    {
        private readonly FridgeItem _fridgeItem;

        public DetailsFridgeItemModal(FridgeItem fridgeItem)
        {
            InitializeComponent();

            this._fridgeItem = fridgeItem;
            BindingContext = fridgeItem;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_fridgeItem.IsExpired(DateTime.Now, false))
            {
                await DisplayAlert("Expired", _fridgeItem.GetExpiredMessage(DateTime.Now), "Ok");
            }

        }

        private async void EditItemButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditFridgeItemPageModal(_fridgeItem));
        }

        private async void DeleteItemButton_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Delete", $"Are you sure you want to delete {_fridgeItem.Name}?", "Yes",
                "No");
            if (result)
            {
                var deleteResult = await App.GetInstance().Storage.Dao.FridgeItemsDao.Delete(_fridgeItem.Id);
                if (deleteResult)
                {
                    await DisplayAlert("Success", "Successfully deleted item!", "Ok!");
                    await Navigation.PopModalAsync();
                }

            }
            else
            {
                await Navigation.PopModalAsync();
            }
        }

        private async void BackButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}