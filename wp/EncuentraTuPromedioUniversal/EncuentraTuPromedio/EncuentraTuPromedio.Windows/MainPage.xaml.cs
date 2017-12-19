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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EncuentraTuPromedio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Curso> cursos;
        public MainPage()
        {
            this.InitializeComponent();
            cursos = new List<Curso>();
            
            Curso cursoAux = new Curso() { Nombre = "hola2", Creditos = 2, CicloId = 1 };
            SQLiteConnection db = new SQLiteConnection(App.DBPath);
            if (db.Table<Ciclo>().ToList().Count <= 0)
            {
                Ciclo nuevoCiclo = new Ciclo();

                nuevoCiclo.Nombre = "Nombre";
                nuevoCiclo.NotaAprobatoria = 12.50;
                nuevoCiclo.MaximaNota = 20.0;
                nuevoCiclo.MinimaNota = 0.00;
                db.Insert(nuevoCiclo);
            }
            db.DeleteAll<Curso>();
            db.Insert(cursoAux);

            lvcursos.ItemsSource = db.Table<Curso>().ToList();
            gvcursos.ItemsSource = db.Table<Curso>().ToList();
            db.Close();
        }

        private void llcursoItem_Holding(object sender, HoldingRoutedEventArgs e)
        {

        }

        private void MFI_EliminarCurso_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MFI_EdicionCurso_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_feedback_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_configuracionCiclo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void llcursoItem_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void btnbar_añadircurso_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnbar_acerca_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
