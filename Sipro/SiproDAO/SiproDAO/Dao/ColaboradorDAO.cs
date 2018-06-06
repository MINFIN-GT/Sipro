﻿using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class ColaboradorDAO
    {
        public class EstructuraPojo
        {
            public int id;
            public String primerNombre;
            public String segundoNombre;
            public String otrosNombres;
            public String primerApellido;
            public String segundoApellido;
            public String otrosApellidos;
            public long cui;
            public int unidadEjecutora;
            public String nombreUnidadEjecutora;
            public String usuario;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public String nombreCompleto;
        }

        public static Colaborador getColaborador(int colaboradorId)
        {
            Colaborador ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Colaborador>("SELECT * FROM COLABORADOR WHERE id=:id", new { id = colaboradorId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ColaboradorDAO.class", e);
            }
            return ret;
        }

	 /*public static boolean guardar(Integer codigo, String primerNombre, String segundoNombre, String otrosNombres,
			String primerApellido, String segundoApellido, String otrosApellidos, Long cui,
			Integer ejercicio, Integer entidad,Integer codigoUnidadEjecutora, String usuario, String usuario_creacion, Date fecha_creacion) {

		Colaborador pojo = getColaborador(codigo);
		boolean ret = false;

		if (pojo == null) {

			pojo = new Colaborador(UnidadEjecutoraDAO.getUnidadEjecutora(ejercicio, entidad,codigoUnidadEjecutora),UsuarioDAO.getUsuario(usuario),
					primerNombre, segundoNombre, primerApellido, segundoApellido, cui, 1, usuario_creacion, null, fecha_creacion, null,
					null, null, null,null,null);
			Session session = CHibernateSession.getSessionFactory().openSession();
			try {
				session.beginTransaction();
				session.save(pojo);
				session.getTransaction().commit();
				ret = true;
			} catch (Throwable e) {
				CLogger.write("3", ColaboradorDAO.class, e);
			} finally {
				session.close();
			}
		}

		return ret;
	}

	public static boolean actualizar(Integer codigo, String primerNombre, String segundoNombre, 
			String primerApellido, String segundoApellido, Long cui,
			Integer ejercicio, Integer entidad,Integer codigoUnidadEjecutora, String usuario, String usuarioc) {

		Colaborador pojo = getColaborador(codigo);
		boolean ret = false;

		if (pojo != null) {
			pojo.setPnombre(primerNombre);
			pojo.setSnombre(segundoNombre);
			pojo.setPapellido(primerApellido);
			pojo.setSapellido(segundoApellido);
			pojo.setCui(cui);
			pojo.setUsuarioActualizo(usuarioc);
			pojo.setFechaActualizacion(new Date());

			pojo.setUnidadEjecutora(UnidadEjecutoraDAO.getUnidadEjecutora(ejercicio, entidad,codigoUnidadEjecutora));
			if(usuario!=null)
				pojo.setUsuario(UsuarioDAO.getUsuario(usuario));

			Session session = CHibernateSession.getSessionFactory().openSession();
			try {
				session.beginTransaction();
				session.update(pojo);
				session.getTransaction().commit();
				ret = true;
			} catch (Throwable e) {
				CLogger.write("4", ColaboradorDAO.class, e);
			} finally {
				session.close();
			}
		}

		return ret;
	}
	
	public static boolean borrar(Integer id, String usuarioc) {

		Colaborador pojo = getColaborador(id);
		pojo.setEstado(0);
		pojo.setUsuarioActualizo(usuarioc);
		pojo.setFechaActualizacion(new Date());
		boolean ret = false;

		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
				session.beginTransaction();
				session.update(pojo);
				session.getTransaction().commit();
				ret = true;
		} catch (Throwable e) {
			CLogger.write("5", ColaboradorDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static List<Colaborador> getPagina(int pagina, int registros,String filtro_pnombre, String filtro_snombre, String filtro_papellido, String filtro_sapellido,
			String filtro_cui, String filtro_unidad_ejecutora, String columna_ordenada, String orden_direccion, String excluir) {
		List<Colaborador> ret = new ArrayList<Colaborador>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			String query = "SELECT c FROM Colaborador c WHERE c.estado=1 ";
			String query_a="";
			if(filtro_pnombre!=null && filtro_pnombre.trim().length()>0)
				query_a = String.join("",query_a, " c.pnombre LIKE '%",filtro_pnombre,"%' ");
			if(filtro_snombre!=null && filtro_snombre.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.snombre LIKE '%", filtro_snombre,"%' ");
			if(filtro_papellido!=null && filtro_papellido.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.papellido LIKE '%", filtro_papellido,"%' ");
			if(filtro_sapellido!=null && filtro_sapellido.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.sapellido LIKE '%", filtro_sapellido,"%' ");
			if(filtro_cui!=null && filtro_cui.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(c.cui) LIKE '%", filtro_cui,"%' ");
			if(filtro_unidad_ejecutora!=null && filtro_unidad_ejecutora.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.unidadEjecutora.nombre LIKE '%", filtro_unidad_ejecutora,"%' ");
			query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
			query = String.join(" ", query, (excluir!=null && excluir.length()>0 ? "and c.id not in (" + excluir + ")" : ""));
			query = columna_ordenada!=null && columna_ordenada.trim().length()>0 ? String.join(" ",query,"ORDER BY",columna_ordenada,orden_direccion ) : query;
			
			Query<Colaborador> criteria = session.createQuery(query,Colaborador.class);
			criteria.setFirstResult(((pagina-1)*(registros)));
			criteria.setMaxResults(registros);
			ret = criteria.getResultList();
		} catch (Throwable e) {
			CLogger.write("6", ColaboradorDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}

	public static String getJson(int pagina, int registros, String filtro_pnombre, String filtro_snombre, String filtro_papellido, String filtro_sapellido,
			String filtro_cui, String filtro_unidad_ejecutora, String columna_ordenada, String orden_direccion) {
		String jsonEntidades = "";

		List<Colaborador> pojos = getPagina(pagina, registros, filtro_pnombre, filtro_snombre, filtro_papellido, filtro_sapellido,
				filtro_cui, filtro_unidad_ejecutora, columna_ordenada, orden_direccion,null);

		List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

		for (Colaborador pojo : pojos) {
			EstructuraPojo estructuraPojo = new EstructuraPojo();
			estructuraPojo.id = pojo.getId();
			estructuraPojo.primerNombre = pojo.getPnombre();
			estructuraPojo.segundoNombre = pojo.getSnombre();
			estructuraPojo.primerApellido = pojo.getPapellido();
			estructuraPojo.segundoApellido = pojo.getSapellido();
			estructuraPojo.cui = pojo.getCui();

			estructuraPojo.usuario = pojo.getUsuario().getUsuario();
			
			estructuraPojo.unidadEjecutora = pojo.getUnidadEjecutora().getId().getUnidadEjecutora();
			
			estructuraPojo.nombreUnidadEjecutora = pojo.getUnidadEjecutora().getNombre();
			
			estructuraPojo.usuarioCreo = pojo.getUsuarioCreo();
			estructuraPojo.usuarioActualizo = pojo.getUsuarioActualizo();
			estructuraPojo.fechaCreacion = Utils.formatDateHour(pojo.getFechaCreacion());
			estructuraPojo.fechaActualizacion = Utils.formatDateHour(pojo.getFechaActualizacion());
			estructuraPojo.nombreCompleto = String.join(" ", estructuraPojo.primerNombre,
					estructuraPojo.segundoNombre!=null ? estructuraPojo.segundoNombre : "" ,
					estructuraPojo.primerApellido !=null ? estructuraPojo.primerApellido : "" ,
					estructuraPojo.segundoApellido !=null ? estructuraPojo.segundoApellido : "");

			listaEstructuraPojos.add(estructuraPojo);
		}

		jsonEntidades = Utils.getJSonString("colaboradores", listaEstructuraPojos);

		return jsonEntidades;
	}
	public static String getJson2() {
		String jsonEntidades = "";

		List<Colaborador> pojos = getPagina(1, 10000, null, null, null, null, null, null, null, null,null);

		List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

		for (Colaborador pojo : pojos) {
			if(pojo.getUsuario()==null){
				EstructuraPojo estructuraPojo = new EstructuraPojo();
				estructuraPojo.id = pojo.getId();
				estructuraPojo.primerNombre = pojo.getPnombre();
				estructuraPojo.segundoNombre = pojo.getSnombre();
				estructuraPojo.primerApellido = pojo.getPapellido();
				estructuraPojo.segundoApellido = pojo.getSapellido();
				estructuraPojo.cui = pojo.getCui();
				if(pojo.getUsuario()!=null)
					estructuraPojo.usuario = pojo.getUsuario().getUsuario();
				estructuraPojo.unidadEjecutora = pojo.getUnidadEjecutora().getId().getUnidadEjecutora();
				estructuraPojo.nombreUnidadEjecutora = pojo.getUnidadEjecutora().getNombre();
				estructuraPojo.usuarioCreo = pojo.getUsuarioCreo();
				estructuraPojo.usuarioActualizo = pojo.getUsuarioActualizo();
				estructuraPojo.fechaCreacion = Utils.formatDateHour(pojo.getFechaCreacion());
				estructuraPojo.fechaActualizacion = Utils.formatDateHour(pojo.getFechaActualizacion());
				estructuraPojo.nombreCompleto = String.join(" ", estructuraPojo.primerNombre,
						estructuraPojo.segundoNombre!=null ? estructuraPojo.segundoNombre : "" ,
						estructuraPojo.primerApellido !=null ? estructuraPojo.primerApellido : "" ,
						estructuraPojo.segundoApellido !=null ? estructuraPojo.segundoApellido : "");
				
				listaEstructuraPojos.add(estructuraPojo);
			}
			
		}

		jsonEntidades = Utils.getJSonString("colaboradores", listaEstructuraPojos);

		return jsonEntidades;
	}

	public static Long getTotal(String filtro_pnombre, String filtro_snombre, String filtro_papellido, String filtro_sapellido, String filtro_cui, String filtro_unidad_ejecutora) {
		Long ret = 0L;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try {
			String query = "SELECT count(c.id) FROM Colaborador c WHERE c.estado=1 ";
			String query_a="";
			if(filtro_pnombre!=null && filtro_pnombre.trim().length()>0)
				query_a = String.join("",query_a, " c.pnombre LIKE '%",filtro_pnombre,"%' ");
			if(filtro_snombre!=null && filtro_snombre.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.snombre LIKE '%", filtro_snombre,"%' ");
			if(filtro_papellido!=null && filtro_papellido.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.papellido LIKE '%", filtro_papellido,"%' ");
			if(filtro_sapellido!=null && filtro_sapellido.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.sapellido LIKE '%", filtro_sapellido,"%' ");
			if(filtro_cui!=null && filtro_cui.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " str(c.cui) LIKE '%", filtro_cui,"%' ");
			if(filtro_unidad_ejecutora!=null && filtro_unidad_ejecutora.trim().length()>0)
				query_a = String.join("",query_a,(query_a.length()>0 ? " OR " :""), " c.unidadEjecutora.nombre LIKE '%", filtro_unidad_ejecutora,"%' ");
			query = String.join(" ", query, (query_a.length()>0 ? String.join("","AND (",query_a,")") : ""));
			Query<Long> criteria = session.createQuery(query,Long.class);
			ret = criteria.getSingleResult();
		} catch (Throwable e) {
			CLogger.write("7", ColaboradorDAO.class, e);
		} finally {
			session.close();
		}
		return ret;
	}
	
	public static boolean validarUsuario(String usuario){
			return UsuarioDAO.getUsuario(usuario) != null;
	}
	
	public static String getColaboradorByUsuario(String usuario){
		String ret = "";
		List<?> retList = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			String str_Query = String.join(" ", "select concat(pnombre, ' ', snombre, ' ', papellido, ' ',sapellido) nombre_colaborador",
					"from colaborador c, usuario u",
					"where c.usuariousuario=u.usuario",
					"and u.usuario=:usuario");
			Query<?> criteria = session.createNativeQuery(str_Query);
			criteria.setParameter("usuario", usuario);
			retList = criteria.getResultList();
			if(!retList.isEmpty()){
				ret = (String)retList.get(0);
			}
		}catch(Exception e){
			CLogger.write("8", ColaboradorDAO.class, e);
		}finally {
        	session.close();
		}
		
		return ret;
	}*/

        public static List<Colaborador> getColaboradorPorUnidadEjecutora(int ejercicio, int unidadEjecutora, int entidad)
        {
            List<Colaborador> ret = new List<Colaborador>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String str_Query = String.Join(" ", "select c from Colaborador c",
                                    "where c.estado = 1",
                                    "and c.unidadEjecutora.id.ejercicio=:ejercicio",
                                    "and c.unidadEjecutora.id.entidadentidad=:entidad",
                                    "and c.unidadEjecutora.id.unidadEjecutora=:unidadEjecutora");

                    ret = db.Query<Colaborador>(str_Query, new { ejercicio = ejercicio, entidad = entidad, unidadEjecutora = unidadEjecutora }).AsList<Colaborador>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "ColaboradorDAO.class", e);
            }
            return ret;
        }

        /*public static Colaborador getColaboradorByNombre(String primerNombre, String segundoNombre, 
                String primerApellido, String segundoApellido){
            Colaborador ret = null;
            List<Colaborador> retList = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                String str_Query = String.join(" ", "select c from Colaborador c",
                                    "where c.pnombre = ?1",
                                    "and c.snombre = ?2",
                                    "and c.papellido = ?3",
                                    "and c.sapellido = ?4",
                                    "and c.estado = 1");
                Query<Colaborador> criteria = session.createQuery(str_Query,Colaborador.class);
                criteria.setParameter(1, primerNombre);
                criteria.setParameter(2, segundoNombre);
                criteria.setParameter(3, primerApellido);
                criteria.setParameter(4, segundoApellido);
                retList = criteria.getResultList();
                if(!retList.isEmpty()){
                    ret = retList.get(0);
                }
            }catch(Exception e){
                CLogger.write("10", ColaboradorDAO.class, e);
            }finally {
                session.close();
            }

            return ret;
        }

        public static boolean guardarColaborador(Colaborador colaborador){
            boolean ret = false;
            Session session = CHibernateSession.getSessionFactory().openSession();
            try{
                session.beginTransaction();
                session.saveOrUpdate(colaborador);
                session.flush();
                session.getTransaction().commit();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("11", ColaboradorDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }
             */
    }
}
