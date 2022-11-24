using System;
using System.Diagnostics;
using System.Linq;
using Drinks4Us.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgeItemsPage : ContentPage
    {
        public ObservableRangeCollection<FridgeItem> Items { get; set; }

        public FridgeItemsPage()
        {
            InitializeComponent();

            Items = new ObservableRangeCollection<FridgeItem>();

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var allFridgeItems = await App.GetInstance().Storage.Dao.FridgeItemsDao.GetAll();
            Debug.WriteLine(allFridgeItems.Count);

            var itemsToAdd = allFridgeItems.Where(item => !Items.Contains(item)).ToList();
            Debug.WriteLine("itemsToAdd: " + itemsToAdd.Count);

            Items.AddRange(itemsToAdd);
        }

        private async void AddItemButton_OnClicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new AddFridgeItemModal())); With Toolbar etc
            await Navigation.PushModalAsync(new AddFridgeItemModal());
        }

        private async void FridgeItemsCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not FridgeItem fridgeItem) return;
            // Deselect selected item
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushModalAsync(new DetailsFridgeItemModal(fridgeItem));
        }

        private void FridgeItemsSearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResult = Items.Where(item => item.Name.ToLower().Contains(FridgeItemsSearchBar.Text.ToLower()));
            FridgeItemsCollectionView.ItemsSource = searchResult;
        }
    }
}