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
    public partial class ShoppingListPage : ContentPage
    {
        ShoppingListViewModel vm;
        public ShoppingListPage()
        {
            InitializeComponent();

            vm = new ShoppingListViewModel();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.GetStocksCommand.Execute("");
        }
    }
}
