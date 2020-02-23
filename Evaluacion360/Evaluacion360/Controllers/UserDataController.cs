using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evaluacion360.Models;
using Evaluacion360.Models.ViewModels;

namespace Evaluacion360.Controllers
{
    public class UserDataController : Controller
    {
        private static readonly BD_EvaluacionEntities db = new BD_EvaluacionEntities();

        // GET: Datos_Usuarios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var oUser = new Datos_Usuarios();
            using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
            {
                var dUser = Db.Datos_Usuarios.Find(id);
                oUser = (from usr in Db.Usuarios
                         join dus in Db.Datos_Usuarios on usr.Codigo_Usuario equals dus.Codigo_Usuario
                         where usr.Codigo_Usuario == id
                         select new Datos_Usuarios
                         {
                             Codigo_Usuario = dUser.Codigo_Usuario,
                             Nombre_Completo = dUser.Nombre_Completo,
                             Fecha_Nacimiento = dUser.Fecha_Nacimiento,
                             Rut = dUser.Rut
                         }).FirstOrDefault();
            }

            if (oUser == null)
            {
                return HttpNotFound();
            }
            return View(oUser);
        }

        // GET: Datos_Usuarios/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Datos_Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo_Usuario,Nombre_Completo,Fecha_Nacimiento,Rut")] Models.Datos_Usuarios dUsuarios)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var bd = new BD_EvaluacionEntities())
                    {
                        var oUsuario = new Models.Datos_Usuarios
                        {
                            Codigo_Usuario = dUsuarios.Codigo_Usuario,
                            Nombre_Completo = dUsuarios.Nombre_Completo,
                            Fecha_Nacimiento = dUsuarios.Fecha_Nacimiento,
                            Rut = dUsuarios.Rut
                            
                        };
                        bd.Datos_Usuarios.Add(oUsuario);
                        bd.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Status = false;
                    return RedirectToAction("Create", "User", new { e.Message });
                }

                return RedirectToAction("Index");
            }

            return View(dUsuarios);
        }

        // GET: Datos_Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Datos_Usuarios dUsuarios = db.Datos_Usuarios.Find(id);
            if (dUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(dUsuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo_Usuario,Nombre_Completo,Fecha_Nacimiento,Rut")] Models.Datos_Usuarios dUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dUsuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dUsuarios);
        }

        //// GET: Datos_Usuarios/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Datos_Usuarios dUsuarios = db.Datos_Usuarios.Find(id);
        //    if (dUsuarios == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dUsuarios);
        //}

        //// POST: Datos_Usuarios/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Datos_Usuarios dUsuarios = db.Datos_Usuarios.Find(id);
        //    db.Datos_Usuarios.Remove(dUsuarios);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
