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
        public int Numero_Evaluacion { get; set; }
        //public string NombreEvaluacion { get; set; }

        [Required]
        public int Codigo_Proceso { get; set; }
        public string NombreProceso { get; set; }

        [Required]
        public string Codigo_Usuario { get; set; }
        public string NombreUsuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha { get; set; }

        public string Logros { get; set; }

        public string Metas { get; set; }

        public string Estado_AE { get; set; }
        public string StateDescription { get; set; }

        public Nullable<decimal> Nota_Final_AE { get; set; }

        public string Codigo_Seccion { get; set; }

        public int Numero_Pregunta { get; set; }
        public string TextoPregunta { get; set; }

        public Nullable<decimal> Nota { get; set; }

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

    public class AutoEvQuestionScoreViewModel
    {
        public string Codigo_Seccion { get; set; }
        public int Numero_Pregunta { get; set; }
        public string TextoPregunta { get; set; }
        public Nullable<decimal> Nota { get; set; }
    }

    public class CreateAutoEvaluationQuestionViewModel
    {
        public int Numero_Evaluacion { get; set; }

        public int Codigo_Proceso { get; set; }

        public string Codigo_Usuario { get; set; }

        public Nullable<DateTime> Fecha { get; set; }

        public string Logros { get; set; }

        public string Metas { get; set; }

        public string Estado_AE { get; set; }

        public Nullable<decimal> Nota_Final_AE { get; set; }

        public string Codigo_Seccion { get; set; }

        public int Numero_Pregunta { get; set; }

        public Nullable<decimal> Nota { get; set; }

    }
}