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
    public class StocksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Stock> Stocks { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public StocksViewModel()
        {
            Stocks = new ObservableCollection<Stock>();
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
            }
        }

        async Task GetSpeakers()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                var items = await service.GetSpeakers();

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
