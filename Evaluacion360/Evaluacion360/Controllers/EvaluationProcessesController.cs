using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class EvaluationProcessesController : Controller
    {
        string Mensaje = string.Empty;

        // GET: EvaluationProcesses
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult List(int pagina = 1)
        {
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;

            var oEP = new List<EvProcsViewModel>();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oEP = (from ep in Db.Procesos_Evaluacion orderby ep.Nombre_Proceso, ep.Ano_Proceso, ep.Mes_Proceso
                       select new EvProcsViewModel 
                       {
                           Codigo_Proceso = ep.Codigo_Proceso,
                           Nombre_Proceso = ep.Nombre_Proceso,
                           Ano_Proceso = ep.Ano_Proceso,
                           Mes_Proceso = ep.Mes_Proceso,
                           Retroalimentacion = ep.Retroalimentacion,
                           Estado_PE = ep.Estado_PE
                       }).ToList();
            }
            var TotalRegistros = oEP.Count();
            List<EvProcsViewModel> lista = oEP.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
            var Modelo = new ListEvProcsViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View(Modelo);
        }

        // GET: EvaluationProcesses/Details/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Details(int id)
        {
            var oEP = new EvProcsViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oEP = (from ep in Db.Procesos_Evaluacion
                       where ep.Codigo_Proceso == id
                       select new EvProcsViewModel
                       {
                           Codigo_Proceso = ep.Codigo_Proceso,
                           Nombre_Proceso = ep.Nombre_Proceso,
                           Ano_Proceso = ep.Ano_Proceso,
                           Mes_Proceso = ep.Mes_Proceso,
                           Retroalimentacion = ep.Retroalimentacion
                       }).FirstOrDefault();
            }
            return View(oEP);
        }

        // GET: EvaluationProcesses/Create
        [HttpGet]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Create(int? codProc, string mensaje)
        {
            ViewBag.StateEval = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);

            ViewBag.Status = true;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Proceso de Evaluación Creado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            if (codProc != null && codProc != 0)
            {
                var oPE = new EvProcsViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPE = (from pe in Db.Procesos_Evaluacion
                        where pe.Codigo_Proceso == codProc 
                        select new EvProcsViewModel
                        {
                            Codigo_Proceso = pe.Codigo_Proceso,
                            Nombre_Proceso = pe.Nombre_Proceso,
                            Ano_Proceso = pe.Ano_Proceso,
                            Mes_Proceso = pe.Mes_Proceso,
                            Retroalimentacion = pe.Retroalimentacion,
                            Estado_PE = pe.Estado_PE,
                            IdState = pe.IdState
                        }).FirstOrDefault();
                return View(oPE);
            }
            else
            {
                return View();
            }
        }

        // POST: EvaluationProcesses/Create
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Create(EvProcsViewModel ep)
        {
            ViewBag.StateEval = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oEP = new Procesos_Evaluacion
                    {
                        Codigo_Proceso = ep.Codigo_Proceso,
                        Nombre_Proceso = ep.Nombre_Proceso,
                        Ano_Proceso = ep.Ano_Proceso,
                        Mes_Proceso = ep.Mes_Proceso,
                        Retroalimentacion = ep.Retroalimentacion,
                        Estado_PE = ep.Estado_PE,
                        IdState = ep.IdState
                    };
                    bd.Procesos_Evaluacion.Add(oEP);
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
                    }
                    Mensaje += " Contacte al Administrador";
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
            }
            return RedirectToAction("Create", "EvaluationProcesses", new { codProc = ep.Codigo_Proceso, Mensaje });
        }

        // GET: EvaluationProcesses/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(int codProc, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Proceso de Evaluación Creado exitosamente";
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
                var oEP = new EvProcsViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEP = (from ep in Db.Procesos_Evaluacion
                       where ep.Codigo_Proceso == codProc
                       select new EvProcsViewModel
                       {
                           Codigo_Proceso = ep.Codigo_Proceso,
                           Nombre_Proceso = ep.Nombre_Proceso,
                           Ano_Proceso = ep.Ano_Proceso,
                           Mes_Proceso = ep.Mes_Proceso,
                           Retroalimentacion = ep.Retroalimentacion,
                           Estado_PE = ep.Estado_PE,
                           IdState = ep.IdState
                       }).FirstOrDefault();
                return View(oEP);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                   + e.Message
                   + " Contactar al administrador";
                return View(new { Mensaje });
            }
        }

        // POST: EvaluationProcesses/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Edit(int codProc, EvProcsViewModel ep)
        {
            try
            {
                using var Db = new BD_EvaluacionEntities();
                Procesos_Evaluacion oEP = Db.Procesos_Evaluacion.Find(codProc);

                oEP.Nombre_Proceso = ep.Nombre_Proceso;
                oEP.Ano_Proceso = ep.Ano_Proceso;
                oEP.Mes_Proceso = ep.Mes_Proceso;
                oEP.Retroalimentacion = ep.Retroalimentacion;
                Db.Entry(oEP).State = EntityState.Modified;
                Mensaje = "Ok";
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
            }
            return View(new { codProc, Mensaje });
        }

        // GET: EvaluationProcesses/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(int codProc, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Proceso de Evaluación Eliminado exitosamente";
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
                var oEP = new EvProcsViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    oEP = (from ep in Db.Procesos_Evaluacion
                           where ep.Codigo_Proceso == codProc
                           select new EvProcsViewModel
                           {
                               Codigo_Proceso = ep.Codigo_Proceso,
                               Nombre_Proceso = ep.Nombre_Proceso,
                               Ano_Proceso = ep.Ano_Proceso,
                               Mes_Proceso = ep.Mes_Proceso,
                               Retroalimentacion = ep.Retroalimentacion
                           }).FirstOrDefault();
                }
                return View(oEP);
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return  View(new { codProc, Mensaje } );
            }
        }

        // POST: EvaluationProcesses/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Delete(int codProc, EvProcsViewModel ep)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var Db = new BD_EvaluacionEntities();
                    var oEP = Db.Procesos_Evaluacion.Find(codProc);
                    oEP.Nombre_Proceso = ep.Nombre_Proceso.ToUpper();
                    oEP.Ano_Proceso = ep.Ano_Proceso;
                    oEP.Mes_Proceso = ep.Mes_Proceso;
                    oEP.Retroalimentacion = ep.Retroalimentacion;
                    oEP.IdState = ep.IdState;
                    Db.Entry(oEP).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
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
                Mensaje = "Ocurrió el siguiente error " 
                        + e.Message 
                        + " Contactar al administrador";
                //return RedirectToAction("Delete", "RandomQuestion", new { Mensaje });
            }
            return View(new { codProc, Mensaje });
        }

        [HttpGet]
        public ActionResult GenerarProcesoEvaluacion(string codUsu, string codProc, string mensaje)
        {
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.Usuarios = new SelectList(Tools.LeerUsuarios(""), "Codigo_Usuario", "Nombre_Usuario", 1);

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Generación de Procesos de Evaluación Terminado exitosamente";
                    ViewBag.Status = true;
                }
                else
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerarProcesoEvaluacion(EvProcsGeneration ePG)
        {
            var Db = new BD_EvaluacionEntities();
            var CodUsu = new SqlParameter("@CodigoUsuario", ePG.Codigo_Usuario);
            var CodProc = new SqlParameter("@Proceso", ePG.Codigo_Proceso);

            var affectedRows = Db.Database.ExecuteSqlCommand("Crea_Evaluaciones @CodigoUsuario = {0}, @Proceso = {1}", CodUsu.Value, CodProc.Value );
            Mensaje = "Error";
            if (affectedRows > 0)
            {
                Mensaje = "Ok";
            }
            return View(new { codUsu = ePG.Codigo_Usuario, codProc = ePG.Codigo_Proceso, Mensaje });
        }

    }
}
