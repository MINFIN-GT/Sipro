using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ComponenteSigadeDAO
    {
        public static bool guardarComponenteSigade(ComponenteSigade ComponenteSigade)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM COMPONENTE_SIGADE WHERE id=:id", new { id = ComponenteSigade.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE COMPONENTE_SIGADE SET nombre=:nombre, codigo_presupuestario=:codigoPresupuestario, numero_componente=:numeroComponente, " +
                            "monto_componente=:montoComponente, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", ComponenteSigade);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_componente_sigade.nextval FROM DUAL");
                        ComponenteSigade.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO COMPONENTE_SIGADE VALUES (:id, :nombre, :codigoPresupuestario, :numeroComponente, :montoComponente, " +
                            ":usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", ComponenteSigade);

                        ret = guardado > 0 ? true : false;
                    }

                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponenteSigadeDAO.class", e);
            }
            return ret;
        }

        public static ComponenteSigade getComponenteSigadePorCodigoNumero(String codigoPresupuestario, int numero)
        {
            ComponenteSigade ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string Str_query = String.Join(" ", "SELECT * FROM COMPONENTE_SIGADE c",
                    "WHERE c.codigo_presupuestario=:codigoPresupuestario",
                    "AND c.numero_componente=:numero",
                    "AND c.estado = 1");

                    ret = db.QueryFirstOrDefault<ComponenteSigade>(Str_query, new { codigoPresupuestario = codigoPresupuestario, numero = numero });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponenteSigadeDAO.class", e);
            }
            return ret;
        }

        public static ComponenteSigade getComponenteSigadePorId(int id)
        {
            ComponenteSigade ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ComponenteSigade>("SELECT * FROM COMPONENTE_SIGADE WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteSigadeDAO.class", e);
            }
            return ret;
        }

        public static ComponenteSigade getComponenteSigadePorIdHistory(int id, String lineaBase)
        {
            ComponenteSigade ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    string Str_query = String.Join(" ", "SELECT * FROM COMPONENTE_SIGADE",
                            "WHERE id=:id",
                            lineaBase != null ? "AND linea_base=:lineaBase" : "AND actual=1");

                    db.QueryFirstOrDefault<ComponenteSigade>(Str_query, new { id = id, lineaBase = lineaBase });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteSigadeDAO.class", e);
            }
            return ret;
        }
    }
}
