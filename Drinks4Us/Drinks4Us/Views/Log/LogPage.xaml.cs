using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drinks4Us.Views.Log
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableRangeCollection<Models.Log> Items { get; set; }

        public ICommand RefreshCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing{
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public LogPage()
        {
            InitializeComponent();

            Items = new ObservableRangeCollection<Models.Log>();

            RefreshCommand = new Command(ExecuteRefreshCommand);

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            var items = await App.GetInstance().Storage.Dao.LogDao.GetAll();
            Items.Clear();

            Items.AddRange(items);
        }

        private void LogSearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResult = Items.Where(item => item.Message.ToLower().Contains(LogSearchBar.Text.ToLower()));
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
    }
}