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
    class ShoppingListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Stock> Stocks { get; set; }
        public Command GetStocksCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ShoppingListViewModel()
        {
            Stocks = new ObservableCollection<Stock>();
            GetStocksCommand = new Command(
                async () => await GetStocks(),
                () => !IsBusy);
        }

        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                //Update the can execute
                GetStocksCommand.ChangeCanExecute();
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
    }
}
