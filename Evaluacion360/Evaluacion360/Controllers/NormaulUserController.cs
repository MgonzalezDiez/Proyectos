using Evaluacion360.Filters;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class NormaulUserController : Controller
    {
        // GET: User
        string mensaje = string.Empty;
        //string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=BD_Evaluacion;";


        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult List(int pagina = 1)
        {
            ActionResult result = null;

            int cantidad = 10;

            int CantidadRegitrosPorPagina = 0;
            cantidad = Convert.ToInt32(10);
            CantidadRegitrosPorPagina = cantidad;

            try
            {
                Usuarios tUser = (Usuarios)Session["User"];
                var oUser = new List<UserIndexViewModel>();

                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oUser = (from usr in Db.Usuarios
                         join car in Db.Cargos on usr.Codigo_Cargo equals car.Codigo_Cargo
                         join rol in Db.Rol on usr.Tipo_Usuario equals rol.Tipo_Usuario
                         join sta in Db.Estado_Componentes on usr.IdState equals sta.IdState
                         where rol.Tipo_Usuario >= tUser.Tipo_Usuario
                         orderby usr.Nombre_Usuario
                         select new UserIndexViewModel
                         {
                             Codigo_Usuario = usr.Codigo_Usuario,
                             Nombre_Usuario = usr.Nombre_Usuario,
                             Tipo_Usuario = rol.Nombre,
                             Codigo_Cargo = car.Nombre_Cargo,
                             IdState = sta.StateDescription
                         }).ToList();

                var TotalRegistros = oUser.Count();
                List<UserIndexViewModel> lista = oUser.Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina).ToList();   //Skip((pagina - 1) * CantidadRegitrosPorPagina).Take(CantidadRegitrosPorPagina);
                var Modelo = new IndexViewModel
                {
                    Secciones = lista,
                    PaginaActual = pagina,
                    TotalDeRegistros = TotalRegistros,
                    RegistrosPorPagina = CantidadRegitrosPorPagina
                };
                return View(Modelo);
            }
            catch (Exception)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                return result;
            }
        }

        [HttpGet]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Create(string mensaje)
        {
            ViewBag.Status = true;
            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Usuario Creado existosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = mensaje;
                    ViewBag.Status = false;
                }
            }
            Usuarios oUser = (Usuarios)Session["User"];
            ViewBag.TipoUsuario = new SelectList(Tools.LeerTipoUsuario(Convert.ToInt32(oUser.Tipo_Usuario)), "Tipo_Usuario", "Nombre", "");
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(Convert.ToInt32(oUser.Tipo_Usuario)), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.UserState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", 1);
            return View();
        }

        [HttpPost]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Create(UsuarioViewModel user)
        {
            Usuarios oUser = (Usuarios)Session["User"];
            ViewBag.TipoUsuario = new SelectList(Tools.LeerTipoUsuario(Convert.ToInt32(oUser.Tipo_Usuario)), "Tipo_Usuario", "Nombre", "");
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(Convert.ToInt32(oUser.Tipo_Usuario)), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.UserState = new SelectList(Utils.Tools.LeerEstados(), "IdState", "StateDescription", "");

            try
            {
                IEnumerable<ModelError> AllErrors = ModelState.Values.SelectMany(v => v.Errors);
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                //Validación del Modelo
                if (ModelState.IsValid)
                {
                    var UCodeExists = IsUserCodeExists(user.Codigo_Usuario);
                    if (!UCodeExists)
                    {
                        ViewBag.Message = "Código de Usuario ya ingresado";
                        ViewBag.Status = false;
                        return View();
                    }
                    else
                    {
                        var UserExists = IsUserExists(user.Nombre_Usuario);
                        if (!UserExists)
                        {
                            ViewBag.Message = "Nombre de Usuario ya ingresado";
                            ViewBag.Status = false;
                            return View();
                        }
                        else
                        {
                            #region encripta Password 
                            user.PASS = Crypto.Hash(user.PASS);

                            #endregion
                            #region Graba Datos
                            try
                            {
                                DateTime dt = new DateTime();
                                using var bd = new BD_EvaluacionEntities();
                                var oDUsuario = new Datos_Usuarios
                                {
                                    Codigo_Usuario = user.Codigo_Usuario.ToUpper(),
                                    Nombre_Completo = user.Nombre_Completo,
                                    Fecha_Nacimiento = user.Fecha_Nacimiento == null ? dt : user.Fecha_Nacimiento,
                                    Rut = Convert.ToInt32(user.Rut),
                                    Fondo = user.Fondo ?? "",
                                    Fecha_Ingreso = user.Fecha_Ingreso == null ? dt : user.Fecha_Ingreso,
                                    Fecha_Termino_Contrato = user.Fecha_Termino_Contrato == null ? dt : user.Fecha_Termino_Contrato,
                                    Calidad_Contrato = user.Calidad_Contrato,
                                    Tipo_Contrato = user.Tipo_Contrato,
                                    Codigo_Contrato = user.Codigo_Contrato
                                };
                                bd.Datos_Usuarios.Add(oDUsuario);
                                bd.SaveChanges();

                                var oUsuario = new Usuarios
                                {
                                    IdState = 1,
                                    Codigo_Usuario = user.Codigo_Usuario.ToUpper(),
                                    Nombre_Usuario = user.Nombre_Usuario,
                                    Tipo_Usuario = user.Tipo_Usuario,
                                    Codigo_Cargo = user.Codigo_Cargo,
                                    PASS = user.PASS
                                };
                                bd.Usuarios.Add(oUsuario);
                                bd.SaveChanges();
                                mensaje = "Ok";
                            }
                            catch (Exception e)
                            {
                                mensaje = e.Message;
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    mensaje = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    mensaje += " Los datos del usuario No fueron grabados";
                }
            }
            catch (DbEntityValidationException ex)
            {
                mensaje = ex.Message;
            }
            return RedirectToAction("Create", "User", new { mensaje });

        }

        // GET: User/Details/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Details(string Id)
        {
            try
            {
                var oUser = new UserListViewModel();
                var Du = new UserListViewModel();
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    string dv;
                    Du = (from du in Db.Datos_Usuarios
                          where du.Codigo_Usuario == Id
                          select new UserListViewModel
                          {
                              Rut = du.Rut.ToString()
                          }
                          ).FirstOrDefault();
                    if (Du != null)
                    {
                        dv = Tools.CalcularDV(Convert.ToInt32(Du.Rut));
                    }
                    else
                    {
                        dv = "";
                    }
                    DateTime dt = new DateTime();
                    oUser = (from usr in Db.Usuarios
                             join car in Db.Cargos on usr.Codigo_Cargo equals car.Codigo_Cargo
                             join rol in Db.Rol on usr.Tipo_Usuario equals rol.Tipo_Usuario
                             join sta in Db.Estado_Componentes on usr.IdState equals sta.IdState
                             join dUs in Db.Datos_Usuarios on usr.Codigo_Usuario equals dUs.Codigo_Usuario into leftJ
                             from lJo in leftJ.DefaultIfEmpty()
                             where usr.Codigo_Usuario == Id
                             select new UserListViewModel
                             {
                                 Codigo_Usuario = usr.Codigo_Usuario,
                                 Nombre_Usuario = usr.Nombre_Usuario,
                                 Tipo_Usuario = rol.Nombre,
                                 Codigo_Cargo = car.Nombre_Cargo,
                                 IdState = sta.StateDescription,
                                 Nombre_Completo = lJo.Nombre_Completo ?? string.Empty,
                                 Fecha_Nacimiento = lJo.Fecha_Nacimiento ?? dt,
                                 Rut = lJo.Rut.ToString() ?? string.Empty,
                                 Dv = dv ?? string.Empty,
                                 Fondo = lJo.Fondo ?? string.Empty,
                                 Fecha_Ingreso = lJo.Fecha_Ingreso ?? dt,
                                 Fecha_Termino_Contrato = lJo.Fecha_Termino_Contrato ?? dt,
                                 Calidad_Contrato = lJo.Calidad_Contrato ?? string.Empty,
                                 Tipo_Contrato = lJo.Tipo_Contrato ?? string.Empty,
                                 Codigo_Contrato = lJo.Codigo_Contrato ?? string.Empty
                             }).FirstOrDefault();
                }
                return View(oUser);
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                ActionResult result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                return result;
            }
        }

        // GET: User/Edit/3
        [AuthorizeUser(IdOperacion: 5)]
        [ValidateInput(false)]
        public ActionResult Edit(string Id, string mensaje)
        {
            Usuarios oUser = (Usuarios)Session["User"];
            ViewBag.TipoUsuario = new SelectList(Tools.LeerTipoUsuario(Convert.ToInt32(oUser.Tipo_Usuario)), "Tipo_Usuario", "Nombre", "");
            ViewBag.Cargos = new SelectList(Tools.LeerCargos(Convert.ToInt32(oUser.Tipo_Usuario)), "Codigo_Cargo", "Nombre_Cargo", "");
            ViewBag.UserState = new SelectList(Tools.LeerEstados(), "IdState", "StateDescription", "");

            ViewBag.Status = false;

            if (mensaje != null && mensaje != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Usuario modificado exitosamente";
                    ViewBag.Status = true;
                }
                else if (mensaje != "Ok")
                {
                    ViewBag.Message = "Usuario no pudo ser modificado " + Environment.NewLine + mensaje;
                    ViewBag.Status = false;
                }
            }
            try
            {
                var eUser = new UserEditViewModel();
                var Du = new UsuarioViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                string dv;
                Du = (from du in Db.Datos_Usuarios
                      where du.Codigo_Usuario == Id
                      select new UsuarioViewModel
                      {
                          Rut = du.Rut
                      }
                      ).FirstOrDefault(); // select du).FirstOrDefault().Rut;
                if (Du != null)
                {
                    dv = Tools.CalcularDV(Convert.ToInt32(Du.Rut));
                }
                else
                {
                    dv = "";
                }

                DateTime dt = new DateTime();
                eUser = (from usr in Db.Usuarios
                         join dUs in Db.Datos_Usuarios on usr.Codigo_Usuario equals dUs.Codigo_Usuario into leftJ
                         from lJo in leftJ.DefaultIfEmpty()
                         where usr.Codigo_Usuario == Id
                         select new UserEditViewModel
                         {
                             Codigo_Usuario = usr.Codigo_Usuario,
                             Nombre_Usuario = usr.Nombre_Usuario,
                             Tipo_Usuario = usr.Tipo_Usuario,
                             Codigo_Cargo = usr.Codigo_Cargo,
                             IdState = usr.IdState.Equals(null) ? 0 : usr.IdState,
                             Nombre_Completo = lJo.Nombre_Completo ?? string.Empty,
                             Fecha_Nacimiento = lJo.Fecha_Nacimiento ?? dt,
                             Rut = lJo.Rut.Equals(null) ? 0 : lJo.Rut,
                             Dv = dv ?? string.Empty,
                             Fondo = lJo.Fondo ?? string.Empty,
                             Fecha_Ingreso = lJo.Fecha_Ingreso ?? dt,
                             Fecha_Termino_Contrato = lJo.Fecha_Termino_Contrato ?? dt,
                             Calidad_Contrato = lJo.Calidad_Contrato ?? string.Empty,
                             Tipo_Contrato = lJo.Tipo_Contrato ?? string.Empty,
                             Codigo_Contrato = lJo.Codigo_Contrato ?? string.Empty
                         }).FirstOrDefault();
                return View(eUser);
            }
            catch (Exception e)
            {
                return View( new { e.Message });
                //return RedirectToAction("~/Error/UnAuthorizedOperation?Error = " + e.Message);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Edit(UserEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                    var oUser = Db.Usuarios.Find(model.Codigo_Usuario);
                    oUser.Codigo_Usuario = model.Codigo_Usuario.ToUpper();
                    oUser.Nombre_Usuario = model.Nombre_Usuario.ToUpper();
                    oUser.Tipo_Usuario = model.Tipo_Usuario;
                    oUser.Codigo_Cargo = model.Codigo_Cargo;
                    if (model.PASS != null && model.PASS.Trim() != "")
                    {
                        oUser.PASS = Crypto.Hash(model.PASS);

                    }
                    if (model.IdState > 0)
                    {
                        oUser.IdState = model.IdState;
                    }
                    Db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;

                    //DateTime dt = new DateTime();

                    var Existe = true;
                    var Dat = (from usr in Db.Datos_Usuarios where usr.Codigo_Usuario == model.Codigo_Usuario select usr);
                    if (Dat.Count() == 0 || Dat == null)
                    {
                        Existe = false;
                    }
                    else
                    {
                        Dat = null;
                    }

                    Datos_Usuarios dUser = new Datos_Usuarios
                    {
                        Codigo_Usuario = model.Codigo_Usuario.ToUpper(),
                        Nombre_Completo = model.Nombre_Completo.ToUpper(),
                        Fecha_Nacimiento = model.Fecha_Nacimiento,
                        Rut = model.Rut,
                        Fondo = model.Fondo,
                        Fecha_Ingreso = model.Fecha_Ingreso,
                        Fecha_Termino_Contrato = model.Fecha_Termino_Contrato,
                        Calidad_Contrato = model.Calidad_Contrato ?? string.Empty,
                        Tipo_Contrato = model.Tipo_Contrato ?? string.Empty,
                        Codigo_Contrato = model.Codigo_Contrato ?? string.Empty
                    };
                    if (Existe)
                    {
                        Db.Entry(dUser).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Db.Datos_Usuarios.Add(dUser);
                    }
                    mensaje = "Ok";
                    Db.SaveChanges();

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
                return RedirectToAction("Edit", "User", new { id = model.Codigo_Usuario, mensaje });

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return RedirectToAction("Edit", "User", new { id = model.Codigo_Usuario, mensaje });
            }
        }

        // GET: User/Delete/5
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Delete(string id, string mensaje)
        {
            if (mensaje != null && mensaje.Trim() != "")
            {
                if (mensaje == "Ok")
                {
                    ViewBag.Message = "Usuario Eliminado exitosamente";
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
                var oUser = new UserDeleteViewModel();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                oUser = (from usr in Db.Usuarios
                         join car in Db.Cargos on usr.Codigo_Cargo equals car.Codigo_Cargo
                         join rol in Db.Rol on usr.Tipo_Usuario equals rol.Tipo_Usuario
                         join sta in Db.Estado_Componentes on usr.IdState equals sta.IdState
                         where usr.Codigo_Usuario == id
                         select new UserDeleteViewModel
                         {
                             Codigo_Usuario = usr.Codigo_Usuario,
                             Nombre_Usuario = usr.Nombre_Usuario,
                             Tipo_Usuario = rol.Nombre,
                             Codigo_Cargo = car.Nombre_Cargo,
                             IdState = sta.StateDescription
                         }).FirstOrDefault();
                return View(oUser);
            }
            catch (Exception)
            {
                ActionResult result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                return result;
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [AuthorizeUser(IdOperacion: 5)]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserDeleteViewModel udvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var bd = new BD_EvaluacionEntities())
                    {

                        var oUser = bd.Usuarios.Find(udvm.Codigo_Usuario);
                        oUser.IdState = 3;

                        bd.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        bd.SaveChanges();
                    }
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
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return RedirectToAction("Delete", "User", new { id = udvm.Codigo_Usuario, mensaje });

        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return RedirectToAction("Login", "Access");
        }


        [NonAction]
        public bool IsUserExists(string UserCode)
        {
            using BD_EvaluacionEntities Bd = new BD_EvaluacionEntities();
            var existe = Bd.Usuarios.Where(a => a.Nombre_Usuario == UserCode).FirstOrDefault();
            return existe == null;
        }

        [NonAction]
        public bool IsUserCodeExists(string UserCode)
        {
            using BD_EvaluacionEntities Bd = new BD_EvaluacionEntities();
            var existe = Bd.Usuarios.Where(a => a.Codigo_Usuario == UserCode).FirstOrDefault();
            return existe == null;
        }
    }
}