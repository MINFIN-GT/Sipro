using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;
using Sipro.Utilities;
using SiproModel.Models;
using static Sipro.Controllers.PrestamoController;

namespace Sipro.Dao
{
    public class PrestamoDAO
    {
        public static bool guardarPrestamo(Prestamo prestamo)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM PRESTAMO WHERE id=:id", new { id = prestamo.id });

                    if (existe > 0)
                    {
                        db.Query<Prestamo>("UPDATE PRESTAMO SET fecha_corte=:fechaCorte, codigo_presupuestario=:codigoPresupuestario, numero_prestamo=:numeroPrestamo, destino=:destino, " +
                            "sector_economico=:sectorEconomico, ueunidad_ejecutora=:ueunidadEjecutora, fecha_firma=:fechaFirma, autorizacion_tipoid=:autorizacionTipoid, numero_autorizacion=:numeroAutorizacion, " +
                            "fecha_autorizacion=:fechaAutorizacion, anios_plazo=:aniosPlazo, anios_gracia=:aniosGracia, fecha_fin_ejecucion=:fechaFinEjecucion, perido_ejecucion=:peridoEjecucion, " +
                            "interes_tipoid=:interesTipoid, porcentaje_interes=:porcentajeInteres, porcentaje_comision_compra=:porcentajeComisionCompra, tipo_monedaid=:tipoMonedaid, " +
                            "monto_contratado=:montoContratado, amortizado=:amortizado, por_amortizar=:porAmortizar, principal_anio=:principalAnio, intereses_anio=:interesesAnio, " +
                            "comision_compromiso_anio=:comisionCompromisoAnio, otros_gastos=:otrosGastos, principal_acumulado=:principalAcumulado, intereses_acumulados=:interesesAcumulados, " +
                            "comision_compromiso_acumulado=:comisionCompromisoAcumulado, otros_cargos_acumulados=:otrosCargosAcumulados, presupuesto_asignado_func=:presupuestoAsignadoFunc, " +
                            "presupuesto_asignado_inv=:presupuestoAsignadoInv, presupuesto_modificado_func=:presupuestoModificadoFunc, presupuesto_modificado_inv=:presupuestoModificadoInv, " +
                            "presupuesto_vigente_func=:presupuestoVigenteFunc, presupuesto_vigente_inv=:presupuestoVigenteInv, presupuesto_devengado_func=:presupuestoDevengadoFunc, " +
                            "presupuesto_devengado_inv=:presupuestoDevengadoInv, presupuesto_pagado_func=:presupuestoPagadoFunc, presupuesto_pagado_inv=:presupuestoPagadoInv, saldo_cuentas=:saldoCuentas, " +
                            "desembolsado_real=:desembolsadoReal, ejecucion_estadoid=:ejecucionEstadoid, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion, estado=:estado, proyecto_programa=:proyectoPrograma, fecha_decreto=:fechaDecreto, fecha_suscripcion=:fechaSuscripcion, " +
                            "fecha_elegibilidad_ue=:fechaElegibilidadUe, fecha_cierre_origianl_ue=:fechaCierreOrigianlUe, fecha_cierre_actual_ue=:fechaCierreActualUe, meses_prorroga_ue=:mesesProrrogaUe, " +
                            "plazo_ejecucion_ue=:plazoEjecucionUe, monto_asignado_ue=:montoAsignadoUe, desembolso_a_fecha_ue=:desembolsoAFechaUe, monto_por_desembolsar_ue=:montoPorDesembolsarUe, " +
                            "fecha_vigencia=:fechaVigencia, monto_contratado_usd=:montoContratadoUsd, monto_contratado_qtz=:montoContratadoQtz, desembolso_a_fecha_usd=:desembolsoAFechaUsd, " +
                            "monto_por_desembolsar_usd=:montoPorDesembolsarUsd, monto_asignado_ue_usd=:montoAsignadoUeUsd, monto_asignado_ue_qtz=:montoAsignadoUeQtz, desembolso_a_fecha_ue_usd=:desembolsoAFechaUeUsd, " +
                            "monto_por_desembolsar_ue_usd=:montoPorDesembolsarUeUsd, entidad=:entidad, ejercicio=:ejercicio, objetivo=objetivo, objetivo_Especifico=:objetivoEspecifico, " +
                            "porcentaje_Avance=:porcentajeAvance, cooperantecodigo=:cooperantecodigo, cooperanteejercicio=:cooperanteejercicio WHERE id=:id", prestamo);
                        ret = true;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_prestamo.nextval FROM DUAL");
                        prestamo.id = sequenceId;
                        db.Query<Prestamo>("INSERT INTO PRESTAMO VALUES (:id, :fechaCorte, :codigoPresupuestario, :numeroPrestamo, :destino, :sectorEconomico, :ueunidadEjecutora, " +
                            ":fechaFirma, :autorizacionTipoid, :numeroAutorizacion, :fechaAutorizacion, :aniosPlazo, :aniosGracia, :fechaFinEjecucion, :peridoEjecucion, :interesTipoid, " +
                            ":porcentajeInteres, :porcentajeComisionCompra, :tipoMonedaid, :montoContratado, :amortizado, :porAmortizar, :principalAnio, :interesesAnio, :comisionCompromisoAnio, " +
                            ":otrosGastos, :principalAcumulado, :interesesAcumulados, :comisionCompromisoAcumulado, :otrosCargosAcumulados, :presupuestoAsignadoFunc, :presupuestoAsignadoInv, " +
                            ":presupuestoModificadoFunc, :presupuestoModificadoInv, :presupuestoVigenteFunc, :presupuestoVigenteInv, :presupuestoDevengadoFunc, :presupuestoDevengadoInv, " +
                            ":presupuestoPagadoFunc, :presupuestoPagadoInv, :saldoCuentas, :desembolsadoReal, :ejecucionEstadoid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":estado, :proyectoPrograma, :fechaDecreto, :fechaSuscripcion, :fechaElegibilidadUe, :fechaCierreOrigianlUe, :fechaCierreActualUe, :mesesProrrogaUe, :plazoEjecucionUe, " +
                            ":montoAsignadoUe, :desembolsoAFechaUe, :montoPorDesembolsarUe, :fechaVigencia, :montoContratadoUsd, :montoContratadoQtz, :desembolsoAFechaUsd, :montoPorDesembolsarUsd, " +
                            ":montoAsignadoUeUsd, :montoAsignadoUeQtz, :desembolsoAFechaUeUsd, :montoPorDesembolsarUeUsd, :entidad, :ejercicio, :objetivo, :objetivoEspecifico, :porcentajeAvance, " +
                            ":cooperantecodigo, :cooperanteejercicio)", prestamo);

                        db.Query<PrestamoUsuario>("INSERT INTO PRESTAMO_USUARIO VALUES (:prestamoid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)",
                            new { prestamoid = prestamo.id, usuario = prestamo.usuarioCreo, usuarioCreo = prestamo.usuarioCreo, fechaCreacion = DateTime.Now, usuarioActualizo = (string)null, fechaActualizacion = (DateTime?)null });

                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PrestamoDAO", e);
            }
            return ret;
        }

        public static List<Prestamo> getPrestamosPagina(int pagina, int elementosPorPagina, String filtro_nombre, long? filtro_codigo_presupuestario, String filtro_numero_prestamo,
            String filtro_usuario_creo, String filtro_fecha_creacion, String columna_ordenada, String orden_direccion, String usuario)
        {
            List<Prestamo> ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT p.*, ue.unidad_ejecutora, e.entidad as entidadentidad " +
                        "FROM PRESTAMO p " +
                        "INNER JOIN UNIDAD_EJECUTORA ue ON ue.unidad_ejecutora=p.ueunidad_ejecutora " +
                        "INNER JOIN ENTIDAD e ON e.entidad=ue.entidadentidad AND e.entidad=p.entidad " +
                        "WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.proyectoPrograma LIKE '%", filtro_nombre, "%' ");
                    if (filtro_codigo_presupuestario != null)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.codigo_Presupuestario= :filtro_codigo_presupuestario");
                    if (filtro_numero_prestamo != null && filtro_numero_prestamo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.numero_prestamo LIKE '%", filtro_numero_prestamo, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    if (usuario != null)
                        query = String.Join("", query, " AND p.id in (SELECT u.prestamoid from Prestamo_Usuario u where u.usuario=:usuario )");

                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) :
                        String.Join(" ", query, "ORDER BY p.fecha_creacion ASC");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + elementosPorPagina + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + elementosPorPagina + ") + 1)");

                    ret = db.Query<Prestamo, UnidadEjecutora, Entidad, Prestamo>(query, 
                        (p, ue, e) => 
                        {
                            p.unidadEjecutoras = ue;
                            ue.entidads = e;
                            return p;
                        }
                        , new { filtro_codigo_presupuestario = filtro_codigo_presupuestario,
                        filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion, usuario = usuario }, 
                        splitOn : "unidad_ejecutora, entidadentidad").AsList<Prestamo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoDAO", e);
            }

            return ret;
        }

        public static long getTotalPrestamos(String filtro_nombre, long? filtro_codigo_presupuestario, String filtro_numero_prestamo, String filtro_usuario_creo,
            String filtro_fecha_creacion, String usuario)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM PRESTAMO p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.proyectoPrograma LIKE '%", filtro_nombre, "%' ");
                    if (filtro_codigo_presupuestario != null)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.codigo_presupuestario= :filtro_codigo_presupuestario");
                    if (filtro_numero_prestamo != null && filtro_numero_prestamo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.numero_prestamo LIKE '%", filtro_numero_prestamo, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    if (usuario != null)
                        query = String.Join("", query, " AND p.id in (SELECT prestamoid from Prestamo_Usuario u where u.usuario=:usuario )");

                    ret = db.ExecuteScalar<long>(query, new { filtro_codigo_presupuestario = filtro_codigo_presupuestario, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion, usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoDAO", e);
            }
            return ret;
        }

        public static bool borrarPrestamo(Prestamo prestamo, string usuario)
        {
            bool ret = false;

            try
            {
                prestamo.estado = 0;
                prestamo.usuarioActualizo = usuario;
                prestamo.fechaActualizacion = DateTime.Now;
                guardarPrestamo(prestamo);
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("4", "PrestamoDAO", e);
            }

            return ret;
        }

        public static Prestamo getPrestamoById(int idPrestamo)
        {
            Prestamo ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Prestamo, UnidadEjecutora, Entidad, Prestamo>("SELECT p.*, ue.unidad_ejecutora, e.entidad as entidadentidad " +
                        "FROM PRESTAMO p " +
                        "INNER JOIN UNIDAD_EJECUTORA ue ON ue.unidad_ejecutora=p.ueunidad_ejecutora " +
                        "INNER JOIN ENTIDAD e ON e.entidad=ue.entidadentidad AND e.entidad=p.entidad " + 
                        "WHERE id=:id", 
                        (p, ue, e) => 
                        {
                            p.unidadEjecutoras = ue;
                            ue.entidads = e;
                            return p;
                        },
                        new { id = idPrestamo }, splitOn : "unidad_ejecutora,entidadentidad").ElementAtOrDefault<Prestamo>(0);
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "PrestamoDAO", e);
            }

            return ret;
        }

        public static List<Prestamo> getPrestamos(String usuario)
        {
            List<Prestamo> ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT p.*, ue.unidad_ejecutora, e.entidad as entidadentidad " +
                        "FROM PRESTAMO p " +
                        "INNER JOIN UNIDAD_EJECUTORA ue ON ue.unidad_ejecutora=p.ueunidad_ejecutora " +
                        "INNER JOIN ENTIDAD e ON e.entidad=ue.entidadentidad AND e.entidad=p.entidad " + 
                        "WHERE p.estado=1 ";
                    if (usuario != null)
                        query += "and p.id in (SELECT u.prestamoid FROM Prestamo_Usuario u where u.usuario=:usuario ) ";

                    ret = db.Query<Prestamo, UnidadEjecutora, Entidad, Prestamo>(query,
                        (p, ue, e) => 
                        {
                            p.unidadEjecutoras = ue;
                            ue.entidads = e;
                            return p;
                        },
                        new { usuario = usuario }, splitOn: "unidad_ejecutora,entidadentidad").AsList<Prestamo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "PrestamoDAO", e);
            }

            return ret;
        }

        //Falta schema sipro_history
        #region sipro_historia
        public static Prestamo getPrestamoByIdHistory(int idPrestamo, String lineaBase)
        {
            Prestamo ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT * FROM PRESTAMO p ",
                    "WHERE p.id=:id AND p.estado = 1",
                    lineaBase != null ? "and p.linea_base like '%" + lineaBase + "%'" : "and p.actual = 1 ");

                    ret = db.QueryFirstOrDefault<Prestamo>(query, new { id = idPrestamo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "PrestamoDAO", e);
            }

            return ret;
        }

        public static bool actualizarMatriz(int prestamoId, List<stunidadejecutora> unidadesEjecutoras)//Se debe crear otro schema de sipro_history
        {
            bool actualizada = false;

            try
            {
                if (unidadesEjecutoras != null)
                {
                    int version = getVersionHistoriaMatriz(prestamoId);
                    version++;
                    using (DbConnection db = new OracleContext().getConnectionHistory())
                    {
                        for (int u = 0; u < unidadesEjecutoras.Count; u++)
                        {
                            stunidadejecutora unidadEjecutora = unidadesEjecutoras[u];
                            String query = "INSERT INTO componente_matriz "
                                    + "(unidad_ejecutoraid, componente_sigadeid, prestamoid, entidadid, ejercicio, "
                                    + " fuente_prestamo, fuente_donacion, fuente_nacional, techo, version, fecha_actualizacion) "
                                    + " VALUES "
                                    + "(" + unidadEjecutora.id
                                    + ", " + unidadEjecutora.componenteSigadeId
                                    + ", " + prestamoId
                                    + ", " + unidadEjecutora.entidadId
                                    + ", " + unidadEjecutora.ejercicio
                                    + ", " + (unidadEjecutora.prestamo != 0 ? (unidadEjecutora.prestamo).ToString() : "0")
                                    + ", " + (unidadEjecutora.donacion != 0 ? (unidadEjecutora.donacion).ToString() : "0")
                                    + ", " + (unidadEjecutora.nacional != 0 ? (unidadEjecutora.nacional).ToString() : "0")
                                    + ", " + (unidadEjecutora.techo != 0 ? unidadEjecutora.techo.ToString() : "0")
                                    + ", " + version
                                    + ", '" + DateTime.Now.ToString("dd/MM/yyyy") + "') ";

                            db.Query(query);
                        }
                        actualizada = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "PrestamoDAO", e);
            }
            return actualizada;
        }

        public static int getVersionHistoriaMatriz(int prestamoId) {//Se debe crear otro schema de sipro_history
            int version = 0;

            try {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT NVL(MAX(version),0) version "
                    + " FROM componente_matriz "
                    + " WHERE prestamoid = " + prestamoId;

                    version = db.ExecuteScalar<int>(query);
                }
            }
            catch (Exception e) {
                CLogger.write("9", "PrestamoDAO", e);
            }

            return version;
        }

        public static String getVersionesMatriz(int id)//Se debe crear otro schema de sipro_history
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT DISTINCT(version) FROM componente_matriz WHERE prestamoid=:id ";

                    List<string> versiones = db.Query<string>(query, new { id = id }).AsList<string>();

                    if (versiones != null)
                    {
                        for (int i = 0; i < versiones.Count; i++)
                        {
                            if (!resultado.Equals(""))
                            {
                                resultado += ",";
                            }
                            resultado += versiones[i];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "PrestamoDAO", e);
            }            

            return resultado;
        }

        /*
        public static decimal[] getComponenteMatrizHistoria(int prestamoId, int orden, int entidadId, int ejercicio, int unidadEjuctoraId, int version){
            decimal[] resultado = new decimal[5];
            List<?> ret = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{	
                String query = "SELECT cm.techo, cm.fuente_prestamo, cm.fuente_donacion, cm.fuente_nacional, cm.fecha_actualizacion "
                        + " FROM sipro_history.componente_matriz cm "
                        + " JOIN sipro.componente_sigade cs ON cm.componente_sigadeid = cs.id "
                        + " WHERE cm.prestamoid = " + prestamoId
                        + " AND cs.numero_componente = " + orden
                        + " AND cm.entidadid = " + entidadId
                        + " AND cm.ejercicio = " + ejercicio
                        + " AND cm.unidad_ejecutoraid = " + unidadEjuctoraId
                        + " AND cm.version = " + version;

                Query<?> criteria = session.createNativeQuery(query);
                ret = criteria.getResultList();
                if(ret!=null){
                    Object[] dato = (Object[])ret.get(0);
                    resultado[0] = dato[0]!=null?(BigDecimal)dato[0]:new BigDecimal(0);
                    resultado[1] = dato[1]!=null?(BigDecimal)dato[1]:new BigDecimal(0);
                    resultado[2] = dato[2]!=null?(BigDecimal)dato[2]:new BigDecimal(0);
                    resultado[3] = dato[3]!=null?(BigDecimal)dato[3]:new BigDecimal(0);
                    resultado[4] = dato[4]!=null?new BigDecimal(((Date)dato[4]).getTime()):new BigDecimal(0);
                }
            }
            catch(Exception e){
                CLogger.write("5", "PrestamoDAO", e);
            }
            return resultado;
        }*/
        #endregion
    }
}
