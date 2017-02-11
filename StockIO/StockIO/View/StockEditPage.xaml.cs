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
        }
    }
}
