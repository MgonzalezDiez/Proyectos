using Newtonsoft.Json;

namespace Evaluacion360.Models.ViewModels
{
    #region Secciones
    public class SectionExcelModel
    {
        [JsonProperty("Codigo_Seccion")]
        public string Codigo_Seccion { get; set; }
        [JsonProperty("Nombre_Seccion")]
        public string Nombre_Seccion { get; set; }
        [JsonProperty("Ponderacion_S")]
        public decimal Ponderacion_S { get; set; }
        [JsonProperty("IdState")]
        public int IdState { get; set; }
    }
    #endregion

    #region Random Question
    public class RQExcelModel
    {
        [JsonProperty("Codigo_Seccion")]
        public string Codigo_Seccion { get; set; }
        [JsonProperty("Numero de Pregunta")]
        public int Numero_Pregunta { get; set; }
        [JsonProperty("Pregunta")]
        public string Texto_Pregunta { get; set; }
        [JsonProperty("Ponderacion")]
        public decimal Ponderacion_P { get; set; }
    }
    #endregion

    #region Postion Question
    public class PQExcelModel
    {
        [JsonProperty("Codigo Cargo")]
        public string Codigo_Cargo { get; set; }
        [JsonProperty("Cod. Cargo Evaluado")]
        public string Cod_Cargo_Evaluado { get; set; }
        [JsonProperty("Código Sección")]
        public string Codigo_Seccion { get; set; }
        [JsonProperty("Número de Pregunta")]
        public int Numero_Pregunta { get; set; }
    }
    #endregion

    #region Evaluation Postion
    public class EPExcelModel
    {
        //Datos Evaluacion Preguntas Cargos
        [JsonProperty("Numero Evaluacion")]
        public int Numer_Evaluacion { get; set; }
        [JsonProperty("Codigo Proceso")]
        public int Codigo_Proceso { get; set; }
        [JsonProperty("Cod. Usuario Evaluado")]
        public string Cod_Usuario_Evaluado { get; set; }
        [JsonProperty("Codigo Seccion")]
        public string Codigo_seccion { get; set; }
        [JsonProperty("Numero Pregunta")]
        public int Numero_Pregunta { get; set; }
        [JsonProperty("Nota")]
        public decimal Nota { get; set; }

        ////Datos Evaluaciones Cargos
        //[JsonProperty("Cod. Cargo Evaluado")]
        //public string Cod_Cargo_Evaluado { get; set; }
        //[JsonProperty("Estado Ev. Cargo")]
        //public string Estado_EC { get; set; }
        //[JsonProperty("Nota Final")]
        //public string Nota_Final { get; set; }
    }
    #endregion

}