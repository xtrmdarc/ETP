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
    public sealed partial class NuevoCurso : Page
    {
     
        public NuevoCurso()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
         
        }

        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
        
        

        private void btnbar_creaCurso_Click(object sender, RoutedEventArgs e)
        {
            int aux;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            MessageDialog message = new MessageDialog("");


            if (nuevoCursoNombre.Text.Trim().Length == 0) { message.Content = loader.GetString("CourseNameValidation"); message.ShowAsync(); nuevoCursoNombre.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (puNuevoCurso_creditos_txtb.Text.Trim().Length == 0) { message.Content = loader.GetString("CourseCreditsValidation"); message.ShowAsync(); puNuevoCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (int.TryParse(puNuevoCurso_creditos_txtb.Text, out aux) == false) { message.Content = loader.GetString("CourseCreditsValidationInteger"); message.ShowAsync(); puNuevoCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux <= 0) { message.Content = loader.GetString("CourseCreditsValidationInteger"); message.ShowAsync(); puNuevoCurso_creditos_txtb.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            int idCiclo = (int)ApplicationData.Current.LocalSettings.Values["idCicloActual"];
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Ciclo cicloActual = db.Table<Ciclo>().Single(p => p.Id == idCiclo);
            Curso nuevoCurso = new Curso();
            nuevoCurso.Nombre = nuevoCursoNombre.Text;
            nuevoCurso.Creditos = Convert.ToInt32(puNuevoCurso_creditos_txtb.Text);
            nuevoCurso.CicloId = idCiclo;
            cicloActual.TotalCreditos = cicloActual.TotalCreditos + nuevoCurso.Creditos;
            db.Insert(nuevoCurso);
            db.Update(cicloActual);
            db.Close();
            nuevoCursoNombre.Text = "";
            puNuevoCurso_creditos_txtb.Text = "";
            Frame.GoBack();
            
        }
        

        

        
    }
}
