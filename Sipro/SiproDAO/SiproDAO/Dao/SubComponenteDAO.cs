using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubComponenteDAO
    {
        public static List<Subcomponente> getSubComponentes(String usuario)
        {
            List<Subcomponente> ret = new List<Subcomponente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM subcomponente p WHERE estado = 1",
                        "AND p.id in (SELECT u.subcomponenteid FROM subcomponente_usuario u WHERE u.usuario=:usuario )");
                    ret = db.Query<Subcomponente>(query, new { usuario = usuario }).AsList<Subcomponente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static Subcomponente getSubComponentePorId(int id, String usuario)
        {
            Subcomponente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM subcomponente WHERE id=:id",
                        usuario != null ? "AND id IN (SELECT u.subcomponenteid FROM subcomponente_usuario u WHERE u.usuario=:usuario)" : "");

                    ret = db.QueryFirstOrDefault<Subcomponente>(query, new { id = id, usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubComponente(Subcomponente SubComponente, bool calcular_valores_agregados)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    if (SubComponente.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_subcomponente.nextval FROM DUAL");
                        SubComponente.id = sequenceId;
                        guardado = db.Execute("INSERT INTO SUBCOMPONENTE VALUES (:id, :nombre, :descripcion, :componenteid, :subcomponenteTipoid, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :ueunidadEjecutora, :snip, :programa, :subprograma, :proyecto, :actividad, " +
                            ":obra, :latitud, :longitud, :costo, :acumulacionCostoid, :renglon, :ubicacionGeografica, :fechaInicio, :fechaFin, :duracion, :duracionDimension, " +
                            ":orden, :treePath, :nivel, :entidad, :ejercicio, :fechaInicioReal, :fechaFinReal, :inversionNueva)", SubComponente);

                        if (guardado > 0)
                        {
                            Componente componente = ComponenteDAO.getComponente(SubComponente.componenteid);
                            SubComponente.componentes = componente;
                            SubComponente.treepath = SubComponente.componentes.treepath + "" + (10000000 + SubComponente.id);
                        }
                    }

                    guardado = db.Execute("UPDATE SUBCOMPONENTE SET nombre=:nombre, descripcion=:descripcion, componenteid=:componenteid, subcomponente_tipoid=:subcomponenteTipoid, " +
                        "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, " +
                        "ueunidad_ejecutora=:ueunidadEjecutora, snip=:snip, programa=:programa, subprograma=:subprograma, proyecto=:proyecto, actividad=:actividad, obra=:obra, " +
                        "latitud=:latitud, longitud=:longitud, costo=:costo, acumulacion_costoid=:acumulacionCostoid, renglon=:renglon, ubicacion_geografica=:ubicacionGeografica, " +
                        "fecha_inicio=:fechaInicio, fecha_fin=:fechaFin, duracion=:duracion, duracion_dimension=:duracionDimension, orden=:orden, treePath=:treePath, nivel=:nivel, " +
                        "entidad=:entidad, ejercicio=:ejercicio, fecha_inicio_real=:fechaInicioReal, fecha_fin_real=:fechaFinReal, inversion_nueva=:inversionNueva WHERE id=:id", SubComponente);

                    if (guardado > 0)
                    {
                        SubcomponenteUsuario cu = new SubcomponenteUsuario();
                        cu.subcomponentes = SubComponente;
                        cu.subcomponenteid = SubComponente.id;
                        cu.usuario = SubComponente.usuarioCreo;
                        cu.usuarioCreo = SubComponente.usuarioCreo;

                        int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM SUBCOMPONENTE_USUARIO WHERE subcomponenteid=:id AND usuario=:usuario", new { id = cu.subcomponenteid, usuario = cu.usuario });

                        if (existe > 0)
                        {
                            guardado = db.Execute("UPDATE SUBCOMPONENTE_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                "fecha_actualizacion=:fechaActualizacion WHERE subcomponenteid=:subcomponenteid AND usuario=:usuario", cu);
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO SUBCOMPONENTE_USUARIO VALUES (:subcomponenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                                ":fechaActualizacion)", cu);
                        }

                        if (guardado > 0 && SubComponente.usuarioCreo.Equals("admin"))
                        {
                            SubcomponenteUsuario cu_admin = new SubcomponenteUsuario();
                            cu_admin.subcomponentes = SubComponente;
                            cu_admin.subcomponenteid = SubComponente.id;
                            cu_admin.usuario = "admin";
                            cu_admin.usuarioCreo = SubComponente.usuarioCreo;

                            existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM SUBCOMPONENTE_USUARIO WHERE subcomponenteid=:id AND usuario=:usuario", new { id = cu_admin.subcomponenteid, usuario = cu.usuario });

                            if (existe > 0)
                            {
                                guardado = db.Execute("UPDATE SUBCOMPONENTE_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE subcomponenteid=:subcomponenteid AND usuario=:usuario", cu_admin);
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO SUBCOMPONENTE_USUARIO VALUES (:subcomponenteid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", cu_admin);
                            }
                        }

                        if (guardado > 0 && calcular_valores_agregados)
                        {
                            ProyectoDAO.calcularCostoyFechas(Convert.ToInt32(SubComponente.treepath.Substring(0, 8)) - 10000000);
                        }

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarSubComponente(Subcomponente subComponente){
            bool ret = false;
            try{
                subComponente.estado = 0;
                subComponente.orden = null;                
                ret = guardarSubComponente(subComponente, false);
            }
            catch(Exception e){
                CLogger.write("4", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalSubComponente(Subcomponente subComponente)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM subcomponente WHERE id=:id", new { id = subComponente.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Subcomponente> getSubComponentesPagina(int pagina, int numeroSubComponentes, String usuario)
        {
            List<Subcomponente> ret = new List<Subcomponente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subcomponente c WHERE c.estado = 1",
                        "AND c.id in (SELECT u.subcomponenteid FROM subcomponente_usuario u WHERE u.usuario=:usuario)");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroSubComponentes + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroSubComponentes + ") + 1)");

                    ret = db.Query<Subcomponente>(query, new { usuario = usuario }).AsList<Subcomponente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static long getTotalSubComponentes(String usuario)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM subcomponente c WHERE c.estado=1 " +
                        "AND c.id IN (SELECT u.subcomponenteid FROM Subcomponente_usuario u where u.usuario=:usuario)", new { usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Subcomponente> getSubComponentesPaginaPorComponente(int pagina, int numeroSubComponentes, int componenteId, String filtro_busqueda, 
            String columna_ordenada, String orden_direccion, String usuario)
        {

            List<Subcomponente> ret = new List<Subcomponente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subcomponente c WHERE c.estado = 1 AND c.componenteid = :compId ";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join("", query, " AND  c.id IN (SELECT u.subcomponenteid FROM subcomponente_usuario u WHERE u.usuario=:usuario) ");
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroSubComponentes + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroSubComponentes + ") + 1)");

                    ret = db.Query<Subcomponente>(query, new { compId = componenteId, usuario = usuario }).AsList<Subcomponente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubComponenteDAO.class", e);
            }
            return ret;
        }


        public static long getTotalSubComponentesPorComponente(int componenteId, String filtro_busqueda, String usuario)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM subcomponente c WHERE c.estado=1 AND c.componenteid = :compId ";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join("", query, " AND  c.id IN (SELECT u.subcomponenteid FROM subcomponente_usuario u WHERE u.usuario=:usuario )");
                    ret = db.ExecuteScalar<long>(query, new { compId = componenteId, usuario = usuario });
                }

            }
            catch (Exception e)
            {
                CLogger.write("9", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        /*public static Subcomponente getSubComponenteInicial(Integer componenteId, String usuario, Session session){
            Subcomponente ret = null;
            List<Subcomponente> listRet = null;
            try{
                String query = "FROM Subcomponente c where c.estado=1 and c.orden=1 and c.componente.id=:componenteId and c.usuarioCreo=:usuario";
                Query<Subcomponente> criteria = session.createQuery(query, Subcomponente.class);
                criteria.setParameter("componenteId", componenteId);
                criteria.setParameter("usuario", usuario);
                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("10", SubComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        /*public static Subcomponente getSubComponenteFechaMaxima(Integer componenteId, String usuario, Session session){
            Subcomponente ret = null;
            List<Subcomponente> listRet = null;
            try{
                String query = "FROM Subcomponente c where c.estado=1 and c.componente.id=:componenteId and c.usuarioCreo=:usuario order by c.fechaFin desc";
                Query<Subcomponente> criteria = session.createQuery(query, Subcomponente.class);
                criteria.setMaxResults(1);
                criteria.setParameter("componenteId", componenteId);
                criteria.setParameter("usuario", usuario);

                listRet = criteria.getResultList();

                ret = !listRet.isEmpty() ? listRet.get(0) : null;
            }catch (NoResultException e){

            } catch(Throwable e){
                CLogger.write("11", SubComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static List<Subcomponente> getSubComponentesOrden(Integer componenteId, String usuario, Session session){
            List<Subcomponente> ret = null;
            try{
                String query = String.join(" ", "SELECT c FROM Subcomponente c where c.estado=1 and c.componente.id=:componenteId");
                query = String.join(" ", query, "AND c.id in (SELECT u.id.subcomponenteid from SubcomponenteUsuario u where u.id.usuario=:usuario)");
                Query<Subcomponente> criteria = session.createQuery(query,Subcomponente.class);
                criteria.setParameter("componenteId", componenteId);
                criteria.setParameter("usuario", usuario);
                ret = criteria.getResultList();
            }catch(Throwable e){
                CLogger.write("12", SubComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static Subcomponente getSubComponentePorIdOrden(int id, String usuario, Session session){
            Subcomponente ret = null;
            List<Subcomponente> listRet = null;
            try{
                Query<Subcomponente> criteria = session.createQuery("FROM Subcomponente where id=:id AND id in (SELECT u.id.subcomponenteid from SubcomponenteUsuario u where u.id.usuario=:usuario )", Subcomponente.class);
                criteria.setParameter("id", id);
                criteria.setParameter("usuario", usuario);
                listRet = criteria.getResultList();

                 ret = !listRet.isEmpty() ? listRet.get(0) : null;
            } catch(Throwable e){
                CLogger.write("13", SubComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }

        public static boolean guardarSubComponenteOrden(Subcomponente SubComponente, Session session){
            boolean ret = false;
            try{
                session.saveOrUpdate(SubComponente);
                session.flush();
                session.clear();
                ret = true;
            }
            catch(Throwable e){
                CLogger.write("14", SubComponenteDAO.class, e);
                session.getTransaction().rollback();
                session.close();
            }
            return ret;
        }*/

        public static Subcomponente getSubComponente(int id)
        {
            Subcomponente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Subcomponente>("SELECT * FROM subcomponente WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("15", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static decimal calcularCosto(Subcomponente subcomponente)
        {
            decimal costo = decimal.Zero;
            try
            {
                List<Producto> productos = ProductoDAO.getProductosBySubComponente(subcomponente.id);
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(subcomponente.id, 2);
                if ((productos != null && productos.Count > 0) || (actividades != null && actividades.Count > 0))
                {
                    if (productos != null)
                    {
                        foreach (Producto producto in productos)
                        {
                            costo += Decimal.Add(costo, producto.costo ?? default(decimal));
                        }
                    }

                    if (actividades != null && actividades.Count > 0)
                    {
                        foreach (Actividad actividad in actividades)
                        {
                            costo += Decimal.Add(costo, actividad.costo ?? default(decimal));
                        }
                    }
                }
                else
                {
                    PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(2, subcomponente.id);
                    if (pa != null)
                    {
                        List<PlanAdquisicionPago> lstpagos = PlanAdquisicionPagoDAO.getPagosByPlan(Convert.ToInt32(pa.id));
                        if (lstpagos != null && lstpagos.Count > 0)
                        {
                            decimal pagos = decimal.Zero;
                            foreach (PlanAdquisicionPago pago in lstpagos)
                                pagos += Decimal.Add(pagos, pago.pago ?? default(decimal));
                            costo = pagos;
                        }
                        else
                            costo = pa.montoContrato;
                    }
                    else
                        costo = subcomponente.costo ?? default(decimal);
                }
            }
            catch (Exception e)
            {
                CLogger.write("16", "Subcomponente.class", e);
            }

            return costo;
        }

        public static List<Subcomponente> getSubComponentesPorComponente(int componenteId)
        {
            List<Subcomponente> ret = new List<Subcomponente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Subcomponente>("SELECT * FROM SUBCOMPONENTE c WHERE c.estado=1 AND c.componenteid=:componenteId ORDER BY c.id ASC",
                        new { componenteId = componenteId }).AsList<Subcomponente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static bool calcularCostoyFechas(int subcomponenteId){
            bool ret = false;
            try
            {
                List<List<Nodo>> listas = EstructuraProyectoDAO.getEstructuraObjetoArbolCalculos(subcomponenteId, 2);
                for (int i = listas.Count - 2; i >= 0; i--)
                {
                    for (int j = 0; j < listas[i].Count; j++)
                    {
                        Nodo nodo = listas[i][j];
                        decimal costo = decimal.Zero;
                        DateTime fecha_maxima = new DateTime(0);
                        DateTime fecha_minima = new DateTime(new DateTime(2999, 12, 31, 0, 0, 0).Ticks);
                        foreach (Nodo nodo_hijo in nodo.children)
                        {
                            costo += nodo_hijo.costo;
                            fecha_minima = (nodo_hijo.fecha_inicio.TimeOfDay < fecha_minima.TimeOfDay) ? nodo_hijo.fecha_inicio : fecha_minima;
                            fecha_maxima = (nodo_hijo.fecha_fin.TimeOfDay > fecha_maxima.TimeOfDay) ? nodo_hijo.fecha_fin : fecha_maxima;
                        }
                        nodo.objeto = ObjetoDAO.getObjetoPorIdyTipo(nodo.id, nodo.objeto_tipo);
                        if (nodo.children != null && nodo.children.Count > 0)
                        {
                            nodo.fecha_inicio = fecha_minima;
                            nodo.fecha_fin = fecha_maxima;
                            nodo.costo = costo;
                        }
                        else
                            nodo.costo = calcularCosto((Subcomponente)nodo.objeto);
                        nodo.duracion = Utils.getWorkingDays(nodo.fecha_inicio, nodo.fecha_fin);
                        setDatosCalculados(nodo.objeto, nodo.fecha_inicio, nodo.fecha_fin, nodo.costo, nodo.duracion);
                    }
                    ret = true;
                }

                ret = ret && guardarSubComponenteBatch(listas);
            }
            catch (Exception e)
            {
                CLogger.write("20", "SubComponenteDAO.class", e);
            }
                        
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
                CLogger.write("20", "SubComponenteDAO.class", e);
            }
        }

        private static bool guardarSubComponenteBatch(List<List<Nodo>> listas)
        {
            bool ret = true;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    for (int i = 0; i < listas.Count - 1; i++)
                    {
                        for (int j = 0; j < listas[i].Count; j++)
                        {
                            guardarSubComponente((Subcomponente)listas[i][j].objeto, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                CLogger.write("21", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static List<Subcomponente> getSubcomponentesPorComponenteHistory(int componenteId, String lineaBase)
        {
            List<Subcomponente> ret = new List<Subcomponente>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT * FROM subcomponente c WHERE c.estado = 1 AND componenteid=:componenteId ",
                        lineaBase != null ? "and c.linea_base like '%" + lineaBase + "%'" : "and c.actual = 1",
                        "order by c.id desc");

                    ret = db.Query<Subcomponente>(query, new { componenteId = componenteId }).AsList<Subcomponente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("22", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static Subcomponente getSubcomponenteHistory(int subcomponenteId, String lineaBase)
        {
            Subcomponente ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "select * FROM subcomponente c WHERE c.estado = 1 AND c.id=:subcomponenteId ",
                        lineaBase != null ? "and c.linea_base like '%" + lineaBase + "%'" : "and c.actual = 1",
                        "order by c.id desc");

                    ret = db.QueryFirstOrDefault<Subcomponente>(query, new { subcomponenteId = subcomponenteId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("23", "SubComponenteDAO.class", e);
            }
            return ret;
        }

        public static String getVersiones(int subComponenteId)
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT DISTINCT(version) FROM subcomponente "
                        + " WHERE id=" + subComponenteId;

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

        public static String getHistoria(int SubComponenteId, int version)
        {
            String resultado = "";
            String query = "SELECT c.version, c.nombre, c.descripcion, ct.nombre tipo, ue.nombre unidad_ejecutora, c.costo, ac.nombre tipo_costo, "
                    + " c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.renglon, c.ubicacion_geografica, c.latitud, c.longitud, "
                    + " c.fecha_inicio, c.fecha_fin, c.duracion, c.fecha_inicio_real, c.fecha_fin_real, "
                    + " c.fecha_creacion, c.usuario_creo, c.fecha_actualizacion, c.usuario_actualizo, "
                    + " CASE WHEN c.estado = 1 "
                    + " THEN 'Activo' "
                    + " ELSE 'Inactivo' "
                    + " END AS estado "
                    + " FROM sipro_history.subcomponente c "
                    + " JOIN sipro_history.unidad_ejecutora ue ON c.unidad_ejecutoraunidad_ejecutora = ue.unidad_ejecutora and c.entidad = ue.entidadentidad and c.ejercicio = ue.ejercicio  JOIN sipro_history.subcomponente_tipo ct ON c.subcomponente_tipoid = ct.id "
                    + " JOIN sipro_history.acumulacion_costo ac ON c.acumulacion_costoid = ac.id "
                    + " WHERE c.id = " + SubComponenteId
                    + " AND c.version = " + version;

            String[] campos = {"Version", "Nombre", "Descripción", "Tipo", "Unidad Ejecutora", "Monto Planificado", "Tipo Acumulación de Monto Planificado",
                "Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "Ubicación Geográfica", "Latitud", "Longitud",
                "Fecha Inicio", "Fecha Fin", "Duración", "Fecha Inicio Real", "Fecha Fin Real",
                "Fuente Préstamo", "Fuente Donación", "Fuente Nacional",
                "Fecha Creación", "Usuario que creo", "Fecha Actualización", "Usuario que actualizó",
                "Estado"};
            resultado = CHistoria.getHistoria(query, campos);
            return resultado;
        }
    }
}
