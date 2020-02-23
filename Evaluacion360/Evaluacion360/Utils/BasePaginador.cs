using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluacion360.Utils
{
    public class BasePaginador
    {
        public int PaginaActual { get; set; }
        public int TotalDeRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
    }
}