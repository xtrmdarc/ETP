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
    public sealed partial class ConfiguracionCiclo : Page
    {
        Ciclo cicloActual;
        public ConfiguracionCiclo()
        {
            int cicloId = (int)ApplicationData.Current.LocalSettings.Values["idCicloActual"];
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            cicloActual = db.Table<Ciclo>().Single(p => p.Id == cicloId);
            this.InitializeComponent();
            if(ApplicationData.Current.LocalSettings.Values["redondeoProm"] != null)
            {
                if ((bool)ApplicationData.Current.LocalSettings.Values["redondeoProm"] == false)
                    TSPromedio.IsOn = false;
                else TSPromedio.IsOn = true;
            }
            this.DataContext = cicloActual;
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnbar_cicloConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            double aux;
            MessageDialog message = new MessageDialog("");
            ResourceLoader loader = new ResourceLoader();
            
            if (mensajeCicloNotaMaxima_tbx.Text.Trim().Length == 0 || mensajeCicloNotaMinima_tbx.Text.Trim().Length == 0 ||
                mensajeCicloNotaAprobatoria_tbx.Text.Trim().Length == 0) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText"); return; }

            if (double.TryParse(mensajeCicloNotaMaxima_tbx.Text, out aux) == false) { message.Content = loader.GetString("MaxGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMaxima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { message.Content = loader.GetString("MaxGradeValidationInteger"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaMaxima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(mensajeCicloNotaMinima_tbx.Text, out aux) == false) { loader.GetString("MinGradeValidationInteger"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaMinima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { loader.GetString("MinGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMinima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(mensajeCicloNotaAprobatoria_tbx.Text, out aux) == false) { loader.GetString("ApprovingGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { loader.GetString("ApprovingGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < Convert.ToDouble(mensajeCicloNotaMinima_tbx.Text)) { loader.GetString("ApprovingGradeValidationMin"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux > Convert.ToDouble(mensajeCicloNotaMaxima_tbx.Text)) { loader.GetString("ApprovingGradeValidationMax"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            //dbContext db = new dbContext("isostore:/prom.sdf");

            Ciclo cicloAEditar = db.Table<Ciclo>().Single(p => p.Id == cicloActual.Id);
            
            cicloAEditar.MaximaNota = Convert.ToDouble(mensajeCicloNotaMaxima_tbx.Text);
            cicloAEditar.MinimaNota = Convert.ToDouble(mensajeCicloNotaMinima_tbx.Text);
            cicloAEditar.NotaAprobatoria = Convert.ToDouble(mensajeCicloNotaAprobatoria_tbx.Text);
            db.Update(cicloAEditar);

            Frame.GoBack();
        }

        private void TSPromedio_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    ApplicationData.Current.LocalSettings.Values["redondeoProm"] = true;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["redondeoProm"] = false;
                }
            }
        }
    }
}
