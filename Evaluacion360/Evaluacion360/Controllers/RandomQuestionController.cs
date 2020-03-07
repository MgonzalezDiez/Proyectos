using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Evaluacion360.Controllers
{
    public class RandomQuestionController : Controller
    {
        string mensaje = string.Empty;

        // GET: RandomQuestion
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult List(int pagina = 1)
        {
            Usuarios tUser = (Usuarios)Session["User"];
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;

            var oRQ = new List<RQViewModel>();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oRQ = (from Rq in Db.Preguntas_Aleatorias orderby Rq.Codigo_Seccion, Rq.Numero_Pregunta
                            select new RQViewModel
                            {
                                Codigo_Seccion = Rq.Codigo_Seccion,
                                Numero_Pregunta = Rq.Numero_Pregunta,
                                Texto_Pregunta = Rq.Texto_Pregunta,
                                Ponderacion_P = Rq.Ponderacion_P
                            }).ToList();
            }
            var TotalRegistros = oRQ.Count();
            List<RQViewModel> lista = oRQ.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
            var Modelo = new ListRQViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View(Modelo);
        }

        // GET: RandomQuestion/Create
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Create(string mensaje)
        {
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.Status = true;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta Aleatoria Creada exitosamente";
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

        // POST: RandomQuestion/Create
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Create(RQViewModel Rq)
        {
            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oRQ = new Preguntas_Aleatorias
                    {
                        Codigo_Seccion = Rq.Codigo_Seccion.ToUpper(),
                        Numero_Pregunta = Rq.Numero_Pregunta,
                        Texto_Pregunta = Rq.Texto_Pregunta,
                        Ponderacion_P = Rq.Ponderacion_P
                    };
                    bd.Preguntas_Aleatorias.Add(oRQ);
                    bd.SaveChanges();

                    mensaje = "Ok";
                    #endregion
                }
                else
                {
                    mensaje = "El modelo no es válido o Falta ingresar Información Contactar al administrador";
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
            }
            return RedirectToAction("Create", "RandomQuestion", new { mensaje });
        }

        // GET: RandomQuestion/Details/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Details(string codSection, int askNo)
        {
            var oRQ = new RQViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oRQ = (from Rq in Db.Preguntas_Aleatorias
                       where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == askNo
                       select new RQViewModel
                       {
                           Codigo_Seccion = Rq.Codigo_Seccion,
                           Numero_Pregunta = Rq.Numero_Pregunta,
                           Texto_Pregunta = Rq.Texto_Pregunta,
                           Ponderacion_P = Rq.Ponderacion_P
                       }).FirstOrDefault();
            }
            return View(oRQ);
        }

        // GET: RandomQuestion/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(string codSection, int askNo, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta modificada exitosamente";
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
                ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion");
                var oRQ = new RQViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oRQ = (from Rq in Db.Preguntas_Aleatorias
                       where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == askNo
                       select new RQViewModel
                       {
                           Codigo_Seccion = Rq.Codigo_Seccion,
                           Numero_Pregunta = Rq.Numero_Pregunta,
                           Texto_Pregunta = Rq.Texto_Pregunta,
                           Ponderacion_P = Rq.Ponderacion_P
                       }).FirstOrDefault();
                return View(oRQ);
            }
            catch (Exception e)
            {
                return RedirectToAction("~/Error/UnAuthorizedOperation?Error = " + e.Message);
            }
        }

        // POST: RandomQuestion/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Edit(RQViewModel Rq)
        {
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            try
            {
                using (var Db = new BD_EvaluacionEntities())
                {
                    Preguntas_Aleatorias oRQ = Db.Preguntas_Aleatorias.Where(i => i.Codigo_Seccion == Rq.Codigo_Seccion && i.Numero_Pregunta == Rq.Numero_Pregunta).SingleOrDefault();

                    oRQ.Numero_Pregunta = Rq.Numero_Pregunta;
                    oRQ.Texto_Pregunta = Rq.Texto_Pregunta;
                    oRQ.Ponderacion_P = Rq.Ponderacion_P;
                    Db.Entry(oRQ).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    mensaje = "Ok";
                }
                return RedirectToAction("Edit", "RandomQuestion", new { codSection = Rq.Codigo_Seccion, askNo = Rq.Numero_Pregunta, mensaje });
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return RedirectToAction("Edit", "RandomQuestion", new { mensaje });
            }
        }

        // GET: RandomQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(string codSection, int askNo, string mensaje)
        {
            ViewBag.Status = false;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Pregunta Eliminada exitosamente";
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
                var oRQ = new RQViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    oRQ = (from Rq in Db.Preguntas_Aleatorias
                           where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == askNo
                           select new RQViewModel
                           {
                               Codigo_Seccion = Rq.Codigo_Seccion,
                               Numero_Pregunta = Rq.Numero_Pregunta,
                               Texto_Pregunta = Rq.Texto_Pregunta,
                               Ponderacion_P = Rq.Ponderacion_P
                           }).FirstOrDefault();
                }
                return View(oRQ);
            }
            catch (Exception e)
            {
                return RedirectToAction("~/Home/Error/UnAuthorizedOperation?Error = " + e.Message);
            }
        }

        // POST: RandomQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Delete(RQViewModel Rq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var Db = new BD_EvaluacionEntities();
                    Preguntas_Aleatorias oRQ = Db.Preguntas_Aleatorias.Where(i => i.Codigo_Seccion == Rq.Codigo_Seccion && i.Numero_Pregunta == Rq.Numero_Pregunta).SingleOrDefault();

                    Db.Entry(oRQ).State = System.Data.Entity.EntityState.Deleted;
                    Db.SaveChanges();
                    mensaje = "Ok";
                }
                else
                {
                    mensaje = "El modelo no es válido, Contactar con el administrador";
                    //return RedirectToAction("Delete", "RandomQuestion", new { sectionId = Rq.Codigo_Seccion, askNo = Rq.Numero_Pregunta, mensaje });
                }
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error " + e.Message +  " Contactar al administrador";
                //return RedirectToAction("Delete", "RandomQuestion", new { sectionId = Rq.Codigo_Seccion, askNo = Rq.Numero_Pregunta, mensaje });
            }
            return RedirectToAction("Delete", "RandomQuestion", new { codSection = Rq.Codigo_Seccion, askNo = Rq.Numero_Pregunta, mensaje });
        }
    }
}
