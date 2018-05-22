using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class LineaBaseDAO
    {
        /*
         public static List<LineaBase> getLineasBaseById(Integer proyectoid){
		List<LineaBase> ret = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<LineaBase> criteria = session.createQuery("FROM LineaBase lb where lb.proyecto.id=:proyectoid", LineaBase.class);
			criteria.setParameter("proyectoid", proyectoid);
			ret = criteria.getResultList();
		}catch(Throwable e){
			CLogger.write("1", LineaBaseDAO.class, e);
		}finally{
			session.close();
		}
		return ret;
	}
	
	public static LineaBase getLineaBasePorId(int id){
		Session session = CHibernateSession.getSessionFactory().openSession();
		List<LineaBase> listRet = null;
		LineaBase ret = null;
		try{
			String query = "FROM LineaBase l where l.id=:id";
			Query<LineaBase> criteria = session.createQuery(query, LineaBase.class);
			criteria.setParameter("id", id);
			listRet = criteria.getResultList();
			
			ret = !listRet.isEmpty() ? listRet.get(0) : null;
		} catch(Throwable e){
			CLogger.write("2", LineaBaseDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}*/

        public static bool guardarLineaBase(LineaBase lineaBase, String lineaBaseEditar)
        {
            bool ret = false;
            int guardado = 0;
            if (lineaBaseEditar != null && lineaBaseEditar.Trim().Length > 0)
                ret = eliminarLinaeBase(lineaBaseEditar) >= 0;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) LINEA_BASE WHERE id=:id", new { id = lineaBase.id });

                    if (existe > 0)
                    {
                        guardado = db.Execute("UPDATE LINEA_BASE SET nombre=:nombre, proyectoid=:proyectoid, usuario_creo=:usuarioCreo, usuario_actualizo=:usuario_actualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, tipo_linea_base=:tipoLineaBase, sobreescribir=:sobreescribir " +
                            "WHERE id=:id", lineaBase);

                        ret = existe > 0 ? true : false;
                    }
                    else
                    {
                        guardado = db.Execute("INSERT INTO LINEA_BASE VALUES (:id, :nombre, :proyectoid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":tipoLineaBase, :sobreescribir)", lineaBase);

                        ret = existe > 0 ? true : false;
                    }

                    ret = ret && lineaBasePEP(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBasePEPPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseComponentes(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseComponentesPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseSubcomponentes(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseSubcomponentesPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseProducto(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseProductoPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseSubproducto(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseSubproductoPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseActividad(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseActividadPropiedadValor(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseDesembolsos(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseMetas(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseMetasPlanificado(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBaseMetasAvance(lineaBase) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisiciones(lineaBase, 3) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisiciones(lineaBase, 4) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisiciones(lineaBase, 5) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisicionPagos(lineaBase, 3) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisicionPagos(lineaBase, 4) >= 0;
                    if (ret)
                        ret = ret && lineaBasePlanAdquisicionPagos(lineaBase, 5) >= 0;
                    if (ret)
                        ret = ret && lineaBaseRiesgos(lineaBase, 0) >= 0;
                    if (ret)
                        ret = ret && lineaBaseRiesgos(lineaBase, 1) >= 0;
                    if (ret)
                        ret = ret && lineaBaseRiesgos(lineaBase, 2) >= 0;
                    if (ret)
                        ret = ret && lineaBaseRiesgos(lineaBase, 3) >= 0;
                    if (ret)
                        ret = ret && lineaBaseRiesgos(lineaBase, 4) >= 0;
                    if (ret)
                        ret = ret && lineaBaseMatrizRaci(lineaBase, 5) >= 0;
                    if (ret)
                        ret = ret && lineaBaseComponenteSigade(lineaBase) >= 0;
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBasePEP(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE PROYECTO",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE id =:proyectoId",
                        "AND estado = 1",
                        "AND actual = 1");

                    ret = db.Execute(query, lineaBase.proyectoid);
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBasePEPPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE PROYECTO_PROPIEDAD_VALOR",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE proyectoid =:proyectoId",
                        "AND estado = 1",
                        "AND actual = 1");

                    ret = db.Execute(query, new { proyectoId = lineaBase.id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseComponentes(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE COMPONENTE",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE treepath LIKE '" + lineaBase.proyectos.treepath.Trim() + "%' ",
                        "AND estado = 1",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseComponentesPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE COMPONENTE_PROPIEDAD_VALOR",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE componenteid IN ( ",
                            "SELECT c.id FROM COMPONENTE c",
                            "WHERE c.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                            "AND c.estado = 1 AND c.actual = 1",
                        ")",
                        "AND actual = 1");

                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseSubcomponentes(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE SUBCOMPONENTE",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                        "AND estado = 1",
                        "AND actual = 1");
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseSubcomponentesPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT * FROM SUBCOMPONENTE_PROPIEDAD_VALOR",
                        "WHERE subcomponenteid in ( ",
                            "SELECT c.id FROM SUBCOMPONENTE c",
                            "WHERE c.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                            "AND c.estado = 1 AND c.actual = 1",
                        ")",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseProducto(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE PRODUCTO",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "where treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                        "AND estado = 1",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseProductoPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE PRODUCTO_PROPIEDAD_VALOR",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE productoid IN ( ",
                            "SELECT p.id FROM PRODUCTO p",
                            "WHERE p.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                            "AND p.estado = 1 AND p.actual = 1",
                        ")",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseSubproducto(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE SUBPRODUCTO",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                        "AND estado = 1",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("11", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseSubproductoPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE SUBPRODUCTO_PROPIEDAD_VALOR",
                            "set linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                            "WHERE subproductoid in ( ",
                                "SELECT p.id FROM SUBPRODUCTO p",
                                "WHERE p.treepath like '" + lineaBase.proyectos.treepath + "%' ",
                                "AND p.estado = 1 AND p.actual = 1",
                            ")",
                            "AND actual = 1");

                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("12", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseActividad(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE ACTIVIDAD",
                                "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                                "WHERE treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                                "AND estado = 1",
                                "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("13", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseActividadPropiedadValor(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE ACTIVIDAD_PROPIEDAD_VALOR",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE actividadid IN ( ",
                            "SELECT a.id FROM ACTIVIDAD a",
                            "WHERE a.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                            "AND a.estado = 1 AND a.actual = 1",
                        ")",
                        "AND actual = 1");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("14", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseDesembolsos(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE DESEMBOLSO",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE proyectoid =:proyectoId",
                        "AND estado = 1");
                    ret = db.Execute(query, new { proyectoId = lineaBase.proyectoid });
                }
            }
            catch (Exception e)
            {
                CLogger.write("15", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseMetas(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE META",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE objeto_tipo = 3",
                        "AND estado = 1",
                        "AND actual = 1",
                        "AND objeto_id IN (",
                            "SELECT c.id FROM PRODUCTO c",
                            "WHERE c.estado = 1",
                            "AND c.actual = 1",
                            "AND c.treepath like '" + lineaBase.proyectos.treepath + "%' ",
                        ")");

                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("16", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseMetasPlanificado(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE META_PLANIFICADO",
                    "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                    "WHERE version = 1",
                    "AND estado = 1",
                    "AND metaid IN (",
                        "SELECT m.id FROM PRODUCTO p, META m",
                        "WHERE p.id = m.objeto_id",
                        "AND p.estado = 1",
                        "AND p.actual = 1",
                        "AND p.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                        "AND m.estado = 1",
                        "AND m.actual = 1",
                    ")");

                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("17", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseMetasAvance(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE META_AVANCE",
                    "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                    "WHERE version = 1",
                    "AND estado = 1",
                    "AND metaid IN (",
                        "SELECT m.id FROM PRODUCTO p, META m",
                        "where p.id = m.objeto_id",
                        "AND p.estado = 1",
                        "AND p.actual = 1",
                        "AND p.treepath LIKE '" + lineaBase.proyectos.treepath + "%' ",
                        "AND m.estado = 1",
                        "AND m.actual = 1",
                    ")");
                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("17", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBasePlanAdquisiciones(LineaBase lineaBase, int objetoTipo)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String tabla = "";
                    if (objetoTipo == 3)
                        tabla = "producto";
                    else if (objetoTipo == 4)
                        tabla = "subproducto";
                    else if (objetoTipo == 5)
                        tabla = "actividad";

                    String query = String.Join(" ", "UPDATE PLAN_ADQUISICION",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE actual = 1",
                        "AND estado = 1",
                        "AND objeto_tipo =:objetoTipo",
                        "AND objeto_id IN (",
                            "SELECT t.id FROM " + tabla + " t",
                            "WHERE t.estado = 1",
                            "AND t.actual = 1",
                            "AND t.treepath LIKE '" + lineaBase.proyectos.treepath + "%') ");

                    ret = db.Execute(query, new { objetoTipo = objetoTipo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("18", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBasePlanAdquisicionPagos(LineaBase lineaBase, int objetoTipo)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String tabla = "";
                    if (objetoTipo == 3)
                        tabla = "producto";
                    else if (objetoTipo == 4)
                        tabla = "subproducto";
                    else if (objetoTipo == 5)
                        tabla = "actividad";

                    String query = String.Join(" ", "UPDATE PLAN_ADQUISICION_PAGO",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE estado = 1",
                        "AND actual = 1",
                        "AND plan_adquisicionid IN (",
                            "SELECT p.id",
                            "FROM " + tabla + " t, PLAN_ADQUISICION p",
                            "WHERE t.id = p.objeto_id",
                            "AND p.objeto_tipo=:objetoTipo",
                            "AND t.estado = 1",
                            "AND t.actual = 1",
                            "AND t.treepath like '" + lineaBase.proyectos.treepath + "%') ");
                    ret = db.Execute(query, new { objetoTipo = objetoTipo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseRiesgos(LineaBase lineaBase, int objetoTipo)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String tabla = "";
                    if (objetoTipo == 0)
                        tabla = "proyecto";
                    if (objetoTipo == 1)
                        tabla = "componente";
                    if (objetoTipo == 2)
                        tabla = "subcomponente";
                    if (objetoTipo == 3)
                        tabla = "producto";
                    else if (objetoTipo == 4)
                        tabla = "subproducto";
                    else if (objetoTipo == 5)
                        tabla = "actividad";

                    String query = String.Join(" ", "UPDATE OBJETO_RIESGO o, RIESGO r",
                        "SET r.linea_base = CONCAT(NVL(r.linea_base,''),'|lb',", lineaBase.id, ",'|'),",
                        "o.linea_base = CONCAT(NVL(o.linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE o.riesgoid = r.id",
                        "AND o.actual = 1",
                        "AND o.objeto_tipo =:objetoTipo",
                        "AND r.actual = 1",
                        "AND r.estado = 1",
                        "AND o.objeto_id IN (",
                            "SELECT t.id ",
                            "FROM " + tabla + " t",
                            "WHERE t.actual = 1",
                            "AND t.estado = 1",
                            "AND t.treepath like '" + lineaBase.proyectos.treepath + "%') ");

                    ret = db.Execute(query, new { objetoTipo = objetoTipo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("20", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseMatrizRaci(LineaBase lineaBase, int objetoTipo)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String tabla = "";
                    if (objetoTipo == 0)
                        tabla = "proyecto";
                    if (objetoTipo == 1)
                        tabla = "componente";
                    if (objetoTipo == 2)
                        tabla = "subcomponente";
                    if (objetoTipo == 3)
                        tabla = "producto";
                    else if (objetoTipo == 4)
                        tabla = "subproducto";
                    else if (objetoTipo == 5)
                        tabla = "actividad";

                    String query = String.Join(" ", "UPDATE ASIGNACION_RACI ",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE actual = 1 ",
                        "AND estado = 1",
                        "AND objeto_tipo=:objetoTipo",
                        "AND objeto_id in (",
                            "SELECT t.id ",
                            "FROM " + tabla + " t",
                            "WHERE t.actual = 1",
                            "AND t.estado = 1",
                            "AND t.treepath LIKE '" + lineaBase.proyectos.treepath + "%') ");
                    ret = db.Execute(query, new { objetoTipo = objetoTipo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("21", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int lineaBaseComponenteSigade(LineaBase lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "UPDATE COMPONENTE_SIGADE",
                        "SET linea_base = CONCAT(NVL(linea_base,''),'|lb',", lineaBase.id, ",'|')",
                        "WHERE actual = 1",
                        "AND estado = 1",
                        "AND id IN (",
                            "SELECT c.componente_sigadeid",
                            "FROM COMPONENTE c",
                            "WHERE c.actual = 1",
                            "AND c.estado = 1",
                            "AND c.treepath LIKE '" + lineaBase.proyectos.treepath + "%') ");

                    ret = db.Execute(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("22", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        private static int eliminarLinaeBase(String lineaBase)
        {
            int ret = -1;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "UPDATE sipro_history.actividad SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = db.Execute(query);

                    query = "UPDATE sipro_history.actividad_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.asignacion_raci SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.componente SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.componente_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.componente_sigade SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.desembolso SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.meta SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.meta_avance SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.meta_planificado SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.plan_adquisicion SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.plan_adquisicion_pago SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.prestamo SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.producto SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.producto_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.proyecto SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.proyecto_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.riesgo SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.riesgo_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.subcomponente SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.subcomponente_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.subproducto SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);

                    query = "UPDATE sipro_history.subproducto_propiedad_valor SET linea_base = REPLACE(linea_base,'" + lineaBase + "','')";
                    ret = ret + db.Execute(query);
                }
            }
            catch (Exception e)
            {
                ret = -1;
                CLogger.write("23", "LineaBaseDAO.class", e);
            }

            return ret;
        }
	
	/*public static boolean eliminarTotalLineaBase(LineaBase lineaBase){
		boolean ret = false;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			session.beginTransaction();
			session.delete(lineaBase);
			session.getTransaction().commit();
			ret = true;
		}
		catch(Throwable e){
			CLogger.write("24", LineaBaseDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}
	
	public static LineaBase getLineasBaseByNombre(Integer proyectoId, String nombre){
		LineaBase ret = null;
		List<LineaBase> listRet = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<LineaBase> criteria = session.createQuery("select l from LineaBase l where l.proyecto.id = ?1 and l.nombre = ?2 ", LineaBase.class);
			criteria.setParameter(1, proyectoId);
			criteria.setParameter(2, nombre);
			listRet = criteria.getResultList();
			
			ret = !listRet.isEmpty() ? listRet.get(0) : null;
			
		}catch(Throwable e){
			CLogger.write("25", LineaBaseDAO.class, e);
		}finally{
			session.close();
		}
		return ret;
	}*/

        public static LineaBase getUltimaLinaBasePorProyecto(int proyectoId, int tipoLineaBase)
        {
            LineaBase ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<LineaBase>("SELECT * FROM LINEA_BASE WHERE proyectoid=:proyectoId AND tipo_linea_base=:tipoLineaBase ORDER BY id DESC",
                        new { proyectoId = proyectoId, tipoLineaBase = tipoLineaBase });
                }
            }
            catch (Exception e)
            {
                CLogger.write("25", "LineaBaseDAO.class", e);
            }
            return ret;
        }

        public static bool guardarLineaBase(LineaBase lineaBase)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) LINEA_BASE WHERE id=:id", new { id = lineaBase.id });

                    if (existe > 0)
                    {
                        guardado = db.Execute("UPDATE LINEA_BASE SET nombre=:nombre, proyectoid=:proyectoid, usuario_creo=:usuarioCreo, usuario_actualizo=:usuario_actualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, tipo_linea_base=:tipoLineaBase, sobreescribir=:sobreescribir " +
                            "WHERE id=:id", lineaBase);

                        ret = existe > 0 ? true : false;
                    }
                    else
                    {
                        guardado = db.Execute("INSERT INTO LINEA_BASE VALUES (:id, :nombre, :proyectoid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":tipoLineaBase, :sobreescribir)", lineaBase);

                        ret = existe > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("26", "LineaBaseDAO.class", e);
            }
            return ret;
        }
	
	/*public static List<LineaBase> getLineasBaseByIdProyectoTipo(Integer proyectoid,Integer tipoLineaBase){
		List<LineaBase> ret = new ArrayList<LineaBase>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<LineaBase> criteria = session.createQuery("FROM LineaBase lb where lb.proyecto.id=:proyectoid and lb.tipoLineaBase = :tipoLinea", LineaBase.class);
			criteria.setParameter("proyectoid", proyectoid);
			criteria.setParameter("tipoLinea", tipoLineaBase);
			ret = criteria.getResultList();
		}catch(Throwable e){
			CLogger.write("1", LineaBaseDAO.class, e);
		}finally{
			session.close();
		}
		return ret;
	}
         
         
         */
    }
}
