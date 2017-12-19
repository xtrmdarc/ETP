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
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EdicionCurso : Page
    {
        Curso cursoActual;
        Ciclo cicloActual;
        public EdicionCurso()
        {
            int idCursoActual;
            idCursoActual = (int)ApplicationData.Current.LocalSettings.Values["idCursoActual"];
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            cursoActual = db.Table<Curso>().Single(p => p.Id == idCursoActual);
            db.Close();
            this.InitializeComponent();
            this.DataContext = cursoActual;
             
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnbar_guardaEdicion_Click(object sender, RoutedEventArgs e)
        {
            int aux;
            MessageDialog message = new MessageDialog("");
            ResourceLoader loader = new ResourceLoader();

            if (editaCursoNombre.Text.Trim().Length == 0) { message.Content = loader.GetString("CourseNameValidation"); message.Title = loader.GetString("AlertText"); editaCursoNombre.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (puEditaCurso_creditos_txtb.Text.Trim().Length == 0) { message.Content = loader.GetString("CourseCreditsValidation"); message.Title = loader.GetString("AlertText"); puEditaCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (int.TryParse(puEditaCurso_creditos_txtb.Text, out aux) == false) { message.Content = loader.GetString("CourseCreditsValidationInteger"); message.Title = loader.GetString("AlertText"); puEditaCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux <= 0) { message.Content = loader.GetString("CourseCreditsValidationInteger"); message.Title = loader.GetString("AlertText"); ; puEditaCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            
            Curso CursoAEditaruax = db.Table<Curso>().Single(p => p.Id == cursoActual.Id);
            cicloActual = db.Table<Ciclo>().Single(p => p.Id == cursoActual.CicloId);
            cicloActual.TotalCreditos = cicloActual.TotalCreditos - CursoAEditaruax.Creditos;
            CursoAEditaruax.Nombre = editaCursoNombre.Text;
            CursoAEditaruax.Creditos = Convert.ToInt32(puEditaCurso_creditos_txtb.Text);
            cicloActual.TotalCreditos = cicloActual.TotalCreditos + CursoAEditaruax.Creditos;
            db.Update(cicloActual);
            db.Update(CursoAEditaruax);
            db.Close();

            Frame.GoBack();
        }
    }
}
