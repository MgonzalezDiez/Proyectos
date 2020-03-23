using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


// Tabla Proceso_Evaluacion

namespace Evaluacion360.Models.ViewModels
{
    public class EvaluationProcessViewModel
    {
        [Required(ErrorMessage = "Debe Ingresar Código de Proceso")]
        [Key]
        public int Codigo_Proceso { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre de Proceso")]
        [MaxLength(50)]
        public string Nombre_Proceso { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Año de Proceso")]
        public int Ano_Proceso { get; set; }

        [Required]
        [Range(1, 12)]
        public int Mes_Proceso { get; set; }

        public string Retroalimentacion { get; set; }

        [Required]
        public string Estado_PE { get; set; }

        [Required(ErrorMessage = "Debe Seleccionar Estado")]
        public int IdState { get; set; }

    }

    public class EvProcsGeneration
    {
        [Required]
        public int Codigo_Proceso { get; set; }
    }

    public class EvProcsGenerationUno
    {
        [Required]
        public string Codigo_Usuario { get; set; }
        [Required]
        public int Codigo_Proceso { get; set; }
    }


}