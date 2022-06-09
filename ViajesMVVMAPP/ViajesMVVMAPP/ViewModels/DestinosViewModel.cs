using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ViajesMVVMAPP.Models;
using Xamarin.Forms;
using ViajesMVVMAPP.Views;
using System.IO;
using Newtonsoft.Json;
using ViajesMVVMAPP;

namespace ViajesMVVM.ViewModels
{
    public class DestinosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Destinos> Viajes { get; set; } = new ObservableCollection<Destinos>();
        public Destinos DestinoViaje { get; set; }
        public string Error { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand DetallesCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public DestinosViewModel()
        {
            CargarInfo();
            CambiarVistaCommand = new Command<string>(CambiarVista);
            AgregarCommand = new Command(Agregar);
            EliminarCommand = new Command<Destinos>(Eliminar);
            DetallesCommand = new Command<Destinos>(MostrarDetalles);
            EditarCommand = new Command<Destinos>(Editar);
            GuardarCommand = new Command(GuardarDatos);
        }

        AgregarDestinoView agregarDestino;
        DetallesViajeView detallesViaje;
        EditarDestinoView editarDestino;

        int indice;
        private void ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(DestinoViaje.NombreLugar))
            {
                Error = "Escriba el nombre del lugar";
                PropertyChange();
                return;

            }
            if (string.IsNullOrWhiteSpace(DestinoViaje.Descripcion))
            {
                Error = "Agregue una descripción de su destino";
                PropertyChange();
                return;
            }
            if (string.IsNullOrWhiteSpace(DestinoViaje.Ciudad))
            {
                Error = "Agregue la ciudad de su destino";
                PropertyChange();
                return;
            }
            if (string.IsNullOrWhiteSpace(DestinoViaje.Pais))
            {
                Error = "Agregue el país de su destino";
                PropertyChange();
                return;
            }
            if (DestinoViaje.PresupuestoRequerido < 0)
            {
                Error = "El presupuesto requerido no puede ser menor a $0.00";
                PropertyChange();
                return;
            }
            if (string.IsNullOrWhiteSpace(DestinoViaje.ImagenLugar))
            {
                Error = "Agregue el URL de su imágen";
                PropertyChange();
                return;
            }
            if (!Uri.TryCreate(DestinoViaje.ImagenLugar, UriKind.Absolute, out var uri))
            {
                Error = "Agregue una URL válida";
                PropertyChange();
                return;
            }
        }
        private void GuardarDatos(object obj)
        {
            if (DestinoViaje != null)
            {
                Error = "";
                ValidarDatos();
                if (string.IsNullOrWhiteSpace(Error))
                {
                    Viajes[indice] = DestinoViaje;
                    GuardarInfo();
                    App.Current.MainPage.Navigation.PopToRootAsync();
                }
            }
        }

        private void Editar(Destinos destinos)
        {
            indice = Viajes.IndexOf(destinos);
            DestinoViaje = new Destinos
            {
                ImagenLugar = destinos.ImagenLugar,
                NombreLugar = destinos.NombreLugar,
                Ciudad = destinos.Ciudad,
                Pais = destinos.Pais,
                Descripcion = destinos.Descripcion,
                PresupuestoRequerido = destinos.PresupuestoRequerido
            };
            editarDestino = new EditarDestinoView()
            {

                BindingContext = this
            };
            App.Current.MainPage.Navigation.PushAsync(editarDestino);
        }

        private void MostrarDetalles(Destinos destinos)
        {
            detallesViaje = new DetallesViajeView()
            {
                BindingContext = destinos
            };
            App.Current.MainPage.Navigation.PushAsync(detallesViaje);

        }


        private void Eliminar(Destinos destinos)
        {
            if (destinos != null)
            {
                Viajes.Remove(destinos);
                GuardarInfo();

            }
        }

        private void Agregar(object obj)
        {
            if (DestinoViaje != null)
            {
                Error = "";
                ValidarDatos();
                if (string.IsNullOrWhiteSpace(Error))
                {
                    Viajes.Add(DestinoViaje);
                    GuardarInfo();
                    CambiarVista("Ver");
                }
            }
            PropertyChange();
        }

        private void CambiarVista(string vista)
        {
            if (vista == "Agregar")
            {
                Error = "";
                DestinoViaje = new Destinos();
                agregarDestino = new AgregarDestinoView()
                {
                    BindingContext = this
                };
                App.Current.MainPage.Navigation.PushAsync(agregarDestino);

            }
            else if (vista == "Ver")
            {
                App.Current.MainPage.Navigation.PopToRootAsync();
            }

        }
        private void GuardarInfo()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "destino.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(Viajes));
        }
        private void CargarInfo()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "destino.json";
            if (File.Exists(file))
            {
                Viajes = JsonConvert.DeserializeObject<ObservableCollection<Destinos>>(File.ReadAllText(file));
            }
        }
        private void PropertyChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}