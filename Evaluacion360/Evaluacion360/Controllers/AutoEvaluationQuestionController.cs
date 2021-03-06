﻿using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Evaluacion360.Controllers
{
    public class AutoEvaluationQuestionController : Controller
    {
        string Mensaje;
        // GET: EvaluacionPreguntas
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult List(int pagina = 1)
        {
            ActionResult result = null;
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;
            try
            {
                Usuarios tUser = (Usuarios)Session["User"];
                var oEvQuestion = new List<AutoEvaluationQuestionListModel>();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();

                oEvQuestion = (from EvQ in Db.Auto_Evaluacion_Preguntas
                               join proc in Db.Procesos_Evaluacion on EvQ.Codigo_Proceso equals proc.Codigo_Proceso
                               join secc in Db.Secciones on EvQ.Codigo_seccion equals secc.Codigo_Seccion
                               join ranq in Db.Preguntas_Aleatorias on EvQ.Codigo_seccion equals ranq.Codigo_Seccion

                               where EvQ.Numero_Pregunta == ranq.Numero_Pregunta
                               orderby EvQ.Codigo_Proceso, EvQ.Codigo_seccion, EvQ.Numero_Pregunta
                               select new AutoEvaluationQuestionListModel
                               {
                                   Numero_Evaluacion = EvQ.Numero_Evaluacion,
                                   Codigo_Proceso = EvQ.Codigo_Proceso.ToString(),
                                   Codigo_Seccion = EvQ.Codigo_seccion,
                                   Numero_Pregunta = EvQ.Numero_Pregunta,
                                   Nota = EvQ.Nota
                               }).ToList();

                var TotalRegistros = oEvQuestion.Count();
                List<AutoEvaluationQuestionListModel> lista = oEvQuestion.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
                var Modelo = new ListAEQViewModel
                {
                    Secciones = lista,
                    PaginaActual = pagina,
                    TotalDeRegistros = TotalRegistros,
                    RegistrosPorPagina = CantidadRegitrosPorPagina
                };
                return View(Modelo);
            }
            catch (Exception exc)
            {
                Mensaje = exc.Message;
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                return result;
            }

        }

        // GET: EvaluacionPreguntas/Details/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Details(AutoEvaluationQuestionViewModel AEQ)
        {
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            try
            {
                //Usuarios tUser = (Usuarios)Session["User"];
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();

                Auto_Evaluacion_Preguntas oAeq = Db.Auto_Evaluacion_Preguntas.Find(AEQ.Numero_Evaluacion, AEQ.Codigo_Proceso, AEQ.Codigo_Seccion, AEQ.Numero_Pregunta);
                oAeq.Numero_Evaluacion = AEQ.Numero_Evaluacion;
                oAeq.Codigo_Proceso = AEQ.Codigo_Proceso;
                oAeq.Codigo_seccion = AEQ.Codigo_Seccion.ToString();
                oAeq.Numero_Pregunta = AEQ.Numero_Pregunta;
                oAeq.Nota = AEQ.Nota;
                Mensaje = "Ok";
                return View(new { oAeq, Mensaje });
            }

            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return View(new { Mensaje });
        }

        // GET: EvaluacionPreguntas/Create
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Create(string mensaje)
        {
            ViewBag.Status = true;
            Usuarios oUser = (Usuarios)Session["User"];

            ViewBag.Users = new SelectList(Tools.LeerUsuarios(oUser.Codigo_Usuario), "Codigo_Usuario", "Nombre_Usuario", 1);
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            ViewBag.Sections = new SelectList(Tools.DominiosPorUsuario(oUser.Codigo_Usuario), "Codigo_Seccion", "Nombre_Seccion");


            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Auto Evaluación Actualizada exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            try
            {
                var oAE = new AutoEvaluationQuestionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAE = (from pev in Db.Procesos_Evaluacion
                       join aev in Db.Auto_Evaluaciones on pev.Codigo_Proceso equals aev.Codigo_Proceso
                       join usu in Db.Usuarios on aev.Codigo_Usuario equals usu.Codigo_Usuario
                       join eev in Db.Estado_Evaluaciones on aev.Estado_AE equals eev.IdState
                       where usu.Codigo_Usuario == oUser.Codigo_Usuario && aev.Estado_AE == "A" 
                       orderby pev.Nombre_Proceso
                       select new AutoEvaluationQuestionViewModel
                       {
                           Numero_Evaluacion = aev.Numero_Evaluacion,
                           Codigo_Proceso = pev.Codigo_Proceso,
                           NombreProceso = pev.Nombre_Proceso,
                           Codigo_Usuario = aev.Codigo_Usuario,
                           NombreUsuario = usu.Nombre_Usuario,
                           Fecha = aev.Fecha,
                           Logros = aev.Logros ?? "",
                           Metas = aev.Metas ?? "",
                           Estado_AE = aev.Estado_AE ?? "",
                           StateDescription = eev.StateDescription ?? "",
                           Nota_Final_AE = aev.Nota_Final_AE ?? 0,
                           Codigo_Seccion = "",
                           Numero_Pregunta = 0,
                           TextoPregunta = "",
                           Nota = oAE.Nota,
                       }).FirstOrDefault();
                if (oAE == null)
                {
                    ViewBag.Message = "Todas las preguntas de Auto Evaluación ya fueron respondidas";
                    ViewBag.Status = false;
                }
                return View(oAE);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return View(mensaje);
            }

        }

        // POST: EvaluacionPreguntas/Create
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Create(AutoEvaluationQuestionViewModel ae)
        {
            ViewBag.Status = true;
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            // TODO: Add insert logic here
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oAe = new Auto_Evaluacion_Preguntas
                    {
                        Numero_Evaluacion = ae.Numero_Evaluacion,
                        Codigo_Proceso = ae.Codigo_Proceso,
                        Codigo_seccion = ae.Codigo_Seccion,
                        Numero_Pregunta = ae.Numero_Pregunta,
                        Nota = ae.Nota,
                    };
                    bd.Auto_Evaluacion_Preguntas.Add(oAe);
                    bd.SaveChanges();

                    Mensaje = "Ok";
                    #endregion
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        Mensaje += " Contacte al Administrador";
                    }
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error " +
                          e.InnerException.InnerException.Message +
                          " Contacte al Administrador";
            }
            return Json(data: new { error = true, data = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: EvaluacionPreguntas/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(int numEval, int codProc, string codSecc, int numQuest, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta de Auto Evaluación modificada exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
                return View();
            }
            try
            {
                AutoEvaluationQuestionViewModel oAEQ = new AutoEvaluationQuestionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAEQ = (from aeq in Db.Auto_Evaluacion_Preguntas
                        where aeq.Numero_Evaluacion == numEval && aeq.Codigo_Proceso == codProc && aeq.Codigo_seccion == codSecc && aeq.Numero_Pregunta == numQuest
                        select new AutoEvaluationQuestionViewModel
                        {
                            Numero_Evaluacion = aeq.Numero_Evaluacion,
                            Codigo_Proceso = aeq.Codigo_Proceso,
                            Codigo_Seccion = aeq.Codigo_seccion,
                            Numero_Pregunta = aeq.Numero_Pregunta,
                            Nota = aeq.Nota
                        }).FirstOrDefault();
                return View(oAEQ);
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
                return RedirectToAction("Edit", "AutoEvaluationQuestion", new { numEval, codProc, codSecc, numQuest, Mensaje });
            }
        }

        // POST: EvaluacionPreguntas/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Edit(AutoEvaluationQuestionEditViewModel aeq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var Db = new BD_EvaluacionEntities();
                    Auto_Evaluacion_Preguntas oAEQ = Db.Auto_Evaluacion_Preguntas.Find(aeq.Numero_Evaluacion, aeq.Codigo_Proceso, aeq.Codigo_Seccion, aeq.Numero_Pregunta);
                    if (aeq.Nota != 0)
                    {
                        oAEQ.Nota = aeq.Nota;
                        Db.Entry(oAEQ).State = EntityState.Modified;
                    }
                    Mensaje = "Ok";
                    Db.SaveChanges();

                    var Usu = Db.Usuarios.Find(aeq.Codigo_Usuario);

                    var oAEp = Db.Auto_Evaluacion_Preguntas.Where(a => a.Numero_Evaluacion == aeq.Numero_Evaluacion && a.Codigo_Proceso == aeq.Codigo_Proceso && a.Nota == 0).FirstOrDefault();
                    if (oAEp == null)
                    {
                        var oEvc = Db.Evaluaciones_Cargos.Find(aeq.Numero_Evaluacion, aeq.Codigo_Proceso, aeq.Codigo_Usuario, Usu.Codigo_Cargo);
                        if (oEvc != null)
                        {
                            oEvc.Estado_EC = "C";
                            Db.Entry(oEvc).State = System.Data.Entity.EntityState.Modified;
                            Db.SaveChanges();
                        }
                    }

                    var EstadoAE = Db.Evaluaciones_Cargos.Find(aeq.Numero_Evaluacion, aeq.Codigo_Proceso, aeq.Codigo_Usuario, Usu.Codigo_Cargo);
                    var oAE = Db.Auto_Evaluaciones.Find(aeq.Numero_Evaluacion, aeq.Codigo_Proceso, aeq.Codigo_Usuario);

                    if (EstadoAE.Estado_EC == "C")
                    {
                        oAE.Estado_AE = "C";
                    }

                    oAE.Logros = aeq.Logros;
                    oAE.Metas = aeq.Metas;
                    Db.Entry(oAE).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    return Json(Mensaje);
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        Mensaje += " Contacte al Administrador";
                    }
                    return Json(Mensaje);
                    //return Json(data: new { error = true, data = Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error " + e.Message;
                Mensaje += " Contacte al Administrador";
                return Json(Mensaje);
                //return Json(data: new { error = true, data = Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(int numEval, int codProc, string codSecc, int numQuest, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta de Auto Evaluación Eliminada exitosamente";
                    ViewBag.Status = true;
                }
                else
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            try
            {
                var oAEQ = new AutoEvaluationQuestionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAEQ = (from aeq in Db.Auto_Evaluacion_Preguntas
                        where aeq.Numero_Evaluacion == numEval && aeq.Codigo_Proceso == codProc && aeq.Codigo_seccion == codSecc && aeq.Numero_Pregunta == numQuest
                        select new AutoEvaluationQuestionViewModel
                        {
                            Numero_Evaluacion = aeq.Numero_Evaluacion,
                            Codigo_Proceso = aeq.Codigo_Proceso,
                            Codigo_Seccion = aeq.Codigo_seccion,
                            Numero_Pregunta = aeq.Numero_Pregunta,
                            Nota = aeq.Nota
                        }).FirstOrDefault();

                return View(oAEQ);
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
                return RedirectToAction("Delete", "AutoEvaluationQuestion", new { numEval, codProc, codSecc, numQuest, Mensaje });
            }
        }

        // POST: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Delete(int numEval, int codProc, string codSecc, int numQuest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var bd = new BD_EvaluacionEntities();
                    var oAEQ = bd.Auto_Evaluacion_Preguntas.Find(numEval, codProc, codSecc, numQuest);

                    bd.Entry(oAEQ).State = System.Data.Entity.EntityState.Deleted;
                    bd.SaveChanges();
                    Mensaje = "Ok";
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        Mensaje += " Contacte al Administrador";
                    }
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Delete", "AutoEvaluationQuestion", new { numEval, codProc, codSecc, numQuest, Mensaje });
        }

        [HttpPost]
        public JsonResult GetQuestionByUser(int numEval, int codProc, string codSecc)
        {
            string ModelJson;
            List<AutoEvQuestionScoreViewModel> QuestByUser; /*= new List<AutoEvQuestionScoreViewModel>();*/

            QuestByUser = QuestionsByUser(numEval, codProc, codSecc).ToList();

            if (QuestByUser != null)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ModelJson = ser.Serialize(QuestByUser);
            }
            else
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ModelJson = ser.Serialize(QuestByUser);
                return Json(ModelJson, JsonRequestBehavior.AllowGet);
            }

            return Json(ModelJson, JsonRequestBehavior.AllowGet);
            //return Json( new { Data = Tools.QuestionsByUser(numEval, codProc, codSecc).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet });
            ;
        }

        public static IEnumerable<AutoEvQuestionScoreViewModel> QuestionsByUser(int numEval, int codProc, string sectionCode)
        {
            List<AutoEvQuestionScoreViewModel> RandomQ = new List<AutoEvQuestionScoreViewModel>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "select distinct aep.Numero_Pregunta, pal.Texto_Pregunta, aep.Nota " +
                         "from Preguntas_Cargos pec " +
                         "join Auto_Evaluacion_Preguntas aep on pec.Codigo_seccion = aep.Codigo_seccion " +
                         "join Auto_Evaluaciones aev on aep.Numero_Evaluacion = aev.Numero_Evaluacion and aep.Codigo_Proceso = aev.Codigo_Proceso " +
                         "join Preguntas_Aleatorias pal on aep.Codigo_seccion = pal.Codigo_Seccion and aep.Numero_Pregunta = pal.Numero_Pregunta " +
                         "where aep.Numero_Evaluacion = " + numEval + " and aep.Codigo_Proceso = " + codProc + " and aep.Codigo_seccion = '" + sectionCode + "'" +
                         " And pec.Cod_Cargo_Evaluado = pec.Codigo_Cargo And aep.Nota = 0";

            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
                Connection = Cnn
            };
            Cnn.Open();
            using SqlDataReader sec = cmd.ExecuteReader();
            if (sec.HasRows)
            {
                try
                {
                    while (sec.Read())
                    {
                        AutoEvQuestionScoreViewModel scc = new AutoEvQuestionScoreViewModel()
                        {
                            Codigo_Seccion = sectionCode,
                            Numero_Pregunta = sec.GetInt32(0),
                            Texto_Pregunta = sec.GetString(1),
                            Nota = sec.GetDecimal(2),
                        };

                        RandomQ.Add(scc);
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = ex.InnerException.InnerException.Message;
                    RandomQ.Add(new AutoEvQuestionScoreViewModel()
                    {
                        Codigo_Seccion = "Error",
                        Texto_Pregunta = mensaje + " Valide la información"

                    });
                }
            }
            else
            {
                RandomQ.Add(new AutoEvQuestionScoreViewModel()
                {
                    Codigo_Seccion = "Error",
                    Texto_Pregunta = "No hay preguntas sin responder para este Dominio, Valide la información"

                });
            }
            if(RandomQ == null)
            {
                RandomQ.Clear();
                RandomQ.Add(new AutoEvQuestionScoreViewModel()
                {
                    Codigo_Seccion = "Error",
                    Texto_Pregunta = "Todas las preguntas de Auto Evaluación ya fueron respondidas"
                });
            }
            return RandomQ;
        }

    }
}
