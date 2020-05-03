using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion360.Models.ViewModels
{
    public class PositionEvaluationsViewModel
    {
        [Required]
        public int Numero_Evaluacion { get; set; }

        [Required]
        public int Codigo_Proceso { get; set; }

        [Required]
        public string Codigo_Usuario_Evaluado { get; set; }

        public string Cod_Cargo_Evaluado { get; set; }

        public string Estado_EC { get; set; }

        public Nullable<decimal> Nota_Final_EC { get; set; }

    }

    public class PositionEvaluationsListViewModel
    {
        public int Numero_Evaluacion { get; set; }
        public int Codigo_Proceso { get; set; }
        public string NombreProceso { get; set; }
        public string Codigo_Usuario_evaluado { get; set; }
        public string Nombre_Usuario_Evaluado { get; set; }
        public string Cod_Cargo_Evaluado { get; set; }
        public string Nombre_Cargo_Evaluado { get; set; }
        public string Estado_EC { get; set; }
        public string Nombre_Estado_EC { get; set; }
        public decimal Nota_Final_EC { get; set; }
    }
}
