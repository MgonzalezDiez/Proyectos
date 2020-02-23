using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Tabla Cargos_Evaluadores
namespace Evaluacion360.Models.ViewModels
{
    public class EvPositionViewModel
    {
        [Required]
        public string Codigo_Cargo { get; set; }
        
        [Required]
        public string Cod_Cargo_Evaluado { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+\,\d{0,1}$")]
        [Range(0, 99.9)]
        [Display(Name = "Poderación")]
        public decimal Ponderacion { get; set; }

        public int IdState { get; set; }
    }

    public class EvPositionListViewModel
    {
        public string Codigo_Cargo { get; set; }
        public string Nombre_Cargo { get; set; }
        public string Cod_Cargo_Evaluado { get; set; }
        public string Nombre_Cargo_Evaluado { get; set; }
        public decimal Ponderacion { get; set; }
        public string IdState { get; set; }

    }
}