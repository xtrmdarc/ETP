using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using SQLite;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EdicionNota : Page
    {
        Nota notaSeleccionada;
        Curso cursoActual;
        public EdicionNota()
        {
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            int idNotaSeleccionada = (int)ApplicationData.Current.LocalSettings.Values["idNotaSeleccionada"];
            notaSeleccionada = db.Table<Nota>().Single(p => p.Id == idNotaSeleccionada);
            db.Close();
            this.InitializeComponent();
            this.DataContext = notaSeleccionada;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            double aux;
            MessageDialog message = new MessageDialog("");
            ResourceLoader loader = new ResourceLoader();

            if (puTxtb_NombreNotaEdicion.Text.Trim().Length == 0) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); puTxtb_NombreNotaEdicion.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(puTxtb_PorcentajeNotaEdicion.Text, out aux) == false) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); puTxtb_PorcentajeNotaEdicion.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (aux < 0 || aux > 1.00) { message.Content = loader.GetString("GradePercentageValidation"); message.Title = loader.GetString("AlertText"); puTxtb_PorcentajeNotaEdicion.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(puTxtb_valorNotaEdicion.Text, out aux) == false) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); puTxtb_valorNotaEdicion.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (aux < 0) { message.Content = loader.GetString("GradeGradeValidationInteger"); message.Title = loader.GetString("AlertText"); puTxtb_valorNotaEdicion.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }


            int idNotaEditar = notaSeleccionada.Id;
            
            //dbContext context = new dbContext("isostore:/prom.sdf");
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            //Nota notaAEditarAux = context.Notas.Single(p => p.Id == idNotaEditar);
            cursoActual = db.Table<Curso>().Single(p => p.Id == notaSeleccionada.CursoId);
            cursoActual.PorcentajeProm = cursoActual.PorcentajeProm - notaSeleccionada.Porcentaje;
            cursoActual.PuntosProm = cursoActual.PuntosProm - notaSeleccionada.Puntos;
            notaSeleccionada.Nombre = puTxtb_NombreNotaEdicion.Text;
            notaSeleccionada.Valor = Convert.ToDouble(puTxtb_valorNotaEdicion.Text);
            notaSeleccionada.Porcentaje = Convert.ToDouble(puTxtb_PorcentajeNotaEdicion.Text);
            notaSeleccionada.Puntos = (notaSeleccionada.Valor * notaSeleccionada.Porcentaje) * 1.00;
            cursoActual.PorcentajeProm = cursoActual.PorcentajeProm + notaSeleccionada.Porcentaje;
            cursoActual.PuntosProm = cursoActual.PuntosProm + notaSeleccionada.Puntos;
            db.Update(notaSeleccionada);
            db.Update(cursoActual);
            //context.SubmitChanges();

            Frame.GoBack();

        }

    }
}
