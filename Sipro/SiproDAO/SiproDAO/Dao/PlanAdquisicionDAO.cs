﻿using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
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

        /*public static List<PlanAdquisicion> getPlanAdquisicionesByObjeto(int objetoTipo, int ObjetoId){
            List<PlanAdquisicion> retList = null;
            Session session = CHibernateSession.getSessionFactory().openSession();

            try{
                String query = "FROM PlanAdquisicion pa where pa.objetoId=:objetoId and pa.objetoTipo=:objetoTipo and pa.estado=1";
                Query<PlanAdquisicion> criteria = session.createQuery(query, PlanAdquisicion.class);
                criteria.setParameter("objetoId", ObjetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                retList = criteria.getResultList();

            }catch(Throwable e){
                CLogger.write("4", PlanAdquisicionDAO.class, e);
            }
            finally{
                session.close();
                retList = (retList.size()>0) ? retList : null;
            }
            return retList;
        }

        public static boolean borrarPlan(PlanAdquisicion plan){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                session.beginTransaction();
                session.delete(plan);
                session.getTransaction().commit();
                ret = true;
            }catch(Throwable e){
                CLogger.write("5", PlanAdquisicionDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean borrarTodosPlan(Integer objetoId, Integer objetoTipo){
            boolean ret = false;
            if(objetoId!=null){
                List<PlanAdquisicion> planes = getPlanAdquisicionesByObjeto(objetoTipo, objetoId);
                Session session = CHibernateSession.getSessionFactory().openSession();
                try{
                    session.beginTransaction();
                    if(planes!=null){
                        for(int i=0; i<planes.size();i++){
                            planes.get(i).setEstado(0);
                            session.saveOrUpdate(planes.get(i));
                        }
                    }
                    session.getTransaction().commit();
                    ret = true;
                }catch(Throwable e){
                    CLogger.write("6", PlanAdquisicionDAO.class, e);
                }
                finally{
                    session.close();
                }
            }
            return ret;
        }

        public static List<PlanAdquisicion> getAdquisicionesNotIn(Integer objetoId, Integer objetoTipo,List<Integer> adquisiciones){
            List<PlanAdquisicion> ret = new ArrayList<PlanAdquisicion>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT pa FROM PlanAdquisicion as pa "
                        + "WHERE pa.estado = 1 "
                        + "and pa.objetoId = :objid "
                        + "and pa.objetoTipo = :objetoTipo "
                        + "and pa.id NOT IN (:ids)";

                Query<PlanAdquisicion> criteria = session.createQuery(query,PlanAdquisicion.class);
                criteria.setParameter("objid", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                criteria.setParameterList("ids", adquisiciones);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("7", PlanAdquisicionDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static void actualizarCostoPlanificadoObjeto(PlanAdquisicion pa,Session session){
            BigDecimal ret = new BigDecimal(0);
            Integer objetoId = pa.getObjetoId();
            Integer objetoTipo = pa.getObjetoTipo();
            List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(objetoId, objetoTipo);
            for(Actividad actividad: actividades)
                ret = ret.add(actividad.getCosto());
            switch(objetoTipo){
                case 0: Proyecto proyecto = ProyectoDAO.getProyecto(objetoId);
                    if(proyecto.getComponentes()==null || proyecto.getComponentes().size()==0){
                        if(actividades==null || actividades.size()==0){
                            if(pa.getPlanAdquisicionPagos()!=null && pa.getPlanAdquisicionPagos().size()>0){
                                Iterator<PlanAdquisicionPago> iPagos = pa.getPlanAdquisicionPagos().iterator();
                                while(iPagos.hasNext()){
                                    PlanAdquisicionPago pago = iPagos.next();
                                    ret=ret.add(pago.getPago()!=null ? pago.getPago() : new BigDecimal(0));
                                }
                            }
                            else
                                ret=pa.getTotal();
                        }
                    }
                    else{
                        for(Componente componente:proyecto.getComponentes())
                            ret = ret.add(componente.getCosto()!=null ? componente.getCosto() : new BigDecimal(0));
                    }
                    proyecto.setCosto(ret);
                    session.saveOrUpdate(proyecto);
                    break;
                case 1: Componente componente = ComponenteDAO.getComponente(objetoId);
                    if(componente.getProductos()==null || componente.getProductos().size()==0 || componente.getSubcomponentes()==null || componente.getSubcomponentes().size()==0 ){
                        if(actividades==null || actividades.size()==0){
                            if(pa.getPlanAdquisicionPagos()!=null && pa.getPlanAdquisicionPagos().size()>0){
                                Iterator<PlanAdquisicionPago> iPagos = pa.getPlanAdquisicionPagos().iterator();
                                while(iPagos.hasNext()){
                                    PlanAdquisicionPago pago = iPagos.next();
                                    ret=ret.add(pago.getPago()!=null ? pago.getPago() : new BigDecimal(0));
                                }
                            }
                            else
                                ret=pa.getTotal();
                        }
                    }
                    else{
                        if(componente.getProductos()!=null)
                            for(Producto producto:componente.getProductos())
                                ret = ret.add(producto.getCosto()!=null ? producto.getCosto() : new BigDecimal(0));
                        if(componente.getSubcomponentes()!=null)
                            for(Subcomponente subcomponente:componente.getSubcomponentes())
                                ret = ret.add(subcomponente.getCosto()!=null ? subcomponente.getCosto() : new BigDecimal(0));
                    }
                    componente.setCosto(ret);
                    session.saveOrUpdate(componente);
                    break;
                case 3: Producto producto = ProductoDAO.getProductoPorId(objetoId);
                    if(producto.getSubproductos()==null || producto.getSubproductos().size()==0){
                        if(actividades==null || actividades.size()==0){
                            if(pa.getPlanAdquisicionPagos()!=null && pa.getPlanAdquisicionPagos().size()>0){
                                Iterator<PlanAdquisicionPago> iPagos = pa.getPlanAdquisicionPagos().iterator();
                                while(iPagos.hasNext()){
                                    PlanAdquisicionPago pago = iPagos.next();
                                    ret=ret.add(pago.getPago()!=null ? pago.getPago() : new BigDecimal(0));
                                }
                            }
                            else
                                ret= pa.getTotal();
                        }
                    }
                    else{
                        for(Subproducto subproducto:producto.getSubproductos())
                            ret = ret.add(subproducto.getCosto()!=null ? subproducto.getCosto() : new BigDecimal(0));
                    }
                    producto.setCosto(ret);
                    session.saveOrUpdate(producto);
                    break;
                case 4:
                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId);
                    if(actividades!=null && actividades.size()>0){
                        subproducto.setCosto(ret);
                        session.saveOrUpdate(subproducto);
                    }
                    break;
                case 5:
                    Actividad actividad = ActividadDAO.getActividadPorId(objetoId);
                    if(actividades!=null && actividades.size()>0){
                        actividad.setCosto(ret);
                        session.saveOrUpdate(actividad);
                    }
                    break;
            }
        }

        public static List<PlanAdquisicionPago> getPagos(Integer planadquisicionId){
            List<PlanAdquisicionPago> ret = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                Query<PlanAdquisicionPago> criteria = session.createQuery("FROM PlanAdquisicionPago pap where pap.planAdquisicion.id=:planadquisicionId order by pap.fechaPago", PlanAdquisicionPago.class);
                criteria.setParameter("planadquisicionId", planadquisicionId);
                ret = criteria.getResultList();
            }catch(Exception e){
                CLogger.write("8", PlanAdquisicionDAO.class, e);
            }finally {
                session.close();
            }

            return ret;
        }

        public static List<?> getInfoNog(Integer nog){
            List<?> ret = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = String.join(" ", "Select * from sipro_analytic.mv_gc_adquisiciones",
                        "where nog=?1");
                Query<?> criteria = session.createNativeQuery(query);
                criteria.setParameter("1", nog);
                ret = criteria.getResultList();
            }catch(Exception e){
                CLogger.write("9", PlanAdquisicionDAO.class, e);
            }finally{
                session.close();
            }

            return ret;
        }


        public static List<PlanAdquisicion> getPlanAdquisicionesByObjetoLB(int objetoTipo, int ObjetoId, String lineaBase){
            List<PlanAdquisicion> retList = null;
            Session session = CHibernateSession.getSessionFactory().openSession();

            try{
                String query = String.join(" ", "SELECT pa.* FROM sipro_history.plan_adquisicion pa",
                        "where pa.objeto_id=:objetoId",
                        "and pa.objeto_tipo=:objetoTipo",
                        lineaBase != null ? "and pa.linea_base like '%"+lineaBase+"%'" : "and pa.actual=1",
                        "and pa.estado=1");
                Query<PlanAdquisicion> criteria = session.createNativeQuery(query, PlanAdquisicion.class);
                criteria.setParameter("objetoId", ObjetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                retList = criteria.getResultList();

            }catch(Throwable e){
                CLogger.write("4", PlanAdquisicionDAO.class, e);
            }
            finally{
                session.close();
                retList = (retList.size()>0) ? retList : null;
            }
            return retList;
        }

        public static PlanAdquisicion getPlanAdquisicionByObjetoLB(int objetoTipo, int ObjetoId, String lineaBase){
            List<PlanAdquisicion> retList = null;
            Session session = CHibernateSession.getSessionFactory().openSession();

            try{
                String query = String.join(" ", "SELECT pa.* FROM sipro_history.plan_adquisicion pa",
                        "where pa.objeto_id=:objetoId",
                        "and pa.objeto_tipo=:objetoTipo",
                        lineaBase != null ? "and pa.linea_base like '%" + lineaBase + "%'" : "and pa.actual=1",
                        "and pa.estado=1");
                Query<PlanAdquisicion> criteria = session.createNativeQuery(query, PlanAdquisicion.class);
                criteria.setParameter("objetoId", ObjetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                retList = criteria.getResultList();

            }catch(Throwable e){
                CLogger.write("3", PlanAdquisicionDAO.class, e);
            }
            finally{
                retList = (retList.size()>0) ? retList : null;
                session.close();
            }
            return retList!=null ? retList.get(0) : null;
        }

        public static String getVersiones(Integer objeto_id, Integer objeto_tipo){
            String resultado = "";
            String query = "SELECT DISTINCT(version) "
                    + " FROM sipro_history.plan_adquisicion "
                    + " WHERE objeto_id = "+objeto_id
                    + " and objeto_tipo= " +objeto_tipo;
            List<?> versiones = CHistoria.getVersiones(query);
            if(versiones!=null){
                for(int i=0; i<versiones.size(); i++){
                    if(!resultado.isEmpty()){
                        resultado+=",";
                    }
                    resultado+=(Integer)versiones.get(i);
                }
            }
            return resultado;
        }

        public static String getHistoria(Integer objeto_id, Integer objeto_tipo, Integer version){
            String resultado = "";
            String query = "SELECT pa.version, ta.nombre as tipo_adquisicion, ca.nombre as categoria_adquisicion, pa.unidad_medida, pa.cantidad, pa.precio_unitario, pa.total, "
                    + "pa.numero_contrato, pa.monto_contrato, pa.nog, " 
                    + "CASE pa.tipo_revision "
                    + "WHEN 1 THEN 'RevisiÃ³n Ex-Ante' "
                    + "WHEN 2 THEN 'RevisiÃ³n Ex-Post' "
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

            String [] campos = {"Version", "Tipo adquisicion", "CategorÃ­a adquisiciÃ³n", "Unidad Medida", "Cantidad", "Costo", "Total", 
                    "NÃºmero del Contrato", "Monto del Contrato", "NOG", "Tipo RevisiÃ³n", 
                    "PreparaciÃ³n de Documentos (Planificado)", "PreparaciÃ³n de Documentos (Real)", "Lanzamiento del Evento(Planificado)", "Lanzamiento del Evento(Real)", 
                    "RecepciÃ³n de Ofertas (Planificado)", "RecepciÃ³n de Ofertas (Real)", "AdjudicaciÃ³n (Planificado)", "AdjudicaciÃ³n (Real)", 
                    "Firma de Contrato (Planificado)", "Firma de Contrato (Real)", "Fecha CreaciÃ³n", "Usuario que creo", "Fecha ActualizaciÃ³n", "Usuario que actualizÃ³", 
                    "Estado"};
            resultado = CHistoria.getHistoria(query, campos);
            return resultado;
        }

             */

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
