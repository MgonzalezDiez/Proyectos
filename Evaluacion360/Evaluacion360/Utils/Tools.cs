using Evaluacion360.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
                CommandText = "Select Codigo_Seccion, Nombre_Seccion from Secciones Where IdState = 1 Order by Nombre_Seccion",
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


        public static IEnumerable<Secciones> DominiosPorUsuario(string codUsuario)
        {
            List<Secciones> Section = new List<Secciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "Select Distinct Sec.Codigo_Seccion, Sec.Nombre_Seccion from Secciones Sec " +
                         "Join Preguntas_Cargos Prc on Sec.Codigo_Seccion = Prc.Codigo_seccion " +
                         "Join Auto_Evaluacion_Preguntas AEP on Sec.Codigo_Seccion = AEP.Codigo_Seccion And Prc.Numero_Pregunta = AEP.Numero_Pregunta " +
                         "Join Auto_Evaluaciones AEv on AEP.Numero_Evaluacion = AEv.Numero_Evaluacion and AEP.Codigo_Proceso = AEV.Codigo_Proceso " +
                         "Where Codigo_Usuario = '" + codUsuario + "'" +
                         "And Prc.Cod_Cargo_Evaluado = Prc.Codigo_Cargo And AEP.Nota = 0";


            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
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

        public static IEnumerable<Secciones> DominiosPorUsuarioCargo(string codUsuario)
        {
            List<Secciones> Section = new List<Secciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "Select Distinct Sec.Codigo_Seccion, Sec.Nombre_Seccion from Secciones Sec " +
                         "Join Preguntas_Cargos EPC on Sec.Codigo_Seccion = EPC.Codigo_Seccion " +
                         "Join Evaluaciones_CargoS EVC on EPC.Cod_Cargo_Evaluado = EVC.Cod_Cargo_Evaluado " +
                         "Where EVC.Cod_Usuario_Evaluado = '" + codUsuario + "'" +
                         "And epc.Codigo_Cargo <> epc.Cod_Cargo_Evaluado";


            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
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

        public static IEnumerable<Preguntas_Aleatorias> PreguntasPorPonderacion(string CodSec, string Ponderacion, string CodUser)
        {
            List<Preguntas_Aleatorias> RQ = new List<Preguntas_Aleatorias>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "Select Distinct Numero_Pregunta, Texto_Pregunta from Preguntas_Aleatorias Pal " +
                         "Join Secciones Sec on Pal.Codigo_Seccion = Sec.Codigo_Seccion " +
                         "Where Sec.Codigo_Seccion = '" + CodSec + "' And " +
                         "And Pal.Ponderacion_P = " + Ponderacion ;

            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
                Connection = Cnn
            };
            Cnn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Preguntas_Aleatorias pal = new Preguntas_Aleatorias()
                        {
                            Codigo_Seccion = CodSec,
                            Numero_Pregunta = int.Parse( dr.GetString(0)),
                            Texto_Pregunta = dr.GetString(1),
                        };
                        RQ.Add(pal);
                    }
                }
            }
            return RQ;
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

        public static IEnumerable<Usuarios> LeerUsuarios(string CodUser)
        {
            List<Usuarios> Proc = new List<Usuarios>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "Select Codigo_Usuario, Nombre_Usuario from Usuarios";
            if (!string.IsNullOrEmpty(CodUser))
            {
                sql += " where codigo_usuario = '" + CodUser + "'";
            }
            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
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

        public static IEnumerable<Usuarios> LeerUsuariosPoEvaluador(string CodUser)
        {
            List<Usuarios> Proc = new List<Usuarios>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            string sql = "select Us1.Codigo_Cargo CargoEvaluado, Dus.Nombre_Completo " +
                         "from cargos_evaluadores Cev " +
                         "Join Usuarios Usu on Cev.Codigo_Cargo = Usu.Codigo_Cargo " +
                         "Join Usuarios Us1 on Cev.Cod_Cargo_Evaluado = Us1.Codigo_Cargo " +
                         "Join Datos_Usuarios Dus on Us1.Codigo_Usuario = Dus.Codigo_Usuario " +
                         "where Usu.Codigo_Usuario = '" + CodUser + "' And Cev.Codigo_Cargo <> Cev.Cod_Cargo_Evaluado ";

            using SqlConnection Cnn = new SqlConnection(CnnStr);
            using SqlCommand cmd = new SqlCommand
            {
                CommandText = sql,
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

        public static IEnumerable<Evaluacion_Preguntas_Cargos> EvaluacionResultadoCargos(int NumEva, int CodPro, string CodSec, string CodCarE, string CodUsEval, int NumPre)
        {
            List<Evaluacion_Preguntas_Cargos> Proc = new List<Evaluacion_Preguntas_Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
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

        public static IEnumerable<Preguntas_Aleatorias> LeerPreguntasAleatorias(string codSec, int askNo)
        {
            string texto;
            List<Preguntas_Aleatorias> PA = new List<Preguntas_Aleatorias>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
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

        public static IEnumerable<Estado_Evaluaciones> LeerEstadoEvaluaciones()
        {
            List<Estado_Evaluaciones> Position = new List<Estado_Evaluaciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using SqlConnection Cnn = new SqlConnection(CnnStr);
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

        public static int ValidaDominios(string codSec)
        {
            BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            var data = Db.Secciones.Where(x => x.Codigo_Seccion == codSec).FirstOrDefault();
            if (!(data == null))
            {
                return data.IdState;
            }
            return 0;
        }

        public static IEnumerable<Estado_Evaluaciones> EvaluationState()
        {
            BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            IEnumerable<Estado_Evaluaciones> evState = Db.Estado_Evaluaciones.ToList();

            return evState;
        }

        public static int ValidaPreguntas(string codSec, int askNo)
        {
            BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
            var data = Db.Preguntas_Aleatorias.Where(x => x.Codigo_Seccion == codSec && x.Numero_Pregunta == askNo).FirstOrDefault();
            if (!(data == null))
            {
                return 1;
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