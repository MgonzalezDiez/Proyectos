using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Tabla Cargos_Evaluadores
namespace Evaluacion360.Models.ViewModels
{
    public class EvaluatorPositionViewModel
    {
        [Required]
        public string Codigo_Cargo { get; set; }
        
        [Required]
        public string Cod_Cargo_Evaluado { get; set; }

        public decimal Ponderacion { get; set; }

        public int IdState { get; set; }
    }

    public class EvaluatorPositionListViewModel
    {
        public string Codigo_Cargo { get; set; }
        public string Nombre_Cargo { get; set; }
        public string Cod_Cargo_Evaluado { get; set; }
        public string Nombre_Cargo_Evaluado { get; set; }
        public decimal Ponderacion { get; set; }
        public string IdState { get; set; }

    }
}