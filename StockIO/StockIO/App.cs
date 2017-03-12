using System.Threading.Tasks;

using StockIO.View;

using Xamarin.Forms;

namespace StockIO
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
    public class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
        
        public App()
        {
            // The root page of your application
            var content = new TopTabbedPage();

            MainPage = new NavigationPage(content);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
