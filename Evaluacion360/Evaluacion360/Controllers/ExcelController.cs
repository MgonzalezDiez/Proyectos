using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Evaluacion360.Utils;
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
                        if (!Directory.Exists(uploadRootFolderInput))
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
                            string CodSec = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper();
                            int res = Tools.ValidaDominios(CodSec);
                            Secciones oSection = new Secciones();
                            if (res >= 1)
                            {
                                oSection.Codigo_Seccion = CodSec;
                                oSection.Nombre_Seccion = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper();
                                oSection.Ponderacion_S = decimal.Parse(wrkSheet.Cells[row, 3].Value.ToString().Trim());
                                oSection.IdState = 1;
                                bd.Entry(oSection).State = System.Data.Entity.EntityState.Modified;
                                bd.SaveChanges();
                            }
                            else
                            {
                                oSection = new Secciones
                                {
                                    Codigo_Seccion = CodSec,
                                    Nombre_Seccion = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper(),
                                    Ponderacion_S = decimal.Parse(wrkSheet.Cells[row, 3].Value.ToString().Trim()),
                                    IdState = 1
                                };
                                bd.Secciones.Add(oSection);
                                bd.SaveChanges();
                            }

                            excelData.Add(new SectionExcelModel()
                            {
                                Codigo_Seccion = CodSec,
                                Nombre_Seccion = oSection.Nombre_Seccion,
                                Ponderacion_S = oSection.Ponderacion_S ?? 0,
                                IdState = oSection.IdState
                            });
                        }
                    }
                }
                return excelData;
            }
            catch (Exception ex)
            {
                string mensaje = ex.InnerException.InnerException.Message;

                excelData.Clear();
                excelData.Add(new SectionExcelModel()
                {

                    Codigo_Seccion = "Error",
                    Nombre_Seccion = mensaje + " Contactar al administrador",
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

                        excelModel = ReadRQExcel(fname);

                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        ExcelJson = ser.Serialize(excelModel);
                        return Json(ExcelJson, JsonRequestBehavior.AllowGet);
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
                    try
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            if (wrkSheet.Cells[row, 1].Value != null)
                            {
                                var oRQ = new Preguntas_Aleatorias
                                {
                                    Codigo_Seccion = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper(),
                                    Numero_Pregunta = Convert.ToInt32(wrkSheet.Cells[row, 2].Value.ToString().Trim()),
                                    Texto_Pregunta = wrkSheet.Cells[row, 3].Value.ToString().Trim(),
                                    Ponderacion_P = decimal.Parse(wrkSheet.Cells[row, 4].Value.ToString().Trim())
                                };
                                bd.Preguntas_Aleatorias.Add(oRQ);
                                excelData.Add(new RQExcelModel()
                                {
                                    Codigo_Seccion = oRQ.Codigo_Seccion,
                                    Numero_Pregunta = oRQ.Numero_Pregunta,
                                    Texto_Pregunta = oRQ.Texto_Pregunta,
                                    Ponderacion_P = oRQ.Ponderacion_P
                                });
                                //bd.SaveChanges();
                            }
                        }
                        bd.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.InnerException.InnerException.Message;

                        excelData.Clear();
                        excelData.Add(new RQExcelModel()
                        {

                            Codigo_Seccion = "Error",
                            Texto_Pregunta = mensaje + " Valide la información a Ingresar o Contátese con el administrador",
                            Numero_Pregunta = 0,
                            Ponderacion_P = 0
                        });
                    }

                }
                return excelData;
            }
            catch (Exception ex)
            {
                string mensaje = ex.InnerException.InnerException.Message;

                excelData.Clear();
                excelData.Add(new RQExcelModel()
                {

                    Codigo_Seccion = "Error",
                    Texto_Pregunta = mensaje + " Contactar al administrador",
                    Numero_Pregunta = 0,
                    Ponderacion_P = 0
                });

                return excelData;
            }
        }
        #endregion

        
    }
}