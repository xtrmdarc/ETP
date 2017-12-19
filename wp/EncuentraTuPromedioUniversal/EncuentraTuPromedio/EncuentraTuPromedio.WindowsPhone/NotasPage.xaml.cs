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
using SQLite;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotasPage : Page
    {
        int idCursoActual;
        public NotasPage()
        {
            idCursoActual = (int)ApplicationData.Current.LocalSettings.Values["idCursoActual"];
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Curso cursoActual = db.Table<Curso>().ToList().Single(p => p.Id == idCursoActual);
            this.DataContext = cursoActual;
            db.Close();
            this.InitializeComponent();
            update();
        }
        void update()
        {
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            llnotas.ItemsSource = db.Table<Nota>().ToList().Where(p => p.CursoId == idCursoActual);
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            update();
        }

        private void btnbar_añadirnota_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NuevaNota));
        }

        private void MI_EdicionNota_Click(object sender, RoutedEventArgs e)
        {
            Nota notaSeleccionada = (Nota)((MenuFlyoutItem)sender).DataContext;
            ApplicationData.Current.LocalSettings.Values["idNotaSeleccionada"] = notaSeleccionada.Id;
            Frame.Navigate(typeof(EdicionNota));
        }

        private void MI_EliminarNota_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader loader = new ResourceLoader();
            MessageDialog message = new MessageDialog(loader.GetString("DeleteGradeMessage"),loader.GetString("AlertText"));
            message.Commands.Add(new UICommand(loader.GetString("YesText"),OnYesMessageClick));
            message.Commands.Add(new UICommand(loader.GetString("NoText")));
            ApplicationData.Current.LocalSettings.Values["idNotaSeleccionada"] = (int)((Nota)((MenuFlyoutItem)sender).DataContext).Id;
            message.ShowAsync();


            
        }

        private void OnYesMessageClick(IUICommand command)
        {
            
            int IdNotaAElimnar;
            IdNotaAElimnar = (int)ApplicationData.Current.LocalSettings.Values["idNotaSeleccionada"];

            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            Nota NotaAEliminar = db.Table<Nota>().Single(p => p.Id == IdNotaAElimnar);
            Curso CursoActual = db.Table<Curso>().Single(p => p.Id == idCursoActual);
            Ciclo cicloActual = db.Table<Ciclo>().Single(p => p.Id == CursoActual.CicloId);
            CursoActual.PuntosProm = CursoActual.PuntosProm - NotaAEliminar.Puntos;
            CursoActual.PorcentajeProm = CursoActual.PorcentajeProm - NotaAEliminar.Porcentaje;
            cicloActual.TotalNotas = cicloActual.TotalNotas - 1;
            db.Update(cicloActual);
            db.Update(CursoActual);
            db.Delete(NotaAEliminar);
            db.Close();
            update();
        }

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
        
    }
}
