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
        public static List<Componente> getComponentes(String usuario)
        {
            List<Componente> ret = new List<Componente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM componente p WHERE p.estado=1 AND p.id IN (SELECT u.componenteid FROM componente_usuario u WHERE u.usuario=:usuario)");
                    ret = db.Query<Componente>(query, new { usuario = usuario }).AsList<Componente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static Componente getComponentePorId(int id, String usuario)
        {
            Componente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String Str_query = String.Join(" ", "SELECT * FROM componente c WHERE c.id=:id");
                    if (usuario != null)
                    {
                        Str_query = String.Join(" ", Str_query, "AND id in (SELECT u.componenteid FROM componente_usuario u WHERE u.usuario=:usuario)");
                    }

                    ret = db.QueryFirstOrDefault<Componente>(Str_query, new { id = id, usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponenteDAO.class", e);
            }
            return ret;
        }

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
                        "latitud=:latitud, longitud=:longitud, costo=:costo, acumulacion_costoid=:acumulacionCostoid, renglon=:renglon, ubicacion_geografica=:ubicacionGeografica, " +
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
                            ret = guardado > 0 ? true : false;
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO COMPONENTE_USUARIO VALUES (:componenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", cu);
                            ret = guardado > 0 ? true : false;
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
                                ret = guardado > 0 ? true : false;
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO COMPONENTE_USUARIO VALUES (:componenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", cu_admin);
                                ret = guardado > 0 ? true : false;
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

        public static bool eliminarComponente(Componente Componente)
        {
            bool ret = false;
            try
            {
                Componente.estado = 0;
                Componente.orden = null;
                Componente.fechaActualizacion = DateTime.Now;
                ret = guardarComponente(Componente, false);
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalComponente(Componente Componente)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM componente WHERE id=:id", new { id = Componente.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Componente> getComponentesPagina(int pagina, int numeroComponentes, String usuario)
        {
            List<Componente> ret = new List<Componente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT c.* FROM componente c WHERE c.estado = 1 AND c.id in ",
                        "(SELECT u.componenteid FROM componente_usuario u WHERE u.usuario=:usuario)");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroComponentes + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroComponentes + ") + 1)");

                    ret = db.Query<Componente>(query, new { usuario = usuario }).AsList<Componente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static long getTotalComponentes(String usuario)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT COUNT(*) FROM componente c WHERE c.estado=1 AND c.id in ",
                        "(SELECT u.componenteid FROM componente_usuario u WHERE u.usuario=:usuario)");
                    ret = db.ExecuteScalar<long>(query, new { usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Componente> getComponentesPaginaPorProyecto(int pagina, int numeroComponentes, int proyectoId,
                String filtro_busqueda, String columna_ordenada, String orden_direccion, String usuario)
        {

            List<Componente> ret = new List<Componente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM componente c WHERE estado = 1 AND c.proyectoid=:proyectoId ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join("", query, " AND  c.id in (SELECT u.componenteid FROM Componente_usuario u WHERE u.usuario=:usuario ) ");
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroComponentes + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroComponentes + ") + 1)");

                    ret = db.Query<Componente>(query, new { proyectoId = proyectoId, usuario = usuario }).AsList<Componente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static long getTotalComponentesPorProyecto(int proyectoId, String filtro_busqueda, String usuario)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM componente c WHERE c.estado=1 AND c.proyectoid=:proyectoId";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join("", query, " AND  c.id in (SELECT u.componenteid FROM componente_usuario u WHERE u.usuario=:usuario )");

                    ret = db.ExecuteScalar<long>(query, new { proyectoId = proyectoId, usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "ComponenteDAO.class", e);
            }
            return ret;
        }

        /*public static Componente getComponenteInicial(Integer proyectoId, String usuario, Session session){
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

        /*public static Componente getComponenteFechaMaxima(Integer proyectoId, String usuario, Session session){
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

        public static decimal calcularCosto(Componente componente)
        {
            decimal costo = decimal.Zero;
            try
            {
                List<Producto> productos = ProductoDAO.getProductosByComponente(componente.id);
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(componente.id, 2);
                if ((productos != null && productos.Count > 0) || (actividades != null && actividades.Count > 0))
                {
                    if (productos != null)
                    {
                        foreach (Producto producto in productos)
                        {
                            costo += producto.costo ?? decimal.Zero;
                        }
                    }

                    if (actividades != null && actividades.Count > 0)
                    {
                        foreach (Actividad actividad in actividades)
                        {
                            costo += actividad.costo ?? decimal.Zero;
                        }
                    }
                }
                else
                {
                    PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(2, componente.id);
                    if (pa != null)
                    {
                        List<PlanAdquisicionPago> lstpagos = PlanAdquisicionPagoDAO.getPagosByObjetoTipo(2, componente.id);
                        if (lstpagos != null && lstpagos.Count > 0)
                        {
                            decimal pagos = decimal.Zero;
                            foreach (PlanAdquisicionPago pago in lstpagos)
                                pagos += pago.pago ?? default(decimal);
                            costo = pagos;
                        }
                        else
                            costo = pa.montoContrato;
                    }
                    else
                        costo = componente.costo ?? default(decimal);
                }
            }
            catch (Exception e)
            {
                CLogger.write("16", "Proyecto.class", e);
            }

            return costo;
        }

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

        public static bool calcularCostoyFechas(int componenteId){
            bool ret = false;
            List<List<Nodo>> listas = EstructuraProyectoDAO.getEstructuraObjetoArbolCalculos(componenteId, 2);
            for(int i=listas.Count-2; i>=0; i--){
                for(int j=0; j<listas[i].Count; j++){
                    Nodo nodo = listas[i][j];
                    decimal costo = decimal.Zero;
                    DateTime fecha_maxima = new DateTime();
                    DateTime fecha_minima =new DateTime(new DateTime(2999,12,31,0,0,0).Ticks);
                    foreach (Nodo nodo_hijo in nodo.children)
                    { 
                        costo += nodo_hijo.costo;
                        fecha_minima = (nodo_hijo.fecha_inicio.TimeOfDay < fecha_minima.TimeOfDay) ? nodo_hijo.fecha_inicio : fecha_minima;
                        fecha_maxima = (nodo_hijo.fecha_fin.TimeOfDay > fecha_maxima.TimeOfDay) ? nodo_hijo.fecha_fin : fecha_maxima;
                    }
                    nodo.objeto = ObjetoDAO.getObjetoPorIdyTipo(nodo.id, nodo.objeto_tipo);
                    if(nodo.children!=null && nodo.children.Count>0){
                        nodo.fecha_inicio = fecha_minima;
                        nodo.fecha_fin = fecha_maxima;
                        nodo.costo = costo;
                    }
                    else
                        nodo.costo = calcularCosto((Componente)nodo.objeto);
                    nodo.duracion = Utils.getWorkingDays(nodo.fecha_inicio, nodo.fecha_fin);
                    setDatosCalculados(nodo.objeto,nodo.fecha_inicio,nodo.fecha_fin, nodo.costo, nodo.duracion);
                }
                ret = true;
            }
            ret= ret && guardarComponenteBatch(listas);	
            return ret;
        }

        private static void setDatosCalculados(Object objeto, DateTime fecha_inicio, DateTime fecha_fin, decimal costo, int duracion)
        {
            try
            {
                if (objeto != null)
                {
                    var setFechaInicio = objeto.GetType().GetProperty("fechaInicio");
                    var setFechaFin = objeto.GetType().GetProperty("fechaFin");
                    var setCosto = objeto.GetType().GetProperty("costo");
                    var setDuracion = objeto.GetType().GetProperty("duracion");
                    setFechaInicio.SetValue(objeto, fecha_inicio);
                    setFechaFin.SetValue(objeto, fecha_fin);
                    setCosto.SetValue(objeto, costo);
                    setDuracion.SetValue(objeto, duracion);
                }
            }
            catch (Exception e)
            {
                CLogger.write("20", "ComponenteDAO.class", e);
            }
        }

        private static bool guardarComponenteBatch(List<List<Nodo>> listas)
        {
            bool ret = true;
            try
            {
                for (int i = 0; i < listas.Count - 1; i++)
                {
                    for (int j = 0; j < listas[i].Count; j++)
                    {
                        switch (listas[i][j].objeto_tipo)
                        {
                            case 1:
                                ComponenteDAO.guardarComponente((Componente)listas[i][j].objeto, false);
                                break;
                            case 2:
                                SubComponenteDAO.guardarSubComponente((Subcomponente)listas[i][j].objeto, false);
                                break;
                            case 3:
                                ProductoDAO.guardarProducto((Producto)listas[i][j].objeto, false);
                                break;
                            case 4:
                                SubproductoDAO.guardarSubproducto((Subproducto)listas[i][j].objeto, false);
                                break;
                            case 5:
                                ActividadDAO.guardarActividad((Actividad)listas[i][j].objeto, false);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                CLogger.write("21", "ComponenteDAO.class", e);
            }
            return ret;
        }

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
                    + "FROM COMPONENTE c WHERE c.proyectoid = :proyId "
                    + "AND c.componente_sigadeid = :compSigId "
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

        public static Componente getComponenteHistory(int componenteId, String lineaBase)
        {
            Componente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    string query = String.Join(" ", "SELECT * FROM componente c WHERE c.estado = 1 AND c.id=:componenteId ",
                        lineaBase != null ? "AND c.linea_base like '%" + lineaBase + "%'" : "AND c.actual = 1 ORDER BY c.id desc");
                    ret = db.QueryFirstOrDefault<Componente>(query, new { componenteId = componenteId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("20", "ComponenteDAO.class", e);
            }
            return ret;
        }

        public static String getVersiones(int componenteId)
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT DISTINCT(version) FROM componente "
                        + " WHERE id=" + componenteId;

                    List<dynamic> versiones = db.Query<dynamic>(query).AsList<dynamic>();

                    if (versiones != null)
                    {
                        for (int i = 0; i < versiones.Count; i++)
                        {
                            if (resultado.Length > 0)
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
                CLogger.write("21", "ComponenteDAO.class", e);
            }
            return resultado;
        }

        public static String getHistoria(int componenteId, int version)
        {
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
                    + " FROM componente c "
                    + " INNER JOIN sipro.unidad_ejecutora ue ON c.unidad_ejecutoraunidad_ejecutora = ue.unidad_ejecutora and c.entidad = ue.entidadentidad and c.ejercicio = ue.ejercicio  JOIN sipro_history.componente_tipo ct ON c.componente_tipoid = ct.id "
                    + " LEFT JOIN acumulacion_costo ac ON c.acumulacion_costoid = ac.id "
                    + " WHERE c.id = " + componenteId
                    + " AND c.version = " + version;

            String[] campos = {"Version", "Nombre", "DescripciÃ³n", "Tipo", "Unidad Ejecutora", "Monto Planificado", "Tipo AcumulaciÃ³n de Monto Planificado",
                "Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "UbicaciÃ³n GeogrÃ¡fica", "Latitud", "Longitud",
                "Fecha Inicio", "Fecha Fin", "DuraciÃ³n", "Fecha Inicio Real", "Fecha Fin Real",
                "Fuente PrÃ©stamo", "Fuente DonaciÃ³n", "Fuente Nacional",
                "Fecha CreaciÃ³n", "Usuario que creo", "Fecha ActualizaciÃ³n", "Usuario que actualizÃ³",
                "Estado"};
            resultado = CHistoria.getHistoria(query, campos);
            return resultado;
        }
    }
}
