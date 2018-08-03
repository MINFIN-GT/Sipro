using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class PagoPlanificadoDAO
    {
        public static List<PagoPlanificado> getPagosPlanificadosPorObjeto(int objetoId, int objetoTipo)
        {
            List<PagoPlanificado> ret = new List<PagoPlanificado>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<PagoPlanificado>("SELECT * FROM pago_planificado p WHERE p.objeto_id=:objetoId AND p.objeto_tipo=:objetoTipo AND p.estado=1",
                        new { objetoId = objetoId, objetoTipo = objetoTipo }).AsList<PagoPlanificado>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PagoPlanificado.class", e);
            }
            return ret;
        }

        public static PagoPlanificado getPagosPlanificadosPorId(int id)
        {
            PagoPlanificado ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<PagoPlanificado>("SELECT * FROM pago_planificado p WHERE p.id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PagoPlanificado.class", e);
            }
            return ret;
        }

        public static bool Guardar(PagoPlanificado pagoPlanificado)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT * FROM pago_planificado WHERE id=:id", new { id = pagoPlanificado.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE pago_planificado SET fecha_pago=:fechaPago, pago=:pago, objeto_id=:objetoId, objeto_tipo=:objetoTipo, usuario_creo=:usuarioCreo, " +
                            "usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", pagoPlanificado);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_pago_planificado.nextval FROM DUAL");
                        pagoPlanificado.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO pago_planificado VALUES (:id, :fechaPago, :pago, :objetoId, :objetoTipo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado)", pagoPlanificado);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "PagoPlanificadoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarPagoPlanificado(PagoPlanificado pagoPlanificado)
        {
            bool ret = false;
            try
            {
                pagoPlanificado.estado = 0;
                pagoPlanificado.fechaActualizacion = DateTime.Now;
                ret = Guardar(pagoPlanificado);
            }
            catch (Exception e)
            {
                CLogger.write("4", "PagoPlanificadoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalPagoPlanificado(PagoPlanificado pagoPlanificado)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM pago_planificado WHERE id=:id", new { id = pagoPlanificado.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "PagoPlanificadoDAO.class", e);
            }
            return ret;
        }
    }
}
