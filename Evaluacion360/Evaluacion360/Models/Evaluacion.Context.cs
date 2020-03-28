﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Evaluacion360.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BD_EvaluacionEntities : DbContext
    {
        public BD_EvaluacionEntities()
            : base("name=BD_EvaluacionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Auto_Evaluacion_Preguntas> Auto_Evaluacion_Preguntas { get; set; }
        public virtual DbSet<Auto_Evaluacion_Resultado> Auto_Evaluacion_Resultado { get; set; }
        public virtual DbSet<Auto_Evaluaciones> Auto_Evaluaciones { get; set; }
        public virtual DbSet<Cargos> Cargos { get; set; }
        public virtual DbSet<Cargos_Evaluadores> Cargos_Evaluadores { get; set; }
        public virtual DbSet<Datos_Usuarios> Datos_Usuarios { get; set; }
        public virtual DbSet<Estado_Componentes> Estado_Componentes { get; set; }
        public virtual DbSet<Estado_Evaluaciones> Estado_Evaluaciones { get; set; }
        public virtual DbSet<Evaluacion_Preguntas_Cargos> Evaluacion_Preguntas_Cargos { get; set; }
        public virtual DbSet<Evaluacion_Resultado_Cargos> Evaluacion_Resultado_Cargos { get; set; }
        public virtual DbSet<Evaluaciones_Cargos> Evaluaciones_Cargos { get; set; }
        public virtual DbSet<Operacion> Operacion { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Rol_Operacion> Rol_Operacion { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Procesos_Evaluacion> Procesos_Evaluacion { get; set; }
        public virtual DbSet<Preguntas_Aleatorias> Preguntas_Aleatorias { get; set; }
        public virtual DbSet<Secciones> Secciones { get; set; }
        public virtual DbSet<Preguntas_Cargos> Preguntas_Cargos { get; set; }
    
        public virtual int Crea_Evaluaciones(string codigoUsuario, Nullable<int> proceso)
        {
            var codigoUsuarioParameter = codigoUsuario != null ?
                new ObjectParameter("CodigoUsuario", codigoUsuario) :
                new ObjectParameter("CodigoUsuario", typeof(string));
    
            var procesoParameter = proceso.HasValue ?
                new ObjectParameter("Proceso", proceso) :
                new ObjectParameter("Proceso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Crea_Evaluaciones", codigoUsuarioParameter, procesoParameter);
        }
    
        public virtual int ObtenerDV(Nullable<int> rut, ObjectParameter dv)
        {
            var rutParameter = rut.HasValue ?
                new ObjectParameter("Rut", rut) :
                new ObjectParameter("Rut", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ObtenerDV", rutParameter, dv);
        }
    
        public virtual int Calcula_Notas(string codigoUsuario, string proceso)
        {
            var codigoUsuarioParameter = codigoUsuario != null ?
                new ObjectParameter("CodigoUsuario", codigoUsuario) :
                new ObjectParameter("CodigoUsuario", typeof(string));
    
            var procesoParameter = proceso != null ?
                new ObjectParameter("Proceso", proceso) :
                new ObjectParameter("Proceso", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Calcula_Notas", codigoUsuarioParameter, procesoParameter);
        }
    
        public virtual ObjectResult<Crea_Evaluaciones_Todos_Result> Crea_Evaluaciones_Todos(Nullable<int> proceso, ObjectParameter result)
        {
            var procesoParameter = proceso.HasValue ?
                new ObjectParameter("Proceso", proceso) :
                new ObjectParameter("Proceso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Crea_Evaluaciones_Todos_Result>("Crea_Evaluaciones_Todos", procesoParameter, result);
        }
    
        public virtual ObjectResult<Crea_Evaluaciones_Uno_Result> Crea_Evaluaciones_Uno(string codigoUsuario, Nullable<int> proceso, ObjectParameter result)
        {
            var codigoUsuarioParameter = codigoUsuario != null ?
                new ObjectParameter("CodigoUsuario", codigoUsuario) :
                new ObjectParameter("CodigoUsuario", typeof(string));
    
            var procesoParameter = proceso.HasValue ?
                new ObjectParameter("Proceso", proceso) :
                new ObjectParameter("Proceso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Crea_Evaluaciones_Uno_Result>("Crea_Evaluaciones_Uno", codigoUsuarioParameter, procesoParameter, result);
        }
    }
}
