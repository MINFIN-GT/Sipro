﻿using System;
using System.Collections.Generic;

namespace Utilities
{
    public class CFormaDinamica
    {
        private class TipoTexto
        {
            public int id;
            public string tipo = "texto";
            public string label;
            public string valor;
            public string placeholder = "texto";
        }

        private class TipoEntero
        {
            public int id;
            public string tipo = "entero";
            public string label;
            public string valor;
            public string placeholder = "cantidad";
        }

        private class TipoDecimal
        {
            public int id;
            public string tipo = "decimal";
            public string label;
            public string valor;
            public string placeholder = "cantidad";
        }

        private class TipoBooleano
        {
            public int id;
            public string tipo = "booleano";
            public string label;
            public string valor;
        }

        private class TipoFecha
        {
            public int id;
            public string tipo = "fecha";
            public string label;
            public string valor;
        }

        public static List<object> convertirEstructura(List<Dictionary<string, Object>> campos)
        {
            try
            {
                List<object> estructura = new List<object>();

                foreach (Dictionary<string, Object> c in campos)
                {
                    switch ((int)c["tipo"])
                    {
                        case 1: // String
                            estructura.Add(tipoTexto(c));
                            break;
                        case 2: // entero
                            estructura.Add(tipoEntero(c));
                            break;
                        case 3: // decimal
                            estructura.Add(tipoDecimal(c));
                            break;

                        case 4: // booleano
                            estructura.Add(tipoBooleano(c));
                            break;
                        case 5: // tiempo
                            estructura.Add(tipoFecha(c));
                            break;
                    }
                }
                return estructura;
            }
            catch (Exception e)
            {
                CLogger.write("1", "CFormaDinamica.class", e);
                throw;
            }
            
        }

        private static Object tipoTexto(Dictionary<string, Object> campo)
        {
            try
            {
                TipoTexto tipoTexto = new TipoTexto();
                tipoTexto.id = (int)campo["id"];
                tipoTexto.label = (string)campo["nombre"];
                tipoTexto.valor = campo["valor"] != null ? (string)campo["valor"] : "";

                return tipoTexto;
            }
            catch (Exception e)
            {
                CLogger.write("2", "CFormaDinamica.class", e);
                throw;
            }

        }

        private static Object tipoEntero(Dictionary<string, Object> campo)
        {
            try
            {
                TipoEntero tipoEntero = new TipoEntero();
                tipoEntero.id = (int)campo["id"];
                tipoEntero.label = (string)campo["nombre"];
                tipoEntero.valor = campo["valor"] != null ? campo["valor"].ToString() : "";
                return tipoEntero;
            }
            catch (Exception e)
            {
                CLogger.write("3", "CFormaDinamica.class", e);
                throw;
            }
        }

        private static Object tipoDecimal(Dictionary<string, Object> campo)
        {
            try
            {
                TipoDecimal tipoDecimal = new TipoDecimal();
                tipoDecimal.id = (int)campo["id"];
                tipoDecimal.label = (string)campo["nombre"];
                tipoDecimal.valor = campo["valor"] != null ? campo["valor"].ToString() : "";

                return tipoDecimal;
            }
            catch (Exception e)
            {
                CLogger.write("4", "CFormaDinamica.class", e);
                throw;
            }

        }

        private static Object tipoBooleano(Dictionary<string, Object> campo)
        {
            try
            {
                TipoBooleano tipoBooleano = new TipoBooleano();
                tipoBooleano.id = (int)campo["id"];
                tipoBooleano.label = (string)campo["nombre"];
                tipoBooleano.valor = campo["valor"] != null ? (string)campo["valor"] : "";

                return tipoBooleano;
            }
            catch (Exception e)
            {
                CLogger.write("5", "CFormaDinamica.class", e);
                throw;
            }            
        }

        private static Object tipoFecha(Dictionary<string, Object> campo)
        {
            try
            {
                TipoFecha tipoFecha = new TipoFecha();
                tipoFecha.id = (int)campo["id"];
                tipoFecha.label = (string)campo["nombre"];
                tipoFecha.valor = campo["valor"] != null ? (string)campo["valor"] : "";

                return tipoFecha;
            }
            catch (Exception e)
            {
                CLogger.write("6", "CFormaDinamica.class", e);
                throw;
            }
        }
         
    }
}
