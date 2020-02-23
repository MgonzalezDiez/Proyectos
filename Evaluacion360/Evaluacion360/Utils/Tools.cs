using Evaluacion360.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Mvc;

namespace Evaluacion360.Utils
{
    public class Tools
    {
        public static IEnumerable<Estado_Componentes> LeerEstados()
        {
            List<Estado_Componentes> State = new List<Estado_Componentes>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select IdState, StateDescription from Estado_Componentes ";
                    cmd.Connection = Cnn;
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
            }
        }

        public static IEnumerable<Estado_Evaluaciones> EstadosEvaluaciones()
        {
            List<Estado_Evaluaciones> State = new List<Estado_Evaluaciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = "Select IdState, StateDescription from Estado_Evaluaciones";
                    cmd.Connection = Cnn;
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
            }
        }

        public static IEnumerable<Secciones> LeerSecciones()
        {
            List<Secciones> Section = new List<Secciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select Codigo_Seccion, Nombre_Seccion from Secciones Where IdState = 1 Order by Id";
                    cmd.Connection = Cnn;
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
            }
        }

        public static IEnumerable<Cargos> LeerCargos()
        {
            List<Cargos> Position = new List<Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select Codigo_Cargo, Nombre_Cargo from Cargos";
                    cmd.Connection = Cnn;
                    Cnn.Open();
                    using (SqlDataReader pos = cmd.ExecuteReader())
                    {
                        while (pos.Read())
                        {
                            Cargos car = new Cargos()
                            {
                                Codigo_Cargo = pos.GetString(0),
                                Nombre_Cargo = pos.GetString(1)
                            };
                            Position.Add(car);
                        }
                    }
                    return Position;
                }
            }
        }

        public static IEnumerable<Procesos_Evaluacion> LeerProcesos()
        {
            List<Procesos_Evaluacion> Proc = new List<Procesos_Evaluacion>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select Codigo_Proceso, Nombre_Proceso, Ano_Proceso, Mes_Proceso, Retroalimentacion, Estado_PE from Procesos_Evaluacion"+
                        " where Estado_PE = 'A'";
                    cmd.Connection = Cnn;
                    Cnn.Open();
                    using (SqlDataReader pos = cmd.ExecuteReader())
                    {
                        while (pos.Read())
                        {
                            Procesos_Evaluacion pro = new Procesos_Evaluacion()
                            {
                                Codigo_Proceso = pos.GetInt32(0),
                                Nombre_Proceso = pos.GetString(1),
                                Ano_Proceso = pos.GetInt32(2),
                                Mes_Proceso = pos.GetInt32(3),
                                Retroalimentacion = pos.GetString(4),
                                Estado_PE = pos.GetString(5)
                            };
                            Proc.Add(pro);
                        }
                    }
                    Cnn.Close();
                    return Proc;
                }
            }
        }

        public static IEnumerable<Usuarios> LeerUsuarios()
        {
            List<Usuarios> Proc = new List<Usuarios>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select Codigo_Usuario, Nombre_Usuario from Usuarios";
                    cmd.Connection = Cnn;
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
            }
        }


        public static IEnumerable<Evaluacion_Preguntas_Cargos> EvaluacionResultadoCagos(int NumEva, int CodPro, string CodSec, string CodCarE, string CodUsEval, int NumPre)
        {
            List<Evaluacion_Preguntas_Cargos> Proc = new List<Evaluacion_Preguntas_Cargos>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select * from Procesos_Evaluacion Where " +
                        "Numero_Evaluacion = "+ NumEva + " And " +
                        "Codigo_Proceso = " + CodPro + " And " +
                        "Codigo_seccion = " + CodSec + " And " +
                        "Cod_Cargo_Evaluado = " + CodCarE + " And " +
                        "Cod_Usuario_Evaluado = " + CodUsEval + " And " +
                        "Numero_Pregunta = " + NumPre;
                    cmd.Connection = Cnn;
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
        }

        public static IEnumerable<Preguntas_Aleatorias> LeerPreguntasAleatorias(string codSec)
        {
            List<Preguntas_Aleatorias> PA = new List<Preguntas_Aleatorias>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select * from Preguntas_Aleatorias where Codigo_Seccion > '" + codSec + "'";
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
        }

        public static IEnumerable<Estado_Evaluaciones> LeerEstadoEvaluaciones()
        {
            List<Estado_Evaluaciones> Position = new List<Estado_Evaluaciones>();
            string CnnStr = ConfigurationManager.ConnectionStrings["CnnStr"].ConnectionString;
            using (SqlConnection Cnn = new SqlConnection(CnnStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select IdState, StateDescription from Estado_Evaluaciones";
                    cmd.Connection = Cnn;
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