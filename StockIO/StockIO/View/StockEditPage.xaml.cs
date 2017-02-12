using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;

using StockIO.Model;
using StockIO.Services;

namespace StockIO.View
{
    public partial class StockEditPage : ContentPage
    {
        Stock preStock;
        Stock stock;
        public StockEditPage(Stock item)
        {
            InitializeComponent();

            this.preStock = item;
            this.stock = item;

            BindingContext = this.stock;

            Name.Completed += Name_Completed;
            Amount.TextChanged += Amount_TextChanged;
            ThresholdAmount.TextChanged += ThresholdAmount_TextChanged;
            UpdateButton.Clicked += OnButtonClicked;

        }

        private void Name_Completed(object sender, EventArgs e)
        {
            stock.Name = ((Entry)sender).Text;
            return;
        }

        private void Amount_TextChanged(object sender, EventArgs e)
        {
            int result;
            bool isValid = int.TryParse(((Entry)sender).Text, out result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
            stock.Amount = isValid ? result : stock.Amount;
            return;
        }

        private void ThresholdAmount_TextChanged(object sender, EventArgs e)
        {
            int result;
            bool isValid = int.TryParse(((Entry)sender).Text, out result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
            stock.ThresholdAmount = isValid ? result : stock.ThresholdAmount;
            return;
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            //if (this.preStock != this.stock)
            //{
                await UpdateStocks();
            //}
        }

        async Task UpdateStocks()
        {
            Exception error = null;
            try
            {
                var service = DependencyService.Get<AzureService>();
                await service.UpdateStock(this.stock);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }

            if (error != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "更新しました", "OK");
            }
                
        }
    }
}
