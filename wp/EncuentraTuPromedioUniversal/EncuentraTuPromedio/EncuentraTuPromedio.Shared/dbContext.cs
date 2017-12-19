using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace proyecto1.promdb
{
    public class dbContext : DataContext
    {
        public dbContext(string connstring) : base(connstring) { }

        public Table<Ciclo> Ciclos;
        public Table<Curso> Cursos;
        public Table<Nota> Notas;
        
    }
}
