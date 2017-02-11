using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using StockIO.Model;

namespace StockIO.View
{
    public partial class StockEditPage : ContentPage
    {
        Stock stock;
        public StockEditPage(Stock item)
        {
            InitializeComponent();

            this.stock = item;

            BindingContext = this.stock;

            Name.Completed += Name_Completed;
            Amount.TextChanged += Amount_TextChanged;
            ThresholdAmount.TextChanged += ThresholdAmount_TextChanged;

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
    }
}
