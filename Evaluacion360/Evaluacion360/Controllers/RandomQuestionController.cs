using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
                oRQ = (from Rq in Db.Preguntas_Aleatorias
                       orderby Rq.Codigo_Seccion, Rq.Numero_Pregunta
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
        public ActionResult Create(string codSection, int? questionNo, string mensaje)
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

            try
            {
                var oRQ = new RQViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oRQ = (from pal in Db.Preguntas_Aleatorias
                       where pal.Codigo_Seccion == codSection && pal.Numero_Pregunta == questionNo
                       orderby pal.Codigo_Seccion, pal.Texto_Pregunta
                       select new RQViewModel
                       {
                           Codigo_Seccion = pal.Codigo_Seccion,
                           Numero_Pregunta = pal.Numero_Pregunta,
                           Texto_Pregunta = pal.Texto_Pregunta,
                           Ponderacion_P = pal.Ponderacion_P
                       }).FirstOrDefault();
                return View(oRQ);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return View(mensaje);
            }
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
                    Preguntas_Aleatorias oRQ = new Preguntas_Aleatorias
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
            return RedirectToAction("Create", "RandomQuestion", new { mensaje });
        }

        // GET: RandomQuestion/Details/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Details(string codSection, int quesNo)
        {
            var oRQ = new RQViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oRQ = (from Rq in Db.Preguntas_Aleatorias
                       where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == quesNo
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
        public ActionResult Edit(string codSection, int quesNo, string mensaje)
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
                       where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == quesNo
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
                return View(new { e.Message });
                //return RedirectToAction("~/Error/UnAuthorizedOperation?Error = " + e.Message);
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
                if (ModelState.IsValid)
                {
                    using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                    Preguntas_Aleatorias oRQ = Db.Preguntas_Aleatorias.Where(i => i.Codigo_Seccion == Rq.Codigo_Seccion && i.Numero_Pregunta == Rq.Numero_Pregunta).SingleOrDefault();

                    oRQ.Numero_Pregunta = Rq.Numero_Pregunta;
                    oRQ.Texto_Pregunta = Rq.Texto_Pregunta;
                    oRQ.Ponderacion_P = Rq.Ponderacion_P;
                    Db.Entry(oRQ).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    mensaje = "Ok";
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
                return View(new { codSection = Rq.Codigo_Seccion, askNo = Rq.Numero_Pregunta, mensaje });
            }
            catch (Exception e)
            {
                mensaje = "Ocurrió el siguiente error "
                    + e.Message
                    + " Contactar al administrador";
                return View(new { mensaje });
            }
        }

        // GET: RandomQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(string codSection, int quesNo, string mensaje)
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
                           where Rq.Codigo_Seccion == codSection && Rq.Numero_Pregunta == quesNo
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
                return View(new { e.Message });
                //return RedirectToAction("~/Home/Error/UnAuthorizedOperation?Error = " + e.Message);
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
                mensaje = "Ocurrió el siguiente error " + e.Message + " Contactar al administrador";
            }
            return View(new { codSection = Rq.Codigo_Seccion, quesNo = Rq.Numero_Pregunta, mensaje });
        }

        #region Obtiene Número de pregunto
        public JsonResult GetQuestionNo(string codSec)
        {
            BD_EvaluacionEntities db = new BD_EvaluacionEntities();
            //db.Configuration.ProxyCreationEnabled = false;

            var valorMaximo = db.Preguntas_Aleatorias.Where(x => x.Codigo_Seccion == codSec).Select(p => p.Numero_Pregunta).DefaultIfEmpty(0).Max();
            valorMaximo += 1;
            return Json(valorMaximo, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
