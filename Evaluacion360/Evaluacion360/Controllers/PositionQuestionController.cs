using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Evaluacion360.Controllers
{
    public class PositionQuestionController : Controller
    {
        string mensaje = string.Empty;
        int userType;

        // GET: PostionQuestion
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult List(int pagina = 1)
        {
            int CantidadRegitrosPorPagina = 0;
            CantidadRegitrosPorPagina = 10;
            try
            {
                #region Muestra Datos
                var oPQ = new List<PositionQuestionListViewModel>();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {

                    oPQ = (from pos in Db.Preguntas_Cargos
                           join car in Db.Cargos on pos.Codigo_Cargo equals car.Codigo_Cargo
                           join crg in Db.Cargos on pos.Cod_Cargo_Evaluado equals crg.Codigo_Cargo
                           join sec in Db.Secciones on pos.Codigo_seccion equals sec.Codigo_Seccion
                           join sta in Db.Estado_Componentes on pos.IdState equals sta.IdState
                           join rq in Db.Preguntas_Aleatorias on new { a = pos.Codigo_seccion, b = pos.Numero_Pregunta } equals new { a = rq.Codigo_Seccion, b = rq.Numero_Pregunta } 
                           orderby car.Nombre_Cargo, crg.Nombre_Cargo, sec.Nombre_Seccion, pos.Numero_Pregunta
                           select new PositionQuestionListViewModel
                           {
                               Codigo_Cargo = pos.Codigo_Cargo,
                               Nombre_Cargo = car.Nombre_Cargo,
                               Cod_Cargo_Evaluado = pos.Cod_Cargo_Evaluado,
                               Nombre_Cargo_Evaluado = crg.Nombre_Cargo,
                               Codigo_seccion = pos.Codigo_seccion,
                               Nombre_Seccion = sec.Nombre_Seccion,
                               Numero_Pregunta = pos.Numero_Pregunta,
                               Texto_Pregunta = rq.Texto_Pregunta,
                               IdState = sta.StateDescription
                           }).ToList();
                }
                var TotalRegistros = oPQ.Count();
                List<PositionQuestionListViewModel> lista = oPQ.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();
                var Modelo = new ListPQViewModel
                {
                    Secciones = lista,
                    PaginaActual = pagina,
                    TotalDeRegistros = TotalRegistros,
                    RegistrosPorPagina = CantidadRegitrosPorPagina
                };
                return View(Modelo);
                #endregion
            }
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return View(new { mensaje });
            }
        }

        // GET: PostionQuestion/Create
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Create(string codCargo, string codCargoEval, string codSection, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias("", 0), "Numero_Pregunta", "Texto_Pregunta", "");

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
            #region Muestra Datos
            var oPQ = new PositionQuestionViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                oPQ = (from pcar in Db.Preguntas_Cargos
                       
                       where pcar.Codigo_Cargo == codCargo && pcar.Cod_Cargo_Evaluado == codCargoEval && pcar.Codigo_seccion == codSection
                       select new PositionQuestionViewModel
                       {
                           Codigo_Cargo = pcar.Codigo_Cargo,
                           Cod_Cargo_Evaluado = pcar.Cod_Cargo_Evaluado,
                           Codigo_seccion = pcar.Codigo_seccion,
                           Numero_Pregunta = pcar.Numero_Pregunta,
                           IdState = pcar.IdState
                       }).SingleOrDefault();
            }
            return View(oPQ);
            #endregion
        }

        // POST: PostionQuestion/Create
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public JsonResult Create(List<PositionQuestionViewModel> ListJson)
        {
            try
            {
                #region Graba Datos
                using var bd = new BD_EvaluacionEntities();

                foreach (var item in ListJson)
                {

                    var existe = bd.Cargos_Evaluadores.Where(x => x.Codigo_Cargo == item.Codigo_Cargo && x.Cod_Cargo_Evaluado == item.Cod_Cargo_Evaluado);
                    if (existe.Count() != 0)
                    {
                        var oPQ = new Preguntas_Cargos
                        {
                            Codigo_Cargo = item.Codigo_Cargo,
                            Cod_Cargo_Evaluado = item.Cod_Cargo_Evaluado,
                            Codigo_seccion = item.Codigo_seccion,
                            Numero_Pregunta = item.Numero_Pregunta,
                            IdState = item.IdState
                        };
                        bd.Preguntas_Cargos.Add(oPQ);
                    }
                    else
                    {
                        mensaje = "Usuarios Evaluador y Evaluado no existen en Cargos Evaluadores Valide la información e intente nuevamente";
                        return Json(mensaje);
                    }
                }
                bd.SaveChanges();
                mensaje = "Ok";
                #endregion
            }
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
            }
            return Json(mensaje);

        }

        // GET: PostionQuestion/Details/5
        [HttpGet]
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Details(string codCargo, string codCargoEval, string codSection, int questionNo)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias("", 0), "Numero_Pregunta", "Texto_Pregunta", "");

            var oPQ = new PositionQuestionViewModel();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {

                oPQ = (from Pq in Db.Preguntas_Cargos
                       where Pq.Codigo_Cargo == codCargo && Pq.Cod_Cargo_Evaluado == codCargoEval && Pq.Codigo_seccion == codSection.Trim() && Pq.Numero_Pregunta == questionNo
                       select new PositionQuestionViewModel
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
        public ActionResult Edit(string codCargo, string codCargoEval, string codSection, int questionNo, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias(codSection, questionNo), "Numero_Pregunta", "Texto_Pregunta", "");

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
                #region Muestra Datos
                var oPQ = new PositionQuestionViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oPQ = (from Pq in Db.Preguntas_Cargos
                       where Pq.Codigo_Cargo == codCargo && Pq.Cod_Cargo_Evaluado == codCargoEval && Pq.Codigo_seccion == codSection
                       select new PositionQuestionViewModel
                       {
                           Codigo_Cargo = Pq.Codigo_Cargo,
                           Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                           Codigo_seccion = Pq.Codigo_seccion,
                           Numero_Pregunta = Pq.Numero_Pregunta,
                           IdState = 1
                       }).FirstOrDefault();
                return View(oPQ);
                #endregion
            }
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return View( new { mensaje});
                //return RedirectToAction("~/Error/UnAuthorizedOperation?Error = " + mensaje);
            }
        }

        // POST: PostionQuestion/Edit/5
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public ActionResult Edit(PositionQuestionViewModel Pq)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias(Pq.Codigo_seccion, Pq.Numero_Pregunta), "Numero_Pregunta", "Texto_Pregunta", "");

            try
            {
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                    Preguntas_Cargos oPq = Db.Preguntas_Cargos.Where(i => i.Codigo_Cargo == Pq.Codigo_Cargo && i.Cod_Cargo_Evaluado == Pq.Cod_Cargo_Evaluado && i.Codigo_seccion == Pq.Codigo_seccion && i.Numero_Pregunta == Pq.Numero_Pregunta).SingleOrDefault();
                    if (oPq != null)
                    {
                        oPq.Numero_Pregunta = Pq.Numero_Pregunta;
                        oPq.IdState = Pq.IdState;
                        Db.Entry(oPq).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        mensaje = "Ok";
                    }
                    else
                    {
                        mensaje = "El registro no se modificó";
                    }
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
                return View(new { mensaje });
            }
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return RedirectToAction("Edit", "PositionQuestion", new {codCargo = Pq.Codigo_Cargo, codCargoEval = Pq.Cod_Cargo_Evaluado, codSection = Pq.Codigo_seccion, questionNo = Pq.Numero_Pregunta, mensaje });
            }
        }

        // GET: PostionQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Delete(string codCargo, string codCargoEval, string codSection, int questionNo, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias(codSection, questionNo), "Numero_Pregunta", "Texto_Pregunta", "");

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
                #region Muestra Datos
                var oPosQ = new PositionQuestionViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    oPosQ = (from Pq in Db.Preguntas_Cargos
                             where Pq.Codigo_Cargo == codCargo && Pq.Cod_Cargo_Evaluado == codCargoEval && Pq.Codigo_seccion == codSection && Pq.Numero_Pregunta == questionNo
                             select new PositionQuestionViewModel
                             {
                                 Codigo_Cargo = Pq.Codigo_Cargo,
                                 Cod_Cargo_Evaluado = Pq.Cod_Cargo_Evaluado,
                                 Codigo_seccion = Pq.Codigo_seccion,
                                 Numero_Pregunta = Pq.Numero_Pregunta,
                                 IdState = Pq.IdState
                             }).FirstOrDefault();
                }
                return View(oPosQ);
                #endregion
            }
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
                return View(new { mensaje });
                //return RedirectToAction("~/Home/Error/UnAuthorizedOperation?Error = " + mensaje);
            }
        }

        // POST: PostionQuestion/Delete/5
        [AuthorizeUser(IdOperacion: 6)]
        [HttpPost]
        public ActionResult Delete(PositionQuestionViewModel Pq)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(userType), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_seccion", "Nombre_Seccion", "");
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.RQuestion = new SelectList(Tools.LeerPreguntasAleatorias(Pq.Codigo_seccion, Pq.Numero_Pregunta), "Numero_Pregunta", "Texto_Pregunta", "");
            try
            {
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using (var Db = new BD_EvaluacionEntities())
                    {
                        //Preguntas_Cargos oPQ = Db.Preguntas_Cargos.Where(i => i.Codigo_Cargo == Pq.Codigo_Cargo && i.Cod_Cargo_Evaluado == Pq.Cod_Cargo_Evaluado && i.Codigo_seccion == Pq.Codigo_seccion && i.Numero_Pregunta == Pq.Numero_Pregunta).SingleOrDefault();
                        Preguntas_Cargos oPQ = Db.Preguntas_Cargos.Find(Pq.Codigo_Cargo, Pq.Cod_Cargo_Evaluado, Pq.Codigo_seccion, Pq.Numero_Pregunta);

                        oPQ.IdState = 3;
                        Db.Entry(oPQ).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        mensaje = "Ok";
                    }
                    return RedirectToAction("Delete", "PositionQuestion", new { codCargo = Pq.Codigo_Cargo, codCargoEval = Pq.Cod_Cargo_Evaluado, codSection = Pq.Codigo_seccion, questionNo = Pq.Numero_Pregunta, mensaje });
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
            catch (Exception ex)
            {
                mensaje = ex.InnerException.InnerException.Message + "Contactar al administrador";
            }
            return View(new { codCargo = Pq.Codigo_Cargo, codCargoEval = Pq.Cod_Cargo_Evaluado, codSection = Pq.Codigo_seccion, questionNo = Pq.Numero_Pregunta, mensaje });
        }

        #region Obtiene preguntas por sección
        public JsonResult GetPreguntas(string codSec, bool ponderacion)
        {
            List<Preguntas_Aleatorias> RQuestion = new List<Preguntas_Aleatorias>();
            BD_EvaluacionEntities db = new BD_EvaluacionEntities();
            db.Configuration.ProxyCreationEnabled = false;


            if (ponderacion)
            {
                RQuestion = db.Preguntas_Aleatorias.Where(x => x.Codigo_Seccion == codSec && x.Ponderacion_P > 0).ToList();
            }
            else
            {
                RQuestion = db.Preguntas_Aleatorias.Where(x => x.Codigo_Seccion == codSec && x.Ponderacion_P == 0).ToList();
            }

            return Json(RQuestion, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
