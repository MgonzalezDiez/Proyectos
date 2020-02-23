using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class PositionQuestionController : Controller
    {
        string mensaje = string.Empty;

        // GET: PostionQuestion
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult List(int pagina = 1)
        {
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;
            try
            {
                var oPQ = new List<PQListViewModel>();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {

                    oPQ = (from pos in Db.Preguntas_Cargos
                           join car in Db.Cargos on pos.Codigo_Cargo equals car.Codigo_Cargo
                           join crg in Db.Cargos on pos.Cod_Cargo_Evaluado equals crg.Codigo_Cargo
                           join sec in Db.Secciones on pos.Codigo_seccion equals sec.Codigo_Seccion
                           join sta in Db.Estado_Componentes on pos.IdState equals sta.IdState
                           select new PQListViewModel
                           {
                               Codigo_Cargo = pos.Codigo_Cargo,
                               Nombre_Cargo = car.Nombre_Cargo,
                               Cod_Cargo_Evaluado = pos.Cod_Cargo_Evaluado,
                               Nombre_Cargo_Evaluado = crg.Nombre_Cargo,
                               Codigo_seccion = pos.Codigo_seccion,
                               Nombre_Seccion = sec.Nombre_Seccion,
                               Numero_Pregunta = pos.Numero_Pregunta,
                               IdState = sta.StateDescription
                           }).ToList();
                }
                var TotalRegistros = oPQ.Count();
                List<PQListViewModel> lista = oPQ.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
                var Modelo = new ListPQViewModel
                {
                    Secciones = lista,
                    PaginaActual = pagina,
                    TotalDeRegistros = TotalRegistros,
                    RegistrosPorPagina = CantidadRegitrosPorPagina
                };
                return View(Modelo);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contacte al admnistrador";
                return View(new { mensaje });
            }
        }

        // GET: PostionQuestion/Create
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Create(string mensaje)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias(""), "Numero_Pregunta", "Texto_Pregunta", "");

            ViewBag.Status = true;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta por Cargo Creada exitosamente";
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

        // POST: PostionQuestion/Create
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public ActionResult Create(PQViewModel Pq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oPQ = new Preguntas_Cargos
                    {
                        Codigo_Cargo = Pq.Codigo_Cargo,
                        Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                        Codigo_seccion = Pq.Codigo_seccion,
                        Numero_Pregunta = Pq.Numero_Pregunta,
                    };

                bd.Preguntas_Cargos.Add(oPQ);
                    bd.SaveChanges();
                    mensaje = "Ok";
                    #endregion
                }
                else
                {
                    mensaje = "El modelo no es válido" + Environment.NewLine + "Contactar al administrador";
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error" + e.Message +  "Contactar al administrador";
            }
            return RedirectToAction("List", "PositionQuestion", new { mensaje });
        }

        // GET: PostionQuestion/Details/5
        [HttpGet]
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Details(string id1, string id2, string id3, int id4, PQListViewModel pQV)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            var oPQ = new PQViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {

                oPQ = (from Pq in Db.Preguntas_Cargos
                       where Pq.Codigo_Cargo == id1 && Pq.Cod_Cargo_Evaluado == id2 && Pq.Codigo_seccion == id3.Trim() && Pq.Numero_Pregunta == id4
                       select new PQViewModel
                       {
                           Codigo_Cargo = Pq.Codigo_Cargo,
                           Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                           Codigo_seccion = Pq.Codigo_seccion,
                           Numero_Pregunta = Pq.Numero_Pregunta
                           
                       }).FirstOrDefault();
            }
            return View(oPQ);
        }

        // GET: PostionQuestion/Edit/5
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Edit(string codCargo, string codCargoEval, string codSection, string mensaje)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta por Cargo modificada exitosamente";
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
                var oPQ = new PQViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPQ = (from Pq in Db.Preguntas_Cargos
                       where Pq.Codigo_Cargo == codCargo && Pq.Cod_Cargo_Evaluado == codCargoEval && Pq.Codigo_seccion == codSection
                       select new PQViewModel
                       {
                           Codigo_Cargo = Pq.Codigo_Cargo,
                           Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                           Codigo_seccion = Pq.Codigo_seccion,
                           Numero_Pregunta = Pq.Numero_Pregunta,
                           IdState = 1
                       }).FirstOrDefault();
                return View(oPQ);
            }
            catch (Exception e)
            {
                return RedirectToAction("~/Error/UnAuthorizedOperation?Error = " + e.Message);
            }
        }

        // POST: PostionQuestion/Edit/5
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public ActionResult Edit(PQViewModel Pq)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            try
            {
                using (var Db = new BD_EvaluacionEntities())
                {
                    Preguntas_Cargos oPq = Db.Preguntas_Cargos.Where(i => i.Codigo_Cargo == Pq.Codigo_Cargo && i.Cod_Cargo_Evaluado == Pq.Cod_Cargo_Evaluado && i.Codigo_seccion == Pq.Codigo_seccion && i.Numero_Pregunta == Pq.Numero_Pregunta).SingleOrDefault();
                    if(oPq != null)
                    {
                        oPq.Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado;
                        oPq.Codigo_seccion = Pq.Codigo_seccion;
                        oPq.Numero_Pregunta = Pq.Numero_Pregunta;
                        Db.Entry(oPq).State = System.Data.Entity.EntityState.Modified;
                        mensaje = "Ok";
                        Db.SaveChanges();
                    }
                    else
                    {
                        mensaje = "El registro no se modificó";
                    }

                }
                return RedirectToAction("Edit", "PositionQuestion", new { mensaje });
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error" + e.Message + "Contactar al administrador";
                return RedirectToAction("Edit", "Position", new { mensaje });
            }
        }

        // GET: PostionQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Delete(PQViewModel oPQ, string mensaje)
        {
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_seccion", "Nombre_Seccion", ""); 
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta por Cargo Eliminada exitosamente";
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
                var oPosQ = new PQViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    oPosQ = (from Pq in Db.Preguntas_Cargos
                           where Pq.Codigo_Cargo == oPQ.Codigo_Cargo && Pq.Cod_Cargo_Evaluado == oPQ.Cod_Cargo_Evaluado && Pq.Codigo_seccion == oPQ.Codigo_seccion && Pq.Numero_Pregunta == oPQ.Numero_Pregunta
                           select new PQViewModel
                           {
                               Codigo_Cargo = Pq.Codigo_Cargo,
                               Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                               Codigo_seccion = Pq.Codigo_seccion,
                               Numero_Pregunta = Pq.Numero_Pregunta,
                               IdState = Pq.IdState
                           }).FirstOrDefault();
                }
                return View(oPosQ);
            }
            catch (Exception e)
            {
                return RedirectToAction("~/Home/Error/UnAuthorizedOperation?Error = " + e.Message);
            }
        }

        // POST: PostionQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public ActionResult Delete(PQViewModel Pq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var Db = new BD_EvaluacionEntities())
                    {
                        //Preguntas_Cargos oPQ = Db.Preguntas_Cargos.Where(i => i.Codigo_Cargo == Pq.Codigo_Cargo && i.Cod_Cargo_Evaluado == Pq.Cod_Cargo_Evaluado && i.Codigo_seccion == Pq.Codigo_seccion && i.Numero_Pregunta == Pq.Numero_Pregunta).SingleOrDefault();
                        Preguntas_Cargos oPQ = Db.Preguntas_Cargos.Find(Pq.Codigo_Cargo, Pq.Cod_Cargo_Evaluado, Pq.Codigo_seccion, Pq.Numero_Pregunta);

                        oPQ.IdState = 3;
                        Db.Entry(oPQ).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        mensaje = "Ok";
                    }
                    return RedirectToAction("Delete", "PositionQuestion", new {oPq = Pq,  mensaje });
                }
                else
                {
                    mensaje = "El modelo no es válido" + Environment.NewLine + "Contactar con el administrador";
                    return RedirectToAction("Delete", "PositionQuestion", new {oPq = Pq,  mensaje });
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error" + e.Message + "Contactar al administrador";
                return RedirectToAction("Delete", "PositionQuestion", new { oPq = Pq, mensaje });
            }
        }
    }
}
