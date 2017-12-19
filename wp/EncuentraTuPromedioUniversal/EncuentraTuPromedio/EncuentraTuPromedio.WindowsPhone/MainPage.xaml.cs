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
using EncuentraTuPromedio;
using Windows.Storage;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.Graphics.Display;
using Windows.System;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Ciclo cicloActual;
        AppBarButton btnbar_aceptaMensaje;
        CommandBar cmdbar_aceptaMensaje;
        int numeroEntradas;
        public MainPage()
        {
            this.InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            cmdbar_aceptaMensaje = new CommandBar();

            btnbar_aceptaMensaje = new AppBarButton();
            btnbar_aceptaMensaje.IsEnabled = true;
            btnbar_aceptaMensaje.Click += btnbar_aceptaMensaje_Click;
            btnbar_aceptaMensaje.Content = new Windows.ApplicationModel.Resources.ResourceLoader().GetString("AgreeAppBarButton");
            btnbar_aceptaMensaje.Icon = new SymbolIcon(Symbol.Accept);

            cmdbar_aceptaMensaje.PrimaryCommands.Add(btnbar_aceptaMensaje);

            Maneja_Opioniones();

            if (ApplicationData.Current.LocalSettings.Values["redondeoProm"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["redondeoProm"] = false;  
            }

            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            if (db.Table<Ciclo>().ToList().Count == 0)
            {
                cicloActual = new Ciclo();
                cicloActual.Nombre = "Ciclo Actual";
                cicloActual.Id = 1;
                db.Insert(cicloActual);
                cicloActual = db.Table<Ciclo>().Single(p => p.Id == 1);
            }
            else cicloActual = db.Table<Ciclo>().Single(p => p.Id == 1);
            update();
            db.Close();
            

        }

        void btnbar_aceptaMensaje_Click(object sender, RoutedEventArgs e)
        {
            double aux;
            MessageDialog message = new MessageDialog("");
            ResourceLoader loader = new ResourceLoader();
            if (mensajeCicloNotaMaxima_tbx.Text.Trim().Length == 0 || mensajeCicloNotaMinima_tbx.Text.Trim().Length == 0 ||
                mensajeCicloNotaAprobatoria_tbx.Text.Trim().Length == 0) { message.Content = loader.GetString("ValidationCompleteFields"); message.Title = loader.GetString("AlertText");return; }

            if (double.TryParse(mensajeCicloNotaMaxima_tbx.Text, out aux) == false) { message.Content = loader.GetString("MaxGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMaxima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { message.Content = loader.GetString("MaxGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMaxima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(mensajeCicloNotaMinima_tbx.Text, out aux) == false) { message.Content = loader.GetString("MinGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMinima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { message.Content = loader.GetString("MinGradeValidationInteger"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaMinima_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            if (double.TryParse(mensajeCicloNotaAprobatoria_tbx.Text, out aux) == false) { message.Content = loader.GetString("ApprovingGradeValidationInteger"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < 0) { message.Content = loader.GetString("ApprovingGradeValidationInteger"); message.Title = loader.GetString("AlertText");  mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux < Convert.ToDouble(mensajeCicloNotaMinima_tbx.Text)) { message.Content = loader.GetString("ApprovingGradeValidationMin"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }
            if (aux > Convert.ToDouble(mensajeCicloNotaMaxima_tbx.Text)) { message.Content = loader.GetString("ApprovingGradeValidationMax"); message.Title = loader.GetString("AlertText"); mensajeCicloNotaAprobatoria_tbx.Focus(Windows.UI.Xaml.FocusState.Programmatic); return; }

            //dbContext db = new dbContext("isostore:/prom.sdf");
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Ciclo cicloAEditar = db.Table<Ciclo>().Single(p => p.Id == cicloActual.Id);
            cicloAEditar.MaximaNota = Convert.ToDouble(mensajeCicloNotaMaxima_tbx.Text);
            cicloAEditar.MinimaNota = Convert.ToDouble(mensajeCicloNotaMinima_tbx.Text);
            cicloAEditar.NotaAprobatoria = Convert.ToDouble(mensajeCicloNotaAprobatoria_tbx.Text);
            db.Update(cicloAEditar);
            update();
            mensajeNotasCiclo.IsOpen = false;
            BottomAppBar = CommandAppBar;

            
        }
        

        void update()
        {


            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            if (db.Table<Ciclo>().Single(p => p.Id == cicloActual.Id).MaximaNota == null &&
                db.Table<Ciclo>().Single(p => p.Id == cicloActual.Id).MinimaNota == null &&
                db.Table<Ciclo>().Single(p => p.Id == cicloActual.Id).NotaAprobatoria == null)
            {

                BottomAppBar = cmdbar_aceptaMensaje;
                mensajeNotasCiclo.IsOpen = true;
            }
            else
            {
                
                double porcentajeCiclo = 0.00;
                int sumaCreditos = 0;
                double sumaPuntosProm = 0.00;
                cicloActual = db.Table<Ciclo>().Single(p => p.Id == 1);
                foreach (Curso curso in cicloActual.Cursos)
                {
                    porcentajeCiclo = porcentajeCiclo + curso.PorcentajeProm;
                    if((bool)ApplicationData.Current.LocalSettings.Values["redondeoProm"]==false)
                    sumaPuntosProm = sumaPuntosProm + curso.PuntosProm * curso.Creditos;

                    else sumaPuntosProm = sumaPuntosProm + redondeo(curso.PuntosProm) * curso.Creditos; 

                }
                if (cicloActual.TotalCreditos == 0) { cicloProgreso.Value = 0.00; sumaCreditos = 1; plEDKi.Value = 0; }
                else
                {
                    sumaCreditos = cicloActual.TotalCreditos;
                    cicloProgreso.Value = porcentajeCiclo / cicloActual.Cursos.Count * 100;
                }
                cicloActual.PromedioProm = sumaPuntosProm / sumaCreditos;
                PuntosLiquidosCiclo.EndAngle = (360.0 * cicloActual.PromedioProm) / 20;

                if (cicloActual.PromedioProm <= 10.0) PuntosLiquidosCiclo.Fill = new SolidColorBrush(Colors.Red);
                else
                    if (cicloActual.PromedioProm <= 13.0) PuntosLiquidosCiclo.Fill = new SolidColorBrush(Colors.Yellow);
                    else PuntosLiquidosCiclo.Fill = new SolidColorBrush(Colors.Green);

                db.Update(cicloActual);

                List<Curso> listaCursos = db.Table<Curso>().ToList();
                llcursos.ItemsSource = listaCursos;
                llcursosPanel.ItemsSource = listaCursos;
                controlPromedioProm.DataContext = cicloActual;
            }
            db.Close();
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            update();
        }

        double redondeo(double x)
        {
            double numeroRedondo;
            numeroRedondo = Math.Round(x, 0);
            return numeroRedondo;
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void btnbar_añadircurso_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["idCicloActual"] = cicloActual.Id;
            Frame.Navigate(typeof(NuevoCurso));
        }

        private void llcursoItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["idCursoActual"] = ((Curso)((ListViewItem)sender).DataContext).Id;
            Frame.Navigate(typeof(NotasPage));

        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (Frame.CanGoBack == true)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void MFI_EdicionCurso_Click(object sender, RoutedEventArgs e)
        {
            Curso cursoSeleccionado = (Curso)((MenuFlyoutItem)sender).DataContext;
            ApplicationData.Current.LocalSettings.Values["idCursoActual"] = cursoSeleccionado.Id;
            Frame.Navigate(typeof(EdicionCurso));
            
        }

        private void MFI_EliminarCurso_Click(object sender, RoutedEventArgs e)
        {
            Curso cursoSeleccionado = (Curso)((MenuFlyoutItem)sender).DataContext;
            ResourceLoader loader = new ResourceLoader();
            MessageDialog message = new MessageDialog(loader.GetString("DeleteMessage1") + cursoSeleccionado.Nombre + loader.GetString("DeleteMessage2"), loader.GetString("AlertText"));
            message.Commands.Add(new UICommand(loader.GetString("YesText"), OnYesButtonClicked));
            message.Commands.Add(new UICommand(loader.GetString("NoText")));
            ApplicationData.Current.LocalSettings.Values["idCursoSeleccionado"] = cursoSeleccionado.Id;
            message.ShowAsync(); 
            
            
        }

        private void OnYesButtonClicked(IUICommand command)
        {

            int idAeliminar = (int)ApplicationData.Current.LocalSettings.Values["idCursoSeleccionado"];
            //dbContext context = new dbContext("isostore:/prom.sdf");
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Curso cursoAEliminar = db.Table<Curso>().ToList().Single(x => x.Id == idAeliminar);
            cicloActual = db.Table<Ciclo>().Single(p => p.Id == 1);
            cicloActual.TotalNotas = cicloActual.TotalNotas - cursoAEliminar.Notas.Count;
            cicloActual.TotalCreditos = cicloActual.TotalCreditos - cursoAEliminar.Creditos;
            foreach (Nota nota in cursoAEliminar.Notas)
            {
                db.Delete(nota);
            }

            db.Delete(cursoAEliminar);
            db.Update(cicloActual);
            db.Close();
            update();
            llcursos.SelectedItem = null;
            ApplicationData.Current.LocalSettings.Values["idCursoSeleccionado"] = null;
        }

        private void menu_configuracionCiclo_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["idCicloActual"] = cicloActual.Id;
            Frame.Navigate(typeof(ConfiguracionCiclo));
        }

        private void menu_feedback_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Feedback));
        }

        private void btnbar_acerca_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Acerca));
        }

        private void Maneja_Opioniones()
        {
            var settings = ApplicationData.Current.LocalSettings;
            if(settings.Values["Calificado"] == null)
            {
                settings.Values["Calificado"] = false;
                settings.Values["Entradas"] = 0;
            
            }

            if((bool)settings.Values["Calificado"]== false)
            {
                numeroEntradas = (int)settings.Values["Entradas"];
                if (numeroEntradas == 5)
                {
                    ResourceLoader loader = new ResourceLoader();
                    MessageDialog message = new MessageDialog(loader.GetString("RateMyApp"), loader.GetString("ApplicationTitle"));
                    message.Commands.Add(new UICommand(loader.GetString("YesText"), aceptaRateMessage));
                    message.Commands.Add(new UICommand(loader.GetString("NoText"), declinaRateMessage));
                    message.ShowAsync();
                    
                }
                else
                {
                    numeroEntradas++;
                }
                settings.Values["Entradas"] = numeroEntradas;

            }
        }

        private void declinaRateMessage(IUICommand command)
        {

            numeroEntradas = 0;
            ApplicationData.Current.LocalSettings.Values["Entradas"] = numeroEntradas;
            //MarketplaceReviewTask rr = new MarketplaceReviewTask();
            //rr.Show();
            //settings["Calificado"] = true;
            //numeroEntradas = 0;
            
        }

        private void aceptaRateMessage(IUICommand command)
        {
            
            Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" +"87ee703a-0b2b-4c08-b9db-297cbde5ed54"));
            ApplicationData.Current.LocalSettings.Values["Calificado"] = true;
            numeroEntradas = 0;
            ApplicationData.Current.LocalSettings.Values["Entradas"] = numeroEntradas;
        }


        
        

        

        
        
        
    }
}
