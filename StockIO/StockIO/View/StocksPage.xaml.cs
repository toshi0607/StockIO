using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using StockIO.ViewModel;

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
        }
    }
}
