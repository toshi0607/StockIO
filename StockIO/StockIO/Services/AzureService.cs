﻿using StockIO.Services;
using StockIO.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;

[assembly: Dependency(typeof(AzureService))]
namespace StockIO.Services
{
    public class AzureService
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Stock> table;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://stockio.azurewebsites.net";

            //Create our client
            //Client = new MobileServiceClient(appUrl);

            Client = new MobileServiceClient("https://stockio.azurewebsites.net");

        //InitialzeDatabase for path
        var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);


            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<Stock>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            table = Client.GetSyncTable<Stock>();
        }

        public async Task<IEnumerable<Stock>> GetStocks()
        {
            await Initialize();
            await SyncStocks();
            return await table.OrderBy(s => s.Name).ToEnumerableAsync();
        }


        public async Task SyncStocks()
        {
            try
            {
                await Client.SyncContext.PushAsync();
                await table.PullAsync("allStocks", table.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync stocks, that is alright as we have offline capabilities: " + ex);
            }

        }
    }
}