using Xamarin.Forms;

using StockIO.ViewModel;
using StockIO.Model;

namespace StockIO.View
{
    public partial class StockEditPage : ContentPage
    {
        StockEditViewModel vm;

        public StockEditPage(Stock item)
        {
            InitializeComponent();

            vm = new StockEditViewModel(item);

            vm.OnUpdate += async () =>
            {
                await Navigation.PushAsync(new StocksPage());
            };
            vm.OnDelete += async () =>
            {
                await Navigation.PushAsync(new StocksPage());
            };

            BindingContext = vm;

            if (item.ID == null)
                DeleteButton.IsEnabled = false;
        }

    }
}
