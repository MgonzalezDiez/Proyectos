using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// Cargos Evaluadores
namespace Evaluacion360.Controllers
{
    public class EvaluationPositionController : Controller
    {
        string mensaje = string.Empty;

        // GET: EvaluationPosition
        public ActionResult List(int pagina = 1)
        {
            ViewBag.Status = true;
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.States = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            int CantidadRegitrosPorPagina = 10;

            var oEP = new List<EvPositionListViewModel>();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oEP = (from cev in Db.Cargos_Evaluadores
                   join car in Db.Cargos on cev.Codigo_Cargo equals car.Codigo_Cargo
                   join cae in Db.Cargos on cev.Cod_Cargo_Evaluado equals cae.Codigo_Cargo
                   join sta in Db.Estado_Componentes on cev.IdState equals sta.IdState
                   orderby car.Nombre_Cargo
                   select new EvPositionListViewModel
                   {
                       Codigo_Cargo = cev.Codigo_Cargo,
                       Nombre_Cargo = car.Nombre_Cargo,
                       Cod_Cargo_Evaluado = cev.Cod_Cargo_Evaluado,
                       Nombre_Cargo_Evaluado = cae.Nombre_Cargo,
                       Ponderacion = cev.Ponderacion,
                       IdState = sta.StateDescription

                   }).ToList();
            var TotalRegistros = oEP.Count();
            List<EvPositionListViewModel> lista = oEP.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();   //Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina);
            var Modelo = new ListEPViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View(Modelo);
        }

        // GET: EvaluationPosition/Details/5
        public ActionResult Details(string id, string id2)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.States = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            var oEP = new EvPositionViewModel();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oEP = (from evp in Db.Cargos_Evaluadores
                   where evp.Codigo_Cargo == id && evp.Cod_Cargo_Evaluado == id2
                   select new EvPositionViewModel
                   {
                       Codigo_Cargo = evp.Codigo_Cargo,
                       Cod_Cargo_Evaluado = evp.Cod_Cargo_Evaluado,
                       Ponderacion = evp.Ponderacion,
                       IdState = evp.IdState,
                   }).FirstOrDefault();
            return View(oEP);
        }

        // GET: EvaluationPosition/Create
        public ActionResult Create(string mensaje)
        {
            ViewBag.Status = true;
            ViewBag.Cargos = new SelectList(Utils.Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.States = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);

            try
            {
                if (mensaje != null && mensaje != "")
                {
                    if (mensaje == "Ok")
                    {
                        ViewBag.Message = "Cargo Evaluador Creado exitosamente";
                        ViewBag.Status = true;
                    }
                    else if (mensaje != "Ok")
                    {
                        ViewBag.Message = mensaje;
                        ViewBag.Status = false;
                    }
                }
                //return View();
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return View(mensaje);
        }


        // POST: EvaluationPosition/Create
        [HttpPost]
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Create(EvPositionViewModel EvPosition)
        {
            ViewBag.SectionState = new SelectList(Utils.Tools.LeerEstados(), "IdState", "StateDescription", "");
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oEP = new Cargos_Evaluadores
                    {
                        Codigo_Cargo = EvPosition.Codigo_Cargo,
                        Cod_Cargo_Evaluado = EvPosition.Cod_Cargo_Evaluado,
                        Ponderacion = EvPosition.Ponderacion,
                        IdState = 1
                    };
                    bd.Cargos_Evaluadores.Add(oEP);
                    bd.SaveChanges();

                    mensaje = "Ok";
                    ViewBag.Status = true;
                    #endregion
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error " + e.Message + " Contactar al administrador";
            }
            return RedirectToAction("List", "EvaluationPosition", new { mensaje });

        }

        // GET: EvaluationPosition/Edit/5
        public ActionResult Edit(string cargo, string cargoEv, string mensaje)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.States = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Cargo Evaluador modificado exitosamente";
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
                var oEP = new EvPositionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEP = (from evp in Db.Cargos_Evaluadores
                       where evp.Codigo_Cargo == cargo && evp.Cod_Cargo_Evaluado == cargoEv
                       select new EvPositionViewModel
                       {
                           Codigo_Cargo = evp.Codigo_Cargo,
                           Cod_Cargo_Evaluado = evp.Cod_Cargo_Evaluado,
                           Ponderacion = evp.Ponderacion,
                           IdState = evp.IdState,
                       }).FirstOrDefault();
                return View(oEP);


            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
                return View(cargo, cargoEv, mensaje);
            }
        }

        // POST: EvaluationPosition/Edit/5
        [HttpPost]
        public ActionResult Edit(EvPositionViewModel EvPosition)
        {
            try
            {
                using (var Db = new BD_EvaluacionEntities())
                {
                    var oEP = Db.Cargos_Evaluadores.Find(EvPosition.Codigo_Cargo, EvPosition.Cod_Cargo_Evaluado);
                    oEP.Ponderacion = EvPosition.Ponderacion;
                    oEP.IdState = EvPosition.IdState;
                    Db.Entry(oEP).State = System.Data.Entity.EntityState.Modified;
                    mensaje = "Ok";
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mensaje = "Se ha producido el siguiente error "
                        + ex.Message
                        + " Contacte al Administrador";

            }
            return RedirectToAction("Edit", "EvaluationPosition", new { id = EvPosition.Codigo_Cargo, id2 = EvPosition.Cod_Cargo_Evaluado, mensaje });
        }

        // GET: EvaluationPosition/Delete/5
        public ActionResult Delete(string  codCargo, string codCargoEval, string mensaje)
        {
            ViewBag.Status = true;
            ViewBag.Cargos = new SelectList(Utils.Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.States = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            int cant = ViewBag.Cargos.Items.Count;
            string[] PosEv = new string[cant];
            for (int i = 0; i < cant; i++)
            {
                PosEv[i] = ViewBag.Cargos.Items[i].Nombre_Cargo.ToString();
            }
            ViewBag.NombreCargos = PosEv;

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta de Evaluador Eliminada exitosamente";
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
                var oEP = new EvPositionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEP = (from cev in Db.Cargos_Evaluadores
                       where cev.Codigo_Cargo == codCargo && cev.Cod_Cargo_Evaluado == codCargoEval
                       orderby cev.Codigo_Cargo
                       select new EvPositionViewModel
                       {
                           Codigo_Cargo = cev.Codigo_Cargo,
                           Cod_Cargo_Evaluado = cev.Cod_Cargo_Evaluado,
                           Ponderacion = cev.Ponderacion,
                           IdState = cev.IdState

                       }).FirstOrDefault();
                return View(oEP);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return View(codCargo, codCargoEval, mensaje);
        }

        // POST: EvaluationPosition/Delete/5
        [HttpPost]
        public ActionResult Delete(EvPositionViewModel evPos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var bd = new BD_EvaluacionEntities())
                    {
                        var oUser = bd.Cargos_Evaluadores.Find(evPos.Codigo_Cargo, evPos.Cod_Cargo_Evaluado);
                        oUser.IdState = 3;

                        bd.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        bd.SaveChanges();
                        mensaje = "Ok";
                    }
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            mensaje += string.Format("{0} \n", item.Errors[0].ErrorMessage);
                        }
                    }
                    mensaje += " Contacte al Administrador";
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Delete", "EvaluationPosition", new { codCargo=evPos.Codigo_Cargo, codCargoEval= evPos.Cod_Cargo_Evaluado, mensaje });
        }
    }
}
