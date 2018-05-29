using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using SiproModelAnalyticCore.Models;
using Newtonsoft.Json.Linq;

namespace SiproDAO.Dao
{
    public class PlanAdquisicionDAO
    {

        public static int guardarPlanAdquisicion(PlanAdquisicion planAdquisicion)
        {
            int ret = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PLAN_ADQUISICION WHERE id=:id", new { id = planAdquisicion.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE PLAN_ADQUISICION SET tipo_adquisicion=:tipoAdquisicion, categoria_adquisicion=:categoriaAdquisicion, " +
                            "unidad_medida=:unidadMedida, cantidad=:cantidad, total=:total, precio_unitario=:precioUnitario, preparacion_doc_planificado=:preparacionDocPlanificado, " +
                            "preparacion_doc_real=:preparacionDocReal, lanzamiento_evento_planificado=:lanzamientoEventoPlanificado, lanzamiento_evento_real=:lanzamientoEventoReal, " +
                            "recepcion_ofertas_planificado=:recepcionOfertasPlanificado, recepcion_ofertas_real=:recepcionOfertasReal, adjudicacion_planificado=:adjudicacionPlanificado, " +
                            "adjudicacion_real=:adjudicacionReal, firma_contrato_planificado=:firmaContratoPlanificado, firma_contrato_real=:firmaContratoReal, objeto_id=:objetoId, " +
                            "objeto_tipo=:objetoTipo, usuario_creo=:usuarioCreo, usuarioActualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado, bloqueado=:bloqueado, numero_contrato=:numeroContrato, monto_contrato=:montoContrato, nog=:nog, tipo_revision=:tipoRevision WHERE id=:id", planAdquisicion);

                        ret = guardado > 0 ? Convert.ToInt32(planAdquisicion.id) : 0;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_plan_adquisicion.nextval FROM DUAL");
                        planAdquisicion.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO PLAN_ADQUISICION VALUES (:id, :tipoAdquisicion, :categoriaAdquisicion, :unidadMedida, :cantidad, :total, " +
                            ":precioUnitario, :preparacionDocPlanificado, :preparacionDocReal, :lanzamientoEventoPlanificado, :lanzamientoEventoReal, :recepcionOfertasPlanificado, " +
                            ":recepcionOfertasReal, :adjudicacionPlanificado, :adjudicacionReal, :firmaContratoPlanificado, :firmaContratoReal, :objetoId, :objetoTipo, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :bloqueado, :numeroContrato, :montoContrato, :nog, :tipoRevision)", planAdquisicion);

                        ret = guardado > 0 ? Convert.ToInt32(planAdquisicion.id) : 0;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static PlanAdquisicion getPlanAdquisicionById(int planAdquisicionId)
        {
            PlanAdquisicion ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<PlanAdquisicion>("SELECT * FROM PLAN_ADQUISICION WHERE id=:planAdquisicionId",
                        new { planAdquisicionId = planAdquisicionId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static PlanAdquisicion getPlanAdquisicionByObjeto(int objetoTipo, int ObjetoId)
        {
            PlanAdquisicion ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<PlanAdquisicion>("SELECT * FROM PLAN_ADQUISICION pa WHERE pa.objeto_id=:objetoId AND pa.objeto_tipo=:objetoTipo AND pa.estado=1",
                        new { objetoTipo = objetoTipo, ObjetoId = ObjetoId });
                }

            }
            catch (Exception e)
            {
                CLogger.write("3", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<PlanAdquisicion> getPlanAdquisicionesByObjeto(int objetoTipo, int ObjetoId)
        {
            List<PlanAdquisicion> retList = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT * FROM PLAN_ADQUISICION pa " +
                        "WHERE pa.objetoId=:objetoId AND pa.objetoTipo=:objetoTipo AND pa.estado=1";

                    retList = db.Query<PlanAdquisicion>(query, new { objetoId = ObjetoId, objetoTipo = objetoTipo }).AsList<PlanAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "PlanAdquisicionDAO.class", e);
            }
            return retList;
        }

        public static bool borrarPlan(PlanAdquisicion plan)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM PLAN_ADQUISICION WHERE id=:id", new { id = plan.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static bool borrarTodosPlan(int objetoId, int objetoTipo)
        {
            bool ret = false;
            List<PlanAdquisicion> planes = getPlanAdquisicionesByObjeto(objetoTipo, objetoId);
            try
            {
                if (planes != null)
                {
                    for (int i = 0; i < planes.Count; i++)
                    {
                        planes[i].estado = 0;
                        guardarPlanAdquisicion(planes[i]);
                    }
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("6", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<PlanAdquisicion> getAdquisicionesNotIn(int objetoId, int objetoTipo, List<int> adquisiciones)
        {
            List<PlanAdquisicion> ret = new List<PlanAdquisicion>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT pa FROM PlanAdquisicion as pa "
                        + "WHERE pa.estado = 1 "
                        + "and pa.objetoId = :objid "
                        + "and pa.objetoTipo = :objetoTipo "
                        + "and pa.id NOT IN (:ids)";

                    ret = db.Query<PlanAdquisicion>(query, new { objid = objetoId, objetoTipo = objetoTipo, ids = adquisiciones }).AsList<PlanAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static void actualizarCostoPlanificadoObjeto(PlanAdquisicion pa)
        {
            decimal ret = decimal.Zero;
            int objetoId = Convert.ToInt32(pa.objetoId);
            int objetoTipo = Convert.ToInt32(pa.objetoTipo);
            List<PlanAdquisicionPago> pagos = PlanAdquisicionPagoDAO.getPagosByPlan(Convert.ToInt32(pa.id));
            List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(objetoId, objetoTipo);
            foreach (Actividad actividad in actividades)
                ret += actividad.costo ?? default(decimal);

            switch (objetoTipo)
            {
                case 0:
                    Proyecto proyecto = ProyectoDAO.getProyecto(objetoId);
                    List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyecto.id);
                    if (componentes == null || componentes.Count == 0)
                    {
                        if (actividades == null || actividades.Count == 0)
                        {
                            if (pagos != null && pagos.Count > 0)
                            {
                                foreach (PlanAdquisicionPago pago in pagos)
                                    ret += pago.pago ?? default(decimal);
                            }
                            else
                                ret = pa.total ?? default(decimal);
                        }
                    }
                    else
                    {
                        foreach (Componente cmp in componentes)
                            ret += cmp.costo ?? default(decimal);
                    }
                    proyecto.costo = ret;
                    ProyectoDAO.guardarProyecto(proyecto, false);
                    break;
                case 1:
                    Componente componente = ComponenteDAO.getComponente(objetoId);
                    List<Producto> productos = ProductoDAO.getProductosByComponente(componente.id);
                    List<Subcomponente> subcomponentes = SubComponenteDAO.getSubComponentesPorComponente(componente.id);
                    if (productos == null || productos.Count == 0 || subcomponentes == null || subcomponentes.Count == 0)
                    {
                        if (actividades == null || actividades.Count == 0)
                        {
                            if (pagos != null && pagos.Count > 0)
                            {
                                foreach (PlanAdquisicionPago pago in pagos)
                                    ret += pago.pago ?? default(decimal);
                            }
                            else
                                ret = pa.total ?? default(decimal);
                        }
                    }
                    else
                    {
                        if (productos != null)
                            foreach (Producto prod in productos)
                                ret += prod.costo ?? default(decimal);
                        if (subcomponentes != null)
                            foreach (Subcomponente subcomponente in subcomponentes)
                                ret += subcomponente.costo ?? default(decimal);
                    }
                    componente.costo = ret;
                    ComponenteDAO.guardarComponente(componente, false);
                    break;
                case 3:
                    Producto producto = ProductoDAO.getProductoPorId(objetoId);
                    List<Subproducto> subproductos = SubproductoDAO.getSubproductosByProductoid(producto.id);
                    if (subproductos == null || subproductos.Count == 0)
                    {
                        if (actividades == null || actividades.Count == 0)
                        {
                            if (pagos != null && pagos.Count > 0)
                            {
                                foreach (PlanAdquisicionPago pago in pagos)
                                    ret += pago.pago ?? default(decimal);
                            }
                            else
                                ret = pa.total ?? default(decimal);
                        }
                    }
                    else
                    {
                        foreach (Subproducto subprod in subproductos)
                            ret += subprod.costo ?? default(decimal);
                    }
                    producto.costo = ret;
                    ProductoDAO.guardarProducto(producto, false);
                    break;
                case 4:
                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId);
                    if (actividades != null && actividades.Count > 0)
                    {
                        subproducto.costo = ret;
                        SubproductoDAO.guardarSubproducto(subproducto, false);
                    }
                    break;
                case 5:
                    Actividad actividad = ActividadDAO.getActividadPorId(objetoId);
                    if (actividades != null && actividades.Count > 0)
                    {
                        actividad.costo = ret;
                        ActividadDAO.guardarActividad(actividad, false);
                    }
                    break;
            }
        }

        public static List<PlanAdquisicionPago> getPagos(int planadquisicionId)
        {
            List<PlanAdquisicionPago> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<PlanAdquisicionPago>("SELECT * FROM PLAN_ADQUISICION_PAGO pap WHERE pap.id=:planadquisicionId ORDER BY pap.fecha_pago",
                        new { planadquisicionId = planadquisicionId }).AsList<PlanAdquisicionPago>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<MvGcAdquisiciones> getInfoNog(int nog)
        {
            List<MvGcAdquisiciones> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT * FROM sipro_analytic.MV_GC_ADQUISICIONES",
                        "WHERE nog=:nog");

                    ret = db.Query<MvGcAdquisiciones>(query, new { nog = nog }).AsList<MvGcAdquisiciones>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<PlanAdquisicion> getPlanAdquisicionesByObjetoLB(int objetoTipo, int ObjetoId, String lineaBase)
        {
            List<PlanAdquisicion> retList = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT pa.* FROM sipro_history.PLAN_ADQUISICION pa",
                        "WHERE pa.objeto_id=:objetoId",
                        "AND pa.objeto_tipo=:objetoTipo",
                        lineaBase != null ? "AND pa.linea_base LIKE '%" + lineaBase + "%'" : "AND pa.actual=1",
                        "AND pa.estado=1");
                    retList = db.Query<PlanAdquisicion>(query, new { objetoId = ObjetoId, objetoTipo = objetoTipo }).AsList<PlanAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "PlanAdquisicionDAO.class", e);
            }
            return retList;
        }

        public static PlanAdquisicion getPlanAdquisicionByObjetoLB(int objetoTipo, int ObjetoId, String lineaBase)
        {
            PlanAdquisicion ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT pa.* FROM sipro_history.plan_adquisicion pa",
                        "WHERE pa.objeto_id=:objetoId",
                        "AND pa.objeto_tipo=:objetoTipo",
                        lineaBase != null ? "AND pa.linea_base LIKE '%" + lineaBase + "%'" : "AND pa.actual=1",
                        "AND pa.estado=1");

                    ret = db.QueryFirstOrDefault<PlanAdquisicion>(query, new { objetoId = ObjetoId, objetoTipo = objetoTipo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "PlanAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static String getVersiones(int objeto_id, int objeto_tipo)
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT DISTINCT(version) "
                        + " FROM sipro_history.PLAN_ADQUISICION "
                        + " WHERE objeto_id=:objetoId"
                        + " AND objeto_tipo=:objetoTipo";

                    List<dynamic> versiones = db.Query<dynamic>(query, new { objetoId = objeto_id, objetoTipo = objeto_tipo }).AsList<dynamic>();
                    if (versiones.Count > 0)
                    {
                        for (int i = 0; i < versiones.Count; i++)
                        {
                            if (resultado.Length != 0)
                            {
                                resultado += ",";
                            }
                            resultado += (int)versiones[i];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "PlanAdquisicionDAO.class", e);
            }
            return resultado;
        }

        public static String getHistoria(int objeto_id, int objeto_tipo, int version)
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT pa.version, ta.nombre as tipo_adquisicion, ca.nombre as categoria_adquisicion, pa.unidad_medida, pa.cantidad, pa.precio_unitario, pa.total, "
                    + "pa.numero_contrato, pa.monto_contrato, pa.nog, "
                    + "CASE pa.tipo_revision "
                    + "WHEN 1 THEN 'Revisión Ex-Ante' "
                    + "WHEN 2 THEN 'Revisión Ex-Post' "
                    + "WHEN null THEN '' "
                    + "END as tipo_revision, "
                    + "pa.preparacion_doc_planificado, pa.preparacion_doc_real, pa.lanzamiento_evento_planificado, pa.lanzamiento_evento_real, "
                    + "pa.recepcion_ofertas_planificado, pa.recepcion_ofertas_real, pa.adjudicacion_planificado, pa.adjudicacion_real, "
                    + "pa.firma_contrato_planificado, pa.firma_contrato_real, "
                    + "pa.usuario_creo, pa.usuario_actualizo, pa.fecha_creacion, pa.fecha_actualizacion, "
                    + " CASE WHEN pa.estado = 1 "
                    + " THEN 'Activo' "
                    + " ELSE 'Inactivo' "
                    + " END AS estado "
                    + "FROM sipro_history.plan_adquisicion pa, sipro.tipo_adquisicion ta, sipro.categoria_adquisicion ca "
                    + "WHERE pa.objeto_id = " + objeto_id + " AND pa.objeto_tipo= " + objeto_tipo
                    + " AND ta.id=pa.tipo_adquisicion "
                    + " AND ca.id=pa.categoria_adquisicion "
                    + " AND pa.version=" + version;

                    String[] campos = {"Version", "Tipo adquisicion", "Categoría adquisición", "Unidad Medida", "Cantidad", "Costo", "Total",
                    "Número del Contrato", "Monto del Contrato", "NOG", "Tipo Revisión",
                    "Preparación de Documentos (Planificado)", "Preparación de Documentos (Real)", "Lanzamiento del Evento(Planificado)", "Lanzamiento del Evento(Real)",
                    "Recepció de Ofertas (Planificado)", "Recepción de Ofertas (Real)", "Adjudicación (Planificado)", "Adjudicación (Real)",
                    "Firma de Contrato (Planificado)", "Firma de Contrato (Real)", "Fecha Creación", "Usuario que creo", "Fecha Actualización", "Usuario que actualizó",
                    "Estado"};

                    List<dynamic> datos = db.Query<dynamic>(query).AsList<dynamic>();

                    for (int d = 0; d < datos.Count; d++)
                    {
                        Object[] dato = (Object[])datos[d];
                        if (resultado.Length != 0)
                        {
                            resultado += ", ";
                        }
                        resultado += "[";
                        String objeto = "";
                        for (int c = 0; c < campos.Length; c++)
                        {
                            if (objeto.Length != 0)
                            {
                                objeto += ", ";
                            }
                            objeto += "{\"nombre\": \"" + campos[c] + "\", \"valor\": \"" + (dato[c] != null ? ((Object)dato[c]).ToString() : "") + "\"}";
                        }
                        resultado += objeto + "]";
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "PlanAdquisicionDAO.class", e);
            }
            return resultado;
        }

        public static bool actualizarNivelesPagos(String pagosString, PlanAdquisicion pa, string usuario, int objetoId, int objetoTipo)
        {
            bool ret = false;
            try
            {
                decimal bpagos = decimal.Zero;
                bool tiene_pagos = false;
                if (pagosString != null)
                {
                    JArray pagos = JArray.Parse(pagosString);
                    for (int j = 0; j < pagos.Count; j++)
                    {
                        JObject objeto_pago = new JObject(new JProperty("fecha", pagos[j]), new JProperty("pago", pagos[j]));
                        DateTime fechaPago = (DateTime)objeto_pago["fecha"];
                        decimal dpago = (decimal)objeto_pago["pago"];
                        PlanAdquisicionPago pago = new PlanAdquisicionPago();
                        pago.planAdquisicions = pa;
                        pago.planAdquisicionid = pa.id;
                        pago.fechaPago = fechaPago;
                        pago.pago = dpago;
                        pago.usuarioCreo = usuario;
                        pago.fechaCreacion = DateTime.Now;
                        pago.estado = 1;

                        PlanAdquisicionPagoDAO.guardarPago(pago);
                        bpagos += dpago;
                        tiene_pagos = true;
                    }
                }

                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(objetoId, objetoTipo);
                switch (objetoTipo)
                {
                    case 3:
                        Producto producto = ProductoDAO.getProductoPorId(objetoId);
                        List<Subproducto> subproductos = SubproductoDAO.getSubproductosByProductoid(producto.id);
                        if (!(subproductos != null && subproductos.Count > 0) &&
                                !(actividades != null && actividades.Count > 0))
                        {
                            if (tiene_pagos)
                                producto.costo = bpagos;
                            else
                                producto.costo = pa.total;
                            ProductoDAO.guardarProducto(producto, true);
                        }
                        break;
                    case 4:
                        Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId);
                        if (!(actividades != null && actividades.Count > 0))
                        {
                            if (tiene_pagos)
                                subproducto.costo = bpagos;
                            else
                                subproducto.costo = pa.total;
                            SubproductoDAO.guardarSubproducto(subproducto, true);
                        }
                        break;
                    case 5:
                        Actividad actividad = ActividadDAO.getActividadPorId(objetoId);
                        if (!(actividades != null && actividades.Count > 0))
                        {
                            if (tiene_pagos)
                                actividad.costo = bpagos;
                            else
                                actividad.costo = pa.total;
                            ActividadDAO.guardarActividad(actividad, true);
                        }
                        break;
                }

                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("50", "PlanAdquisicionDAO.class", e);
            }

            return ret;
        }
    }
}
