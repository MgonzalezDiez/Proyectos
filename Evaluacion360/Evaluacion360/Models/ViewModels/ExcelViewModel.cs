using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}