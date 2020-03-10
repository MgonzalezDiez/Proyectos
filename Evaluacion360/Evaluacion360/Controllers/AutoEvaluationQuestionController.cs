using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
                               orderby EvQ.Codigo_Proceso, EvQ.Codigo_seccion, EvQ.Codigo_seccion, EvQ.Numero_Pregunta
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
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.Users = new SelectList(Tools.LeerUsuarios(), "Codigo_Usuario", "Nombre_Usuario", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");


            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta de Auto Evaluación Creada exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            return View();
        }

        // POST: EvaluacionPreguntas/Create
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Create(AutoEvaluationQuestionViewModel ae)
        {
            ViewBag.Status = true;
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription","");
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
                        Numero_Evaluacion=ae.Numero_Evaluacion,
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
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return View( new { ae.Numero_Evaluacion, Mensaje });
        }

        // GET: EvaluacionPreguntas/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(int numEval, int codProc, string codSecc, int numAsk, string mensaje)
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
                var oAEQ = new AutoEvaluationQuestionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAEQ = (from aeq in Db.Auto_Evaluacion_Preguntas
                            where aeq.Numero_Evaluacion == numEval && aeq.Codigo_Proceso == codProc && aeq.Codigo_seccion == codSecc && aeq.Numero_Pregunta == numAsk
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
                return RedirectToAction("Edit", "AEQ", new { numEval, codProc, codSecc, numAsk, Mensaje });
            }
        }

        // POST: EvaluacionPreguntas/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Edit(AutoEvaluationQuestionViewModel model)
        {
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            try
            {
                using var Db = new BD_EvaluacionEntities();
                var oAEQ = Db.Auto_Evaluacion_Preguntas.Find(model.Numero_Evaluacion, model.Codigo_Proceso, model.Codigo_Seccion, model.Numero_Pregunta);
                oAEQ.Nota = model.Nota;
                Db.Entry(oAEQ).State = System.Data.Entity.EntityState.Modified;
                Mensaje = "Ok";
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Edit", "AEQ", new { numEval = model.Numero_Evaluacion, codProc = model.Codigo_Proceso, codSecc = model.Codigo_Seccion, numAsk = model.Numero_Pregunta, Mensaje });
        }

        // GET: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(int numEval, int codProc, string codSecc, int numAsk, string mensaje)
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
                        where aeq.Numero_Evaluacion == numEval && aeq.Codigo_Proceso == codProc && aeq.Codigo_seccion == codSecc && aeq.Numero_Pregunta == numAsk
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
                return RedirectToAction("Delete", "AEQ", new {numEval, codProc, codSecc, numAsk, Mensaje });
            }
        }

        // POST: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Delete(int numEval, int codProc, string codSecc, int numAsk)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var bd = new BD_EvaluacionEntities();
                    var oAEQ = bd.Auto_Evaluacion_Preguntas.Find(numEval, codProc, codSecc, numAsk);

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
            return RedirectToAction("Delete", "Section", new { numEval,codProc, codSecc, numAsk, Mensaje });
        }
    }
}
