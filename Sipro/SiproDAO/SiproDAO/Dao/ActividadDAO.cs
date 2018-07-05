﻿using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class ActividadDAO
    {
        /*
         class duracionFecha{
	int id;
	int duracion;
	Date fecha_inicial;
	Date fecha_final;
	String dimension;
	
	public void setId(int id){
		this.id = id;
	}
	
	public int getId(){
		return this.id;
	}
	 
	public void setDuracion(int duracion){
		this.duracion = duracion;
	}
	
	public int getDuracion(){
		return this.duracion;
	} 
	 
	public void setFechaInicial(Date fechaInicial){
		this.fecha_inicial = fechaInicial;
	}
	
	public Date getFechaInicial(){
		return this.fecha_inicial;
	}
	
	public void setFechaFin(Date fechaFin){
		this.fecha_final = fechaFin;
	}
	
	public Date getFechaFin(){
		return this.fecha_final;
	}
	
	public void setDimension(String dimension){
		this.dimension = dimension;
	}
	
	public String getDimension(){
		return this.dimension;
	}
}

        public static List<Actividad> getActividads(String usuario){
		List<Actividad> ret = new ArrayList<Actividad>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<Actividad> criteria = session.createQuery("FROM Actividad p where p.id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario )", Actividad.class);
			criteria.setParameter("usuario", usuario);
			ret = criteria.getResultList();
		}
		catch(Throwable e){
			CLogger.write("1", ActividadDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}*/

        public static Actividad getActividadPorId(int id)
        {
            Actividad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Actividad>("SELECT * FROM ACTIVIDAD WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarActividad(Actividad Actividad, bool calcular_valores_agregados)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    if (Actividad.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_actividad.nextval FROM DUAL");
                        Actividad.id = sequenceId;
                        guardado = db.Execute("INSERT INTO ACTIVIDAD VALUES (:id, :nombre, :descripcion, :fechaInicio, :fechaFin, :porcentajeAvance, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :actividadTipoid, :snip, :programa, :subprograma, :proyecto, :actividad, " +
                            ":obra, :objetoId, :objetoTipo, :duracion, :duracionDimension, :predObjetoId, :predObjetoTipo, :latitud, :longitud, :costo, :acumulacionCosto, " +
                            ":renglon, :ubicacionGeografica, :orden, :treePath, :nivel, :proyectoBase, :componenteBase, :productoBase, :fechaInicioReal, :fechaFinReal, " +
                            ":inversionNueva)", Actividad);

                        if (guardado > 0)
                        {
                            switch (Actividad.objetoTipo)
                            {
                                case 0:
                                    Proyecto proyecto = ProyectoDAO.getProyecto(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = proyecto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 1:
                                    Componente componente = ComponenteDAO.getComponente(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = componente.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 2:
                                    Subcomponente subcomponente = SubComponenteDAO.getSubComponente(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = subcomponente.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 3:
                                    Producto producto = ProductoDAO.getProductoPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = producto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 4:
                                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = subproducto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 5:
                                    Actividad actividad = ActividadDAO.getActividadPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = actividad.treepath + "" + (10000000 + Actividad.id);
                                    break;
                            }
                        }
                    }

                    guardado = db.Execute("UPDATE actividad SET nombre=:nombre, descripcion=:descripcion, fecha_inicio=:fechaInicio, fecha_fin=:fechaFin, porcentaje_avance=:porcentajeAvance, " +
                        "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                        "estado=:estado, actividad_tipoid=:actividadTipoid, snip=:snip, programa=:programa, subprograma=:subprograma, proyecto=:proyecto, actividad=:actividad, " +
                        "obra=:obra, objeto_id=:objetoId, objeto_tipo=:objetoTipo, duracion=:duracion, duracion_dimension=:duracionDimension, pred_objeto_id=:predObjetoId, " +
                        "pred_objeto_tipo=:predObjetoTipo, latitud=:latitud, longitud=:longitud, costo=:costo, acumulacion_costo=:acumulacionCosto, renglon=:renglon, " +
                        "ubicacion_geografica=:ubicacionGeografica, orden=:orden, treePath=:treePath, nivel=:nivel, proyecto_base=:proyectoBase, componente_base=:componenteBase, " +
                        "producto_base=:productoBase, fecha_inicio_real=:fechaInicioReal, fecha_fin_real=:fechaFinReal, inversion_nueva=:inversionNueva WHERE id=:id", Actividad);

                    if (guardado > 0)
                    {
                        ActividadUsuario au = new ActividadUsuario();
                        au.actividads = Actividad;
                        au.actividadid = Actividad.id;
                        au.usuario = Actividad.usuarioCreo;
                        au.fechaCreacion = DateTime.Now;
                        au.usuarioCreo = Actividad.usuarioCreo;

                        int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ACTIVIDAD_USUARIO WHERE actividadid=:id AND usuario=:usuario", new { id = au.actividadid, usuario = au.usuario });

                        if (existe > 0)
                        {
                            guardado = db.Execute("UPDATE ACTIVIDAD_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                "fecha_actualizacion=:fechaActualizacion WHERE actividadid=:actividadid AND usuario=:usuario", au);
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO actividad_usuario(:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au);
                        }

                        if (guardado > 0 && !Actividad.usuarioCreo.Equals("admin"))
                        {
                            ActividadUsuario au_admin = new ActividadUsuario();
                            au_admin.actividads = Actividad;
                            au_admin.actividadid = Actividad.id;
                            au_admin.usuario = "admin";
                            au_admin.fechaCreacion = DateTime.Now;
                            au.usuarioCreo = Actividad.usuarioCreo;

                            existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ACTIVIDAD_USUARIO WHERE actividadid=:id AND usuario=:usuario", new { id = au_admin.actividadid, usuario = au_admin.usuario });

                            if (existe > 0)
                            {
                                guardado = db.Execute("UPDATE ACTIVIDAD_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE actividadid=:actividadid AND usuario=:usuario", au_admin);
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO actividad_usuario(:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au_admin);
                            }
                        }

                        if (calcular_valores_agregados)
                        {
                            ProyectoDAO.calcularCostoyFechas(Convert.ToInt32(Actividad.treepath.Substring(0, 8)) - 10000000);
                        }

                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadDAO.class", e);
            }
            return ret;
        }

        /*public static boolean eliminarActividad(Actividad Actividad){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                Actividad.setEstado(0);
                Actividad.setOrden(null);
                session.beginTransaction();
                session.update(Actividad);
                session.getTransaction().commit();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("4", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static boolean eliminarTotalActividad(Actividad Actividad){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                session.beginTransaction();
                session.delete(Actividad);
                session.getTransaction().commit();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("5", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static List<Actividad> getActividadsPagina(int pagina, int numeroActividads,
                String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion,
                String columna_ordenada, String orden_direccion, String usuario){
            List<Actividad> ret = new ArrayList<Actividad>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT c FROM Actividad c WHERE estado = 1";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " c.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(c.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                query = String.join("", query, " AND c.id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario ) ");
                query = columna_ordenada!=null && columna_ordenada.trim().length()>0 ? String.join(" ",query,"ORDER BY",columna_ordenada,orden_direccion ) : query;
                Query<Actividad> criteria = session.createQuery(query,Actividad.class);
                criteria.setParameter("usuario", usuario);
                criteria.setFirstResult(((pagina-1)*(numeroActividads)));
                criteria.setMaxResults(numeroActividads);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("6", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Long getTotalActividads(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion, String usuario){
            Long ret=0L;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT count(c.id) FROM Actividad c WHERE c.estado=1";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " c.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(c.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join("", query, " AND c.id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario ) ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                Query<Long> conteo = session.createQuery(query,Long.class);
                conteo.setParameter("usuario", usuario);
                ret = conteo.getSingleResult();
            } catch(Throwable e){
                CLogger.write("7", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }


        public static List<Actividad> getActividadsPaginaPorObjeto(int pagina, int numeroActividads, int objetoId, int objetoTipo,String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String columna_ordenada, String orden_direccion, String usuario){
            List<Actividad> ret = new ArrayList<Actividad>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT a FROM Actividad a WHERE a.estado = 1 AND a.objetoId = :objetoId AND a.objetoTipo = :objetoTipo ";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " a.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " a.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(a.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                if(usuario!=null)
                    query = String.join("", query, "AND a.estado=1 AND a.id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario )");
                query = columna_ordenada!=null && columna_ordenada.trim().length()>0 ? String.join(" ",query," ORDER BY",columna_ordenada,orden_direccion ) : query;
                Query<Actividad> criteria = session.createQuery(query,Actividad.class);
                criteria.setParameter("objetoId", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                if (usuario!=null){
                    criteria.setParameter("usuario", usuario);
                }
                criteria.setFirstResult(((pagina-1)*(numeroActividads)));
                criteria.setMaxResults(numeroActividads);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("8", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Long getTotalActividadsPorObjeto(int objetoId, int objetoTipo, String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String usuario){
            Long ret=0L;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT count(a.id) FROM Actividad a WHERE a.estado=1 and a.objetoId=:objetoId and a.objetoTipo=:objetoTipo ";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " a.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " a.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(a.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                if(usuario!=null)
                    query = String.join("", query, " AND a.id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario )");
                Query<Long> criteria = session.createQuery(query,Long.class);
                criteria.setParameter("objetoId", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                criteria.setParameter("usuario", usuario);
                ret = criteria.getSingleResult();
            }catch(Throwable e){
                CLogger.write("9", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static String getFechaInicioFin(Actividad actividad, String usuario){
            List<duracionFecha> objetoActividadFechas = new ArrayList<duracionFecha>();
            String fecha = "";

            duracionFecha df = new duracionFecha();
            df.setId(actividad.getId());
            df.setDimension(actividad.getDuracionDimension());
            df.setDuracion(actividad.getDuracion());
            objetoActividadFechas.add(df);
            if(actividad.getPredObjetoId() != null && actividad.getPredObjetoId() != actividad.getId()){
                objetoActividadFechas = getPredecesora(getActividadPorId(actividad.getPredObjetoId()),usuario, objetoActividadFechas);

                Date fechaFI = null;
                for(int i = objetoActividadFechas.size()-1; i >= 0; i--){
                    if(fechaFI != null)
                        objetoActividadFechas.get(i).setFechaInicial(getFechaFinal(fechaFI,1,objetoActividadFechas.get(i).getDimension().charAt(0)));
                    Date fechaI = objetoActividadFechas.get(i).fecha_inicial;
                    objetoActividadFechas.get(i).setFechaFin(getFechaFinal(fechaI,objetoActividadFechas.get(i).getDuracion(),objetoActividadFechas.get(i).getDimension().charAt(0)));
                    fechaFI = objetoActividadFechas.get(i).getFechaFin();
                }

                fecha = Utils.formatDate(objetoActividadFechas.get(0).getFechaInicial()) + ";" + Utils.formatDate(objetoActividadFechas.get(0).getFechaFin()); 
            }else{
                fecha = Utils.formatDate(actividad.getFechaInicio()) + ";" + Utils.formatDate(getFechaFinal(actividad.getFechaInicio(),actividad.getDuracion(),actividad.getDuracionDimension().charAt(0))); 
            }

            return fecha;
        }

        private static List<duracionFecha> getPredecesora(Actividad actividad, String usuario, List<duracionFecha> fechasPredecesoras){
            duracionFecha df = new duracionFecha();
            df.setId(actividad.getId());
            df.setDimension(actividad.getDuracionDimension());
            df.setDuracion(actividad.getDuracion());
            fechasPredecesoras.add(df);

            if(actividad.getPredObjetoId() != null && actividad.getPredObjetoId() != actividad.getId()){
                fechasPredecesoras = getPredecesora(getActividadPorId(actividad.getPredObjetoId()),usuario, fechasPredecesoras);
            }else{
                fechasPredecesoras.get(fechasPredecesoras.size()-1).setFechaInicial(actividad.getFechaInicio());
            }

            return fechasPredecesoras;
        }

        public static List<Actividad> getActividadsSubactividadsPorObjeto(int objetoId, int objetoTipo){
            List<Actividad> ret = new ArrayList<Actividad>();
            List<Actividad> subactividades = new ArrayList<Actividad>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "SELECT a FROM Actividad a WHERE a.estado = 1 AND a.objetoId = :objetoId AND a.objetoTipo = :objetoTipo ";
                Query<Actividad> criteria = session.createQuery(query,Actividad.class);
                criteria.setParameter("objetoId", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                ret = criteria.getResultList();
                for(Actividad actividad : ret){
                    subactividades.addAll(getActividadsSubactividadsPorObjeto(actividad.getId(), 5));
                }
                ret.addAll(subactividades);			
            }
            catch(Throwable e){
                CLogger.write("10", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Actividad getFechasActividad(Actividad actividad){
            int predecesor = (actividad.getPredObjetoId()!= null)?actividad.getPredObjetoId():0; 
            if( predecesor > 0){
                Actividad actividad_pred = getActividadPorId(predecesor);
                actividad_pred = getFechasActividad(actividad_pred);
                Calendar fecha_final_pred = Calendar.getInstance();
                fecha_final_pred.setTime(actividad_pred.getFechaFin());
                fecha_final_pred.add(Calendar.DAY_OF_MONTH, 1);
                actividad.setFechaInicio(fecha_final_pred.getTime());
            }

            Date fechaInicio = actividad.getFechaInicio();
            if (fechaInicio != null && actividad.getDuracionDimension() != null && !actividad.getDuracionDimension().isEmpty()){
                Date fechaFinal = getFechaFinal(fechaInicio, actividad.getDuracion(), actividad.getDuracionDimension().charAt(0));
                actividad.setFechaFin(fechaFinal);
            }

            return actividad;
        }

        public static Date getFechaFinal(Date fecha_inicio, int duracion, char dimension){
            Calendar fecha_final = Calendar.getInstance();
            fecha_final.setTime(fecha_inicio);

            if (Character.toUpperCase(dimension) == 'D'){ //Restamos un dÃ­a para validar que la fecha de inicio sea dÃ­a hÃ¡bil
                fecha_final.add(Calendar.DAY_OF_MONTH, -1);
            }
            Integer contador=0;
            while (contador < duracion){
                switch(Character.toUpperCase(dimension)){
                    case 'D':  //dÃ­a
                        fecha_final.add(Calendar.DAY_OF_MONTH, 1);
                        break;
                    default: 
                }
                boolean esFechaHabil = esFechaHabil(fecha_final); 
                if (esFechaHabil) {
                    contador++;
                }
            }
            return new Date(fecha_final.getTimeInMillis());
        }

        public static boolean esFechaHabil(Calendar fecha) {
              switch (fecha.get(Calendar.DAY_OF_WEEK)){
                case Calendar.SUNDAY:
                    return false; 
                case Calendar.SATURDAY:
                    return false; 
                default:
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 1 && fecha.get(Calendar.MONTH) == Calendar.JANUARY) //AÃ±o nuevo
                      return false;
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 1 && fecha.get(Calendar.MONTH) == Calendar.MARCH) //DÃ­a del Trabajo
                      return false;
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 15 && fecha.get(Calendar.MONTH) == Calendar.SEPTEMBER) //Independencia
                      return false;
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 20 && fecha.get(Calendar.MONTH) == Calendar.OCTOBER) //RevoluciÃ³n
                      return false;
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 1 && fecha.get(Calendar.MONTH) == Calendar.NOVEMBER) //Todos los Santos
                      return false;
                  if (fecha.get(Calendar.DAY_OF_MONTH) == 25 && fecha.get(Calendar.MONTH) == Calendar.DECEMBER) //Navidad
                      return false;
                  break;
                }
                return true;
        }

        public static List<Actividad> getActividadsPorObjetos(Integer idPrestamo,int anio_inicio, int anio_fin, String lineaBase){
            List<Actividad> ret = new ArrayList<Actividad>();

            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = String.join(" ", "select a.* ", 
                                "from sipro_history.asignacion_raci ar, sipro_history.actividad a ",
                                "Where ar.objeto_id = a.id",
                                "and ar.objeto_tipo = 5",
                                "and ar.rol_raci = 'r'",
                                "and ar.estado=1",
                                "and a.treePath like '"+(10000000+idPrestamo)+"%'",
                                lineaBase != null ? "and ar.linea_base like '%"+lineaBase +"%'": "and ar.actual=1",
                                lineaBase != null ? "and a.linea_base like '%"+lineaBase +"%'": "and a.actual=1",
                                "and year(a.fecha_fin ) between ?1 and ?2");
                Query<Actividad> criteria = session.createNativeQuery(query,Actividad.class);
                criteria.setParameter("1", anio_inicio);
                criteria.setParameter("2", anio_fin);
                ret = criteria.getResultList();			
            }
            catch(Throwable e){
                CLogger.write("11", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static List<?> getActividadesTerminadas(Integer proyectoId, Integer colaboradorid, int anio_inicio, int anio_fin, String lineaBase){
            List<?> ret= null;

            Session session = CHibernateSession.getSessionFactory().openSession();
            try{

                String query = String.join(" ", "select year(t1.fecha_fin) anio, month(t1.fecha_fin) mes, count(t1.id) total from(",
                        "select distinct a.*", 
                        "from sipro_history.actividad a, sipro_history.asignacion_raci ar",
                        "where a.treePath like '"+(10000000+proyectoId)+"%'", 
                        "and a.porcentaje_avance=100",
                        "and year(a.fecha_fin) between ?2 and ?3",
                        (colaboradorid != null && colaboradorid > 0 ?  "and ar.colaboradorid = ?1" + (lineaBase != null ? " and ar.linea_base like '%" + lineaBase + "%'" : " and ar.actual=1") : ""),
                        "and a.id = ar.objeto_id",
                        "and a.objeto_tipo = ar.objeto_tipo",
                        "and a.estado = 1",
                        lineaBase != null ? "and a.linea_base like '%" + lineaBase + "%'" : "and a.actual=1",
                        ") t1",
                        "group by year(t1.fecha_fin), month(t1.fecha_fin) asc");

                Query<?> criteria = session.createNativeQuery(query);
                if (colaboradorid != null && colaboradorid > 0)
                    criteria.setParameter("1", colaboradorid);
                criteria.setParameter("2", anio_inicio);
                criteria.setParameter("3", anio_fin);
                ret = criteria.getResultList();

            }
            catch(Throwable e){
                CLogger.write("12", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Actividad getActividadPorIdResponsable(int id, String responsables,String rol,String lineaBase){
            Session session = CHibernateSession.getSessionFactory().openSession();
            Actividad ret = null;
            List<Actividad> listRet = null;
            try{
                String query = String.join(" ", "select a.*",
                    "from sipro_history.actividad a,sipro_history.asignacion_raci ar",
                    "where a.id = ar.objeto_id ",
                    "and ar.objeto_tipo = 5",
                    "and a.estado = 1",
                    "and a.id = ?1",
                    "and ar.colaboradorid in (",responsables ,")",
                    "and ar.rol_raci = ?3",
                    lineaBase != null ? "and a.linea_base like '%" + lineaBase + "%'" : "and a.actual = 1",
                    lineaBase != null ? "and ar.linea_base like '%" + lineaBase + "%'" : "and ar.actual = 1"

                    );
                Query<Actividad> criteria = session.createNativeQuery(query, Actividad.class);
                criteria.setParameter("1", id);
                criteria.setParameter("3", rol);

                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("13", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }


        public static Actividad getActividadInicial(Integer objetoId, Integer objetoTipo, String usuario){
            Actividad ret = null;
            List<Actividad> listRet = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "FROM Actividad a where a.estado=1 and a.orden=1 and a.objetoId=:objetoId and a.objetoTipo=:objetoTipo and a.usuarioCreo=:usuario";
                Query<Actividad> criteria = session.createQuery(query, Actividad.class);
                criteria.setParameter("objetoId", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                criteria.setParameter("usuario", usuario);
                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("15", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Actividad getActividadFechaMaxima(Integer objetoId, Integer objetoTipo, String usuario){
            Actividad ret = null;
            List<Actividad> listRet = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = "FROM Actividad a where a.estado=1 and a.objetoId=:objetoId and a.objetoTipo=:objetoTipo and a.usuarioCreo=:usuario order by a.fechaFin desc";
                Query<Actividad> criteria = session.createQuery(query, Actividad.class);
                criteria.setMaxResults(1);
                criteria.setParameter("objetoId", objetoId);
                criteria.setParameter("objetoTipo", objetoTipo);
                criteria.setParameter("usuario", usuario);

                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;

            } catch(Throwable e){
                CLogger.write("16", ActividadDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Actividad getActividadPorIdOrden(int id, String usuario, Session session){
            Actividad ret = null;
            List<Actividad> listRet = null;
            try{
                String query = "FROM Actividad where id=:id";
                if (usuario != null){
                    query += " AND id in (SELECT u.id.actividadid from ActividadUsuario u where u.id.usuario=:usuario )";
                }
                Query<Actividad> criteria = session.createQuery(query, Actividad.class);
                criteria.setParameter("id", id);
                if(usuario != null){
                criteria.setParameter("usuario", usuario);
                }

                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;

            } catch(Throwable e){
                CLogger.write("17", ActividadDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static boolean guardarActividadOrden(Actividad Actividad, Session session){
            boolean ret = false;
            try{
                session.saveOrUpdate(Actividad);
                session.flush();
                session.clear();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("18", ActividadDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }*/

        public static List<Actividad> getActividadesPorObjeto(int objetoId, int objetoTipo)
        {
            List<Actividad> ret = new List<Actividad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Actividad>("SELECT * FROM ACTIVIDAD WHERE estado=1 AND objeto_id=:objetoId AND objeto_tipo=:objetoTipo",
                        new { objetoId = objetoId, objetoTipo = objetoTipo }).AsList<Actividad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "ActividadDAO.class", e);
            }
            return ret;
        }
		
	/*public static BigDecimal calcularActividadCosto(Actividad actividad){
		BigDecimal costo = new BigDecimal(0);
		List<Actividad> subactividades = getActividadesPorObjeto(actividad.getId(), 5);
		if(subactividades!=null && subactividades.size()>0){
			Iterator<Actividad> actual = subactividades.iterator();
			while (actual.hasNext()) {
				Actividad hija = actual.next();
				BigDecimal costoHija = calcularActividadCosto(hija);
				costo = costo.add(costoHija!=null ? costoHija : new BigDecimal(0));
			}
		}else{
			PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(5, actividad.getId());
			if(pa!=null){
					if(pa.getPlanAdquisicionPagos()!=null && pa.getPlanAdquisicionPagos().size()>0){
						BigDecimal pagos = new BigDecimal(0);
						for(PlanAdquisicionPago pago: pa.getPlanAdquisicionPagos())
							pagos=pagos.add(pago.getPago());
						costo = pagos;
					}
					else
						costo = pa.getTotal();
			}
			else
				costo = actividad.getCosto();
			costo = costo!=null ? costo : new BigDecimal(0);
		}
		return costo;
	}
	
	public static List<Actividad> obtenerActividadesHijas(Integer proyectoId){
		List<Actividad> ret = new ArrayList<Actividad>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			String query = "SELECT * FROM actividad a where a.id IN (" +
					"select min(a1.id) "+
					"from actividad a1 " +
					"where a1.treePath like '"+(10000000+proyectoId)+"%' "+
					"and not exists (select * from actividad a2 where a2.objeto_id=a1.id and a2.objeto_tipo=5) " +
					"group by left(a1.treePath,length(a1.treePath)-8) "
					+ ")";
			Query<Actividad> criteria = session.createNativeQuery(query,Actividad.class);
			ret = criteria.getResultList();
		}
		catch(Throwable e){
			CLogger.write("20", ActividadDAO.class, e);
		}
		finally{
			session.close();
		}
		
		return ret;
	}
	
	public static boolean calcularCostoyFechas(Integer actividadId){
		boolean ret = false;
		ArrayList<ArrayList<Nodo>> listas = EstructuraProyectoDAO.getEstructuraObjetoArbolCalculos(actividadId, 5);
		for(int i=listas.size()-2; i>=0; i--){
			for(int j=0; j<listas.get(i).size(); j++){
				Nodo nodo = listas.get(i).get(j);
				Double costo=0.0d;
				Timestamp fecha_maxima=new Timestamp(0);
				Timestamp fecha_minima=new Timestamp((new DateTime(2999,12,31,0,0,0)).getMillis());
				for(Nodo nodo_hijo:nodo.children){
					costo += nodo_hijo.costo;
					fecha_minima = (nodo_hijo.fecha_inicio.getTime()<fecha_minima.getTime()) ? nodo_hijo.fecha_inicio : fecha_minima;
					fecha_maxima = (nodo_hijo.fecha_fin.getTime()>fecha_maxima.getTime()) ? nodo_hijo.fecha_fin : fecha_maxima;
				}
				nodo.objeto = ObjetoDAO.getObjetoPorIdyTipo(nodo.id, nodo.objeto_tipo);
				if(nodo.children!=null && nodo.children.size()>0){
					nodo.fecha_inicio = fecha_minima;
					nodo.fecha_fin = fecha_maxima;
					nodo.costo = costo;
				}
				else{
					nodo.costo = calcularActividadCosto((Actividad)nodo.objeto).doubleValue();
				}
				nodo.duracion = Utils.getWorkingDays(new DateTime(nodo.fecha_inicio), new DateTime(nodo.fecha_fin));
				setDatosCalculados(nodo.objeto,nodo.fecha_inicio,nodo.fecha_fin,nodo.costo, nodo.duracion);
			}
			ret = true;
		}
		ret= ret && guardarActividadBatch(listas);	
		return ret;
	}
	
	private static void setDatosCalculados(Object objeto,Timestamp fecha_inicio, Timestamp fecha_fin, Double costo, int duracion){
		try{
			if(objeto!=null){
				Method setFechaInicio =objeto.getClass().getMethod("setFechaInicio",Date.class);
				Method setFechaFin =  objeto.getClass().getMethod("setFechaFin",Date.class);
				Method setCosto = objeto.getClass().getMethod("setCosto",BigDecimal.class);
				Method setDuracion = objeto.getClass().getMethod("setDuracion", int.class);
				setFechaInicio.invoke(objeto, new Date(fecha_inicio.getTime()));
				setFechaFin.invoke(objeto, new Date(fecha_fin.getTime()));
				setCosto.invoke(objeto, new BigDecimal(costo));
				setDuracion.invoke(objeto, duracion);
			}
		}
		catch(Throwable e){
			CLogger.write("17", ActividadDAO.class, e);
		}
		
	}
	
	private static boolean guardarActividadBatch(ArrayList<ArrayList<Nodo>> listas){
		boolean ret = true;
		try{
			Session session = CHibernateSession.getSessionFactory().openSession();
			session.beginTransaction();
			int count=0;
			for(int i=0; i<listas.size()-1; i++){
				for(int j=0; j<listas.get(i).size();j++){
					session.saveOrUpdate(listas.get(i).get(j).objeto);
					if ( ++count % 20 == 0 ) {
				        session.flush();
				        session.clear();
				    }
				}
			}
			session.flush();
			session.getTransaction().commit();
			session.close();
		}
		catch(Throwable e){
			ret = false;
			CLogger.write("18", ActividadDAO.class, e);
		}
		return ret;
	}
	
	public static List<Actividad> getActividadsPaginaPorObjetoHistory( int objetoId, int objetoTipo,String lineaBase){
		List<Actividad> ret = new ArrayList<Actividad>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			String query = String.join(" ","select * from sipro_history.actividad a ",
					"where a.estado = 1",
					"and a.objeto_tipo = ?1",
					"and a.objeto_id = ?2",
					lineaBase != null ? "and a.linea_base like '%" + lineaBase + "%'" : "and a.actual = 1");
			Query<Actividad> criteria = session.createNativeQuery(query,Actividad.class);
			criteria.setParameter(1, objetoTipo);
			criteria.setParameter(2, objetoId);
			ret = criteria.getResultList();
		}
		catch(Throwable e){
			CLogger.write("19", ActividadDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}
	
	public static Actividad getActividadHistory(Integer actividadId,String lineaBase){
		Actividad ret = null;
		List<Actividad> listRet = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			String query = String.join(" ", "select * ", 
					"from sipro_history.actividad a ",
					"where a.estado = 1 ",
					"and a.id = ?1 ",
					lineaBase != null ? "and a.linea_base like '%" + lineaBase + "%'" : "and a.actual = 1",
							"order by a.id desc");
			Query<Actividad> criteria = session.createNativeQuery(query, Actividad.class);
			criteria.setParameter(1, actividadId);
			listRet =   criteria.getResultList();
			ret = !listRet.isEmpty() ? listRet.get(0) : null;
		}
		catch(Throwable e){
			CLogger.write("20", ActividadDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}
	
	public static String getVersiones (Integer id){
		String resultado = "";
		String query = "SELECT DISTINCT(version) "
				+ " FROM sipro_history.actividad "
				+ " WHERE id = "+id;
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
	
	public static String getHistoria (Integer id, Integer version){
		String resultado = "";
		String query = "SELECT a.version, a.nombre, a.descripcion, ati.nombre tipo, a.costo, ac.nombre tipo_costo, "
				+ " a.programa, a.subprograma, a.proyecto, a.actividad, a.obra, a.renglon, a.ubicacion_geografica, a.latitud, a.longitud, "
				+ " a.fecha_inicio, a.fecha_fin, a.duracion, a.fecha_inicio_real, a.fecha_fin_real, "
				+ " a.porcentaje_avance, "
				+ " a.fecha_creacion, a.usuario_creo, a.fecha_actualizacion, a.usuario_actualizo, "
				+ " CASE WHEN a.estado = 1 "
				+ " THEN 'Activo' "
				+ " ELSE 'Inactivo' "
				+ " END AS estado "
				+ " FROM sipro_history.actividad a "
				+ " JOIN sipro_history.actividad_tipo ati ON a.actividad_tipoid = ati.id "
				+ " JOIN sipro_history.acumulacion_costo ac ON a.acumulacion_costo = ac.id "
				+ " WHERE a.id = "+id
				+ " AND a.version = " +version;
		
		String [] campos = {"Version", "Nombre", "DescripciÃ³n", "Tipo", "Monto Planificado", "Tipo AcumulaciÃ³n de Monto Planificado", 
				"Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "UbicaciÃ³n GeogrÃ¡fica", "Latitud", "Longitud", 
				"Fecha Inicio", "Fecha Fin", "DuraciÃ³n", "Fecha Inicio Real", "Fecha Fin Real",
				"Porcentaje de Avance", 
				"Fecha CreaciÃ³n", "Usuario que creo", "Fecha ActualizaciÃ³n", "Usuario que actualizÃ³", 
				"Estado"};
		resultado = CHistoria.getHistoria(query, campos);
		return resultado;
	}
         
         */
    }
}
