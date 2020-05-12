using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Evaluacion360.Controllers
{
    public class EvaluationPositionsController : Controller
    {
        private string Mensaje = string.Empty;
        int userType;


        // GET: EvaluationPosition
        public ActionResult List(int pagina = 1)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Status = true;

            int CantidadRegitrosPorPagina = 10;

            var oEP = new List<EvaluationPositionsListViewModel>();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oEP = (from cev in Db.Evaluacion_Preguntas_Cargos
                   join pro in Db.Procesos_Evaluacion on cev.Codigo_Proceso equals pro.Codigo_Proceso
                   join sec in Db.Secciones on cev.Codigo_seccion equals sec.Codigo_Seccion
                   join usr in Db.Usuarios on cev.Codigo_Usuario equals usr.Codigo_Usuario
                   join pre in Db.Preguntas_Aleatorias on new { a = cev.Codigo_seccion, b = cev.Numero_Pregunta } equals new { a = pre.Codigo_Seccion, b = pre.Numero_Pregunta }
                   orderby cev.Numero_Evaluacion, cev.Codigo_Proceso
                   select new EvaluationPositionsListViewModel
                   {
                       Numero_Evaluacion = cev.Numero_Evaluacion,
                       Codigo_Proceso = pro.Nombre_Proceso,
                       Codigo_Usuario = usr.Nombre_Usuario,
                       Codigo_seccion = sec.Nombre_Seccion,
                       Numero_Pregunta = pre.Texto_Pregunta,
                       Nota = cev.Nota

                   }).ToList();
            var TotalRegistros = oEP.Count();
            List<EvaluationPositionsListViewModel> lista = oEP.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();   //Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina);
            var Modelo = new ListEvaluationPositionsViewModel
            {
                Secciones = lista,
                PaginaActual = pagina,
                TotalDeRegistros = TotalRegistros,
                RegistrosPorPagina = CantidadRegitrosPorPagina
            };
            return View();
        }

        // GET: EvaluationPosition/Details/5
        public ActionResult Details(Evaluacion_Preguntas_Cargos evq)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);

            var oEP = new List<EvaluationPositionsListViewModel>();
            using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            oEP = (from epc in Db.Evaluacion_Preguntas_Cargos
                   join pro in Db.Procesos_Evaluacion on epc.Codigo_Proceso equals pro.Codigo_Proceso
                   join sec in Db.Secciones on epc.Codigo_seccion equals sec.Codigo_Seccion
                   join usr in Db.Usuarios on epc.Codigo_Usuario equals usr.Codigo_Usuario
                   join pre in Db.Preguntas_Aleatorias on new { a = epc.Codigo_seccion, b = epc.Numero_Pregunta } equals new { a = pre.Codigo_Seccion, b = pre.Numero_Pregunta }
                   where epc.Numero_Evaluacion == evq.Numero_Evaluacion && epc.Codigo_Proceso == evq.Codigo_Proceso && epc.Codigo_Usuario == evq.Codigo_Usuario
                   orderby epc.Numero_Evaluacion, epc.Codigo_Proceso
                   select new EvaluationPositionsListViewModel
                   {
                       Numero_Evaluacion = epc.Numero_Evaluacion,
                       Codigo_Proceso = pro.Nombre_Proceso,
                       Codigo_Usuario = usr.Nombre_Usuario,
                       Codigo_seccion = sec.Nombre_Seccion,
                       Numero_Pregunta = pre.Texto_Pregunta,
                       Nota = epc.Nota

                   }).ToList();
            return View(oEP);
        }

        // GET: EvaluationPosition/Create
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Create(string mensaje)
        {
            ViewBag.Status = true;
            Usuarios oUser = (Usuarios)Session["User"];

            ViewBag.Users = new SelectList(Tools.LeerUsuariosPoEvaluador(oUser.Codigo_Usuario), "Codigo_Usuario", "Nombre_Usuario", oUser.Codigo_Usuario);
            ViewBag.Process = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "A");
            ViewBag.Sections = new SelectList(Tools.DominiosPorUsuarioCargo(oUser.Codigo_Usuario), "Codigo_Seccion", "Nombre_Seccion");
            ViewBag.Positions = new SelectList(Tools.LeerCargos(oUser.Tipo_Usuario), "Codigo_Cargo", "Nombre_Cargo");

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Auto Evaluación Actualizada exitosamente";
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
                var oEC = new EvaluationPositionsViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEC = (from pev in Db.Procesos_Evaluacion
                       join evc in Db.Evaluaciones_Cargos on pev.Codigo_Proceso equals evc.Codigo_Proceso
                       join usu in Db.Usuarios on evc.Cod_Usuario_Evaluado equals usu.Codigo_Usuario
                       join dus in Db.Datos_Usuarios on usu.Codigo_Usuario equals dus.Codigo_Usuario
                       join eev in Db.Estado_Evaluaciones on evc.Estado_EC equals eev.IdState
                       where usu.Codigo_Usuario == oUser.Codigo_Usuario && evc.Estado_EC == "A"
                       orderby pev.Nombre_Proceso

                       select new EvaluationPositionsViewModel
                       {
                           Numero_Evaluacion = evc.Numero_Evaluacion,
                           Codigo_Proceso = evc.Codigo_Proceso,
                           NombreProceso = pev.Nombre_Proceso,
                           Codigo_Usuario_Evaluado = evc.Cod_Usuario_Evaluado,
                           Nombre_Usuario_Evaluado = usu.Nombre_Usuario,
                           Cod_Cargo_Evaluado = evc.Cod_Cargo_Evaluado,
                           Estado_EC = eev.IdState ?? "",
                           Nota_Final_EC = evc.Nota_Final_EC ?? 0,
                           Numero_Pregunta = 0,
                           TextoPregunta = "",
                           Cod_Cargo_Evaluador =  oUser.Codigo_Cargo,
                           Usuario_Evaluador = dus.Nombre_Completo,

                       }).FirstOrDefault();
                if (oEC == null)
                {
                    ViewBag.Message = "Todas las preguntas por Cargo ya fueron respondidas";
                    ViewBag.Status = false;
                }
                return View(oEC);
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return View(Mensaje);
            }

        }

        // POST: EvaluacionPreguntas/Create
        [AuthorizeUser(IdOperacion: 5)]
        [HttpPost]
        public ActionResult Create(EvaluationPositionsViewModel evp)
        {
            Usuarios oUser = (Usuarios)Session["User"];
            ViewBag.Status = true;
            ViewBag.State = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");
            ViewBag.Sections = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", "");
            ViewBag.Procesos = new SelectList(Tools.LeerProcesos(), "Codigo_Proceso", "Nombre_Proceso", "");
            ViewBag.EvState = new SelectList(Tools.EstadosEvaluaciones(), "IdState", "StateDescription", "");
            ViewBag.Positions = new SelectList(Tools.LeerCargos(oUser.Tipo_Usuario), "Codigo_Cargo", "Nombre_Cargo");

            try
            {
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    #region Graba Datos
                    using var bd = new BD_EvaluacionEntities();
                    var oAe = new Evaluacion_Preguntas_Cargos
                    {
                        Numero_Evaluacion = evp.Numero_Evaluacion,
                        Codigo_Proceso = evp.Codigo_Proceso,
                        Codigo_seccion = evp.Codigo_seccion,
                        Numero_Pregunta = evp.Numero_Pregunta,
                        Nota = evp.Nota,
                    };
                    bd.Evaluacion_Preguntas_Cargos.Add(oAe);
                    bd.SaveChanges();

                    Mensaje = "Ok";
                    return Json(data: new { success = true, data = Mensaje }, JsonRequestBehavior.AllowGet);
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
                    return Json(data: new { success = false, data = Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {

                Mensaje = "Ocurrió el siguiente error " +
                          e.InnerException.InnerException.Message +
                          " Contacte al Administrador";
                return Json(data: new { success = false, data = Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: EvaluationPosition/Edit/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(int numEval, int codProceso, string codUsuario, string codSeccion, int numQuestion, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Status = false;
            ViewBag.Users = new SelectList(Tools.LeerUsuarios(""), "Codigo_Usuario", "Nombre_Usuario", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", 1);
            ViewBag.Process = new SelectList(Tools.LeerProcesos(), "Codio_Proceso", "Nombre_Proceso");
            ViewBag.Questions = new SelectList(Tools.LeerPreguntasAleatorias(codSeccion, numQuestion), "Codigo_Seccion", "Nombre_Seccion");

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
                var oEP = new EvaluationPositionsViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEP = (from evp in Db.Evaluacion_Preguntas_Cargos
                       where evp.Numero_Evaluacion == numEval && evp.Codigo_Proceso == codProceso && evp.Codigo_Usuario == codUsuario
                          && evp.Codigo_seccion == codSeccion && evp.Numero_Pregunta == numQuestion
                       select new EvaluationPositionsViewModel
                       {
                           Numero_Evaluacion = evp.Numero_Evaluacion,
                           Codigo_Proceso = evp.Codigo_Proceso,
                           Codigo_Usuario_Evaluado = evp.Codigo_Usuario,
                           Codigo_seccion = evp.Codigo_seccion,
                           Numero_Pregunta = evp.Numero_Pregunta,
                           Nota = evp.Nota
                       }).FirstOrDefault();
                return View(oEP);
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.InnerException.Message
                          + " Contacte al Administrador";
                return RedirectToAction("Edit", "EvaluationPositions", new { numEval, codProceso, codUsuario, codSeccion, numQuestion, Mensaje });
            }
        }

        // POST: EvaluationPosition/Edit/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(EvaluationPositionsEditViewModel EvPos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var Db = new BD_EvaluacionEntities();
                    var oEP = Db.Evaluacion_Preguntas_Cargos.Where(x => x.Numero_Evaluacion == EvPos.Numero_Evaluacion && x.Codigo_Proceso == EvPos.Codigo_Proceso && x.Codigo_Usuario == EvPos.Codigo_Usuario_Evaluado && x.Codigo_seccion == EvPos.Codigo_seccion && x.Numero_Pregunta == EvPos.Numero_Pregunta).FirstOrDefault();
                    oEP.Nota = EvPos.Nota;
                    Db.Entry(oEP).State = System.Data.Entity.EntityState.Modified;
                    Mensaje = "Ok";
                    Db.SaveChanges();

                    var oEvP = Db.Evaluaciones_Cargos.Find(EvPos.Numero_Evaluacion, EvPos.Codigo_Proceso, EvPos.Codigo_Usuario_Evaluado, EvPos.Cod_Cargo_Evaluado);
                    oEvP.Estado_EC = "C";
                    Db.Entry(oEvP).State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    return Json(Mensaje);
                    //return Json(data: new { success = true, data = Mensaje }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Mensaje = "Se ha producido el siguiente error "
                            + ex.Message
                            + " Contacte al Administrador";
                    return Json(Mensaje);
                    //return Json(data: new { success = false, data = Mensaje }, JsonRequestBehavior.AllowGet);
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
                    Mensaje += " Contacte al Administrador";
                }
                return Json(Mensaje);
                //return Json(data: new { success = false, data = Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: EvaluationPosition/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(EvaluationPositionsViewModel evPos, string mensaje)
        {
            userType = Convert.ToInt32(Request.RequestContext.HttpContext.Session["TipoUsuario"]);
            ViewBag.Status = false;
            ViewBag.Users = new SelectList(Tools.LeerUsuarios(""), "Codigo_Usuario", "Nombre_Usuario", "");
            ViewBag.Section = new SelectList(Tools.LeerSecciones(), "Codigo_Seccion", "Nombre_Seccion", 1);
            ViewBag.Process = new SelectList(Tools.LeerProcesos(), "Codio_Proceso", "Nombre_Proceso");
            ViewBag.Questions = new SelectList(Tools.LeerPreguntasAleatorias(evPos.Codigo_seccion, evPos.Numero_Pregunta), "Codigo_Seccion", "Nombre_Seccion");

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Evaluación Preguntas por Cargo Eliminada exitosamente";
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
                var oEP = new EvaluationPositionsViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oEP = (from evc in Db.Evaluacion_Preguntas_Cargos
                       where evc.Numero_Evaluacion == evPos.Numero_Evaluacion && evc.Codigo_Proceso == evPos.Codigo_Proceso && evc.Codigo_Usuario == evPos.Codigo_Usuario_Evaluado && evc.Codigo_seccion == evPos.Codigo_seccion && evc.Numero_Pregunta == evPos.Numero_Pregunta
                       select new EvaluationPositionsViewModel
                       {
                           Numero_Evaluacion = evc.Numero_Evaluacion,
                           Codigo_Proceso = evc.Codigo_Proceso,
                           Codigo_Usuario_Evaluado = evc.Codigo_Usuario,
                           Codigo_seccion = evc.Codigo_seccion,
                           Numero_Pregunta = evc.Numero_Pregunta,
                           Nota = oEP.Nota
                       }).FirstOrDefault();
                return View(oEP);
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.Message
                          + " Contacte al Administrador";
            }
            return View(new { evPos, Mensaje });
        }

        // POST: EvaluationPosition/Delete/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(EvaluationPositionsViewModel evPos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var Db = new BD_EvaluacionEntities();
                    var oUser = Db.Evaluacion_Preguntas_Cargos.Find(evPos.Numero_Evaluacion, evPos.Codigo_Proceso, evPos.Codigo_Usuario_Evaluado, evPos.Codigo_seccion, evPos.Numero_Pregunta);

                    Db.Entry(oUser).State = System.Data.Entity.EntityState.Deleted;
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
                    }
                    Mensaje += " Contacte al Administrador";
                }
            }
            catch (Exception e)
            {
                Mensaje = "Ocurrió el siguiente error"
                          + e.InnerException.Message
                          + " Contacte al Administrador";
            }
            return RedirectToAction("Delete", "EvaluationPositions", new { evPos, Mensaje });
        }

        [HttpPost]
        public JsonResult GetQuestion(int codCargoEvaluador, int codCargoEvaluado, int codProc, int numEval, string codSecc)
        {
            string ModelJson;
            List<AutoEvQuestionScoreViewModel> QuestByUser; /*= new List<AutoEvQuestionScoreViewModel>();*/

            QuestByUser = GetQuestions(codCargoEvaluador, codCargoEvaluado, numEval, codProc, codSecc).ToList();

            if (QuestByUser != null)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ModelJson = ser.Serialize(QuestByUser);
            }
            else
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ModelJson = ser.Serialize(QuestByUser);
                return Json(ModelJson, JsonRequestBehavior.AllowGet);
            }

            return Json(ModelJson, JsonRequestBehavior.AllowGet);
            ;
        }

        public static IEnumerable<AutoEvQuestionScoreViewModel> GetQuestions(int codCargoEvaluador, int codCargoEvaluado, int numEval, int codProc, string sectionCode)
        {
            List<AutoEvQuestionScoreViewModel> RandomQ = new List<AutoEvQuestionScoreViewModel>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "select epc.Numero_Pregunta, pal.Texto_Pregunta, epc.Numero_Evaluacion " +
                         "from Evaluacion_Preguntas_Cargos epc " +
                         "Join Evaluaciones_Cargos evc on epc.Numero_Evaluacion = evc.Numero_Evaluacion " +
                         " and epc.Codigo_Proceso = evc.Codigo_Proceso and epc.Codigo_Usuario = evc.Cod_Usuario_Evaluado " +
                         "join Preguntas_Cargos pca on evc.Cod_Cargo_Evaluado = pca.Cod_Cargo_Evaluado and epc.Codigo_seccion = pca.Codigo_seccion  " +
                         "and epc.Numero_Pregunta = pca.Numero_Pregunta " +
                         "join Preguntas_Aleatorias pal on epc.Codigo_seccion = pal.Codigo_Seccion and epc.Numero_Pregunta = pal.Numero_Pregunta " +
                         "Where pca.Codigo_Cargo = " + codCargoEvaluador + 
                         " And pca.Cod_Cargo_Evaluado = " + codCargoEvaluado + 
                         " And evc.Codigo_Proceso = '" + codProc + "' " +
                         " And epc.Numero_Evaluacion = " + numEval +
                         " And epc.Codigo_seccion = '" + sectionCode + "' " +
                         " And pca.Cod_Cargo_Evaluado <> pca.Codigo_Cargo ";

            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
                Connection = Cnn
            };
            Cnn.Open();
            using SqlDataReader sec = cmd.ExecuteReader();
            if (sec.HasRows)
            {
                try
                {
                    while (sec.Read())
                    {
                        AutoEvQuestionScoreViewModel scc = new AutoEvQuestionScoreViewModel()
                        {
                            Codigo_Seccion = sectionCode,
                            Numero_Pregunta = sec.GetInt32(0),
                            Texto_Pregunta = sec.GetString(1),

                        };

                        RandomQ.Add(scc);
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = ex.InnerException.InnerException.Message;
                    RandomQ.Add(new AutoEvQuestionScoreViewModel()
                    {
                        Codigo_Seccion = "Error",
                        Texto_Pregunta = mensaje + " Valide la información"

                    });
                }
            }
            return RandomQ;
        }

        public JsonResult GetPositionByUser(int codUsuario)
        {
            List<Cargos> cargos = new List<Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select car.Codigo_Cargo, car.Nombre_Cargo from Cargos car " +
                                  "Join Usuarios usu on car.Codigo_Cargo = usu.Codigo_Cargo " +
                                  "join Datos_Usuarios dus on usu.Codigo_Usuario = dus.Codigo_Usuario " +
                                  "where usu.Codigo_Cargo = '" + codUsuario + "'";
                cmd.Connection = Cnn;
                Cnn.Open();

                using SqlDataReader rea = cmd.ExecuteReader();
                while (rea.Read())
                {
                    Cargos c = new Cargos
                    {
                        Codigo_Cargo = rea.GetString(0),
                        Nombre_Cargo = rea.GetString(1)
                    };
                    cargos.Add(c);
                }
            }
            return Json(cargos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDomainByUser(string CodCargoEvaluador, string CodCargoEvaluado)
        {
            List<Secciones> sec = new List<Secciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Select Distinct Sec.Codigo_Seccion, Sec.Nombre_Seccion from Secciones Sec " +
                         "Join Preguntas_Cargos EPC on Sec.Codigo_Seccion = EPC.Codigo_Seccion " +
                         "Where EPC.Cod_Cargo_Evaluado = '" + CodCargoEvaluado + "' And EPC.Codigo_Cargo = '" + CodCargoEvaluador + "'";

                cmd.Connection = Cnn;
                Cnn.Open();

                using SqlDataReader rea = cmd.ExecuteReader();
                while (rea.Read())
                {
                    Secciones c = new Secciones
                    {
                        Codigo_Seccion = rea.GetString(0),
                        Nombre_Seccion = rea.GetString(1)
                    };
                    sec.Add(c);
                }
            }
            return Json(sec, JsonRequestBehavior.AllowGet);
        }
    }
}
