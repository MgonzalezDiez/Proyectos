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
    public class AutoEvaluationController : Controller
    {
        private string Mensaje = string.Empty;
        // GET: EvaluacionPreguntas
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult List(int pagina = 1)
        {
            ActionResult result = null;
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;
            try
            {
                //Usuarios tUser = (Usuarios)Session["User"];
                var oAE = new List<AutoEvaluationListViewModel>();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();

                oAE = (from EvQ in Db.Auto_Evaluaciones
                       join pro in Db.Procesos_Evaluacion on EvQ.Codigo_Proceso equals pro.Codigo_Proceso
                       join usr in Db.Usuarios on EvQ.Codigo_Usuario equals usr.Codigo_Usuario
                               select new AutoEvaluationListViewModel
                               {
                                   Numero_Evaluacion = EvQ.Numero_Evaluacion,
                                   Codigo_Proceso = EvQ.Codigo_Proceso,
                                   NombreProceso = pro.Nombre_Proceso,
                                   Codigo_Usuario = EvQ.Codigo_Usuario,
                                   Nombre_Usuario = usr.Nombre_Usuario,
                                   Fecha = EvQ.Fecha,
                                   Metas = EvQ.Metas,
                                   Estado_AE = EvQ.Estado_AE
                               }).ToList();

                var TotalRegistros = oAE.Count();
                List<AutoEvaluationListViewModel> lista = oAE.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
                var Modelo = new ListAutoEvaluationViewModel
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
        public ActionResult Details(AutoEvaluationViewModel AE)
        {
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            try
            {
                //Usuarios tUser = (Usuarios)Session["User"];
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();

                Auto_Evaluaciones oAeq = Db.Auto_Evaluaciones.Find(AE.Numero_Evaluacion, AE.Codigo_Proceso, AE.Codigo_Usuario);
                oAeq.Numero_Evaluacion = AE.Numero_Evaluacion;
                oAeq.Codigo_Proceso = AE.Codigo_Proceso;
                oAeq.Codigo_Usuario = AE.Codigo_Usuario;
                oAeq.Fecha = AE.Fecha;
                oAeq.Logros = AE.Logros;
                oAeq.Metas = AE.Metas;
                oAeq.Estado_AE = AE.Estado_AE;
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
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Auto Evaluación Creada exitosamente";
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
        public ActionResult Create(AutoEvaluationViewModel ae)
        {
            ViewBag.Status = true;
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");

            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oAe = new Auto_Evaluaciones
                    {
                        Numero_Evaluacion = ae.Numero_Evaluacion,
                        Codigo_Proceso = ae.Codigo_Proceso,
                        Codigo_Usuario = ae.Codigo_Usuario,
                        Fecha = ae.Fecha,
                        Logros = ae.Logros,
                        Metas = ae.Metas,
                        Nota_Final_AE = ae.Nota_Final_AE,
                        Estado_AE = ae.Estado_AE,
                    };
                    bd.Auto_Evaluaciones.Add(oAe);
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
        public ActionResult Edit(int numEval, int codProc, string codUsu, string mensaje)
        {
            ViewBag.Status = true;
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");

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
            }
            try
            {
                var oAE = new AutoEvaluationViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAE = (from ae in Db.Auto_Evaluaciones
                            where ae.Numero_Evaluacion == numEval && ae.Codigo_Proceso == codProc && ae.Codigo_Usuario == codUsu
                       select new AutoEvaluationViewModel
                            {
                                Numero_Evaluacion = ae.Numero_Evaluacion,
                                Codigo_Proceso = ae.Codigo_Proceso,
                                Codigo_Usuario = ae.Codigo_Usuario,
                                Fecha = ae.Fecha,
                                Logros = ae.Logros,
                                Metas = ae.Metas,
                                Nota_Final_AE = ae.Nota_Final_AE == null ? 0 : ae.Nota_Final_AE,
                                Estado_AE = ae.Estado_AE,

                            }).FirstOrDefault();
                return View(oAE);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
                return View(numEval.ToString(), mensaje);

            }
        }

        // POST: EvaluacionPreguntas/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Edit(AutoEvaluationViewModel model)
        {
            ViewBag.Status = true;
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");

            try
            {
                using var Db = new BD_EvaluacionEntities();
                var oAE = Db.Auto_Evaluaciones.Find(model.Numero_Evaluacion, model.Codigo_Proceso, model.Codigo_Usuario);
                oAE.Fecha = model.Fecha;
                oAE.Logros = model.Logros;
                oAE.Metas = model.Metas;
                oAE.Nota_Final_AE = model.Nota_Final_AE;
                oAE.Estado_AE = model.Estado_AE;
                Db.Entry(oAE).State = System.Data.Entity.EntityState.Modified;
                Mensaje = "Ok";
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Edit", "Section", new { numEval = model.Numero_Evaluacion, codProc = model.Codigo_Proceso, codUsu = model.Codigo_Usuario, Mensaje });
        }

        // GET: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(int numEval, int codProc, string codUsu, string mensaje)
        {
            ViewBag.Status = true;
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Auto Evaluación Eliminada exitosamente";
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
                var oAE = new AutoEvaluationViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oAE = (from ae in Db.Auto_Evaluaciones
                        where ae.Numero_Evaluacion == numEval && ae.Codigo_Proceso == codProc && ae.Codigo_Usuario == codUsu                         
                        select new AutoEvaluationViewModel
                        {
                            Numero_Evaluacion = ae.Numero_Evaluacion,
                            Codigo_Proceso = ae.Codigo_Proceso,
                            Codigo_Usuario = ae.Codigo_Usuario,
                            Fecha = ae.Fecha,
                            Logros = ae.Logros,
                            Metas = ae.Metas,
                            Nota_Final_AE = ae.Nota_Final_AE == null ? 0: ae.Nota_Final_AE,
                            Estado_AE = ae.Estado_AE,
                            
                        }).FirstOrDefault();

                return View(oAE);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
                return RedirectToAction("Delete", "Section", new { mensaje });
            }
        }

        // POST: EvaluacionPreguntas/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Delete(int numEval, int codProc, string codUsu)
        {
            ViewBag.Status = true;
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");

            try
            {
                if (ModelState.IsValid)
                {
                    using var bd = new BD_EvaluacionEntities();
                    var oAE = bd.Auto_Evaluaciones.Find(numEval, codProc, codUsu);

                    bd.Entry(oAE).State = System.Data.Entity.EntityState.Deleted;
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
            return RedirectToAction("Delete", "Section", new { numEval, codProc, codUsu, Mensaje });
        }
    }
}
