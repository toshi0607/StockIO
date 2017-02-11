using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using StockIO.ViewModel;
using StockIO.Model;

namespace StockIO.View
{
    public partial class StocksPage : ContentPage
    {
        StocksViewModel vm;
        public StocksPage()
        {
            InitializeComponent();

            vm = new StocksViewModel();

            BindingContext = vm;

            ListViewStocks.ItemSelected += ListViewStocks_ItemSelected;
        }

        private async void ListViewStocks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var stock = e.SelectedItem as Stock;
            if (stock == null)
                return;

            await Navigation.PushAsync(new StockEditPage(stock));

            ListViewStocks.SelectedItem = null;

        }
    }
}
