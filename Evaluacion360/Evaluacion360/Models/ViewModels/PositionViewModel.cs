using System.ComponentModel.DataAnnotations;

//Tabla Cargos
namespace Evaluacion360.Models.ViewModels
{
    public class PositionViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Codigo_Cargo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre_Cargo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Fondo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ciclo { get; set; }

        [Required]
        public int IdState { get; set; }
    }

    public class PositionListViewModel
    {

        public string Codigo_Cargo { get; set; }
        public string Nombre_Cargo { get; set; }
        public string Fondo { get; set; }
        public string Ciclo { get; set; }
        public string IdState { get; set; }
    }
}