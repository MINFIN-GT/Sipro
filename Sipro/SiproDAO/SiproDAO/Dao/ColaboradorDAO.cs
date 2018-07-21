using System;
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

        public static bool guardar(Colaborador colaborador)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM colaborador WHERE id=:id", new { id = colaborador.id });

                    if (existe > 1)
                    {
                        int guardado = db.Execute("UPDATE colaborador SET pnombre=:pnombre, snombre=:snombre, papellido=:papellido, sapellido=:sapellido, cui=:cui, ueunidad_ejecutora=:ueunidadEjecutora," +
                            "usuariousuario=:usuariousuario, estado=:estado, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "ejercicio=:ejercicio, entidad=:entidad WHERE id=:id", colaborador);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_colaborador.nextval FROM DUAL");
                        colaborador.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO colaborador(:id, :pnombre, :snombre, :papellido, :sapellido, :cui, :ueunidadEjecutora, :usuariousuario, :estado, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :ejercicio, :entidad)", colaborador);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ColaboradorDAO.class", e);
            }

            return ret;
        }

        public static bool borrar(Colaborador colaborador)
        {
            bool ret = false;

            try
            {
                colaborador.estado = 0;
                colaborador.fechaActualizacion = new DateTime();
                ret = guardar(colaborador);
            }
            catch (Exception e)
            {
                CLogger.write("5", "ColaboradorDAO.class", e);
            }
            return ret;
        }

        public static List<Colaborador> getPagina(int pagina, int registros, String filtro_busqueda, String columna_ordenada, String orden_direccion, String excluir)
        {
            List<Colaborador> ret = new List<Colaborador>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT c.* FROM Colaborador c ",
                        filtro_busqueda != null && filtro_busqueda.Length > 0 ? "INNER JOIN unidad_ejecutora ue ON ue.unidad_ejecutora=c.ueunidad_ejecutora AND ue.ejercicio=c.ejercicio AND ue.entidadentidad=c.entidad" : "",
                        "WHERE c.estado=1 ");
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.pnombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.snombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.papellido LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.sapellido LIKE '%" + filtro_busqueda + "%' ");

                        long cui;
                        if (long.TryParse(filtro_busqueda, out cui))
                        {
                            query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " str(c.cui) LIKE '%" + cui + "%' ");
                        }

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " ue.nombre LIKE '%" + filtro_busqueda + "%' ");                        
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join(" ", query, (excluir != null && excluir.Length > 0 ? "and c.id not in (" + excluir + ")" : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<Colaborador>(query).AsList<Colaborador>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ColaboradorDAO.class", e);
            }
            return ret;
        }

        /*public static String getJson(int pagina, int registros, String filtro_pnombre, String filtro_snombre, String filtro_papellido, String filtro_sapellido,
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
        }*/

        public static long getTotal(String filtro_busqueda, String excluir)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT COUNT(c.id) FROM colaborador c",
                        filtro_busqueda != null && filtro_busqueda.Length > 0 ? "INNER JOIN unidad_ejecutora ue ON ue.unidad_ejecutora=c.ueunidad_ejecutora AND ue.ejercicio=c.ejercicio AND ue.entidadentidad=c.entidad" : "",
                        "WHERE c.estado=1");
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.pnombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.snombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.papellido LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.sapellido LIKE '%" + filtro_busqueda + "%' ");

                        long cui;
                        if (long.TryParse(filtro_busqueda, out cui))
                        {
                            query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " str(c.cui) LIKE '%" + cui + "%' ");
                        }

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " ue.nombre LIKE '%" + filtro_busqueda + "%' ");

                        query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                        query = String.Join(" ", query, (excluir != null && excluir.Length > 0 ? "and c.id not in (" + excluir + ")" : ""));
                    }

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ColaboradorDAO.class", e);
            }
            return ret;
        }

        public static bool validarUsuario(String usuario)
        {
            return UsuarioDAO.getUsuario(usuario) != null;
        }
	
	/*public static String getColaboradorByUsuario(String usuario){
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
