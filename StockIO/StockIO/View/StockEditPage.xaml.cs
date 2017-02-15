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
            SaveButton.Clicked += OnSaveButtonClicked;
            DeleteButton.Clicked += OnDeleteButtonClicked;

            if (item.ID == null)
                DeleteButton.IsEnabled = false;
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

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            await DeleteStock();
        }

        async Task DeleteStock()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("", "本当に削除しますか？", "はい", "いいえ");
            if (!result)
                return;

            Exception error = null;
            try
            {
                var service = DependencyService.Get<AzureService>();
                await service.DeleteStock(this.stock);
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
                await Application.Current.MainPage.DisplayAlert("", "削除しました。", "OK");
            }

            await Navigation.PushAsync(new StocksPage());
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //if (this.preStock != this.stock)
            //{
                await SaveStock();
            //}
        }

        async Task SaveStock()
        {
            Exception error = null;
            try
            {
                var service = DependencyService.Get<AzureService>();
                await service.SaveStock(this.stock);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }

            var result = false;
            if (error != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
            }
            else
            {
                result = await Application.Current.MainPage.DisplayAlert("", "保存しました。一覧に戻りますか？", "はい", "いいえ");
            }
                
            if (result)
                await Navigation.PushAsync(new StocksPage());
        }
    }
}
