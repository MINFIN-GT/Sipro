using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiproModel.Models;
using Sipro.Utilities;
using Dapper;
using MySql.Data.MySqlClient;
using Dapper.FastCrud;

namespace Sipro.Dao
{
    public class ActividadPropiedadDAO
    {
        public ActividadPropiedadDAO()
        {
            new CMariaDB();
        }

        public static List<Actividadpropiedad> getActividadPropiedadesPorTipoActividadPagina(int idTipoActividad)
        {
            List<Actividadpropiedad> ret = new List<Actividadpropiedad>();
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "select * from Actividadpropiedad ap, atipo_propiedad atp, actividad_tipo pt ",
                        "where Actividadpropiedadid = ap.id and atp.actividad_tipoid = pt.id and pt.id=@idTipoActividad");
                    using (var connection = CMariaDB.getConnection())
                    {
                        ret = (List<Actividadpropiedad>)connection.Query<Actividadpropiedad>(query, new { idTipoActividad = idTipoActividad });
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static long getTotalActividadPropiedades()
        {
            long ret = 0L;
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "select count(p.id) from Actividadpropiedad p where p.estado=1");
                    using (var connection = CMariaDB.getConnection())
                    {
                        ret = connection.ExecuteScalar<long>(query);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static List<Actividadpropiedad> getActividadPropiedadPaginaTotalDisponibles(int pagina, int numeroactividadpropiedades, String idPropiedades)
        {
            List<Actividadpropiedad> ret = new List<Actividadpropiedad>();
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "select * from Actividadpropiedad ap",
                        "where ap.estado = 1 and ap.id not in (@idPropiedades) limit @pagina,@numDatos");
                    using (var connection = CMariaDB.getConnection())
                    {
                        ret = (List<Actividadpropiedad>)connection.Query<Actividadpropiedad>(query, new { idPropiedades = idPropiedades, pagina = pagina, numDatos = numeroactividadpropiedades });
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static Actividadpropiedad getActividadPropiedadPorId(int id)
        {
            Actividadpropiedad ret = new Actividadpropiedad();
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "select * from actividad_propiedad ap",
                        "where ap.estado = 1 and ap.id=@id");
                    using (var connection = CMariaDB.getConnection())
                    {
                        ret = connection.QueryFirstOrDefault<Actividadpropiedad>(query, new { id = id });
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static Boolean guardarActividadPropiedad(Actividadpropiedad actividadPropiedad)
        {
            Boolean ret = false;
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "select count(p.id) from Actividadpropiedad p where p.estado=1 and p.id=@id");
                    using (var connection = CMariaDB.getConnection())
                    {
                        Int32 existe = connection.QuerySingle<Int32>(query, new { id = actividadPropiedad.id });
                        if (existe > 0)
                        {
                            query = String.Join(" ", "update Actividadpropiedad ap set ap.nombre=@nombre, ap.descripcion=@descripcion, ap.usaurio_creo=@usuarioCreo, ap.usuario_actualizo=@usuarioActualizo,",
                                "ap.fecha_creacion=@fechaCreacion, ap.fecha_actualizacion=@fechaActualizacion, ap.estado=@estado, ap.dato_tipoid=@datoTipoId");

                            var affectedRows = connection.Execute(query, new
                            {
                                nombre = actividadPropiedad.nombre,
                                descripcion = actividadPropiedad.descripcion,
                                usuarioCreo = actividadPropiedad.usuario_creo,
                                usuarioActualizo = actividadPropiedad.usuario_actualizo,
                                fechaCreacion = actividadPropiedad.fecha_creacion,
                                fechaActualizacion = actividadPropiedad.fecha_actualizacion,
                                estado = actividadPropiedad.estado,
                                datoTipoId = actividadPropiedad.dato_tipoid
                            });

                            ret = affectedRows > 1 ? true : false;
                        }
                        else
                        {
                            query = String.Join(" ", "insert into Actividadpropiedad (nombre, descripcion, usuario_creo, fecha_creacion, estado, dato_tipoid) values (@nombre,@descripcion, @usuarioCreo, @fechaCreacion, @estado, @datoTipoId");

                            var affectedRows = connection.Execute(query, new
                            {
                                nombre = actividadPropiedad.nombre,
                                descripcion = actividadPropiedad.descripcion,
                                usuarioCreo = actividadPropiedad.usuario_creo,
                                fechaCreacion = actividadPropiedad.fecha_creacion,
                                estado = actividadPropiedad.estado,
                                datoTipoId = actividadPropiedad.dato_tipoid
                            });

                            ret = affectedRows > 1 ? true : false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }

        public static Boolean eliminarActividadPropiedad(Actividadpropiedad actividadPropiedad)
        {
            Boolean ret = false;
            try
            {
                if (CMariaDB.connect())
                {
                    String query = String.Join(" ", "update from Actividadpropiedad set estado=0 where id=@id");
                    using (var connection = CMariaDB.getConnection())
                    {
                        actividadPropiedad.estado = 0;
                        //var isSuccess = connection.Execute(query, new { id = actividadPropiedad.id });
                        connection.Update<Actividadpropiedad>(actividadPropiedad);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadPropiedadDAO", e);
            }
            finally
            {
                CMariaDB.close();
            }

            return ret;
        }


    }
}
