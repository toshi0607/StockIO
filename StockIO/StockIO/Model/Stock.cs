﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockIO.Model
{
    public class Stock
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int Amount { get; set; }
        public int ThresholdAmount { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }
    }
}
