using Drinks4Us.Models;
using Drinks4Us.Views.FridgeItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Fridges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgesPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableRangeCollection<Fridge> Items { get; set; }

        public ICommand RefreshCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing{
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public FridgesPage()
        {
            InitializeComponent();

            Items = new ObservableRangeCollection<Fridge>();

            RefreshCommand = new Command(ExecuteRefreshCommand);

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            Items.Clear();
            try
            {
                var allFridges = await App.GetInstance().Storage.Dao.FridgeDao.GetAll();

                Items.AddRange(allFridges);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void FridgesCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Fridge fridge) return;
            // Deselect selected item
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushAsync(new FridgeItemsPage(fridge));
        }

        private void FridgesSearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResult = Items.Where(item => item.Name.ToLower().Contains(FridgesSearchBar.Text.ToLower()));
            FridgeItemsCollectionView.ItemsSource = searchResult;
        }

        private async void AddFridgeButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFridgePage());
        }

        private async void AddFridgeToolbarItem_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFridgePage());
        }

        private async void ViewFridgeSwipeItem_OnInvoked(object sender, EventArgs e)
        {
            if (sender is not SwipeItem { BindingContext: Fridge fridge }) return;
            await Navigation.PushAsync(new DetailsFridgePage(fridge));
        }

        private async void EditFridgeSwipeItem_OnInvoked(object sender, EventArgs e)
        {
            if (sender is not SwipeItem { BindingContext: Fridge fridge }) return;
            await Navigation.PushAsync(new EditFridgePage(fridge));
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
    }
}