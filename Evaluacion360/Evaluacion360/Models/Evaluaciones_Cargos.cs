//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Evaluacion360.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Evaluaciones_Cargos
    {
        public int Numero_Evaluacion { get; set; }
        public int Codigo_Proceso { get; set; }
        public string Cod_Usuario_Evaluado { get; set; }
        public string Cod_Cargo_Evaluado { get; set; }
        public string Estado_EC { get; set; }
        public Nullable<decimal> Nota_Final_EC { get; set; }
    }
}
