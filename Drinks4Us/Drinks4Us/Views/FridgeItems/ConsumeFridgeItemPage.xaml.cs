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

        public ConsumeFridgeItemPage(FridgeItem fridgeItem)
        {
            InitializeComponent();
            this._fridgeItem = fridgeItem;

            BindingContext = fridgeItem;
        }

        private void Stepper_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var value = ((Stepper)sender).Value;
            _fridgeItem.Quantity = Convert.ToInt32(value);
            Debug.WriteLine(_fridgeItem.Quantity);
        }
    }
}