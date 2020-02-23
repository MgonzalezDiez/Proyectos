using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Auto Evaluacion Preguntas

namespace Evaluacion360.Models.ViewModels
{
    public class AutoEvaluationQuestionViewModel        
    {
        [Required]
        [Key]
        public int Numero_Evaluacion { get; set; }

        [Required]
        [Key]
        public int Codigo_Proceso { get; set; }

        [Required]
        [Key]
        public string Codigo_Seccion { get; set; }

        [Required]
        [Key]
        public int Numero_Pregunta { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+\,\d{0,1}$")]
        [Range(0, 99.9)]
        [Display(Name = "Nota")]
        [Required]
        public decimal Nota { get; set; }

    }

    public class AutoEvaluationQuestionListModel
    {
        public int Numero_Evaluacion { get; set; }
        public string Codigo_Proceso { get; set; }
        public string Codigo_Seccion { get; set; }
        public int Numero_Pregunta { get; set; }
        public decimal Nota { get; set; }
        public string Estado_AE { get; set; }
    }
}