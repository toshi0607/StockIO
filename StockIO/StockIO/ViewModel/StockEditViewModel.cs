using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xamarin.Forms;

using StockIO.Services;
using StockIO.Model;

namespace StockIO.ViewModel
{
    class StockEditViewModel : INotifyPropertyChanged
    {
        public Stock Stock { set; get; }
        public Command SaveStockCommand { get; set; }
        public Command DeleteStockCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Action OnUpdate;
        public Action OnDelete;

        public StockEditViewModel(Stock stock)
        {
            Stock = stock;
            SaveStockCommand = new Command(
                async () => await SaveStock(),
                () => !IsBusy);
            DeleteStockCommand = new Command(
                async () => await DeleteStock(),
                () => !IsBusy);
        }

        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                //Update the can execute
                SaveStockCommand.ChangeCanExecute();
                DeleteStockCommand.ChangeCanExecute();
            }
        }

        async Task SaveStock()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                await service.SaveStock(Stock);

                var result = false;
                if (error != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
                }
                else
                {
                    IsBusy = false;
                    result = await Application.Current.MainPage.DisplayAlert("", "保存しました。一覧に戻りますか？", "はい", "いいえ");
                }

                if (result)
                    OnUpdate?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task DeleteStock()
        {
            if (IsBusy)
                return;

            bool result = await Application.Current.MainPage.DisplayAlert("", "本当に削除しますか？", "はい", "いいえ");
            if (!result)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                await service.DeleteStock(Stock);

                if (error != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
                }
                else
                {
                    IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert("", "削除しました。", "OK");
                }

                OnDelete?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
