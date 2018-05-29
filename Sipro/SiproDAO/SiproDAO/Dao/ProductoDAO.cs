using System;
using System.Collections.Generic;
using System.Text;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class ProductoDAO
    {
        /*
         public static List<Producto> getProductos(String usuario) {
		List<Producto> ret = new ArrayList<Producto>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			Query<Producto> criteria = session.createQuery("FROM Producto p where p.id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )", Producto.class);
			criteria.setParameter("usuario", usuario);
			ret =   criteria.getResultList();
		} catch (Throwable e) {
			CLogger.write("1", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static Producto getProductoPorId(int id, String usuario) {
		Session session = CHibernateSession.getSessionFactory().openSession();
		Producto ret = null;
		try {
			String Str_query = String.join(" ","Select p FROM Producto p",
					"where id=:id");
			String Str_usuario = "";
			if(usuario != null){
				Str_usuario = String.join(" ", "AND id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )");
			}
			
			Str_query = String.join(" ", Str_query, Str_usuario);
			Query<Producto> criteria = session.createQuery(Str_query, Producto.class);
			criteria.setParameter("id", id);
			if(usuario != null){
				criteria.setParameter("usuario", usuario);
			}
			List<Producto> lista = criteria.getResultList();
			ret = !lista.isEmpty() ? lista.get(0) : null;
		} catch (Throwable e) {
			CLogger.write("2", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}*/

        public static bool guardarProducto(Producto producto, bool calcular_valores_agregados)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    if (producto.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_producto.nextval FROM DUAL");
                        producto.id = sequenceId;
                        guardado = db.Execute("INSERT INTO PRODUCTO VALUES (:id, :nombre, :descripcion, :componenteid, :subcomponenteid, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion, :productoTipoid, :estado, :ueunidadEjecutora, :snip, :programa, :subprograma, :proyecto, :actividad, :obra, " +
                            ":latitud, :longitud, :peso, :costo, :acumulacionCostoid, :renglon, :ubicacionGeografica, :fechaInicio, :fechaFin, :duracion, :duracionDimension, :orden, " +
                            ":treePath, :nivel, :ejercicio, :entidad, :fechaInicioReal, :fechaFinReal, :inversionNueva)", producto);


                        if (guardado > 0 && producto.componentes != null)
                        {
                            producto.treepath = producto.componentes.treepath + "" + (10000000 + producto.id);
                        }
                        else if (producto.subcomponentes != null)
                        {
                            producto.treepath = producto.subcomponentes.treepath + "" + (10000000 + producto.id);
                        }
                    }

                    guardado = db.Execute("UPDATE producto SET nombre=:nombre, descripcion=:descripcion, componenteid=:componenteid, subcomponenteid=:subcomponenteid, " +
                        "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                        "producto_tipoid=:productoTipoid, estado=:estado, ueunidad_ejecutora=:ueunidadEjecutora, snip=:snip, programa=:programa, subprograma=:subprograma, " +
                        "proyecto=:proyecto, actividad=:actividad, obra=:obra, latitud=:latitud, longitud=:longitud, peso=:peso, costo=:costo, acumulacion_costoid=:acumulacionCostoid, " +
                        "renglon=:renglon, ubicacion_geografica=:ubicacionGeografica, fecha_inicio=:fechaInicio, fecha_fin=:fechaFin, duracion=:duracion, duracion_dimension=:duracionDimension, " +
                        "orden = ?, treePath = ?, nivel = ?, ejercicio = ?, entidad = ?, fecha_inicio_real = ?, fecha_fin_real = ?, inversion_nueva = ? WHERE id = ?", producto);

                    if (guardado > 0)
                    {
                        Usuario usu = UsuarioDAO.getUsuario(producto.usuarioCreo);
                        ProductoUsuario pu = new ProductoUsuario();
                        pu.productos = producto;
                        pu.productoid = producto.id;
                        pu.usuario = usu.usuario;
                        pu.usuarioCreo = usu.usuario;
                        pu.fechaCreacion = DateTime.Now;

                        int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PRODUCTO_USUARIO WHERE productoid=:id AND usuario=:usuario", new { id = pu.productoid, usuario = pu.usuario });

                        if (existe > 0)
                        {
                            guardado = db.Execute("UPDATE PRODUCTO_USUARIO SET usuario_creo=:usuario_creo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                "fecha_actualizacion=:fechaActualizacion WHERE productoid=:productoid AND usuario=:usuario", pu);
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO PRODUCTO_USUARIO VALUES (:productoid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", pu);
                        }

                        if (guardado > 0 && !producto.usuarioCreo.Equals("admin"))
                        {
                            ProductoUsuario pu_admin = new ProductoUsuario();
                            pu_admin.productos = producto;
                            pu_admin.productoid = producto.id;
                            pu_admin.usuario = "admin";
                            pu_admin.usuarioCreo = producto.usuarioCreo;
                            pu_admin.fechaCreacion = DateTime.Now;

                            existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PRODUCTO_USUARIO WHERE productoid=:id AND usuario=:usuario", new { id = pu_admin.productoid, usuario = pu_admin.usuario });

                            if (existe > 0)
                            {
                                guardado = db.Execute("UPDATE PRODUCTO_USUARIO SET usuario_creo=:usuario_creo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE productoid=:productoid AND usuario=:usuario", pu_admin);
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO PRODUCTO_USUARIO VALUES (:productoid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", pu_admin);
                            }
                        }

                        if (calcular_valores_agregados)
                        {
                            ProyectoDAO.calcularCostoyFechas(Convert.ToInt32(producto.treepath.Substring(0, 8)) - 10000000);
                        }

                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoDAO.class", e);
            }
            return ret;
        }

	/*public static boolean eliminarProducto(Producto Producto) {
		boolean ret = false;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			Producto.setEstado(0);
			session.beginTransaction();
			session.update(Producto);
			session.getTransaction().commit();
			ret = true;
		} catch (Throwable e) {
			CLogger.write("4", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static boolean eliminarTotalProducto(Producto Producto) {
		boolean ret = false;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			session.beginTransaction();
			session.delete(Producto);
			session.getTransaction().commit();
			ret = true;
		} catch (Throwable e) {
			CLogger.write("5", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static List<Producto> getProductosPagina(int pagina, int numeroProductos,Integer componenteid, Integer subcomponenteid,
			String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion, String columna_ordenada, 
			String orden_direccion,String usuario) {
		List<Producto> ret = new ArrayList<Producto>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			
			String query = "SELECT p FROM Producto p WHERE p.estado = 1 ";
			if(componenteid!=null && componenteid > 0){
				query += "AND p.componente.id = :idComp ";
			}
			if(subcomponenteid!=null && subcomponenteid > 0){
				query += "AND p.subcomponente.id = :idSubComp ";
			}

			String query_a="";
			if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
				query_a = String.join("",query_a, " p.nombre LIKE '%",filtro_nombre,"%' ");
			if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " p.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
			if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(p.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
			query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
			if(usuario!=null)
				query = String.join("", query, " AND p.id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )");
			query = columna_ordenada!=null && columna_ordenada.trim().length()>0 ? String.join(" ",query,"ORDER BY",columna_ordenada,orden_direccion ) : query;
			
			Query<Producto> criteria = session.createQuery(query,Producto.class);
			criteria.setParameter("usuario", usuario);
			if (componenteid!=null && componenteid>0){
				criteria.setParameter("idComp", componenteid);
			}
			if (subcomponenteid!=null && subcomponenteid>0){
				criteria.setParameter("idSubComp", subcomponenteid);
			}
			criteria.setFirstResult(((pagina - 1) * (numeroProductos)));
			criteria.setMaxResults(numeroProductos);
			
			ret = criteria.getResultList();
		} catch (Throwable e) {
			CLogger.write("6", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static Long getTotalProductos(Integer componenteid, Integer subcomponenteid, String filtro_nombre, String filtro_usuario_creo, 
			String filtro_fecha_creacion, String usuario) {
		Long ret = 0L;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			
			String query = "SELECT count(p.id) FROM Producto p WHERE p.estado = 1 ";
			if(componenteid!=null && componenteid > 0){
				query += "AND p.componente.id = :idComp ";
			}
			if(subcomponenteid!=null && subcomponenteid > 0){
				query += "AND p.subcomponente.id = :idSubComp ";
			}

			String query_a="";
			if(filtro_nombre!=null && filtro_nombre.trim().length()>0)
				query_a = String.join("",query_a, " p.nombre LIKE '%",filtro_nombre,"%' ");
			if(filtro_usuario_creo!=null && filtro_usuario_creo.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " p.usuarioCreo LIKE '%", filtro_usuario_creo,"%' ");
			if(filtro_fecha_creacion!=null && filtro_fecha_creacion.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(date_format(p.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion,"%' ");
			query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
			if(usuario!=null)
				query = String.join("", query, " AND p.id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )");
			
			
			Query<Long> conteo = session.createQuery(query,Long.class);
			conteo.setParameter("usuario", usuario);
			if (componenteid!=null && componenteid > 0){
				conteo.setParameter("idComp", componenteid);
			}
			if (subcomponenteid!=null && subcomponenteid > 0){
				conteo.setParameter("idSubComp", subcomponenteid);
			}
			ret = conteo.getSingleResult();
		} catch (Throwable e) {
			CLogger.write("7", ProductoDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

		public static boolean eliminar(Integer productoId, String usuario) {
		boolean ret = false;

		Producto pojo = getProductoPorId(productoId,usuario);

		if (pojo != null) {
			pojo.setEstado(0);
			pojo.setOrden(null);
			Session session = CHibernateSession.getSessionFactory().openSession();

			try {
				session.beginTransaction();
				session.update(pojo);
				session.getTransaction().commit();

				ret = true;

			} catch (Throwable e) {
				CLogger.write("8", ProductoDAO.class, e);
			} finally {
				session.close();
			}
		}
		return ret;
	}*/

        public static Producto getProductoPorId(int id)
        {
            Producto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Producto>("SELECT * FROM PRODUCTO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProductoDAO.class", e);
            }
            return ret;
        }

        /*public static List<Producto> getProductosPorProyecto(Integer idProyecto,String usuario,String lineaBase) {
            List<Producto> ret = new ArrayList<Producto>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try {
                String query = String.join(" ", "select t.*"
                        ,"from (",
                        "SELECT pr.* FROM sipro_history.producto pr JOIN sipro_history.componente c ON c.id = pr.componenteid "
                        ,"JOIN sipro_history.proyecto p ON p.id = c.proyectoid"
                        ,"where p.id = :idProy" 
                        ,lineaBase != null ? "and p.linea_base like '%" + lineaBase + "%'" : "and p.actual = 1"
                        ,"UNION"
                        ,"SELECT pr.* FROM sipro_history.producto pr" 
                        ,"JOIN sipro_history.subcomponente s ON s.id = pr.subcomponenteid" 
                        ,"JOIN sipro_history.componente c ON c.id = s.componenteid" 
                        ,"JOIN sipro_history.proyecto p ON p.id = c.proyectoid"
                        ,"where p.id = :idProy"
                        ,lineaBase != null ? "and pr.linea_base like '%" + lineaBase + "%'"  : "and pr.actual = 1"
                        ,lineaBase != null ? "and s.linea_base like '%" + lineaBase + "%'"  : "and s.actual = 1"
                        ,lineaBase != null ? "and c.linea_base like '%" + lineaBase + "%'" : "and c.actual = 1"
                        ,lineaBase != null ? "and p.linea_base like '%" + lineaBase + "%'"  : "and p.actual = 1"
                        ,") as t"
                        ,usuario!=null && usuario.length()>0 ? 
                         "join producto_usuario pu on pu.productoid = t.id where pu.usuario = :usuario ":"",
                         usuario!=null && usuario.length()>0 ? "and" : "where", "t.estado = 1");

                Query<Producto> criteria = session.createNativeQuery(query,Producto.class);
                criteria.setParameter("idProy", idProyecto);
                if (usuario !=null && usuario.length()>0)
                    criteria.setParameter("usuario", usuario);
                ret =   criteria.getResultList();
            } catch (Throwable e) {
                CLogger.write("10", ProductoDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static Producto getProductoInicial(Integer componenteId, Integer subcomponenteId, String usuario, Session session){
            Producto ret = null;
            try{
                String query = "FROM Producto p where p.estado=1 and p.orden=1 ";
                if(componenteId!=null && componenteId > 0){
                    query += "AND p.componente.id = :idComp ";
                }
                if(subcomponenteId!=null && subcomponenteId > 0){
                    query += "AND p.subcomponente.id = :idSubComp ";
                }

                query += "and p.usuarioCreo=:usuario";
                Query<Producto> criteria = session.createQuery(query, Producto.class);
                if (componenteId!=null && componenteId > 0){
                    criteria.setParameter("idComp", componenteId);
                }
                if (subcomponenteId!=null && subcomponenteId > 0){
                    criteria.setParameter("idSubComp", subcomponenteId);
                }
                criteria.setParameter("usuario", usuario);
                List<Producto> lista = criteria.getResultList();
                ret = !lista.isEmpty() ? lista.get(0) : null;
            }catch(Throwable e){
                CLogger.write("11", ProductoDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static Producto getProductoFechaMaxima(Integer componenteId, Integer subcomponenteId, String usuario, Session session){
            Producto ret = null;
            try{
                String query = "FROM Producto p where p.estado=1 ";
                if(componenteId!=null && componenteId > 0){
                    query += "AND p.componente.id = :idComp ";
                }
                if(subcomponenteId!=null && subcomponenteId > 0){
                    query += "AND p.subcomponente.id = :idSubComp ";
                } 
                query += " and p.usuarioCreo=:usuario order by p.fechaFin desc";
                Query<Producto> criteria = session.createQuery(query, Producto.class);
                criteria.setMaxResults(1);
                if (componenteId!=null && componenteId > 0){
                    criteria.setParameter("idComp", componenteId);
                }
                if (subcomponenteId!=null && subcomponenteId > 0){
                    criteria.setParameter("idSubComp", subcomponenteId);
                }
                criteria.setParameter("usuario", usuario);
                List<Producto> lista = criteria.getResultList();
                ret = !lista.isEmpty() ? lista.get(0) : null;
            }catch(Throwable e){
                CLogger.write("12", ProductoDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static List<Producto> getProductosOrden(Integer componenteId, Integer subcomponenteId, String usuario, Session session) {
            List<Producto> ret = new ArrayList<Producto>();
            try {

                String query = "SELECT p FROM Producto p WHERE p.estado = 1 ";
                if(componenteId!=null && componenteId > 0){
                    query += "AND p.componente.id = :idComp ";
                }
                if(subcomponenteId!=null && subcomponenteId > 0){
                    query += "AND p.subcomponente.id = :idSubComp ";
                } 

                query = String.join("", query, " AND p.id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )");

                Query<Producto> criteria = session.createQuery(query,Producto.class);
                criteria.setParameter("usuario", usuario);
                if (componenteId!=null && componenteId > 0){
                    criteria.setParameter("idComp", componenteId);
                }
                if (subcomponenteId!=null && subcomponenteId > 0){
                    criteria.setParameter("idSubComp", subcomponenteId);
                }
                ret = criteria.getResultList();
            } catch (Throwable e) {
                CLogger.write("13", ProductoDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            } 
            return ret;
        }

        public static Producto getProductoPorIdOrden(int id, String usuario, Session session) {
            Producto ret = null;
            try {
                Query<Producto> criteria = session.createQuery("FROM Producto where id=:id AND id in (SELECT u.id.productoid from ProductoUsuario u where u.id.usuario=:usuario )", Producto.class).setLockMode(LockModeType.PESSIMISTIC_READ);
                criteria.setParameter("id", id);
                criteria.setParameter("usuario", usuario);
                List<Producto> lista = criteria.getResultList();
                ret = !lista.isEmpty() ? lista.get(0) : null;
            } catch (Throwable e) {
                CLogger.write("14", ProductoDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static boolean guardarProductoOrden(Producto producto, Session session) {
            boolean ret = false;
            try {
                session.saveOrUpdate(producto);
                session.flush();
                session.clear();
                ret = true;
            } catch (Throwable e) {
                CLogger.write("15", ProductoDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static BigDecimal calcularCosto(Producto producto){
            BigDecimal costo = new BigDecimal(0);
            try{
                Set<Subproducto> subproductos = producto.getSubproductos();
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(producto.getId(), 3);
                if((subproductos != null && subproductos.size() > 0) || (actividades!=null && actividades.size()>0)){

                    if(subproductos!=null){
                        Iterator<Subproducto> iterador = subproductos.iterator();
                        while(iterador.hasNext()){
                            Subproducto subproducto = iterador.next();
                            costo = costo.add(subproducto.getCosto() != null ? subproducto.getCosto() : new BigDecimal(0));
                        }
                    }


                    if(actividades != null && actividades.size() > 0){
                        for(Actividad actividad : actividades){
                            costo = costo.add(actividad.getCosto() != null ? actividad.getCosto() : new BigDecimal(0));
                        }
                    }
                }else{
                    PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(3, producto.getId());
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
                            costo = producto.getCosto();
                }			
            }catch(Exception e){
                CLogger.write("16", Proyecto.class, e);
            }

            return costo;
        }

        public static boolean calcularCostoyFechas(Integer productoId){
            boolean ret = false;
            ArrayList<ArrayList<Nodo>> listas = EstructuraProyectoDAO.getEstructuraObjetoArbolCalculos(productoId, 3);
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
                        nodo.costo = calcularCosto((Producto)nodo.objeto).doubleValue();
                    nodo.duracion = Utils.getWorkingDays(new DateTime(nodo.fecha_inicio), new DateTime(nodo.fecha_fin));
                    setDatosCalculados(nodo.objeto,nodo.fecha_inicio,nodo.fecha_fin,nodo.costo, nodo.duracion);
                }
                ret = true;
            }
            ret= ret && guardarProductoBatch(listas);	
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
                CLogger.write("19", ProductoDAO.class, e);
            }

        }

        private static boolean guardarProductoBatch(ArrayList<ArrayList<Nodo>> listas){
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
                CLogger.write("20", ProductoDAO.class, e);
            }
            return ret;
        }

        public static List<Producto> getProductosHistory(Integer componenteid, Integer subcomponenteid,String lineaBase) {
            List<Producto> ret = new ArrayList<Producto>();
            Session session = CHibernateSession.getSessionFactory().openSession();
            try {

                String query = "select * from sipro_history.producto p where p.estado  = 1 ";
                if(componenteid!=null && componenteid > 0){
                    query += "AND p.componenteid = ?1 ";
                }
                if(subcomponenteid!=null && subcomponenteid > 0){
                    query += "AND p.subcomponenteid = ?2 ";
                }

                query += (lineaBase != null ? "and p.linea_base like '%" + lineaBase + "%'" : "and p.actual = 1");


                Query<Producto> criteria = session.createNativeQuery(query,Producto.class);

                if (componenteid!=null && componenteid>0){
                    criteria.setParameter(1, componenteid);
                }
                if (subcomponenteid!=null && subcomponenteid>0){
                    criteria.setParameter(2, subcomponenteid);
                }
                ret = criteria.getResultList();
            } catch (Throwable e) {
                CLogger.write("21", ProductoDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static Producto getProductoHistory(Integer productoId,String lineaBase){
            Producto ret = null;
            List<Producto> listRet = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String query = String.join(" ", "select * ", 
                        "from sipro_history.producto p ",
                        "where p.estado = 1 ",
                        "and p.id = ?1 ",
                        lineaBase != null ? "and p.linea_base like '%" + lineaBase + "%'" : "and p.actual = 1",
                                "order by p.id desc");
                Query<Producto> criteria = session.createNativeQuery(query, Producto.class);
                criteria.setParameter(1, productoId);
                if (lineaBase != null)
                    criteria.setParameter(2, lineaBase);
                listRet =   criteria.getResultList();
                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            }
            catch(Throwable e){
                CLogger.write("22", ProductoDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static String getVersiones (Integer productoId){
            String resultado = "";
            try{
                String query = "SELECT DISTINCT(version) "
                        + " FROM sipro_history.producto "
                        + " WHERE id = "+productoId;
                List<?> versiones = CHistoria.getVersiones(query);
                if(versiones!=null){
                    for(int i=0; i<versiones.size(); i++){
                        if(!resultado.isEmpty()){
                            resultado+=",";
                        }
                        resultado+=(Integer)versiones.get(i);
                    }
                }
            }catch(Throwable e){
                CLogger.write("23", ProductoDAO.class, e);
            }
            return resultado;
        }

        public static String getHistoria (Integer productoId, Integer version){
            String resultado = "";
            try{
                String query = "SELECT c.version, c.nombre, c.descripcion, ct.nombre tipo, ue.nombre unidad_ejecutora, c.costo, ac.nombre tipo_costo, "
                        + " c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.renglon, c.ubicacion_geografica, c.latitud, c.longitud, "
                        + " c.fecha_inicio, c.fecha_fin, c.duracion, c.fecha_inicio_real, c.fecha_fin_real, "
                        + " c.fecha_creacion, c.usuario_creo, c.fecha_actualizacion, c.usuario_actualizo, "
                        + " CASE WHEN c.estado = 1 "
                        + " THEN 'Activo' "
                        + " ELSE 'Inactivo' "
                        + " END AS estado "
                        + " FROM sipro_history.producto c "
                        + " JOIN sipro.unidad_ejecutora ue ON c.unidad_ejecutoraunidad_ejecutora = ue.unidad_ejecutora and c.entidad = ue.entidadentidad and c.ejercicio = ue.ejercicio  JOIN sipro_history.producto_tipo ct ON c.producto_tipoid = ct.id "
                        + " JOIN sipro_history.acumulacion_costo ac ON c.acumulacion_costoid = ac.id "
                        + " WHERE c.id = "+productoId
                        + " AND c.version = " +version;

                String [] campos = {"Version", "Nombre", "DescripciÃ³n", "Tipo", "Unidad Ejecutora", "Monto Planificado", "Tipo AcumulaciÃ³n de Monto Planificado", 
                        "Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "UbicaciÃ³n GeogrÃ¡fica", "Latitud", "Longitud", 
                        "Fecha Inicio", "Fecha Fin", "DuraciÃ³n", "Fecha Inicio Real", "Fecha Fin Real", 
                        "Fecha CreaciÃ³n", "Usuario que creo", "Fecha ActualizaciÃ³n", "Usuario que actualizÃ³", 
                        "Estado"};
                resultado = CHistoria.getHistoria(query, campos);	
            }catch (Throwable e) {
                CLogger.write("24", ProductoDAO.class, e);
            }
            return resultado;
        }

             */

        public static List<Producto> getProductosByComponente(int componenteId)
        {
            List<Producto> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Producto>("SELECT * FROM PRODUCTO WHERE componenteid=:componenteId AND estado=1", new { componenteId = componenteId }).AsList<Producto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("25", "ProductoDAO.class", e);
            }
            return ret;
        }
    }
}
