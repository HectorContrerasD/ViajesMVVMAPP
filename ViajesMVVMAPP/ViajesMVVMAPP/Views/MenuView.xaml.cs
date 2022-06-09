using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViajesMVVMAPP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentPage
    {
        public MenuView()
        {
            InitializeComponent();
        }
        private async void SwipeItem_Clicked(object sender, EventArgs e)
        {
            var sw = (SwipeItem)sender; //unboxing
            if (await DisplayAlert("Por favor confirme", $"¿Está seguro de eliminar?", "Si", "No") == true)
            {
                vvm.EliminarCommand.Execute(sw.CommandParameter);
            }
        }
    }
}