using System.ComponentModel.DataAnnotations;

namespace Evaluacion360.Models.ViewModels
{
    public class SectionViewModel
    {
        [Required(ErrorMessage = "Debe Ingresar Código de Dominio")]
        [MaxLength(20)]
        public string Codigo_Seccion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre_Seccion { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+\,\d{0,1}$")]
        [Range(typeof(decimal), "00,0", "100")]
        [Display(Name = "Ponderación")]
        public decimal Ponderacion_S { get; set; }

        [Required]
        public int IdState { get; set; }

    }

    public class SectionEditViewModel
    {
        public int Id { get; set; }
        public string Codigo_Seccion { get; set; }
        public string Nombre_Seccion { get; set; }
        public decimal Ponderacion_S { get; set; }
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