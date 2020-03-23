using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Tabla Preguntas_Cargos
namespace Evaluacion360.Models.ViewModels
{
    public class PositionQuestionViewModel
    {
        [Required]
        public string Codigo_Cargo { get; set; }

        [Required]
        public string Cod_Cargo_Evaluado { get; set; }

        [Required]
        public string Codigo_seccion { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public int Numero_Pregunta { get; set; }

        public int IdState { get; set; }
    }

    public class PositionQuestionListViewModel
    {
        [Required]
        public string Codigo_Cargo { get; set; }
        public string Nombre_Cargo { get; set; }

        [Required]
        public string Cod_Cargo_Evaluado { get; set; }
        public string Nombre_Cargo_Evaluado { get; set; }

        [Required]
        public string Codigo_seccion { get; set; }
        public string Nombre_Seccion { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public int Numero_Pregunta { get; set; }
        public string Texto_Pregunta { get; set; }

        public string IdState { get; set; }
    }


    public class ListPQViewModel : BasePaginador
    {
        public List<PositionQuestionListViewModel> Secciones { get; set; }
    }

}