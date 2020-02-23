using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;


using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Evaluacion360.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel Secciones
        public ActionResult List()
        {
            return View();

        }

        #region Recibe datos Json
        [HttpPost]
        public JsonResult Index()
        {
            List<SectionExcelModel> excelModel = new List<SectionExcelModel>();
            
            if (Request.Files.Count > 0)
            {
                var ExcelJson = "";
                try
                {
                    string fname;

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var newName = fname.Split('.');
                        fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Importados";
                        if(!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                       
                        excelModel = ReadSectionExcel(fname);
                        var val1 = excelModel.Exists(n => n.Nombre_Seccion == "Error");
                        if (excelModel != null && !val1)
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);

                            if (System.IO.File.Exists("D:\\Excel.json"))
                            {
                                System.IO.File.Delete("D:\\Excel.json");
                            }
                            System.IO.File.WriteAllText("D:\\Excel.json", ExcelJson);
                        }
                        else
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);
                            return Json(ExcelJson, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (excelModel.Count > 0)
                    {
                        return Json(ExcelJson, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Lee Secciones Excel
        public List<SectionExcelModel> ReadSectionExcel(string FilePath)
        {
            List<SectionExcelModel> excelData = new List<SectionExcelModel>();
            try
            {
                FileInfo existingFile = new FileInfo(FilePath);
                using ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet wrkSheet = package.Workbook.Worksheets[1];
                int rowCount = wrkSheet.Dimension.End.Row;

                using (var bd = new BD_EvaluacionEntities())
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (wrkSheet.Cells[row, 1].Value != null)
                        {
                            var oSection = new Secciones
                            {
                                Codigo_Seccion = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper(),
                                Nombre_Seccion = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper(),
                                Ponderacion_S = decimal.Parse(wrkSheet.Cells[row, 3].Value.ToString().Trim()),
                                IdState = 1
                            };
                            bd.Secciones.Add(oSection);
                            excelData.Add(new SectionExcelModel()
                            {
                                Codigo_Seccion = oSection.Codigo_Seccion,
                                Nombre_Seccion = oSection.Nombre_Seccion,
                                Ponderacion_S = oSection.Ponderacion_S,
                                IdState = oSection.IdState
                            });
                        }
                    }
                    //bd.SaveChanges();
                    //mensaje = "OkExcel";
                }
                //string ExcelJson = JsonConvert.SerializeObject(excelData);
                return excelData;
            }
            catch (Exception e)
            {
                excelData.Clear();
                excelData.Add(new SectionExcelModel() {
                    Codigo_Seccion = "Error",
                    Nombre_Seccion = "Ocurrió el siguiente error " + e.Message + " Contactar al administrador",
                    Ponderacion_S = 0,
                    IdState = 0
            });

                return excelData;
            }
        }
        #endregion

        #region Preguntas Aleatorias
        // *************************************************************************************************//
        // GET: Excel RQ
        public ActionResult ListRQ()
        {
            return View();

        }

        [HttpPost]
        public JsonResult IndexRQ()
        {
            List<RQExcelModel> excelModel = new List<RQExcelModel>();

            if (Request.Files.Count > 0)
            {
                var ExcelJson = "";
                try
                {
                    string fname;

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var newName = fname.Split('.');
                        fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Importados";
                        if (!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);

                        //string xlsFile = fname;
                        excelModel = ReadRQExcel(fname);
                        var val1 = excelModel.Exists(n => n.Codigo_Seccion == "Error");
                        if (excelModel != null && !val1)
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);

                            if (System.IO.File.Exists("D:\\ExcelRQ.json"))
                            {
                                System.IO.File.Delete("D:\\ExcelRQ.json");
                            }
                            System.IO.File.WriteAllText("D:\\ExcelRQ.json", ExcelJson);
                        }
                        else
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);
                            return Json(ExcelJson, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (excelModel.Count > 0)
                    {
                        return Json(ExcelJson, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
        }

        public List<RQExcelModel> ReadRQExcel(string FilePath)
        {
            List<RQExcelModel> excelData = new List<RQExcelModel>();
            try
            {
                FileInfo existingFile = new FileInfo(FilePath);
                using ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet wrkSheet = package.Workbook.Worksheets[1];
                int rowCount = wrkSheet.Dimension.End.Row;

                using (var bd = new BD_EvaluacionEntities())
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (wrkSheet.Cells[row, 1].Value != null)
                        {
                            var oRQuestion = new Preguntas_Aleatorias
                            {
                                Codigo_Seccion = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper(),
                                Numero_Pregunta = Convert.ToInt32( wrkSheet.Cells[row, 2].Value.ToString().Trim()),
                                Texto_Pregunta = wrkSheet.Cells[row, 3].Value.ToString().Trim(),
                                Ponderacion_P = decimal.Parse( wrkSheet.Cells[row, 4].Value.ToString().Trim())
                            };
                            bd.Preguntas_Aleatorias.Add(oRQuestion);
                            excelData.Add(new RQExcelModel()
                            {
                                Codigo_Seccion = oRQuestion.Codigo_Seccion,
                                Numero_Pregunta = oRQuestion.Numero_Pregunta,
                                Texto_Pregunta = oRQuestion.Texto_Pregunta,
                                Ponderacion_P = oRQuestion.Ponderacion_P
                            });
                        }
                    }
                    //bd.SaveChanges();
                }
                return excelData;
            }
            catch (Exception e)
            {
                excelData.Clear();
                excelData.Add(new RQExcelModel()
                {
                    Codigo_Seccion = "Error",
                    Texto_Pregunta = "Ocurrió el siguiente error " + e.Message + " Contactar al administrador",
                    Numero_Pregunta = 0,
                    Ponderacion_P = 0
                });
                return excelData;
            }
        }
        #endregion
    }
}