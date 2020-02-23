using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Tabla Preguntas_Cargos
namespace Evaluacion360.Models.ViewModels
{
    public class PQViewModel
    {
        [Required]
        public string Codigo_Cargo { get; set; }

        [Required]
        public string Cod_Cargo_Evaluado { get; set; }

        [Required]
        public string Codigo_seccion { get; set; }

        [Required]
        public int Numero_Pregunta { get; set; }

        //public List<Preguntas_Aleatorias> GetPreguntas_Aleatorias { get; set; }

        public int IdState { get; set; }
    }

    public class PQListViewModel
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
        public int Numero_Pregunta { get; set; }

        public string IdState { get; set; }
    }


    public class ListPQViewModel : BasePaginador
    {
        public List<PQListViewModel> Secciones { get; set; }
    }

}