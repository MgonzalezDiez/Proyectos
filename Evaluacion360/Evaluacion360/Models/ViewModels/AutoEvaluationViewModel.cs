using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//Tabla Auto_Evaluacion_Preguntas

namespace Evaluacion360.Models.ViewModels
{
    public class AutoEvaluationViewModel
    {
        [Required]
        public int Numero_Evaluacion { get; set; }
        
        [Required]
        public int Codigo_Proceso { get; set; }
        
        [Required]
        public string Codigo_Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha { get; set; }

        public string Logros { get; set; }

        public string Metas { get; set; }

        public string Estado_AE { get; set; }

        public Nullable<decimal> Nota_Final_AE { get; set; }

    }

    public class AutoEvaluationListViewModel
    {
        public int Numero_Evaluacion { get; set; }

        public int Codigo_Proceso { get; set; }
        public string NombreProceso { get; set; }

        public string Codigo_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha { get; set; }

        public string Logros { get; set; }

        public string Metas { get; set; }

        public string Estado_AE { get; set; }

        public decimal Nota_Final_AE { get; set; }


    }
}
