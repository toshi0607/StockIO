using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using StockIO.Model;


namespace StockIO.ViewModel
{
    class StocksViewModel : INotifyPropertyChanged
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
    }
}
