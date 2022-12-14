using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drinks4Us.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumeFridgeItemPage : ContentPage
    {
        private readonly FridgeItem _fridgeItem;
        private int _takeItems;

        public ConsumeFridgeItemPage(FridgeItem fridgeItem)
        {
            InitializeComponent();
            this._fridgeItem = fridgeItem;
            this._takeItems = Convert.ToInt32(QuantityStepper.Value);

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

        private void Stepper_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var value = ((Stepper)sender).Value;
            this._takeItems = Convert.ToInt32(value);
        }

        private async void ConsumeButton_OnClicked(object sender, EventArgs e)
        {
            if (this._takeItems == 0) return;
            _fridgeItem.Quantity -= this._takeItems;

            await App.GetInstance().Storage.Dao.FridgeItemsDao.Update(_fridgeItem);
            var currentAppUser = App.GetInstance().CurrentAppUser;
            if (currentAppUser != null)
            {
                await App.GetInstance().Storage.Dao.LogDao.Add(new Models.Log
                {
                    Message = currentAppUser.Email + " has consumed " + this._takeItems + " " + _fridgeItem.Name,
                    DateTime = DateTime.Now
                });
            }

            await Navigation.PopAsync();
        }
    }
}