using Skrapper.Services;
using Skrapper.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }

        public App()
        {
            InitializeComponent();
            SecureStorage.SetAsync("isUserLogged", "0");
            DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();

            try
            {
                string isUserLogged = SecureStorage.GetAsync("isUserLogged").Result;
                if (isUserLogged == "1")
                {
                    MainPage = new AppShell();
                }
                else
                {
                    MainPage = new LoginPage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR: App.xaml.cs] (App) >> " + ex.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
