using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion360.Models
{
    public class UserListViewModel
    {
        public string Codigo_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Tipo_Usuario { get; set; }
        public string Codigo_Cargo { get; set; }
        public string IdState { get; set; }
        public string Nombre_Completo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Nacimiento { get; set; }

        public string Rut { get; set; }
        public string Dv { get; set; }
        public string Fondo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Ingreso { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Termino_Contrato { get; set; }
        public string Calidad_Contrato { get; set; }
        public string Tipo_Contrato { get; set; }
        public string Codigo_Contrato { get; set; }
    }
}