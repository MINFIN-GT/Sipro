using System;
using SiproModel.Models;
using Sipro.Utilities;
using System.Data.Common;
using Dapper;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Sipro.Dao
{
    public class UsuarioDAO
    {
        public static Usuario getUsuario(String usuario)
        {
            Usuario ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Usuario>("Select * FROM USUARIO WHERE usuario =:usuario", new Usuario { usuario = usuario });
                }
            } catch (Exception e) {
                CLogger.write("1", "UsuarioDAO", e);
            }
            return ret;
        }

        public static void userLoginHistory(String usuario)
        {
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    UsuarioLog usuarioLog = new UsuarioLog();
                    usuarioLog.usuario = usuario;
                    usuarioLog.fecha = DateTime.Now;

                    db.Query<UsuarioLog>("INSERT INTO USUARIO_LOG VALUES (:usuario, :fecha)", usuarioLog);
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "UsuarioDAO", e);
            }

        }

        public static bool tienePermiso(String usuario, String permisonombre)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    Permiso permiso = db.QueryFirstOrDefault<Permiso>("SELECT id FROM PERMISO WHERE nombre=:nombre", new Permiso { nombre = permisonombre });
                    if (permiso != null)
                    {
                        UsuarioPermiso usuarioPermiso = db.QueryFirstOrDefault<UsuarioPermiso>("SELECT * FROM USUARIO_PERMISO WHERE usuariousuario=:usuariousuario and permisoid=:permisoid", new UsuarioPermiso { usuariousuario = usuario, permisoid = permiso.id });
                        if (usuarioPermiso.estado == 1)
                            ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "UsuarioDAO", e);
            }
            return ret;
        }

        public static bool registroUsuario(String cadenausuario, String email, String passwordTextoPlano, String usuarioCreo, Int32 sistemaUsuario)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    Usuario usuario = new Usuario();
                    usuario.usuario = cadenausuario;
                    usuario.email = email;
                    string[] hashedData = Utilities.Identity.SHA256Hasher.ComputeHash(passwordTextoPlano);
                    usuario.password = hashedData[0];
                    usuario.salt = hashedData[1];
                    usuario.fechaCreacion = DateTime.Now;
                    usuario.usuarioCreo = usuarioCreo;
                    usuario.estado = 1;
                    usuario.sistemaUsuario = sistemaUsuario;
                    db.Query<Usuario>("INSERT INTO USUARIO VALUES (:usuario, :password, :salt, :email, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :sistemaUsuario)", usuario);
                    ret = true;
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "UsuarioDAO", e);
            }

            return ret;
        }

        public static bool cambiarPassword(String usuario, String password, String usuarioActualizo)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    Usuario usuarioCambio = getUsuario(usuario);
                    if (usuarioCambio != null)
                    {
                        string[] hashedData = Utilities.Identity.SHA256Hasher.ComputeHash(password);
                        usuarioCambio.password = hashedData[0];
                        usuarioCambio.salt = hashedData[1];
                        usuarioCambio.usuarioActualizo = usuarioActualizo;
                        usuarioCambio.fechaActualizacion = DateTime.Now;

                        db.Query<Usuario>("UPDATE USUARIO SET password=:password, salt=:salt, email=:email, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, sistema_usuario=:sistemaUsuario WHERE usuario=:usuario", usuarioCambio);
                        ret = true;
                    }
                }
            } catch (Exception e) {
                CLogger.write("6", "UsuarioDAO", e);
            }

            return ret;
        }

        public static bool asignarPermisosUsuario(String usuario, List<int> permisos, String usuarioTexto)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    for (int i = 0; i < permisos.Count; i++)
                    {
                        UsuarioPermiso usuariopermiso = new UsuarioPermiso();
                        usuariopermiso.usuariousuario = usuario;
                        usuariopermiso.permisoid = permisos[i];
                        usuariopermiso.usuarioCreo = usuarioTexto;
                        usuariopermiso.estado = 1;
                        usuariopermiso.fechaCreacion = DateTime.Now;
                        db.Query<UsuarioPermiso>("INSERT INTO USUARIO_PERMISO VALUES (:usuariousuario, :permisoid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", usuariopermiso);
                    }
                    ret = true;
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "UsuarioDAO", e);
            }

            return ret;
        }

        /*public static bool asignarPrestamos(String usuario, List<int> prestamos, String usuario_creo)
        {
            bool ret = false;

            try
            {
                for (int i = 0; i < prestamos.Count; i++)
                {
                    Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamos.get(i));
                    PrestamoUsuario pu = new PrestamoUsuario(new PrestamoUsuarioId(prestamos.get(i), usuario), prestamo, UsuarioDAO.getUsuario(usuario),
                            usuario_creo, null, new Date(), null);
                    pu.setPrestamo(prestamo);
                    session.save(pu);
                }
                session.getTransaction().commit();
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("8", "UsuarioDAO", e);
            }

            return ret;
        }


        public static boolean asignarProyectos(String usuario, List<Integer> proyectos, String usuario_creo)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();

            for (int i = 0; i < proyectos.size(); i++)
            {
                Proyecto proyecto = ProyectoDAO.getProyecto(proyectos.get(i));
                ProyectoUsuario pu = new ProyectoUsuario(new ProyectoUsuarioId(proyectos.get(i), usuario), proyecto, UsuarioDAO.getUsuario(usuario),
                        usuario_creo, null, new Date(), null);
                pu.setProyecto(proyecto);
                session.save(pu);
            }
            session.getTransaction().commit();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("8", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean asignarPrestamoRol(String usuario, List<Integer> prestamos, int rol)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();
            for (int i = 0; i < prestamos.size(); i++)
            {
                RolUsuarioProyectoId rolUsuarioProyectoId = new RolUsuarioProyectoId(rol, prestamos.get(i).intValue(), usuario);
                RolUsuarioProyecto rolUsuario = new RolUsuarioProyecto(rolUsuarioProyectoId);
                session.saveOrUpdate(rolUsuario);
                if (i % 20 == 0)
                {
                    session.flush();
                    session.clear();
                }
            }
            session.getTransaction().commit();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("9", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean desasignarPrestamo(String usuario, List<Integer> prestamos)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();
            for (int i = 0; i < prestamos.size(); i++)
            {
                Query<ProyectoUsuario> criteria = session.createQuery("FROM ProyectoUsuario where id.proyectoid=:id AND id.usuario=:usuario ", ProyectoUsuario.class);
                    criteria.setParameter("id", prestamos.get(i));
                    criteria.setParameter("usuario", usuario);
                    List<ProyectoUsuario> listRet = null;
    listRet = criteria.getResultList();				            
                    ProyectoUsuario pu = !listRet.isEmpty() ? listRet.get(0) : null;
    session.delete(pu);
                }			
                session.getTransaction().commit();
    session.flush();
                ret = true;
            }catch(Throwable e){
                CLogger.write("10", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }


        public static boolean desactivarPermisosUsuario(String usuario, List<Integer> permisos, String usuarioTexto)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();
            for (int i = 0; i < permisos.size(); i++)
            {
                UsuarioPermisoId usuariopermisoid = new UsuarioPermisoId(usuario, permisos.get(i));
                UsuarioPermiso usuariopermiso = session.get(UsuarioPermiso.class, usuariopermisoid);
                    usuariopermiso.setUsuarioActualizo(usuarioTexto);
                    usuariopermiso.setEstado(0);
                    usuariopermiso.setFechaActualizacion(new DateTime().toDate());
                    session.saveOrUpdate(usuariopermiso);
                    if(i % 20 == 0 ){
                        session.flush();
                        session.clear();
                    }
                }
                session.getTransaction().commit();
    ret = true;
            }catch(Throwable e){
                CLogger.write("11", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }*/

        public static bool existeUsuario(String usuario)
        {
            bool ret = false;
            try
            {
                Usuario usuarioE = getUsuario(usuario);
                if (usuarioE != null)
                    ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("12", "UsuarioDAO", e);
            }

            return ret;
        }

        public static bool desactivarUsuario(String usuario, String usuarioActualizo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    Usuario usuarioDesactivado = getUsuario(usuario);
                    usuarioDesactivado.estado = 0;
                    usuarioDesactivado.usuarioActualizo = usuarioActualizo;
                    usuarioDesactivado.fechaActualizacion = DateTime.Now;
                    db.Query<Usuario>("UPDATE USUARIO SET password=:password, salt=:salt, email=:email, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                        "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, sistema_usuario=:sistemaUsuario WHERE usuario=:usuario", usuarioDesactivado);
                    ret = true;
                }
            }
            catch (Exception e) {
                CLogger.write("13", "UsuarioDAO", e);
		}
		
		return ret;
	}

        /*public static UnidadEjecutora getUnidadEjecutora(String usuario)
    {
        UnidadEjecutora ret = new UnidadEjecutora();
        Session session = CHibernateSession.getSessionFactory().openSession();
        List<UnidadEjecutora> unidades = new ArrayList<UnidadEjecutora>();
        String consulta = "Select u FROM UnidadEjecutora u, RolUsuarioProyecto r, Proyecto p where p.id =r.id.proyecto and p.unidadEjecutora.id = u.id and r.id.usuario =:usuario ";
        try
        {
            session.beginTransaction();
            Query<UnidadEjecutora> criteria = session.createQuery(consulta, UnidadEjecutora.class);
                criteria.setParameter("usuario", usuario);
                unidades = criteria.getResultList();
                if(unidades.size()>0){
                    ret = unidades.get(0);
                }

            }
            catch(Throwable e){
                CLogger.write("14", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }				
            return ret;
        }

        public static Cooperante getCooperantePorUsuario(String usuario)
    {
        Cooperante ret = new Cooperante();
        Session session = CHibernateSession.getSessionFactory().openSession();
        List<Cooperante> unidades = new ArrayList<Cooperante>();
        String consulta = "Select u FROM Cooperante u, RolUsuarioProyecto r, Proyecto p where r.id.proyecto=p.id and p.cooperante.id=u.id and r.id.usuario =:usuario ";
        try
        {
            session.beginTransaction();
            Query<Cooperante> criteria = session.createQuery(consulta, Cooperante.class);
                criteria.setParameter("usuario", usuario);
                unidades = criteria.getResultList();
                if(unidades.size()>0){
                    ret = unidades.get(0);
                }

            }
            catch(Throwable e){
                CLogger.write("15", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static int getRolPorUsuario(String usuario)
    {
        int ret = 0;
        Session session = CHibernateSession.getSessionFactory().openSession();
        List<Integer> unidades = new ArrayList<Integer>();
        String consulta = "Select r.id.rol FROM  RolUsuarioProyecto r where r.id.usuario =:usuario ";
        try
        {
            session.beginTransaction();
            Query<Integer> criteria = session.createQuery(consulta, Integer.class);
                criteria.setParameter("usuario", usuario);
                unidades = criteria.getResultList();
                if(unidades.size()>0){
                    ret = unidades.get(0);
                }
            }
            catch(Throwable e){
                CLogger.write("16", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }*/

        public static bool editarUsuario(Usuario usuario, String usuarioActualizo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    usuario.usuarioActualizo = usuarioActualizo;
                    usuario.fechaActualizacion = DateTime.Now;
                    db.Query<Usuario>("UPDATE USUARIO SET password=:password, salt=:salt, email=:email, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                        "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, sistema_usuario=:sistemaUsuario WHERE usuario=:usuario", usuario);
                    ret = true;
                }
            }
            catch (Exception e)
            {
                CLogger.write("17", "UsuarioDAO", e);
            }
            return ret;
        }

        public static List<UsuarioPermiso> getPermisosActivosUsuario(String usuario)
        {
            List<UsuarioPermiso> ret = new List<UsuarioPermiso>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<UsuarioPermiso>("SELECT * FROM USUARIO_PERMISO WHERE usuariousuario=:usuario", new { usuario = usuario }).AsList<UsuarioPermiso>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("17", "UsuarioDAO", e);
            }
            return ret;
        }

        /*public static List<ProyectoUsuario> getPrestamosAsignadosPorUsuario(String usuario)
    {
        List<ProyectoUsuario> ret = new ArrayList<ProyectoUsuario>();
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();
            Query<ProyectoUsuario> criteria = session.createQuery("FROM ProyectoUsuario where id.usuario=:usuario", ProyectoUsuario.class);
                criteria.setParameter("usuario", usuario);
                ret = criteria.getResultList();
            }catch(Throwable e){
                CLogger.write("18", UsuarioDAO.class, e);
            }finally{
                session.close();
            }
            return ret;
        }*/

        public static List<Permiso> getPermisosDisponibles(String usuario)
        {
            List<Permiso> ret = new List<Permiso>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Permiso>("SELECT * FROM PERMISO WHERE id not in (SELECT permisoid FROM USUARIO_PERMISO WHERE usuariousuario=:usuario)", new { usuario = usuario }).AsList<Permiso>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "UsuarioDAO", e);
            }
            return ret;
        }

        public static List<Usuario> getUsuarios(int pagina, int numeroUsuarios, String usuario, String email, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            List<Usuario> ret = new List<Usuario>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM USUARIO WHERE estado=:estado";
                    string query_a = "";
                    if (usuario != null && usuario.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, " usuario LIKE :usuario");
                    if (email != null && email.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " email LIKE :email ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND ", query_a, "") : ""));
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroUsuarios + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroUsuarios + ") + 1)");
                    ret = db.Query<Usuario>(query, new { usuario = usuario, email = email, estado = 1, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion }).AsList<Usuario>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("20", "UsuarioDAO", e);
            }
            return ret;
        }

        public static long getTotalUsuarios(String usuario, String email, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT count(usuario) FROM Usuario WHERE estado=1";
                    string query_a = "";
                    if (usuario != null && usuario.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, " usuario LIKE :usuario");
                    if (email != null && email.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " email LIKE :email ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND ", query_a, "") : ""));
                    ret = db.ExecuteScalar<long>(query, new { usuario = usuario, email = email, estado = 1, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("21", "UsuarioDAO", e);
            }
            return ret;
        }

        /*public static Colaborador getColaborador(String usuario)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        Colaborador ret = null;
        try
        {
            Query<Colaborador> criteria = session.createQuery("FROM Colaborador where usuariousuario =:usuario", Colaborador.class);
                criteria.setParameter("usuario",usuario);
                List<Colaborador> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? listRet.get(0) : null;
            } catch (Throwable e) {
                CLogger.write("22", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }*/

        public static List<Usuario> getUsuariosDisponibles()
        {
            List<Usuario> ret = new List<Usuario>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Usuario>("SELECT * FROM USUARIO WHERE estado=1").AsList<Usuario>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("23", "UsuarioDAO", e);
            }
            return ret;
        }

        /*public static List<Proyecto> getPrestamosPorElemento(int elemento, int id_elemento, String usuario)
    {
        String busqueda = "";
        if (elemento == 4 || elemento == 5)
        {
            busqueda = "Select p FROM Proyecto p, ProyectoUsuario u where p.unidadEjecutora.unidadEjecutora =:id and p.estado=1 and p.id =u.id.proyectoid and u.id.usuario=:usuario";
        }
        else if (elemento == 6)
        {
            busqueda = "Select p FROM Proyecto p, ProyectoUsuario u where p.cooperante.id =:id and p.estado=1 and p.id =u.id.proyectoid and u.id.usuario=:usuario";
        }
        else
        {
            busqueda = "Select p FROM Proyecto p where  p.estado=1";
        }
        List<Proyecto> ret = new ArrayList<Proyecto>();

        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            Query<Proyecto> criteria = session.createQuery(busqueda, Proyecto.class);
                if(elemento>=4){
                    criteria.setParameter("id",id_elemento);
                    criteria.setParameter("usuario",usuario);
                }
                ret =criteria.getResultList();
            }catch(Throwable e){
                CLogger.write("24", UsuarioDAO.class, e);
            }finally{
                session.close();
            }
            return ret;
        }


        public static List<RolUsuarioProyecto> getUsuariosPorPrestamo(int proyecto)
    {
        List<RolUsuarioProyecto> ret = new ArrayList<RolUsuarioProyecto>();

        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            Query<RolUsuarioProyecto> criteria = session.createQuery("FROM RolUsuarioProyecto where id.proyecto=:proyecto", RolUsuarioProyecto.class);
                criteria.setParameter("proyecto",proyecto);
                ret =criteria.getResultList();
            }catch(Throwable e){
                CLogger.write("25", UsuarioDAO.class, e);
            }finally{
                session.close();
            }
            return ret;
        }

        public static boolean asignarComponentes(String usuario, List<Integer> componentes, String usuario_creo)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();

            for (int i = 0; i < componentes.size(); i++)
            {
                Componente componente = ComponenteDAO.getComponente(componentes.get(i));
                ComponenteUsuario cu = new ComponenteUsuario(new ComponenteUsuarioId(componentes.get(i), usuario), componente, UsuarioDAO.getUsuario(componente.getUsuarioCreo()));
                cu.setComponente(componente);
                session.save(cu);
            }
            session.getTransaction().commit();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("26", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean asignarSubComponentes(String usuario, List<Integer> subcomponentes, String usuario_creo)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();

            for (int i = 0; i < subcomponentes.size(); i++)
            {
                Subcomponente subcomponente = SubComponenteDAO.getSubComponente(subcomponentes.get(i));
                SubcomponenteUsuario cu = new SubcomponenteUsuario(new SubcomponenteUsuarioId(subcomponentes.get(i), usuario), subcomponente);
                cu.setSubcomponente(subcomponente);
                session.save(cu);
            }
            session.getTransaction().commit();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("27", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean checkUsuarioPrestamo(String usuario, int prestmoid)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        boolean ret = false;
        try
        {
            Query<PrestamoUsuario> criteria = session.createQuery("FROM PrestamoUsuario where usuario.usuario=:usuario and prestamo.id=:id", PrestamoUsuario.class);
                criteria.setParameter("usuario",usuario);
                criteria.setParameter("id", prestmoid);
                List<PrestamoUsuario> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? true : false;
            } catch (Throwable e) {
                CLogger.write("28", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static boolean checkUsuarioProyecto(String usuario, int proyectoid)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        boolean ret = false;
        try
        {
            Query<ProyectoUsuario> criteria = session.createQuery("FROM ProyectoUsuario where usuario.usuario=:usuario and proyecto.id=:id", ProyectoUsuario.class);
                criteria.setParameter("usuario",usuario);
                criteria.setParameter("id", proyectoid);
                List<ProyectoUsuario> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? true : false;
            } catch (Throwable e) {
                CLogger.write("28", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static boolean checkUsuarioComponente(String usuario, int componenteid)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        boolean ret = false;
        try
        {
            Query<ComponenteUsuario> criteria = session.createQuery("FROM ComponenteUsuario where id.usuario=:usuario and id.componenteid=:id", ComponenteUsuario.class);
                criteria.setParameter("usuario",usuario);
                criteria.setParameter("id", componenteid);
                List<ComponenteUsuario> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? true : false;
            } catch (Throwable e) {
                CLogger.write("29", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static boolean checkUsuarioSubComponente(String usuario, int subcomponenteid)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        boolean ret = false;
        try
        {
            Query<SubcomponenteUsuario> criteria = session.createQuery("FROM SubcomponenteUsuario where id.usuario=:usuario and id.subcomponenteid=:id", SubcomponenteUsuario.class);
                criteria.setParameter("usuario",usuario);
                criteria.setParameter("id", subcomponenteid);
                List<SubcomponenteUsuario> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? true : false;
            } catch (Throwable e) {
                CLogger.write("30", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }

        public static boolean checkUsuarioProducto(String usuario, int productoid)
    {
        Session session = CHibernateSession.getSessionFactory().openSession();
        boolean ret = false;
        try
        {
            Query<ProductoUsuario> criteria = session.createQuery("FROM ProductoUsuario where id.usuario=:usuario and id.productoid=:id", ProductoUsuario.class);
                criteria.setParameter("usuario",usuario);
                criteria.setParameter("id", productoid);
                List<ProductoUsuario> listRet = null;
    listRet = criteria.getResultList();
                ret = !listRet.isEmpty()? true : false;
            } catch (Throwable e) {
                CLogger.write("31", UsuarioDAO.class, e);
            } finally {
                session.close();
            }
            return ret;
        }


        public static boolean asignarProductos(String usuario, List<Integer> productos, String usuario_creo)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();

            for (int i = 0; i < productos.size(); i++)
            {
                Producto producto = ProductoDAO.getProductoPorId(productos.get(i));
                ProductoUsuario uu = new ProductoUsuario(new ProductoUsuarioId(productos.get(i), usuario), producto, UsuarioDAO.getUsuario(usuario));
                session.save(uu);
            }
            session.getTransaction().commit();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("32", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }

        public static boolean desasignarEstructurasPermisos(String usuario)
    {
        boolean ret = false;
        Session session = CHibernateSession.getSessionFactory().openSession();
        try
        {
            session.beginTransaction();
            Query <?> criteria_prs = session.createQuery("delete PrestamoUsuario where id.usuario=:usuario ");
            criteria_prs.setParameter("usuario", usuario);
            criteria_prs.executeUpdate();
            Query <?> criteria = session.createQuery("delete ProyectoUsuario where id.usuario=:usuario ");
            criteria.setParameter("usuario", usuario);
            criteria.executeUpdate();
            Query <?> criteria_c = session.createQuery("delete ComponenteUsuario where id.usuario=:usuario ");
            criteria_c.setParameter("usuario", usuario);
            criteria_c.executeUpdate();
            Query <?> criteria_sc = session.createQuery("delete SubcomponenteUsuario where id.usuario=:usuario ");
            criteria_sc.setParameter("usuario", usuario);
            criteria_sc.executeUpdate();
            Query <?> criteria_u = session.createQuery("delete ProductoUsuario where id.usuario=:usuario ");
            criteria_u.setParameter("usuario", usuario);
            criteria_u.executeUpdate();
            session.getTransaction().commit();
            //			session.flush();
            ret = true;
        }
        catch (Throwable e)
        {
            CLogger.write("33", UsuarioDAO.class, e);
            }
            finally{
                session.close();
            }

            return ret;
        }*/

        public static bool desasignarPermisos(String usuario)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eleminados = db.Execute("DELETE USUARIO_PERMISO WHERE usuariousuario=:usuario", new { usuario = usuario });
                    ret = eleminados >= 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("34", "UsuarioDAO", e);
            }

            return ret;
        }


        public static Usuario setNuevoPassword(Usuario usuario, String password)
        {
            try
            {
                if (usuario != null)
                {
                    using (DbConnection db = new OracleContext().getConnection())
                    {
                        string[] hashedData = Utilities.Identity.SHA256Hasher.ComputeHash(password);
                        usuario.password = hashedData[0];
                        usuario.salt = hashedData[1];
                        usuario.fechaCreacion = DateTime.Now;

                        db.Query<Usuario>("UPDATE USUARIO SET password=:password, salt=:salt, email=:email, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, sistema_usuario=:sistemaUsuario WHERE usuario=:usuario", usuario);                      
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("35", "UsuarioDAO", e);
            }
            return usuario;
        }
    }
}
