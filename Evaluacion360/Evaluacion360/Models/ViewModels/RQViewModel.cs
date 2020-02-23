using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Tabla Preguntas_Aleatorias

namespace Evaluacion360.Models.ViewModels
{
    public class RQViewModel
    {
        [Required]
        public string Codigo_Seccion { get; set; }

        [Required]
        public int Numero_Pregunta { get; set; }

        [Required]
        public string Texto_Pregunta { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+\,\d{0,1}$")]
        [Range(0, 100.0)]
        public decimal Ponderacion_P { get; set; }
    }

    public class ListRQViewModel: BasePaginador
    {
        public List<RQViewModel> Secciones { get; set; }
    }
}