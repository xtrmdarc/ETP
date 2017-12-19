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
using Windows.ApplicationModel.Resources;
using Windows.UI;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EncuentraTuPromedio.UserControls
{
    public sealed partial class cursoItemPanel : UserControl
    {
        Curso cursoActual;
        Ciclo cicloActual;
        int prioridad;
        public cursoItemPanel()
        {
            this.InitializeComponent();
            prioridad = 0;
           
        }

        private void itemCursoPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (itemCursoPanel.DataContext != null)
            {
                //dbContext context = new dbContext("isostore:/prom.sdf");
                SQLiteConnection db = new SQLiteConnection(App.DBPath);

                //cursoActual = (Curso)this.DataContext;
                //cicloActual = (Ciclo)context.Ciclos.Single(p => p.Id == cursoActual.cicloId);
                cursoActual = (Curso)this.DataContext;
                cicloActual = (Ciclo)db.Table<Ciclo>().Single(p => p.Id == cursoActual.CicloId);

                puntosAnilloCurso.EndAngle = (cursoActual.PuntosProm * 360) / Convert.ToDouble(cicloActual.MaximaNota);

                MensajeRendimiento.Text = calculaMensaje(cursoActual);
                MensajeCuantoFalta.Text = porcuanto_tevas();
                puntosAnilloCurso.Fill = calcula_ColorPuntos();
                db.Close();
            }
        }

        private String calculaMensaje(Curso cursoACalcular)
        {
            //dbContext context = new dbContext("isostore:/prom.sdf");
            //SQLiteConnection db = new SQLiteConnection(App.DBPath);

            String Mensaje = "";
            double notaMinima = Convert.ToDouble(cicloActual.MinimaNota);
            double notaMaxima = Convert.ToDouble(cicloActual.MaximaNota);
            double notaAprobatoria = Convert.ToDouble(cicloActual.NotaAprobatoria);

            double puntosActual = cursoACalcular.PuntosProm;
            double porcerntajeActual = cursoACalcular.PorcentajeProm;

            ResourceLoader loader = new ResourceLoader();
            if (puntosActual < notaAprobatoria * porcerntajeActual)
            {
                if (puntosActual < (notaAprobatoria - ((notaAprobatoria - notaMinima) * 0.2)) * porcerntajeActual)
                {
                    Mensaje = loader.GetString("AwfulMessage");
                    //AppResources.AwfulMessage;
                    Prioridad_Rect.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Mensaje = loader.GetString("BadMessage");
                    //AppResources.BadMessage;
                    Prioridad_Rect.Fill = new SolidColorBrush(Colors.Orange);
                }
            }
            else
            {
                double D_MaxYAprob = notaMaxima - notaAprobatoria;
                if (puntosActual < ((D_MaxYAprob * 0.3) + notaAprobatoria) * porcerntajeActual)
                {
                    Mensaje = loader.GetString("NormalMessage");
                        //AppResources.NormalMessage;
                    Prioridad_Rect.Fill = new SolidColorBrush(Colors.Yellow);
                }
                else if (puntosActual < ((D_MaxYAprob * 0.7) + notaAprobatoria) * porcerntajeActual)
                {
                    Mensaje = loader.GetString("GoodMessage");
                        //AppResources.GoodMessage;
                    Prioridad_Rect.Fill = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    Mensaje = loader.GetString("GreatMessage");
                        //AppResources.GreatMessage;
                    Prioridad_Rect.Fill = new SolidColorBrush(Colors.Purple);
                }
            }


            return Mensaje;
            
        }

        private String porcuanto_tevas()
        {
            String porcuanto;
            //dbContext context = new dbContext("isostore/prom.sdf");
            //SQLiteConnection db = new SQLiteConnection(App.DBPath);
            double notaAprobatoria = Convert.ToDouble(cicloActual.NotaAprobatoria);
            double notaActual = Convert.ToDouble(cursoActual.PuntosProm);
            double porcentajeActual = cursoActual.PorcentajeProm;
            double PorcentajeFalta = 1 - porcentajeActual;

            ResourceLoader loader = new ResourceLoader();
            if (porcentajeActual >= 1.00)
                porcuanto = loader.GetString("AproxGradeMessage");
            //AppResources.AproxGradeMessage;
            else if (notaActual >= notaAprobatoria && porcentajeActual < 1)
                porcuanto = loader.GetString("CongratsMessage");
            //AppResources.CongratsMessage;
            else
            {
                double notaFalta;
                notaFalta = (notaAprobatoria - notaActual) / PorcentajeFalta;
                porcuanto = loader.GetString("MinGradeMessage") + notaFalta.ToString("0.00");
                //AppResources.MinGradeMessage 
            }
            
            return porcuanto;
        }

        private SolidColorBrush calcula_ColorPuntos()
        {
            //dbContext context = new dbContext("isostore/prom.sdf");
        
            SolidColorBrush colorDePuntos = new SolidColorBrush();
            double notaAprobatoria = Convert.ToDouble(cicloActual.NotaAprobatoria);
            double notaMaxima = Convert.ToDouble(cicloActual.MaximaNota);
            double notaMinima = Convert.ToDouble(cicloActual.MinimaNota);

            double puntosActual = cursoActual.PuntosProm;

            if (puntosActual < notaAprobatoria)
                colorDePuntos.Color = Colors.Red;
            else if (puntosActual < notaAprobatoria + ((notaMaxima - notaAprobatoria) * 0.3))
                colorDePuntos.Color = Colors.Yellow;
            else colorDePuntos.Color = new Color() { A = 180, B = 31, G = 134, R = 55 };

            return colorDePuntos;
        }

        private void itemCursoPanel_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (itemCursoPanel.DataContext != null)
            {
                //dbContext context = new dbContext("isostore:/prom.sdf");
                SQLiteConnection db = new SQLiteConnection(App.DBPath);

                //cursoActual = (Curso)this.DataContext;
                //cicloActual = (Ciclo)context.Ciclos.Single(p => p.Id == cursoActual.cicloId);
                cursoActual = (Curso)this.DataContext;
                cicloActual = (Ciclo)db.Table<Ciclo>().Single(p => p.Id == cursoActual.CicloId);

                puntosAnilloCurso.EndAngle = (cursoActual.PuntosProm * 360) / Convert.ToDouble(cicloActual.MaximaNota);

                MensajeRendimiento.Text = calculaMensaje(cursoActual);
                MensajeCuantoFalta.Text = porcuanto_tevas();
                puntosAnilloCurso.Fill = calcula_ColorPuntos();
                db.Close();
            }

        }



    }
}
