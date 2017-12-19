using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace EncuentraTuPromedio
{
    [Table("Ciclo")]
    public class Ciclo
    {

        public Ciclo()
        {
            promedioProm = 0.00;
            TotalNotas = 0;
            cursos = new List<Curso>();
        }

        private int id;
        [PrimaryKey, AutoIncrement,Column("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String nombre;
        [Column("Nombre")]
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private int totalCreditos;
        [Column("TotalCreditos")]
        public int TotalCreditos
        {
            get { return totalCreditos; }
            set { totalCreditos = value; }
        }

        private int totalNotas;
        [Column("TotalNotas")]
        public int TotalNotas
        {
            get { return totalNotas; }
            set { totalNotas = value; }
        }

        private double promedioProm;
        [Column("PromedioProm")]
        public double PromedioProm
        {
            get { return promedioProm; }
            set { promedioProm = value; }
        }


        //Nullabe<double> = double?
        private Nullable<double> maximaNota;
        [Column("MaximaNota")]
        public Nullable<double> MaximaNota
        {
            get { return maximaNota; }
            set { maximaNota = value; }
        }

        private double? minimaNota;
        [Column("MinimaNota")]
        public double? MinimaNota
        {
            get { return minimaNota; }
            set { minimaNota = value; }
        }

        private double? notaAprobatoria;
        [Column("NotaAprobatoria")]
        public double? NotaAprobatoria
        {
            get { return notaAprobatoria; }
            set { notaAprobatoria = value; }
        }

        private List<Curso> cursos;
        [Ignore]
        public List<Curso> Cursos
        {
            get 
            {
                SQLiteConnection db = new SQLiteConnection(App.DBPath);
                cursos = db.Table<Curso>().Where(p => p.CicloId == id).ToList();
                return cursos; 
            }
            private set { cursos = value; }
        }
    }

    [Table("Cursos")]
    public class Curso  
    {
        public Curso()
        {
            puntosProm = 0;
            notas = new List<Nota>();
        }

        private int id;
        [PrimaryKey, AutoIncrement,Column("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String nombre;
        [Column("Nombre")]
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private int creditos;
        [Column("Creditos")]
        public int Creditos
        {
            get { return creditos; }
            set { creditos = value; }
        }

        private double puntosProm;
        [Column("PuntosProm")]
        public double PuntosProm
        {
            get { return puntosProm; }
            set { puntosProm = value; }
        }

        private double porcentajeProm;
        [Column("PorcentajeProm")]
        public double PorcentajeProm
        {
            get { return porcentajeProm; }
            set { porcentajeProm = value; }
        }

        private int cicloId;

        [Column("CicloId")]
        public int CicloId
        {
            get { return cicloId; }
            set { cicloId = value; }
        }
        
        

        private Ciclo ciclo;
        [Ignore]
        public Ciclo Ciclo
        {
            get { return ciclo; }
            set
            {
                ciclo = value;
                if (value != null)
                    cicloId = value.Id;
            }
        }

        private List<Nota> notas;
        [Ignore]
        public List<Nota> Notas
        {
            get 
            {
                SQLiteConnection db = new SQLiteConnection(App.DBPath);
                notas = db.Table<Nota>().Where(p => p.CursoId == id).ToList();
                db.Close();
                return notas; 
            }
            private set { notas = value; }
        }
        

    }

    [Table("Notas")]
    public class Nota
    {
        public Nota()
        { 
            
        }

        private int id;
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String nombre;
        [Column("Nombre")]
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private double valor;
        [Column("Valor")]
        public double Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private double puntos;
        [Column("Puntos")]
        public double Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }

        private double porcentaje;
        [Column("Porcentaje")]
        public double Porcentaje
        {
            get { return porcentaje ; }
            set { porcentaje = value; }
        }

        private int cursoId;
        [Column("CursoId")]
        public int CursoId
        {
            get { return cursoId; }
            set { cursoId = value; }
        }
        
        

        private Curso curso;
        [Ignore]
        public Curso Curso
        {
            get { return curso; }
            set 
            {
                curso = value;
                if (value != null)
                    cursoId = value.Id;
            }
        
        }
        

    }
}
