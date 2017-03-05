using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using StockIO.Model;
using Xamarin.Forms;
using System.Diagnostics;
using StockIO.Services;

namespace StockIO.ViewModel
{
    public class StocksViewModel : BindableBase
    {
        public ObservableCollection<Stock> Stocks { get; set; }
        public Command GetStocksCommand { get; set; }
        public StocksViewModel()
        {
            Stocks = new ObservableCollection<Stock>();
            GetStocksCommand = new Command(
                async () => await GetStocks(),
                () => !IsBusy);
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
    }
}
