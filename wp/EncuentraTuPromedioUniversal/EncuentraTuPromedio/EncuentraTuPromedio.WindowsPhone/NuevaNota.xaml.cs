using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using Windows.Storage;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NuevaNota : Page
    {
        int idCursoActual;
        public NuevaNota()
        {
            this.InitializeComponent();
            idCursoActual = (int)ApplicationData.Current.LocalSettings.Values["idCursoActual"];
             
        }

        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void btnbar_añadirnota_Click(object sender, RoutedEventArgs e)
        {
            double aux;
            MessageDialog message = new MessageDialog("");
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            if (puTxtb_NombreNota.Text.Trim().Length == 0) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); message.ShowAsync(); puTxtb_NombreNota.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(puTxtb_PorcentajeNota.Text, out aux) == false) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); message.ShowAsync(); puTxtb_PorcentajeNota.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (aux < 0 || aux > 1.00) { message.Content = loader.GetString("GradePercentageValidation"); message.Title = loader.GetString("AlertText"); message.ShowAsync(); puTxtb_PorcentajeNota.Focus(Windows.UI.Xaml.FocusState.Programmatic); ; return; }

            if (double.TryParse(puTxtb_valorNota.Text, out aux) == false) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); message.ShowAsync(); puTxtb_valorNota.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (aux < 0) { message.Content = loader.GetString("GradeGradeValidationInteger"); message.Title = loader.GetString("AlertText"); message.ShowAsync(); puTxtb_valorNota.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }


            
            
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Curso cursoActual = db.Table<Curso>().Single(p => p.Id == idCursoActual);
            Nota NuevaNota = new Nota();
            Ciclo cicloActual = db.Table<Ciclo>().Single(p => p.Id == cursoActual.CicloId);
            NuevaNota.CursoId = idCursoActual;
            NuevaNota.Nombre = puTxtb_NombreNota.Text;
            NuevaNota.Valor = Convert.ToDouble(puTxtb_valorNota.Text) * 1.00;
            NuevaNota.Porcentaje = Convert.ToDouble(puTxtb_PorcentajeNota.Text) * 1.00;
            NuevaNota.Puntos = (NuevaNota.Valor * NuevaNota.Porcentaje) * 1.00;
            cursoActual.PorcentajeProm = cursoActual.PorcentajeProm + NuevaNota.Porcentaje;
            cursoActual.PuntosProm = cursoActual.PuntosProm + NuevaNota.Puntos;
            cicloActual.TotalNotas = cicloActual.TotalNotas + 1;
            db.Insert(NuevaNota);
            db.Update(cursoActual);
            db.Update(cicloActual);
            db.Close();

            puTxtb_valorNota.Text = "";
            puTxtb_PorcentajeNota.Text = "";
            Frame.GoBack();
        }
    }
}
