using System;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using System.Collections.Generic;

namespace SiproDAO.Dao
{
    public class ComponenteDAO
    {
        /*
         public static List<Componente> getComponentes(String usuario){
		List<Componente> ret = new ArrayList<Componente>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<Componente> criteria = session.createQuery("FROM Componente p where estado = 1 AND p.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )", Componente.class);
			criteria.setParameter("usuario", usuario);
			ret =   criteria.getResultList();
		}
		catch(Throwable e){
			CLogger.write("1", ComponenteDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}

	public static Componente getComponentePorId(int id, String usuario){
		Session session = CHibernateSession.getSessionFactory().openSession();
		Componente ret = null;
		List<Componente> listRet = null;
		try{
			String Str_query = String.join(" ", "Select c FROM Componente c",
					"where id=:id");
			String Str_usuario = "";
			if(usuario != null){
				Str_usuario = String.join(" ", "AND id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )");
			}
			
			Str_query = String.join(" ", Str_query, Str_usuario);
			Query<Componente> criteria = session.createQuery(Str_query, Componente.class);
			criteria.setParameter("id", id);
			if(usuario != null){
				criteria.setParameter("usuario", usuario);
			}
			 listRet = criteria.getResultList();
			 
			 ret = !listRet.isEmpty() ? listRet.get(0) : null;
		} catch(Throwable e){
			CLogger.write("2", ComponenteDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}*/

        public static bool guardarComponente(Componente Componente, bool calcular_valores_agregados)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    if (Componente.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_componente.nextval FROM DUAL");
                        Componente.id = sequenceId;
                        guardado = db.Execute("INSERT INTO COMPONENTE VALUES (:id, :nombre, :descripcion, :proyectoid, :componenteTipoid, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :ueunidadEjecutora, :snip, :programa, :subprograma, :proyecto, :actividad, " +
                            ":obra, :latitud, :longitud, :costo, :acumulacionCostoid, :renglon, :ubicacionGeografica, :fechaInicio, :fechaFin, :duracion, :duracionDimension, " +
                            ":orden, :treepath, :nivel, :ejercicio, :entidad, :esDeSigade, :fuentePrestamo, :fuenteDonacion, :fuenteNacional, :componenteSigadeid, :fechaInicioReal, " +
                            ":fechaFinReal, :inversionNueva)", Componente);

                        if (guardado > 0)
                        {
                            Componente.treepath = Componente.proyectos.treepath + "" + (10000000 + Componente.id);
                        }
                    }

                    guardado = db.Execute("UPDATE COMPONENTE SET nombre=:nombre, descripcion=:descripcion, proyectoid=:proyectoid, componente_tipoid=:componenteTipoid, " +
                        "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, " +
                        "ueunidad_ejecutora=:ueunidadEjecutora, snip=:snip, programa=:programa, subprograma=:subprograma, proyecto=:proyecto, actividad=:actividad, obra=:obra, " +
                        "latitud=:latitud, longitud=:longitud, costo=:costo, acumulacion_costoid=:acumulacion_costoid, renglon=:renglon, ubicacion_geografica=:ubicacionGeografica, " +
                        "fecha_inicio=:fechaInicio, fecha_fin=:fechaFin, duracion=:duracion, duracion_dimension=:duracionDimension, orden=:orden, treepath=:treepath, nivel=:nivel, " +
                        "ejercicio=:ejercicio, entidad=:entidad, es_de_sigade=:esDeSigade, fuente_prestamo=:fuentePrestamo, fuente_donacion=:fuenteDonacion, fuente_nacional=:fuenteNacional, " +
                        "componente_sigadeid=:componenteSigadeid, fecha_inicio_real=:fechaInicioReal, fecha_fin_real=:fechaFinReal, inversion_nueva=:inversionNueva WHERE id=:id", Componente);

                    if (guardado > 0)
                    {
                        Usuario usuario = UsuarioDAO.getUsuario(Componente.usuarioCreo);
                        ComponenteUsuario cu = new ComponenteUsuario();
                        cu.componenteid = Componente.id;
                        cu.usuario = Componente.usuarioCreo;
                        cu.componentes = Componente;
                        cu.usuarioCreo = usuario.usuario;

                        int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM COMPONENTE_USUARIO WHERE componenteid=:id AND usuario=:usuario", new { id = cu.componenteid, usuario = cu.usuario });

                        if (existe > 0)
                        {
                            guardado = db.Execute("UPDATE COMPONENTE_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                "fecha_actualizacion=:fechaActualizacion WHERE componenteid=:componenteid AND usuario=:usuario", cu);
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO COMPONENTE_USUARIO(:componenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", cu);
                        }

                        if (guardado > 0 && !Componente.usuarioCreo.Equals("admin"))
                        {
                            ComponenteUsuario cu_admin = new ComponenteUsuario();
                            cu_admin.componenteid = Componente.id;
                            cu_admin.usuario = "admin";
                            cu_admin.componentes = Componente;
                            cu_admin.usuarioCreo = UsuarioDAO.getUsuario("admin").usuario;

                            existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM COMPONENTE_USUARIO WHERE componenteid=:id AND usuario=:usuario", new { id = cu_admin.componenteid, usuario = cu_admin.usuario });

                            if (existe > 0)
                            {
                                guardado = db.Execute("UPDATE COMPONENTE_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE componenteid=:componenteid AND usuario=:usuario", cu_admin);
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO COMPONENTE_USUARIO VALUES (:componenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", cu_admin);
                            }

                            if (guardado > 0 && calcular_valores_agregados)
                            {
                                ret = ProyectoDAO.calcularCostoyFechas(Convert.ToInt32(Componente.treepath.Substring(0, 8)) - 10000000);
                            }

                            ret = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteDAO.class", e);
            }
            return ret;
        }

        /*public static boolean eliminarComponente(Componente Componente){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                Componente.setEstado(0);
                Componente.setOrden(null);
                session.beginTransaction();
                session.update(Componente);
                session.getTransaction().commit();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("4", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static boolean eliminarTotalComponente(Componente Componente){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                session.beginTransaction();
                session.delete(Componente);
                session.getTransaction().commit();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("5", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static List<Componente> getComponentesPagina(int pagina, int numeroComponentes, String usuario){
            List<Componente> ret = new ArrayList<Componente>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                Query<Componente> criteria = session.createQuery("SELECT c FROM Componente c WHERE estado = 1 AND c.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )",Componente.class);
                criteria.setParameter("usuario", usuario);
                criteria.setFirstResult(((pagina-1)*(numeroComponentes)));
                criteria.setMaxResults(numeroComponentes);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("6", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Long getTotalComponentes(String usuario){
            Long ret=0L;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                Query<Long> conteo = session.createQuery("SELECT count(c.id) FROM Componente c WHERE c.estado=1 AND  c.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )",Long.class);
                conteo.setParameter("usuario", usuario);
                ret = conteo.getSingleResult();
            } catch(Throwable e){
                CLogger.write("7", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static List<Componente> getComponentesPaginaPorProyecto(int pagina, int numeroComponentes, int proyectoId,
                String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion, String columna_ordenada, String orden_direccion, String usuario){

            List<Componente> ret = new ArrayList<Componente>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{

                String query = "SELECT c FROM Componente c WHERE estado = 1 AND c.proyecto.id = :proyId ";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " c.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(c.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                query =String.join("",query," AND  c.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario ) ");
                query = columna_ordenada!=null && columna_ordenada.trim().length()>0 ? String.join(" ",query,"ORDER BY",columna_ordenada,orden_direccion ) : query;
                Query<Componente> criteria = session.createQuery(query,Componente.class);
                criteria.setParameter("proyId", proyectoId);
                criteria.setParameter("usuario", usuario);
                criteria.setFirstResult(((pagina-1)*(numeroComponentes)));
                criteria.setMaxResults(numeroComponentes);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("8", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }


        public static Long getTotalComponentesPorProyecto(int proyectoId,
                String filtro_nombre,String filtro_usuario_creo,
                String filtro_fecha_creacion, String usuario){
            Long ret=0L;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{

                String query = "SELECT count(c.id) FROM Componente c WHERE c.estado=1 AND c.proyecto.id = :proyId ";
                String query_a="";
                if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
                    query_a = String.join("",query_a, " c.nombre LIKE '%",filtro_nombre,"%' ");
                if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
                if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
                    query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(c.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
                query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
                query = String.join("", query, " AND  c.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )");
                Query<Long> conteo = session.createQuery(query,Long.class);
                conteo.setParameter("proyId", proyectoId);
                conteo.setParameter("usuario", usuario);
                ret = conteo.getSingleResult();
            }catch (NoResultException e){

            }catch(Throwable e){
                CLogger.write("9", ComponenteDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static Componente getComponenteInicial(Integer proyectoId, String usuario, Session session){
            Componente ret = null;
            List<Componente> listRet = null;
            try{
                String query = "FROM Componente c where c.estado=1 and c.orden=1 and c.proyecto.id=:proyectoId and c.usuarioCreo=:usuario";
                Query<Componente> criteria = session.createQuery(query, Componente.class);
                criteria.setParameter("proyectoId", proyectoId);
                criteria.setParameter("usuario", usuario);
                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("10", ComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static Componente getComponenteFechaMaxima(Integer proyectoId, String usuario, Session session){
            Componente ret = null;
            List<Componente> listRet = null;
            try{
                String query = "FROM Componente c where c.estado=1 and c.proyecto.id=:proyectoId and c.usuarioCreo=:usuario order by c.fechaFin desc";
                Query<Componente> criteria = session.createQuery(query, Componente.class);
                criteria.setMaxResults(1);
                criteria.setParameter("proyectoId", proyectoId);
                criteria.setParameter("usuario", usuario);

                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            }catch (NoResultException e){

            } catch(Throwable e){
                CLogger.write("11", ComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static List<Componente> getComponentesOrden(Integer proyectoId, String usuario, Session session){
            List<Componente> ret = null;
            try{
                String query = String.join(" ", "SELECT c FROM Componente c where c.estado=1 and c.proyecto.id=:proyectoId");
                query = String.join(" ", query, "AND c.id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario)");
                Query<Componente> criteria = session.createQuery(query,Componente.class);
                criteria.setParameter("proyectoId", proyectoId);
                criteria.setParameter("usuario", usuario);
                ret = criteria.getResultList();
            }catch(Throwable e){
                CLogger.write("12", ComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static Componente getComponentePorIdOrden(int id, String usuario, Session session){
            Componente ret = null;
            List<Componente> listRet = null;
            try{
                Query<Componente> criteria = session.createQuery("FROM Componente where id=:id AND id in (SELECT u.id.componenteid from ComponenteUsuario u where u.id.usuario=:usuario )", Componente.class);
                criteria.setParameter("id", id);
                criteria.setParameter("usuario", usuario);
                listRet = criteria.getResultList();

                 ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("13", ComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static boolean guardarComponenteOrden(Componente Componente, Session session){
            boolean ret = false;
            try{
                session.saveOrUpdate(Componente);
                session.flush();
                session.clear();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("14", ComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }*/

        public static Componente getComponente(int id)
        {
            Componente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Componente>("SELECT * FROM COMPONENTE WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("15", "ComponenteDAO.class", e);
            }
            return ret;
        }

        /*public static BigDecimal calcularCosto(Componente componente){
            BigDecimal costo = new BigDecimal(0);
            try{
                Set<Producto> productos = componente.getProductos();
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(componente.getId(), 2);
                if((productos != null && productos.size() > 0) || (actividades!=null && actividades.size()>0) ){
                    if(productos!=null){
                        Iterator<Producto> iterador = productos.iterator();

                        while(iterador.hasNext()){
                            Producto producto = iterador.next();
                            costo = costo.add(producto.getCosto() != null ? producto.getCosto() : new BigDecimal(0));
                        }
                    }

                    if(actividades != null && actividades.size() > 0){
                        for(Actividad actividad : actividades){
                            costo = costo.add(actividad.getCosto() != null ? actividad.getCosto() : new BigDecimal(0));
                        }
                    }
                }else{
                    PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(2, componente.getId());
                    if(pa!=null){
                            if(pa.getPlanAdquisicionPagos()!=null && pa.getPlanAdquisicionPagos().size()>0){
                                BigDecimal pagos = new BigDecimal(0);
                                for(PlanAdquisicionPago pago: pa.getPlanAdquisicionPagos())
                                    pagos.add(pago.getPago());
                                costo = pagos;
                            }
                            else
                                costo = pa.getMontoContrato();
                    }
                    else
                        costo = componente.getCosto();
                }
            }catch(Exception e){
                CLogger.write("16", Proyecto.class, e);
            } 

            return costo;
        }*/

        public static List<Componente> getComponentesPorProyecto(int proyectoId)
        {
            List<Componente> ret = new List<Componente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Componente>("SELECT * FROM COMPONENTE WHERE estado=1 AND proyectoid=:proyectoId AND es_de_sigade=1 ORDER BY id asc",
                        new { proyectoId = proyectoId }).AsList<Componente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "ComponenteDAO.class", e);
            }
            return ret;
        }
	
	/*public static boolean calcularCostoyFechas(Integer componenteId){
		boolean ret = false;
		ArrayList<ArrayList<Nodo>> listas = EstructuraProyectoDAO.getEstructuraObjetoArbolCalculos(componenteId, 2);
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
				else
					nodo.costo = calcularCosto((Componente)nodo.objeto).doubleValue();
				nodo.duracion = Utils.getWorkingDays(new DateTime(nodo.fecha_inicio), new DateTime(nodo.fecha_fin));
				setDatosCalculados(nodo.objeto,nodo.fecha_inicio,nodo.fecha_fin,nodo.costo, nodo.duracion);
			}
			ret = true;
		}
		ret= ret && guardarComponenteBatch(listas);	
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
			CLogger.write("20", ComponenteDAO.class, e);
		}
		
	}
	
	private static boolean guardarComponenteBatch(ArrayList<ArrayList<Nodo>> listas){
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
			CLogger.write("21", ComponenteDAO.class, e);
		}
		return ret;
	}*/

        public static Componente obtenerComponentePorEntidad(string codigo_presupuestario, int ejercicio, int entidad, int unidadEjectora, int numeroComponente, int prestamoId)
        {
            Componente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM COMPONENTE c",
                        "INNER JOIN COMPONENTE_SIGADE cs ON cs.id = c.componente_sigadeid",
                        "INNER JOIN PROYECTO p ON p.id = c.proyectoid",
                        "INNER JOIN PRESTAMO pr ON pr.id = p.prestamoid",
                        "WHERE cs.codigo_presupuestario=:codigo_presupuestario",
                        "AND c.ejercicio=:ejercicio",
                        "AND c.entidad =:entidad",
                        "AND c.ueunidad_ejecutora=:unidadEjectora",
                        "AND cs.numero_componente=:numeroComponente",
                        "AND pr.id=:prestamoId",
                        "AND cs.estado = 1",
                        "ORDER BY c.id DESC");

                    ret = db.QueryFirstOrDefault<Componente>(query, new { codigo_presupuestario = codigo_presupuestario, ejercicio = ejercicio, entidad = entidad, unidadEjectora = unidadEjectora, numeroComponente = numeroComponente, prestamoId = prestamoId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("22", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static Componente getComponentePorProyectoYComponenteSigade(int proyectoId, int componenteSigadeId)
        {
            Componente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Componente>("SELECT * "
                    + "FROM COMPONENTE c WHERE c.proyecto.id = :proyId "
                    + "AND c.componenteSigade.id = :compSigId "
                    + "AND c.estado = 1 ", new { proyId = proyectoId, compSigId = componenteSigadeId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("23", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Componente> getComponentesPorProyectoHistory(int proyectoId, String lineaBase)
        {
            List<Componente> ret = new List<Componente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT * FROM COMPONENTE ",
                    "WHERE estado = 1 AND proyectoid=:proyectoId",
                    lineaBase != null ? "AND linea_base LIKE '%" + lineaBase + "%'" : "AND actual = 1",
                    "ORDER BY id DESC");
                    ret = db.Query<Componente>(query, new { proyectoId = proyectoId }).AsList<Componente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "ComponenteDAO.class", e);
            }
            return ret;
        }
	
	/*public static Componente getComponenteHistory(Integer componenteId,String lineaBase){
		Componente ret = null;
		List<Componente> listRet = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			String query = String.join(" ", "select * ", 
					"from sipro_history.componente c ",
					"where c.estado = 1 ",
					"and c.id = ?1 ",
					lineaBase != null ? "and c.linea_base like '%" + lineaBase + "%'" : "and c.actual = 1",
							"order by c.id desc");
			Query<Componente> criteria = session.createNativeQuery(query, Componente.class);
			criteria.setParameter(1, componenteId);
			listRet =   criteria.getResultList();
			ret = !listRet.isEmpty() ? listRet.get(0) : null;
		}
		catch(Throwable e){
			CLogger.write("20", ComponenteDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}
	
	public static String getVersiones (Integer componenteId){
		String resultado = "";
		String query = "SELECT DISTINCT(version) "
				+ " FROM sipro_history.componente "
				+ " WHERE id = "+componenteId;
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
	
	public static String getHistoria (Integer componenteId, Integer version){
		String resultado = "";
		String query = "SELECT c.version, c.nombre, c.descripcion, ct.nombre tipo, ue.nombre unidad_ejecutora, c.costo, ac.nombre tipo_costo, "
				+ " c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.renglon, c.ubicacion_geografica, c.latitud, c.longitud, "
				+ " c.fecha_inicio, c.fecha_fin, c.duracion, c.fecha_inicio_real, c.fecha_fin_real, "
				+ " c.fuente_prestamo, c.fuente_donacion, c.fuente_nacional, "
				+ " c.fecha_creacion, c.usuario_creo, c.fecha_actualizacion, c.usuario_actualizo, "
				+ " CASE WHEN c.estado = 1 "
				+ " THEN 'Activo' "
				+ " ELSE 'Inactivo' "
				+ " END AS estado "
				+ " FROM sipro_history.componente c "
				+ " JOIN sipro.unidad_ejecutora ue ON c.unidad_ejecutoraunidad_ejecutora = ue.unidad_ejecutora and c.entidad = ue.entidadentidad and c.ejercicio = ue.ejercicio  JOIN sipro_history.componente_tipo ct ON c.componente_tipoid = ct.id "
				+ " JOIN sipro_history.acumulacion_costo ac ON c.acumulacion_costoid = ac.id "
				+ " WHERE c.id = "+componenteId
				+ " AND c.version = " +version;
		
		String [] campos = {"Version", "Nombre", "DescripciÃ³n", "Tipo", "Unidad Ejecutora", "Monto Planificado", "Tipo AcumulaciÃ³n de Monto Planificado", 
				"Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "UbicaciÃ³n GeogrÃ¡fica", "Latitud", "Longitud", 
				"Fecha Inicio", "Fecha Fin", "DuraciÃ³n", "Fecha Inicio Real", "Fecha Fin Real", 
				"Fuente PrÃ©stamo", "Fuente DonaciÃ³n", "Fuente Nacional", 
				"Fecha CreaciÃ³n", "Usuario que creo", "Fecha ActualizaciÃ³n", "Usuario que actualizÃ³", 
				"Estado"};
		resultado = CHistoria.getHistoria(query, campos);
		return resultado;
	}
         
         
         */
    }
}
