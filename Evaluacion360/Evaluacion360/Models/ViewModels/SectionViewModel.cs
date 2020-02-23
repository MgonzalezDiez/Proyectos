using System.ComponentModel.DataAnnotations;

namespace Evaluacion360.Models.ViewModels
{
    public class SectionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Código de Dominio")]
        [MaxLength(10)]
        public string Codigo_Seccion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre_Seccion { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+\,\d{0,1}$")]
        [Range(0, 99.9)]
        [Display(Name = "Ponderación")]
        public decimal Ponderacion_S { get; set; }
        
        [Required]
        public int IdState { get; set; }

    }

    public class SectionListViewModel
    {
        public int Id { get; set; }
        public string Codigo_Seccion { get; set; }
        public string Nombre_Seccion { get; set; }
        public decimal Ponderacion_S { get; set; }
        public string IdState { get; set; }
    }
}