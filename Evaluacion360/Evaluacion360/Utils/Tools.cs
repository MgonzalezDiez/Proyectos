using Evaluacion360.Models;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;

namespace Evaluacion360.Utils
{
    public class Tools
    {
        public static IEnumerable<Estado_Componentes> LeerEstados()
        {
            List<Estado_Componentes> State = new List<Estado_Componentes>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = "Select IdState, StateDescription from Estado_Componentes ",
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader rea = cmd.ExecuteReader())
            {
                while (rea.Read())
                {
                    Estado_Componentes sta = new Estado_Componentes()
                    {
                        IdState = rea.GetInt32(0),
                        StateDescription = rea.GetString(1)
                    };
                    State.Add(sta);
                }
            }
            return State;
        }

        public static IEnumerable<Estado_Evaluaciones> EstadosEvaluaciones()
        {
            List<Estado_Evaluaciones> State = new List<Estado_Evaluaciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = "Select IdState, StateDescription from Estado_Evaluaciones",
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader rea = cmd.ExecuteReader())
            {
                while (rea.Read())
                {
                    Estado_Evaluaciones sta = new Estado_Evaluaciones()
                    {
                        IdState = rea.GetString(0),
                        StateDescription = rea.GetString(1)
                    };
                    State.Add(sta);
                }
            }
            return State;
        }

        public static IEnumerable<Secciones> LeerSecciones()
        {
            List<Secciones> Section = new List<Secciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = "Select Codigo_Seccion, Nombre_Seccion from Secciones Where IdState = 1 Order by Id",
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader sec = cmd.ExecuteReader())
            {
                if (sec.HasRows)
                {
                    while (sec.Read())
                    {
                        Secciones scc = new Secciones()
                        {
                            Codigo_Seccion = sec.GetString(0),
                            Nombre_Seccion = sec.GetString(1)
                        };
                        Section.Add(scc);
                    }
                }
            }
            return Section;
        }

        public static IEnumerable<Cargos> LeerCargos(int userType)
        {
            List<Cargos> cargos = new List<Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using (SqlCommand cmd = new SqlCommand())
            {
                if (userType != 1)
                {
                    cmd.CommandText = "Select Codigo_Cargo, Nombre_Cargo from Cargos Where Codigo_Cargo <> '1'";
                    cmd.Connection = Cnn;
                }
                else
                {
                    cmd.CommandText = "Select Codigo_Cargo, Nombre_Cargo from Cargos";
                    cmd.Connection = Cnn;
                }
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
            return cargos;
        }

        public static IEnumerable<Procesos_Evaluacion> LeerProcesos()
        {
            List<Procesos_Evaluacion> Proc = new List<Procesos_Evaluacion>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = "Select Codigo_Proceso, Nombre_Proceso, Ano_Proceso, Mes_Proceso, Coalesce(Retroalimentacion, ''), Estado_PE from Procesos_Evaluacion" +
                " where Estado_PE = 'A'",
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader pos = cmd.ExecuteReader())
            {
                if (pos.HasRows)
                {
                    while (pos.Read())
                    {
                        Procesos_Evaluacion pro = new Procesos_Evaluacion()
                        {
                            Codigo_Proceso = pos.GetInt32(0),
                            Nombre_Proceso = pos.GetString(1),
                            Ano_Proceso = pos.GetInt32(2),
                            Mes_Proceso = pos.GetInt32(3),
                            Retroalimentacion = pos.GetString(4) ?? string.Empty,
                            Estado_PE = pos.GetString(5)
                        };
                        Proc.Add(pro);
                    }
                }
            }
            Cnn.Close();
            return Proc;
        }

        public static IEnumerable<Usuarios> LeerUsuarios()
        {
            List<Usuarios> Proc = new List<Usuarios>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = "Select Codigo_Usuario, Nombre_Usuario from Usuarios",
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader pos = cmd.ExecuteReader())
            {
                while (pos.Read())
                {
                    Usuarios pro = new Usuarios()
                    {
                        Codigo_Usuario = pos.GetString(0),
                        Nombre_Usuario = pos.GetString(1)
                    };
                    Proc.Add(pro);
                }
            }
            return Proc;
        }

        public static int TipoUsuario(string CodUser)
        {
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand();
            int TUsuario = 0;
            cmd.CommandText = "Select Tipo_Usuario from Usuarios Where Codigo_Usuario = " + CodUser;
            cmd.Connection = Cnn;
            Cnn.Open();
            using (SqlDataReader rea = cmd.ExecuteReader())
            {
                while (rea.Read())
                {
                    TUsuario = rea.GetInt32(0);
                }
            }
            //Cnn.Close();

            return TUsuario;
        }

        public static IEnumerable<Rol> LeerTipoUsuario(int userType)
        {
            List<Rol> tipoUsuarios = new List<Rol>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand();
            if (userType != 1)
            {
                cmd.CommandText = "Select Tipo_Usuario, Nombre from Rol Where Tipo_Usuario <> 1";
                cmd.Connection = Cnn;
            }
            else
            {
                cmd.CommandText = "Select Tipo_Usuario, Nombre from Rol ";
                cmd.Connection = Cnn;
            }
            Cnn.Open();
            using (SqlDataReader rea = cmd.ExecuteReader())
            {
                while (rea.Read())
                {
                    Rol r = new Rol
                    {
                        Tipo_Usuario = rea.GetInt32(0),
                        Nombre = rea.GetString(1)
                    };
                    tipoUsuarios.Add(r);
                }
            }
            return tipoUsuarios;
        }

        public static IEnumerable<Evaluacion_Preguntas_Cargos> EvaluacionResultadoCagos(int NumEva, int CodPro, string CodSec, string CodCarE, string CodUsEval, int NumPre)
        {
            List<Evaluacion_Preguntas_Cargos> Proc = new List<Evaluacion_Preguntas_Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using SqlCommand cmd = new SqlCommand
                {
                    CommandText = "Select * from Procesos_Evaluacion Where " +
                    "Numero_Evaluacion = " + NumEva + " And " +
                    "Codigo_Proceso = " + CodPro + " And " +
                    "Codigo_seccion = " + CodSec + " And " +
                    "Cod_Cargo_Evaluado = " + CodCarE + " And " +
                    "Cod_Usuario_Evaluado = " + CodUsEval + " And " +
                    "Numero_Pregunta = " + NumPre,
                    Connection = Cnn
                };
                Cnn.Open();
                using (SqlDataReader pos = cmd.ExecuteReader())
                {
                    while (pos.Read())
                    {
                        Evaluacion_Preguntas_Cargos pro = new Evaluacion_Preguntas_Cargos()
                        {
                            Numero_Evaluacion = pos.GetInt32(0),
                            Codigo_Proceso = pos.GetInt32(1),
                            Codigo_seccion = pos.GetString(2),
                            Numero_Pregunta = pos.GetInt32(5)
                        };
                        Proc.Add(pro);
                    }
                }
                return Proc;
            }
        }

        public static IEnumerable<Preguntas_Aleatorias> LeerPreguntasAleatorias(string codSec, int askNo)
        {
            string texto;
            List<Preguntas_Aleatorias> PA = new List<Preguntas_Aleatorias>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using SqlCommand cmd = new SqlCommand();
                texto = "Select * from Preguntas_Aleatorias ";
                if (codSec != "")
                {
                    texto += " where Codigo_Seccion = '" + codSec + "'";
                }
                if (askNo > 0)
                {
                    texto += " and Numero_Pregunta = " + askNo;
                }

                cmd.CommandText = texto;
                cmd.Connection = Cnn;
                Cnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Preguntas_Aleatorias rq = new Preguntas_Aleatorias()
                        {
                            Numero_Pregunta = dr.GetInt32(1),
                            Texto_Pregunta = dr.GetString(2),
                            Ponderacion_P = dr.GetDecimal(3)
                        };
                        PA.Add(rq);
                    }
                }
                return PA;
            }
        }

        public static IEnumerable<Estado_Evaluaciones> LeerEstadoEvaluaciones()
        {
            List<Estado_Evaluaciones> Position = new List<Estado_Evaluaciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using SqlCommand cmd = new SqlCommand
                {
                    CommandText = "Select IdState, StateDescription from Estado_Evaluaciones",
                    Connection = Cnn
                };
                Cnn.Open();
                using (SqlDataReader pos = cmd.ExecuteReader())
                {
                    while (pos.Read())
                    {
                        Estado_Evaluaciones sev = new Estado_Evaluaciones()
                        {
                            IdState = pos.GetString(0),
                            StateDescription = pos.GetString(1)
                        };
                        Position.Add(sev);
                    }
                }
                return Position;
            }
        }

        public static int ValidaDominios(string codSec)
        {
            BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            var data = Db.Secciones.Where(x => x.Codigo_Seccion == codSec).FirstOrDefault();
            if (!(data == null)){
                return data.IdState;
            }
            return 0;
        }
        

        public static string CalcularDV(int Rut)
        {
            int suma = 0;
            int multiplicador = 1;
            while (Rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += (Rut % 10) * multiplicador;
                Rut /= 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)
            {
                return "0";
            }
            else if (suma == 10)
            {
                return "K";
            }
            else
            {
                return suma.ToString();
            }
        }
    }
}