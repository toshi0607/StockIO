using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xamarin.Forms;

using StockIO.Model;
using StockIO.Services;

namespace StockIO.ViewModel
{
    class ShoppingListViewModel : BindableBase
    {
        public ObservableCollection<Stock> Stocks { get; set; }
        public Command GetStocksCommand { get; set; }
        public Command IncrementStockCommand { get; set; }

        public ShoppingListViewModel()
        {
            Stocks = new ObservableCollection<Stock>();
            GetStocksCommand = new Command(
                async () => await GetStocks(),
                () => !IsBusy);
            IncrementStockCommand = new Command(
                async (item) => await IncrementStock((Stock)item),
                (_) => !IsBusy);
            IsEmpty = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                SetProperty(ref busy, value);
                OnPropertyChanged();
                //Update the can execute
                GetStocksCommand.ChangeCanExecute();
                IncrementStockCommand.ChangeCanExecute();
            }
        }

        bool empty;
        public bool IsEmpty
        {
            get { return empty; }
            set
            {
                empty = value;
                OnPropertyChanged();
            }
        }

        async Task GetStocks()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                var items = await service.GetStocks();
                items = items.Where(item => item.Amount <= item.ThresholdAmount);

                Stocks.Clear();
                foreach (var item in items)
                    Stocks.Add(item);

                if (items.Count() == 0)
                    IsEmpty = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        async Task IncrementStock(Stock item)
        {
            item.Amount += 1;

            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                await service.SaveStock(item);

                IsBusy = false;
                await GetStocks();

                if (error != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
