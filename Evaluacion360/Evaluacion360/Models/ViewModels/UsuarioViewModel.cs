using System;
using System.ComponentModel.DataAnnotations;

// Tablas Usuarios y Datos_Usuarios
namespace Evaluacion360.Models.ViewModels
{
    [Serializable]
    public class UsuarioViewModel
    {

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Codigo_Usuario { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Nombre_Usuario { get; set; }

        [Required]
        public int Tipo_Usuario { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Codigo_Cargo { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string PASS { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [CompareAttribute("PASS")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int IdState { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre_Completo { get; set; }

        //[Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha_Nacimiento { get; set; }

        [Required]
        public int Rut { get; set; }
        [Required]
        public string Dv { get; set; }

        [Required]
        public string Fondo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha_Ingreso { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Fecha_Termino_Contrato { get; set; }

        [Required]
        public string Calidad_Contrato { get; set; }
        [Required]
        public string Tipo_Contrato { get; set; }
        [Required]
        public string Codigo_Contrato { get; set; }

    }



    public class UserEditViewModel
    {
        public string Codigo_Usuario { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public string Nombre_Usuario { get; set; }

        [Required]
        public int Tipo_Usuario { get; set; }

        [Required]
        public string Codigo_Cargo { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6)]
        public string PASS { get; set; }

        [DataType(DataType.Password)]
        [CompareAttribute("PASS")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int IdState { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre_Completo { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required]
        public int Rut { get; set; }
        public string Dv { get; set; }

        [Required]
        public string Fondo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Fecha_Ingreso { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Fecha_Termino_Contrato { get; set; }

        [Required]
        public string Calidad_Contrato { get; set; }
        [Required]
        public string Tipo_Contrato { get; set; }
        [Required]
        public string Codigo_Contrato { get; set; }

    }

    public class UserDeleteViewModel
    {
        public string Codigo_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Tipo_Usuario { get; set; }
        public string Codigo_Cargo { get; set; }
        public string IdState { get; set; }

        //public UserDataViewModel UserData { get; set; }
    }

    public class UserIndexViewModel
    {
        public string Codigo_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Tipo_Usuario { get; set; }
        public string Codigo_Cargo { get; set; }
        public string IdState { get; set; }
    }
}
