using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluacion360.Models.ViewModels
{
    public class EvaluationPositionsViewModel
    {
        public int Numero_Evaluacion { get; set; }
        public int Codigo_Proceso { get; set; }
        public string Cod_Usuario_Evaluado { get; set; }
        public string Cod_Cargo_Evaluado { get; set; }
        public string Estado_EC { get; set; }
        public Nullable<decimal> Nota_Final_EC { get; set; }

    }
}