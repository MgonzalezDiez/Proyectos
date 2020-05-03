using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class PositionsController : Controller
    {
        string mensaje;
        int userType;
        // GET: Positions
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult List(string mensaje, int pagina = 1)
        {
            int CantidadRegitrosPorPagina = 8;

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Cargo Creado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }

            userType = Convert.ToInt32(System.Web.HttpContext.Current.Session["TipoUsuario"]);
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");

            var oPos = new List<PositionListViewModel>();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oPos = (from pos in Db.Cargos
                    join ec in Db.Estado_Componentes on pos.IdState equals ec.IdState
                    orderby pos.Nombre_Cargo
                    select new PositionListViewModel
                    {
                        Codigo_Cargo = pos.Codigo_Cargo,
                        Nombre_Cargo = pos.Nombre_Cargo,
                        Fondo = pos.Fondo,
                        Ciclo = pos.Ciclo,
                        IdState = ec.StateDescription
                    }).ToList();
            var TotalRegistros = oPos.Count();
            List<PositionListViewModel> lista = oPos.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();   //Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina);
            var Modelo = new ListPositionViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View(Modelo);
        }

        // GET: Positions/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                var oPos = new PositionListViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPos = (from car in Db.Cargos
                        join ec in Db.Estado_Componentes on car.IdState equals ec.IdState
                        where car.Codigo_Cargo == id
                        select new PositionListViewModel
                        {
                            Codigo_Cargo = car.Codigo_Cargo,
                            Nombre_Cargo = car.Nombre_Cargo,
                            Fondo = car.Fondo,
                            Ciclo = car.Ciclo,
                            IdState = ec.StateDescription
                        }).FirstOrDefault();
                return View(oPos);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return RedirectToAction("Edit", "Positions", new { mensaje });
            }
        }

        // GET: Positions/Create
        public ActionResult Create(string id, string mensaje)
        {
            ViewBag.Status = false;
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Cargo Creado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }

            if (id != null && id != "")
            {
                var oPos = new PositionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPos = (from car in Db.Cargos
                        where car.Codigo_Cargo == id
                        select new PositionViewModel
                        {
                            Codigo_Cargo = car.Codigo_Cargo,
                            Nombre_Cargo = car.Nombre_Cargo,
                            Fondo = car.Fondo,
                            Ciclo = car.Ciclo,
                            IdState = car.IdState ?? 0
                        }).FirstOrDefault();
                return View(oPos);
            }
            else
            {
                return View();
            }

        }

        // POST: Positions/Create
        [HttpPost]
        public ActionResult Create(PositionViewModel model)
        {
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oPos = new Cargos
                    {
                        Codigo_Cargo = model.Codigo_Cargo,
                        Nombre_Cargo = model.Nombre_Cargo,
                        Fondo = model.Fondo,
                        Ciclo = model.Ciclo,
                        IdState = model.IdState
                    };
                    bd.Cargos.Add(oPos);
                    bd.SaveChanges();
                    mensaje = "Ok";
                    #endregion
                }
                else
                {
                    #region Errores Modelo
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                        mensaje += " Contacte al Administrador";
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Create", "Positions", new { id = model.Codigo_Cargo, mensaje });
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(string id, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Cargo Modificado exitosamente";
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
                var oPos = new PositionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPos = (from car in Db.Cargos
                        where car.Codigo_Cargo == id
                        select new PositionViewModel
                        {
                            Codigo_Cargo = car.Codigo_Cargo,
                            Nombre_Cargo = car.Nombre_Cargo,
                            Fondo = car.Fondo,
                            Ciclo = car.Ciclo,
                            IdState = car.IdState ?? 0
                        }).FirstOrDefault();
                return View(oPos);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return RedirectToAction("Edit", "Positions", new { mensaje });
            }
        }

        // POST: Positions/Edit/5
        [HttpPost]
        public ActionResult Edit(PositionViewModel model)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            string codCcargo = model.Codigo_Cargo;
            try
            {
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var Db = new BD_EvaluacionEntities();
                    var oPos = Db.Cargos.Find(model.Codigo_Cargo);
                    oPos.Nombre_Cargo = model.Nombre_Cargo.ToUpper();
                    oPos.Fondo = model.Fondo;
                    oPos.Ciclo = model.Ciclo;
                    if (model.IdState > 0)
                    {
                        oPos.IdState = model.IdState;
                    }
                    Db.Entry(oPos).State = System.Data.Entity.EntityState.Modified;
                    mensaje = "Ok";
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
                            mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                    }
                    mensaje += " Contacte al Administrador";
                    #endregion
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
            }
            return RedirectToAction("Edit", "Positions", new { id = codCcargo, mensaje });
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(string id, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Cargo Eliminado exitosamente";
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
                var oPos = new PositionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPos = (from car in Db.Cargos
                        where car.Codigo_Cargo == id
                        select new PositionViewModel
                        {
                            Codigo_Cargo = car.Codigo_Cargo,
                            Nombre_Cargo = car.Nombre_Cargo,
                            Fondo = car.Fondo,
                            Ciclo = car.Ciclo,
                            IdState = car.IdState ?? 0
                        }).FirstOrDefault();
                return View(oPos);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return RedirectToAction("Delete", "Positions", new { mensaje });
            }
        }

        // POST: Positions/Delete/5
        [HttpPost]
        public ActionResult Delete(PositionViewModel model)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            try
            {
                if (ModelState.IsValid)
                {
                    #region Elimina Datos
                    using var Db = new BD_EvaluacionEntities();
                    var oPos = Db.Cargos.Find(model.Codigo_Cargo);
                    oPos.IdState = 3;
                    Db.Entry(oPos).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    mensaje = "Ok";
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
                            mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                    }
                    mensaje += " Contacte al Administrador";
                    #endregion
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
            }
            return RedirectToAction("Delete", "Positions", new { id = model.Codigo_Cargo, mensaje });
        }
    }
}
