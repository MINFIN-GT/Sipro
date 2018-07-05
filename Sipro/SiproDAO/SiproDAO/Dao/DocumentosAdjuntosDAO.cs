using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using SiproModelCore.Models;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class DocumentosAdjuntosDAO
    {
        public static bool guardarDocumentoAdjunto(Documento documento)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM DOCUMENTO WHERE id=:id", new { id = documento.id });

                    if (existe > 0)
                    {
                        guardado = db.Execute("UPDATE DOCUMENTO SET nombre=:nombre, extension=:extension, id_tipo_objeto=:idTipoObjeto, id_objeto=:idObjeto, " +
                            "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE id=:id", documento);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_documento.nextval FROM DUAL");
                        documento.id = sequenceId;

                        guardado = db.Execute("INSERT INTO DOCUMENTO VALUES (:id, :nombre, :extension, :idTipoObjeto, :idObjeto, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion, :estado)", documento);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "DocumentosAdjuntosDAO.class", e);
            }
            return ret;
        }

        public static List<Documento> getDocumentos(int idObjeto, int idTipoObjeto)
        {
            List<Documento> ret = new List<Documento>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Documento>("SELECT d.* FROM DOCUMENTO d WHERE d.estado=1 AND d.id_objeto=:idObjeto AND d.id_tipo_objeto=:idTipoObjeto " +
                        "ORDER BY d.fecha_creacion", new { idObjeto = idObjeto, idTipoObjeto = idTipoObjeto }).AsList<Documento>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "DocumentosAdjuntosDAO.class", e);
            }

            return ret;
        }

        public static Documento getDocumentoById(int idDocumento)
        {
            Documento ret = new Documento();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Documento>("SELECT * FROM DOCUMENTO d WHERE d.id=:id", new { id = idDocumento });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "DocumentosAdjuntosDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarDocumentoAdjunto(Documento documento)
        {
            bool ret = false;

            try
            {
                documento.estado = 0;
                documento.fechaActualizacion = DateTime.Now;
                ret = guardarDocumentoAdjunto(documento);
            }
            catch (Exception e)
            {
                CLogger.write("4", "DocumentosAdjuntosDAO.class", e);
            }
            return ret;
        }
    }
}
