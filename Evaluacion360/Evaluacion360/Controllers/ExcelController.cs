using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        #region Recibe Excel
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

                        Console.WriteLine("Hay archivo");
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        //var x = ReadExcelSheet(fname, true);

                        //var newName = fname.Split('.');
                        //fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];

                        string path = System.AppDomain.CurrentDomain.BaseDirectory;
                        //var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Importados";
                        var uploadRootFolderInput = Server.MapPath("Importados");
                        if (!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        Console.WriteLine("Grabo archivo " + fname);
                        file.SaveAs(fname);

                        Console.WriteLine("ReadSectionExcel");
                        //var x = ReadExcelSheet(fname, true);
                        excelModel = ReadSectionExcel(fname);

                        var val1 = excelModel.Exists(n => n.Nombre_Seccion == "Error");
                        Console.WriteLine(val1);
                        if (excelModel != null && !val1)
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);
                        }
                        else
                        {
                            Console.WriteLine("Serializa archivo");
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            ExcelJson = ser.Serialize(excelModel);
                            Console.WriteLine("Retorna Json " + ExcelJson);
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

        #region Lee y Crea Secciones Excel
        public List<SectionExcelModel> ReadSectionExcel(string file)       //string FilePath  HttpPostedFileBase file
        {
            List<SectionExcelModel> excelData = new List<SectionExcelModel>();
            try
            {
                //var document = SpreadsheetDocument.Open(file.InputStream, false);
                FileInfo existingFile = new FileInfo(file.ToString());
                using ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet wrkSheet = package.Workbook.Worksheets[1];
                int rowCount = wrkSheet.Dimension.End.Row;

                using (var bd = new BD_EvaluacionEntities())
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (wrkSheet.Cells[row, 1].Value != null)
                        {
                            decimal Ponderacion = 0;
                            string CodSec = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper();
                            if (wrkSheet.Cells[row, 3].Value != null)
                            {
                                Ponderacion = decimal.Parse(wrkSheet.Cells[row, 3].Value.ToString().Trim());
                            }
                            int res = Tools.ValidaDominios(CodSec);
                            Secciones oSection = new Secciones();
                            if (res >= 1)
                            {
                                oSection.Codigo_Seccion = CodSec;
                                oSection.Nombre_Seccion = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper();
                                oSection.Ponderacion_S = Ponderacion;
                                oSection.IdState = 1;
                                bd.Entry(oSection).State = System.Data.Entity.EntityState.Modified;
                                bd.SaveChanges();
                            }
                            else
                            {
                                decimal ponderacion = 0;
                                if (wrkSheet.Cells[row, 3].Value != null)
                                {
                                    ponderacion = Decimal.Parse(wrkSheet.Cells[row, 3].Value.ToString());
                                }

                                oSection = new Secciones
                                {
                                    Codigo_Seccion = CodSec,
                                    Nombre_Seccion = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper(),
                                    Ponderacion_S = Decimal.Parse(ponderacion.ToString()),
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

                        var val1 = excelModel.Exists(n => n.Codigo_Seccion == "Error");
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

        public List<RQExcelModel> ReadRQExcel(string FilePath)
        {
            List<RQExcelModel> excelData = new List<RQExcelModel>();
            try
            {
                FileInfo existingFile = new FileInfo(FilePath);
                using ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet wrkSheet = package.Workbook.Worksheets[1];
                int rowCount = wrkSheet.Dimension.End.Row;

                using (BD_EvaluacionEntities db = new BD_EvaluacionEntities())
                {
                    try
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            if (wrkSheet.Cells[row, 1].Value != null)
                            {
                                int total = 0;
                                var CodSec = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper();
                                int res = Tools.ValidaDominios(CodSec);
                                if (res == 0)
                                {
                                    using BD_EvaluacionEntities bd = new BD_EvaluacionEntities();
                                    var oSec = new Secciones
                                    {
                                        Codigo_Seccion = CodSec,
                                        Nombre_Seccion = CodSec,
                                        Ponderacion_S = decimal.Parse("30,0"),
                                        IdState = 1
                                    };
                                    bd.Secciones.Add(oSec);
                                    bd.SaveChanges();
                                }

                                decimal ponderacion = 0;
                                if (wrkSheet.Cells[row, 4].Value != null)
                                {
                                    ponderacion = Decimal.Parse(wrkSheet.Cells[row, 4].Value.ToString());
                                }

                                var oRQ = new Preguntas_Aleatorias
                                {
                                    Codigo_Seccion = CodSec,
                                    Numero_Pregunta = Convert.ToInt32(wrkSheet.Cells[row, 2].Value.ToString().Trim()),
                                    Texto_Pregunta = wrkSheet.Cells[row, 3].Value.ToString().Trim(),
                                    Ponderacion_P = ponderacion
                                };
                                res = Tools.ValidaPreguntas(CodSec, oRQ.Numero_Pregunta);
                                if (res == 0)
                                {
                                    total += 1;
                                    db.Preguntas_Aleatorias.Add(oRQ);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Entry(oRQ).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                excelData.Add(new RQExcelModel()
                                {
                                    Codigo_Seccion = CodSec,
                                    Numero_Pregunta = oRQ.Numero_Pregunta,
                                    Texto_Pregunta = oRQ.Texto_Pregunta,
                                    Ponderacion_P = oRQ.Ponderacion_P
                                });

                                if (total == 0)
                                {
                                    excelData.Clear();
                                    excelData.Add(new RQExcelModel()
                                    {
                                        Codigo_Seccion = "Error",
                                        Texto_Pregunta = "Los registros ya existen, valide la información",
                                        Numero_Pregunta = 0
                                    });
                                }

                            }
                        }
                        //bd.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.InnerException.InnerException.Message;
                        excelData.Clear();
                        excelData.Add(new RQExcelModel()
                        {
                            Codigo_Seccion = "Error",
                            Texto_Pregunta = mensaje + " Valide la información a Ingresar o Contáctese con el administrador",
                            Numero_Pregunta = 0,
                            Ponderacion_P = 0
                        });
                    }
                }
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
            }
            return excelData;
        }
        #endregion

        #region Evaluacion Preguntas Cargos
        // *************************************************************************************************//
        // GET: Excel EP
        public ActionResult ListEP()
        {
            return View();
        }

        [HttpPost]
        public JsonResult IndexEP()
        {
            List<EPExcelModel> excelModel = new List<EPExcelModel>();

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

                        excelModel = SaveEPExcel(fname);

                        var val1 = excelModel.Exists(n => n.Codigo_Seccion == "Error");

                        FileInfo xFile = new FileInfo(fname);
                        xFile.Delete();

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

        public List<EPExcelModel> SaveEPExcel(string FilePath)
        {
            List<EPExcelModel> excelData = new List<EPExcelModel>();
            try
            {
                FileInfo existingFile = new FileInfo(FilePath);
                using ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet wrkSheet = package.Workbook.Worksheets[1];
                int rowCount = wrkSheet.Dimension.End.Row;

                BD_EvaluacionEntities db = new BD_EvaluacionEntities();
                var Proceso = db.Procesos_Evaluacion.Where(x => x.Estado_PE == "A").FirstOrDefault();
                var EvalNo = db.Auto_Evaluaciones.Where(ae => ae.Codigo_Proceso == Proceso.Codigo_Proceso).FirstOrDefault();
                try
                {
                    var NumEval = EvalNo.Numero_Evaluacion;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (wrkSheet.Cells[row, 1].Value != null)
                        {
                            NumEval += 1;

                            var CodCargo = wrkSheet.Cells[row, 1].Value.ToString().Trim().ToUpper();
                            var CodSec = wrkSheet.Cells[row, 2].Value.ToString().Trim().ToUpper();
                            var Pctje = wrkSheet.Cells[row, 3].Value.ToString().Trim().ToUpper();


                            var CodUsuario = db.Usuarios.Where(u => u.Codigo_Cargo == CodCargo).Select(usu => usu.Codigo_Usuario).FirstOrDefault();

                            int res = Tools.ValidaDominios(CodSec);
                            if (res == 1)
                            {
                                int total = 0;
                                IEnumerable<int> Preguntas;
                                if (Pctje == "S")
                                {
                                    var Preg = db.Preguntas_Aleatorias.Where(x => x.Codigo_Seccion == CodSec && x.Ponderacion_P > 0)
                                        .Select(a => a.Numero_Pregunta);
                                    Preguntas = Preg.ToList();
                                }
                                else
                                {
                                    var Preg = db.Preguntas_Aleatorias.Where(xx => xx.Codigo_Seccion == CodSec && xx.Ponderacion_P == 0)
                                        .Select(a => a.Numero_Pregunta);
                                    Preguntas = Preg.ToList();
                                }

                                foreach (var QuestNo in Preguntas)
                                {
                                    var existe = db.Preguntas_Cargos.Find(CodCargo, CodCargo, CodSec, QuestNo);
                                    if (existe == null)
                                    {
                                        total += 1;
                                        var oEpc = new Preguntas_Cargos
                                        {
                                            Codigo_Cargo = CodCargo,
                                            Cod_Cargo_Evaluado = CodCargo,
                                            Codigo_seccion = CodSec,
                                            Numero_Pregunta = QuestNo,
                                            IdState = 1
                                        };
                                        db.Preguntas_Cargos.Add(oEpc);

                                        excelData.Add(new EPExcelModel()
                                        {
                                            Codigo_Cargo = CodCargo,
                                            Cod_Cargo_Evaluado = CodCargo,
                                            Codigo_Seccion = CodSec,
                                            Numero_Pregunta = QuestNo,
                                            IdState = 1
                                        });
                                    }

                                    var EvCargos = db.Cargos_Evaluadores.Where(x => x.Cod_Cargo_Evaluado == CodCargo && x.Codigo_Cargo != CodCargo).Select(ev => ev.Codigo_Cargo).ToList();
                                    foreach (var cargo in EvCargos)
                                    {
                                        var existePC = db.Preguntas_Cargos.Find(cargo, CodCargo, CodSec, QuestNo);
                                        if (existePC == null)
                                        {
                                            var oEvc = new Preguntas_Cargos
                                            {
                                                Codigo_Cargo = cargo,
                                                Cod_Cargo_Evaluado = CodCargo,
                                                Codigo_seccion = CodSec,
                                                Numero_Pregunta = QuestNo,
                                                IdState = 1
                                            };
                                            db.Preguntas_Cargos.Add(oEvc);
                                            excelData.Add(new EPExcelModel()
                                            {
                                                Codigo_Cargo = cargo,
                                                Cod_Cargo_Evaluado = CodCargo,
                                                Codigo_Seccion = CodSec,
                                                Numero_Pregunta = QuestNo,
                                                IdState = 1
                                            });
                                        }
                                    }
                                    db.SaveChanges();

                                    if (total == 0)
                                    {
                                        excelData.Clear();
                                        excelData.Add(new EPExcelModel()
                                        {
                                            Codigo_Seccion = "Error",
                                            Mensaje = "Los registros ya existen, valide la información",
                                            Numero_Pregunta = 0
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = ex.InnerException.InnerException.Message;
                    excelData.Clear();
                    excelData.Add(new EPExcelModel()
                    {
                        Codigo_Seccion = "Error",
                        Mensaje = mensaje,
                        Numero_Pregunta = 0
                    });
                }

                return excelData;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                excelData.Clear();
                excelData.Add(new EPExcelModel()
                {
                    Codigo_Seccion = "Error",
                    Mensaje = mensaje,
                    Numero_Pregunta = 0
                });

                return excelData;
            }
        }
        #endregion

    }
}