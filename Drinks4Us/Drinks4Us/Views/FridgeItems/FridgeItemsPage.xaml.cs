using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Drinks4Us.Models;
using Drinks4Us.Views.Fridges;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.FridgeItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgeItemsPage : ContentPage, INotifyPropertyChanged
    {
        private readonly Fridge _fridge;
        public ObservableRangeCollection<FridgeItem> Items { get; set; }

        public ICommand RefreshCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing{
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public FridgeItemsPage(Fridge fridge)
        {
            InitializeComponent();
            this._fridge = fridge;

            Items = new ObservableRangeCollection<FridgeItem>();

            RefreshCommand = new Command(ExecuteRefreshCommand);

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            var fridge = await App.GetInstance().Storage.Dao.FridgeDao.GetById(_fridge.Id);
            if (fridge == null) return;
            Items.Clear();

            var allFridgeItems = fridge.FridgeItems;
            //Debug.WriteLine(allFridgeItems.Count);

            //var itemsToAdd = allFridgeItems.Where(item => !Items.Contains(item)).ToList();
            //Debug.WriteLine("itemsToAdd: " + itemsToAdd.Count);

            //var fridgeItem = allFridgeItems.FirstOrDefault();
            //Debug.WriteLine(fridgeItem);
            //Debug.WriteLine(fridgeItem?.IsExpiringSoon(DateTime.Today, DateTime.Today.AddDays(3), false));
            //Debug.WriteLine(fridgeItem?.GetExpiringSoonMessage(DateTime.Today));

            /*if (fridgeItem != null)
            {
                var isExpired = fridgeItem.IsExpired(DateTime.Today, false);
                var isExpiringSoon = fridgeItem.IsExpiringSoon(DateTime.Today, DateTime.Today.AddDays(3), false);
                Debug.WriteLine("isExpired: " + isExpired);
                Debug.WriteLine("isExpiringSoon: " + isExpiringSoon);
                if (isExpiringSoon)
                {
                    var expiringSoonMessage = fridgeItem?.GetExpiringSoonMessage(DateTime.Today);
                    Debug.WriteLine(expiringSoonMessage);
                }

                if (isExpired)
                {
                    var expiredMessage = fridgeItem.GetExpiredMessage(DateTime.Today);
                    Debug.WriteLine(expiredMessage);
                }
            }*/

            Items.AddRange(allFridgeItems);
        }

        private async void AddItemButton_OnClicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new AddFridgeItemModal())); With Toolbar etc
            await Navigation.PushAsync(new AddFridgeItemPage(_fridge));
        }

        private async void FridgeItemsCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not FridgeItem fridgeItem) return;
            // Deselect selected item
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushAsync(new ConsumeFridgeItemPage(fridgeItem));
        }

        private void FridgeItemsSearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResult = Items.Where(item => item.Name.ToLower().Contains(FridgeItemsSearchBar.Text.ToLower()));
            FridgeItemsCollectionView.ItemsSource = searchResult;
        }

        void ExecuteRefreshCommand()
        {
            Debug.WriteLine("ExecuteRefresh");
            if (IsRefreshing)
                return;

            IsRefreshing = true;

            Items.Clear();
            OnAppearing();

            // Stop refreshing
            IsRefreshing = false;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void EditFridgeItemSwipeItem_OnInvoked(object sender, EventArgs e)
        {
            if (sender is not SwipeItem { BindingContext: FridgeItem fridgeItem }) return;
            await Navigation.PushAsync(new EditFridgeItemPage(fridgeItem));
        }

        private async void ViewFridgeItemSwipeItem_OnInvoked(object sender, EventArgs e)
        {
            if (sender is not SwipeItem { BindingContext: FridgeItem fridgeItem }) return;
            await Navigation.PushAsync(new DetailsFridgeItemPage(fridgeItem));
        }
    }
}