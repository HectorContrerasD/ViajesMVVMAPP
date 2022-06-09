using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ViajesMVVMAPP.Views;

namespace ViajesMVVMAPP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new MenuView());
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
