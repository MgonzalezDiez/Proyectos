using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion360.Models.ViewModels
{
    public class EvaluationPositionsViewModel
    {
        //Evaluaciones_Cargos
        [Required]
        public int Numero_Evaluacion { get; set; }
        public string NombreEvaluacion { get; set; }

        [Required]
        public int Codigo_Proceso { get; set; }
        public string NombreProceso { get; set; }

        [Required]
        public string Codigo_Usuario_Evaluado { get; set; }
        public string Nombre_Usuario_Evaluado { get; set; }

        [Required]
        public string Cod_Cargo_Evaluado { get; set; }
        [Required]
        public string Estado_EC { get; set; }
        public Nullable<decimal> Nota_Final_EC { get; set; }

        public string Cod_Cargo_Evaluador { get; set; }
        public string Usuario_Evaluador { get; set; }

        //Evaluacion_Preguntas_Cargo
        public string Codigo_seccion { get; set; }
        public int Numero_Pregunta { get; set; }
        public string TextoPregunta { get; set; }
        public decimal Nota { get; set; }

    }

    public class EvaluationPositionsListViewModel
    {
        public int Numero_Evaluacion { get; set; }
        public string Codigo_Proceso { get; set; }
        public string Codigo_Usuario { get; set; }
        public string Codigo_seccion { get; set; }
        public string Numero_Pregunta { get; set; }
        public decimal Nota { get; set; }

    }

    public class EvaluationPositionsEditViewModel
    {
        //Evaluaciones_Cargos
        [Required]
        public int Numero_Evaluacion { get; set; }

        [Required]
        public int Codigo_Proceso { get; set; }

        [Required]
        public string Codigo_Usuario_Evaluado { get; set; }

        [Required]
        public string Cod_Cargo_Evaluado { get; set; }

        //Evaluacion_Preguntas_Cargo
        public string Codigo_seccion { get; set; }
        public int Numero_Pregunta { get; set; }
        public string TextoPregunta { get; set; }
        public decimal Nota { get; set; }

    }
}