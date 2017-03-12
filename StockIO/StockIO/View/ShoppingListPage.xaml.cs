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
        // Track whether the user has authenticated.
        bool authenticated = false;
        ShoppingListViewModel vm;
        public ShoppingListPage()
        {
            InitializeComponent();

            vm = new ShoppingListViewModel();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            if (authenticated == true)
                vm.GetStocksCommand.Execute("");
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
                vm.GetStocksCommand.Execute("");
        }
    }
}
