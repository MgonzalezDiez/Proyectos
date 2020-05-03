using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class PositionEvaluationsController : Controller
    {
        private string Mensaje = string.Empty;
        // GET: Evaluacion Cargos
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult List(int pagina = 1)
        {
            ActionResult result = null;
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;
            try
            {
                #region Muestra Datos
                Usuarios tUser = (Usuarios)Session["User"];
                var oPE = new List<PositionEvaluationsListViewModel>();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();

                oPE = (from evp in Db.Evaluaciones_Cargos
                       join pro in Db.Procesos_Evaluacion on evp.Codigo_Proceso equals pro.Codigo_Proceso
                       join usr in Db.Usuarios on evp.Cod_Cargo_Evaluado equals usr.Codigo_Usuario
                       join car in Db.Cargos on evp.Cod_Cargo_Evaluado equals car.Codigo_Cargo
                       join sta in Db.Estado_Evaluaciones on evp.Estado_EC equals sta.IdState
                       where evp.Cod_Usuario_Evaluado == tUser.Codigo_Usuario
                       select new PositionEvaluationsListViewModel
                       {
                           Numero_Evaluacion = evp.Numero_Evaluacion,
                           Codigo_Proceso = evp.Codigo_Proceso,
                           Codigo_Usuario_evaluado = evp.Cod_Usuario_Evaluado,
                           Nombre_Usuario_Evaluado = usr.Nombre_Usuario,
                           Cod_Cargo_Evaluado = evp.Cod_Cargo_Evaluado,
                           Nombre_Cargo_Evaluado = car.Nombre_Cargo,
                           Estado_EC = evp.Estado_EC,
                           Nombre_Estado_EC = sta.StateDescription,
                           Nota_Final_EC = evp.Nota_Final_EC ?? 0

                       }).ToList();

                var TotalRegistros = oPE.Count();
                List<PositionEvaluationsListViewModel> lista = oPE.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
                var Modelo = new ListPositionEvaluationViewModel
                {
                    Secciones = lista,
                    PaginaActual = pagina,
                    TotalDeRegistros = TotalRegistros,
                    RegistrosPorPagina = CantidadRegitrosPorPagina
                };
                return View(Modelo);
                #endregion
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
                #region Muestra Datos
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
                #endregion
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
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            ViewBag.Sections = new SelectList(Tools.DominiosPorUsuarioCargo(oUser.Codigo_Usuario), "Codigo_Seccion", "Nombre_Seccion");

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
                    #region Errores de Modelo
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        Mensaje += " Contacte al Administrador";
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Create", "PositionEvaluations", new { ae.Numero_Evaluacion, Mensaje });
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
                #region Muestra Datos
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
                #endregion
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
                if (ModelState.IsValid)
                {
                    #region Graba Datos
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
                    #endregion
                }
                else
                {
                    #region Errores de Modelo
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                    }
                    Mensaje += " Contacte al Administrador";
                    #endregion
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Edit", "PositionEvaluations", new { numEval = model.Numero_Evaluacion, codProc = model.Codigo_Proceso, codUsu = model.Codigo_Usuario, Mensaje });
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
                #region Muestra Datos
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
                #endregion
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
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oAE = bd.Auto_Evaluaciones.Find(numEval, codProc, codUsu);

                    bd.Entry(oAE).State = System.Data.Entity.EntityState.Deleted;
                    bd.SaveChanges();
                    Mensaje = "Ok";
                    #endregion
                }
                else
                {
                    #region Errores de Modelo
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            Mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        Mensaje += " Contacte al Administrador";
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Delete", "PositionEvaluations", new { numEval, codProc, codUsu, Mensaje });
        }
    }
}
