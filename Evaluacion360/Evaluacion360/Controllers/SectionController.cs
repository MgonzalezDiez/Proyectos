using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web.Mvc;
using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;

namespace Evaluacion360.Controllers
{
    public class SectionController : Controller
    {
        string Mensaje = string.Empty;

        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult List(string mensaje, int pagina = 1)
        {
            int CantidadRegitrosPorPagina = 8;

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Dominio Creado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            var oSection = new List<SectionListViewModel>();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oSection = (from secc in Db.Secciones
                        join sta in Db.Estado_Componentes on secc.IdState equals sta.IdState
                        where secc.IdState == 1
                        orderby secc.Id
                        select new SectionListViewModel
                        {
                            Id = secc.Id,
                            Codigo_Seccion = secc.Codigo_Seccion,
                            Nombre_Seccion = secc.Nombre_Seccion,
                            Ponderacion_S = secc.Ponderacion_S ?? 0,
                            IdState = sta.StateDescription
                        }).ToList();
            var TotalRegistros = oSection.Count();
            List<SectionListViewModel> lista = oSection.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();   //Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina);
            var Modelo = new ListSectionViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View(Modelo);
        }

        [HttpGet]
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Create(string codSecc, string mensaje)
        {
            ViewBag.Status = true;
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Dominio Creado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }

            var oSection = new SectionViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oSection = (from secc in Db.Secciones
                            where secc.Codigo_Seccion == codSecc
                            select new SectionViewModel
                            {
                                Codigo_Seccion = secc.Codigo_Seccion,
                                Nombre_Seccion = secc.Nombre_Seccion,
                                Ponderacion_S = secc.Ponderacion_S ?? 0,
                                IdState = secc.IdState
                            }).FirstOrDefault();
            }
            return View(oSection);
        }
        [HttpPost]
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Create(SectionViewModel section)
        {
            ViewBag.SectionState = new SelectList(Utils.Tools.LeerEstados(), "IdState", "StateDescription", "");
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    var UCodeExists = IsSectionCodeExists(section.Codigo_Seccion);
                    if (!UCodeExists)
                    {
                        ViewBag.Message = "Código de Dominio ya existe";
                        return View();
                    }
                    else
                    {
                        var SectionExists = IsSectionNameExists(section.Nombre_Seccion);
                        if (!SectionExists)
                        {
                            ViewBag.Message = "Nombre de Dominio ya existe";
                            return View();
                        }
                        else
                        {
                            #region Graba Datos
                            using var bd = new BD_EvaluacionEntities();
                            var oSection = new Secciones
                            {
                                Codigo_Seccion = section.Codigo_Seccion.ToUpper(),
                                Nombre_Seccion = section.Nombre_Seccion,
                                Ponderacion_S = section.Ponderacion_S,
                                IdState = 1
                            };
                            bd.Secciones.Add(oSection);
                            bd.SaveChanges();

                            Mensaje = "Ok";
                            ViewBag.Status = true;
                            #endregion
                        }
                    }
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
            catch (Exception ex)
            {
                Mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
            }
            return RedirectToAction("Create", "Section", new { codSecc = section.Codigo_Seccion, Mensaje });
        }

        // GET: User/Details/5
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Details(int? id)
        {
            ViewBag.SectionState = new SelectList(Utils.Tools.LeerEstados(), "IdState", "StateDescription", "");
            var oSection = new SectionViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {

                oSection = (from secc in Db.Secciones
                            where secc.Id == id
                            select new SectionViewModel
                            {
                                Codigo_Seccion = secc.Codigo_Seccion,
                                Nombre_Seccion = secc.Nombre_Seccion,
                                Ponderacion_S = secc.Ponderacion_S ?? 0,
                                IdState = secc.IdState
                            }).FirstOrDefault();
            }
            return View(oSection);

        }

        // GET: User/Edit/5
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Edit(int? id, string mensaje)
        {
            ViewBag.SectionState = new SelectList(Utils.Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Dominio modificado exitosamente";
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
                var oSection = new SectionEditViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oSection = (from secc in Db.Secciones
                            where secc.Id == id
                            select new SectionEditViewModel
                            {
                                Id = secc.Id,
                                Codigo_Seccion = secc.Codigo_Seccion,
                                Nombre_Seccion = secc.Nombre_Seccion,
                                Ponderacion_S = secc.Ponderacion_S ?? 0,
                                IdState = secc.IdState
                            }).FirstOrDefault();
                return View(oSection);
            }
            catch (Exception ex)
            {
                Mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return View(id.ToString(), Mensaje);

            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Edit(SectionViewModel model)
        {
            ViewBag.SectionState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            try
            {
                using var Db = new BD_EvaluacionEntities();
                var oSection = Db.Secciones.Find(model.Codigo_Seccion.Trim());
                oSection.Nombre_Seccion = model.Nombre_Seccion.ToUpper();
                oSection.Ponderacion_S = model.Ponderacion_S;
                if (model.IdState > 0)
                {
                    oSection.IdState = model.IdState;
                }
                Db.Entry(oSection).State = System.Data.Entity.EntityState.Modified;
                Mensaje = "Ok";
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
            }
            return RedirectToAction("Edit", "Section", new { Mensaje });
        }

        // GET: User/Delete/5
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Delete(int? id, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Dominio Eliminado exitosamente";
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
                var oSection = new SectionEditViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    oSection = (from secc in Db.Secciones
                                where secc.Id == id
                                select new SectionEditViewModel
                                {
                                    Id = secc.Id,
                                    Codigo_Seccion = secc.Codigo_Seccion,
                                    Nombre_Seccion = secc.Nombre_Seccion,
                                    Ponderacion_S = secc.Ponderacion_S ?? 0,
                                    IdState = secc.IdState
                                }).FirstOrDefault();
                }
                ViewBag.DomState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 3);
                return View(oSection);
            }
            catch (Exception ex)
            {
                Mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return View(id.ToString(), Mensaje);
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 4)]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SectionEditViewModel svm)
        {
            try
            {
                if (!ValidaDominios(svm.Codigo_Seccion))
                {
                    if (ModelState.IsValid)
                    {
                        using var bd = new BD_EvaluacionEntities();
                        var oUser = bd.Secciones.Find(svm.Codigo_Seccion);
                        oUser.IdState = 3;

                        bd.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
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
                else
                {
                    Mensaje = "Este dominio tiene preguntas asociadas, No se puede eliminar";
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
            }
            return RedirectToAction("Delete", "Section", new { id = svm.Id, Mensaje });

        }

        protected string Valid(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
                return val.ToString();
            else
                return Convert.ToString(0);
        }

        [NonAction]
        public bool IsSectionNameExists(string SectionName)
        {
            using BD_EvaluacionEntities Bd = new BD_EvaluacionEntities();
            var existe = Bd.Secciones.Where(a => a.Nombre_Seccion == SectionName).FirstOrDefault();
            return existe == null;
        }

        [NonAction]
        public bool IsSectionCodeExists(string SectionCode)
        {
            using BD_EvaluacionEntities Bd = new BD_EvaluacionEntities();
            var existe = Bd.Secciones.Where(a => a.Codigo_Seccion == SectionCode).FirstOrDefault();
            return existe == null;
        }

        [NonAction]
        public bool ValidaDominios(string codSecc)
        {
            using BD_EvaluacionEntities bd = new BD_EvaluacionEntities();

            int total = 0;

            var query = (from se in bd.Secciones
                         join pa in bd.Preguntas_Aleatorias on se.Codigo_Seccion equals pa.Codigo_Seccion
                         where se.Codigo_Seccion == codSecc
                         select pa.Codigo_Seccion);
            total = query.Count();

            query = (from se in bd.Secciones
                     join pc in bd.Preguntas_Cargos on se.Codigo_Seccion equals pc.Codigo_seccion
                     where se.Codigo_Seccion == codSecc
                     select pc.Codigo_seccion);
            total += query.Count();

            if (total > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}