using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using System.Numerics;

namespace SiproDAO.Dao
{
    public class EstructuraProyectoDAO
    {
        public static List<EstructuraProyecto> getEstructuraProyecto(int idProyecto, String lineaBase)
        {
            List<EstructuraProyecto> ret = null;
            try
            {
                using (DbConnection db = (lineaBase != null ? new OracleContext().getConnectionHistory() : new OracleContext().getConnection()))
                {
                    String queryVersionP = "";
                    String queryVersionC = "";
                    String queryVersionS = "";
                    String queryVersionPr = "";
                    String queryVersionSp = "";
                    String queryVersionA = "";

                    if(lineaBase != null)
                    {
                        queryVersionP = " and p.linea_base like '%" + lineaBase + "%' ";
                        queryVersionC = " and c.linea_base like '%" + lineaBase + "%' ";
                        queryVersionS = " and s.linea_base like '%" + lineaBase + "%' ";
                        queryVersionPr = " and pr.linea_base like '%" + lineaBase + "%' ";
                        queryVersionSp = " and sp.linea_base like '%" + lineaBase + "%' ";
                        queryVersionA = " and a.linea_base like '%" + lineaBase + "%' ";
                    }
                    String query =
                            "select * from ( " +
                            "select p.id, p.nombre, 0 objeto_tipo,  p.treePath, p.fecha_inicio, " +
                            "p.fecha_fin, p.duracion, p.duracion_dimension,p.costo,0, p.acumulacion_costoid,  " +
                            "p.programa, p.subprograma, p.proyecto, p.actividad, p.obra, p.fecha_inicio_real, p.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from proyecto p " +
                            "where p.id=:id and p.estado=1 " +
                            queryVersionP +
                            "union " +
                            "select c.id, c.nombre, 1 objeto_tipo,  c.treePath, c.fecha_inicio, " +
                            "c.fecha_fin , c.duracion, c.duracion_dimension,c.costo,0,c.acumulacion_costoid, " +
                            "c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.fecha_inicio_real, c.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from componente c " +
                            "where c.proyectoid=:id and c.estado=1  " +
                            queryVersionC +
                            "union " +
                            "select s.id, s.nombre, 2 objeto_tipo,  s.treePath, s.fecha_inicio, " +
                            "s.fecha_fin , s.duracion, s.duracion_dimension,s.costo,0,s.acumulacion_costoid, " +
                            "s.programa, s.subprograma, s.proyecto, s.actividad, s.obra, s.fecha_inicio_real, s.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from subcomponente s " +
                            "left outer join componente c on c.id=s.componenteid " + queryVersionS +
                            "where c.proyectoid=:id and s.estado=1 and c.estado=1  " +
                            queryVersionC +
                            "union " +
                            "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, " +
                            "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid, " +
                            "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.fecha_inicio_real, pr.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from producto pr " +
                            "left outer join componente c on c.id=pr.componenteid " + queryVersionC +
                            "left outer join proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1   " +
                            queryVersionPr +
                            "union " +
                            "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, " +
                            "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid, " +
                            "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.fecha_inicio_real, pr.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from producto pr " +
                            "left outer join subcomponente s on s.id=pr.subcomponenteid   " + queryVersionS +
                            "left outer join componente c on c.id = s.componenteid   " + queryVersionC +
                            "left outer join proyecto p on p.id=c.proyectoid   " + queryVersionP +
                            "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1   " +
                            queryVersionPr +
                            "union   " +
                            "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                            "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, " +
                            "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.fecha_inicio_real, sp.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from subproducto sp " +
                            "left outer join producto pr on pr.id=sp.productoid " + queryVersionPr +
                            "left outer join componente c on c.id=pr.componenteid " + queryVersionC +
                            "left outer join proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1 and sp.estado=1 " +
                            queryVersionSp +
                            "union   " +
                            "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                            "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, " +
                            "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.fecha_inicio_real, sp.fecha_fin_real,0 porcentaje_avance, 0 objeto_tipo_pred " +
                            "from subproducto sp " +
                            "left outer join producto pr on pr.id=sp.productoid " + queryVersionPr +
                            "left outer join subcomponente s on s.id=pr.subcomponenteid " + queryVersionS +
                            "left outer join componente c on c.id=s.componenteid " + queryVersionC +
                            "left outer join proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1 and sp.estado=1 " +
                            queryVersionSp +
                            "union " +
                            "select a.id, a.nombre, 5 objeto_tipo,  a.treePath, a.fecha_inicio, " +
                            "a.fecha_fin , a.duracion, a.duracion_dimension,a.costo,a.pred_objeto_id,a.acumulacion_costo acumulacion_costoid, " +
                            "a.programa, a.subprograma, a.proyecto, a.actividad, a.obra, a.fecha_inicio_real, a.fecha_fin_real, a.porcentaje_avance, a.objeto_tipo objeto_tipo_pred " +
                            "from actividad a " +
                            "where a.estado=1 and  a.treepath like '" + (10000000 + idProyecto) + "%'" +
                            queryVersionA +
                            ") arbol " +
                            "order by treePath ";

                    ret = db.Query<EstructuraProyecto>(query, new { id = idProyecto }).AsList<EstructuraProyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "EstructuraProyectoDAO.class", e);
            }
            return ret;
        }

        public static List<dynamic> getEstructuraProyecto(int idProyecto, String lineaBase, String usuario)
        {
            List<dynamic> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String queryVersionP = "";
                    String queryVersionC = "";
                    String queryVersionS = "";
                    String queryVersionPr = "";
                    String queryVersionSp = "";
                    String queryVersionA = "";
                    if (lineaBase == null)
                    {
                        queryVersionP = " and p.actual = 1 ";
                        queryVersionC = " and c.actual = 1 ";
                        queryVersionS = " and s.actual = 1 ";
                        queryVersionPr = " and pr.actual = 1 ";
                        queryVersionSp = " and sp.actual = 1 ";
                        queryVersionA = " and a.actual = 1 ";
                    }
                    else
                    {
                        queryVersionP = " and p.linea_base like '%" + lineaBase + "%' ";
                        queryVersionC = " and c.linea_base like '%" + lineaBase + "%' ";
                        queryVersionS = " and s.linea_base like '%" + lineaBase + "%' ";
                        queryVersionPr = " and pr.linea_base like '%" + lineaBase + "%' ";
                        queryVersionSp = " and sp.linea_base like '%" + lineaBase + "%' ";
                        queryVersionA = " and a.linea_base like '%" + lineaBase + "%' ";
                    }
                    String query =
                        "select * from ( " +
                        "select p.id, p.nombre, 0 objeto_tipo,  p.treePath, p.fecha_inicio, " +
                        "p.fecha_fin, p.duracion, p.duracion_dimension,p.costo,0, p.acumulacion_costoid  " +
                        "from sipro_history.proyecto p " +
                        "where p.id=:id and p.estado=1 and p.id in ( select proyectoid from proyecto_usuario where usuario =:usuario ) " +
                        queryVersionP +
                        "union " +
                        "select c.id, c.nombre, 1 objeto_tipo,  c.treePath, c.fecha_inicio, " +
                        "c.fecha_fin , c.duracion, c.duracion_dimension,c.costo,0,c.acumulacion_costoid " +
                        "from sipro_history.componente c " +
                        "where c.proyectoid=:id and c.estado=1 and c.id in (select componenteid from componente_usuario where usuario =:usuario ) " +
                        queryVersionC +
                        "union " +
                        "select s.id, s.nombre, 2 objeto_tipo,  s.treePath, s.fecha_inicio, " +
                        "s.fecha_fin , s.duracion, s.duracion_dimension,s.costo,0,s.acumulacion_costoid " +
                        "from sipro_history.subcomponente s " +
                        "left outer join sipro_history.componente c on c.id=s.componenteid " + queryVersionC +
                        "where c.proyectoid=:id and s.estado=1 and c.estado=1 and s.id in (select subcomponenteid from subcomponente_usuario where usuario =:usuario ) " +
                        queryVersionS +
                        "union " +
                        "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, " +
                        "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid " +
                        "from sipro_history.producto pr " +
                        "left outer join sipro_history.componente c on c.id=pr.componenteid " + queryVersionC +
                        "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                        "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1 and pr.id in ( select productoid from producto_usuario where usuario =:usuario )  " +
                        queryVersionPr +
                        "union " +
                        "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, " +
                        "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid " +
                        "from sipro_history.producto pr " +
                        "left outer join sipro_history.subcomponente s on s.id=pr.subcomponenteid " + queryVersionS +
                        "left outer join sipro_history.componente c on c.id=s.componenteid " + queryVersionC +
                        "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                        "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1 and pr.id in ( select productoid from producto_usuario where usuario =:usuario )  " +
                        queryVersionPr +
                        "union " +
                        "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                        "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid " +
                        "from sipro_history.subproducto sp " +
                        "left outer join sipro_history.producto pr on pr.id=sp.productoid " + queryVersionPr +
                        "left outer join sipro_history.componente c on c.id=pr.componenteid " + queryVersionC +
                        "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                        "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1 and sp.estado=1 and sp.id and pr.id in ( select productoid from producto_usuario where usuario =:usuario ) " +
                        queryVersionSp +
                        "union " +
                        "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                        "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid " +
                        "from sipro_history.subproducto sp " +
                        "left outer join sipro_history.producto pr on pr.id=sp.productoid " + queryVersionPr +
                        "left outer join sipro_history.subcomponente s on s.id=pr.subcomponenteid " + queryVersionS +
                        "left outer join sipro_history.componente c on c.id=s.componenteid " + queryVersionC +
                        "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                        "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1 and sp.estado=1 and sp.id and pr.id in ( select productoid from producto_usuario where usuario =:usuario ) " +
                        queryVersionSp +
                        "union " +
                        "select a.id, a.nombre, 5 objeto_tipo,  a.treePath, a.fecha_inicio, " +
                        "a.fecha_fin , a.duracion, a.duracion_dimension,a.costo,a.pred_objeto_id,a.acumulacion_costo acumulacion_costoid " +
                        "from sipro_history.actividad a " +
                        "where a.estado=1 and  a.treepath like '" + (10000000 + idProyecto) + "%'" +
                        queryVersionA +
                        ") arbol " +
                        "order by treePath ";

                    ret = db.Query<dynamic>(query, new { id = idProyecto, usuario = usuario }).AsList<dynamic>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "EstructuraProyectoDAO.class", e);
            }
            return ret;
        }

        public static Nodo getEstructuraProyectoArbol(int id, String lineaBase, String usuario)
        {
            Nodo root = null;
            List<dynamic> estructuras = EstructuraProyectoDAO.getEstructuraProyecto(id, lineaBase, usuario);
            if (estructuras.Count > 0)
            {
                try
                {
                    Object[] dato = (Object[])estructuras[0];
                    int id_ = dato[0] != null ? (int)dato[0] : 0;
                    int objeto_tipo = dato[2] != null ? Convert.ToInt32((BigInteger)dato[2]) : 0;
                    String nombre = dato[1] != null ? (String)dato[1] : null;
                    int nivel = (dato[3] != null) ? ((String)dato[3]).Length / 8 : 0;

                    root = new Nodo();
                    root.id = id_;
                    root.objeto_tipo = objeto_tipo;
                    root.nombre = nombre;
                    root.nivel = nivel;
                    root.children = new List<Nodo>();
                    root.parent = null;
                    root.estado = false;

                    Nodo nivel_actual_estructura = root;
                    for (int i = 1; i < estructuras.Count; i++)
                    {
                        dato = (Object[])estructuras[i];
                        id_ = dato[0] != null ? (int)dato[0] : 0;
                        objeto_tipo = dato[2] != null ? Convert.ToInt32((BigInteger)dato[2]) : 0;
                        nombre = dato[1] != null ? (String)dato[1] : null;
                        nivel = (dato[3] != null) ? ((String)dato[3]).Length / 8 : 0;

                        Nodo nodo = new Nodo();
                        nodo.id = id_;
                        nodo.objeto_tipo = objeto_tipo;
                        nodo.nombre = nombre;
                        nodo.nivel = nivel;
                        nodo.children = new List<Nodo>();
                        nodo.parent = null;
                        nodo.estado = false;

                        if (nodo.nivel != nivel_actual_estructura.nivel + 1)
                        {
                            if (nodo.nivel > nivel_actual_estructura.nivel)
                            {
                                nivel_actual_estructura = nivel_actual_estructura.children[nivel_actual_estructura.children.Count - 1];
                            }
                            else
                            {
                                int retornar = nivel_actual_estructura.nivel - nodo.nivel + 1;
                                for (int j = 0; j < retornar; j++)
                                    nivel_actual_estructura = nivel_actual_estructura.parent;
                            }
                        }
                        nodo.parent = nivel_actual_estructura;
                        nivel_actual_estructura.children.Add(nodo);
                    }
                }
                catch (Exception e)
                {
                    root = null;
                    CLogger.write("3", "EstructuraProyectoDAO.class", e);
                }
            }
            return root;
        }

        public static Nodo getEstructuraProyectoArbolProyectosComponentesProductos(int id, String lineaBase, String usuario)
        {
            Nodo root = null;
            List<EstructuraProyecto> estructuras = EstructuraProyectoDAO.getEstructuraProyecto(id, lineaBase);
            if (estructuras.Count > 0)
            {
                try
                {
                    EstructuraProyecto dato = estructuras[0];
                    int id_ = dato.id;
                    int objeto_tipo = dato.objeto_tipo;
                    String nombre = dato.nombre;
                    int nivel = (dato.treePath != null) ? (dato.treePath).Length / 8 : 0;
                    bool estado = checkPermiso(id, objeto_tipo, usuario);
                    root = new Nodo();
                    root.id = id_;
                    root.objeto_tipo = objeto_tipo;
                    root.nombre = nombre;
                    root.nivel = nivel;
                    root.children = new List<Nodo>();
                    root.parent = null;
                    root.estado = estado;

                    Nodo nivel_actual_estructura = root;
                    for (int i = 1; i < estructuras.Count; i++)
                    {
                        dato = estructuras[i];
                        id_ = dato.id;
                        objeto_tipo = dato.objeto_tipo;
                        nombre = dato.nombre;
                        nivel = (dato.treePath != null) ? (dato.treePath).Length / 8 : 0;
                        estado = checkPermiso(id_, objeto_tipo, usuario);
                        if (objeto_tipo < 4)
                        {
                            Nodo nodo = new Nodo();
                            nodo.id = id_;
                            nodo.objeto_tipo = objeto_tipo;
                            nodo.nombre = nombre;
                            nodo.nivel = nivel;
                            nodo.children = new List<Nodo>();
                            nodo.parent = null;
                            nodo.estado = estado;

                            if (nodo.nivel != nivel_actual_estructura.nivel + 1)
                            {
                                if (nodo.nivel > nivel_actual_estructura.nivel)
                                {
                                    nivel_actual_estructura = nivel_actual_estructura.children[nivel_actual_estructura.children.Count - 1];
                                }
                                else
                                {
                                    int retornar = nivel_actual_estructura.nivel - nodo.nivel + 1;
                                    for (int j = 0; j < (retornar); j++)
                                        nivel_actual_estructura = nivel_actual_estructura.parent;
                                }
                            }
                            nodo.parent = nivel_actual_estructura;
                            nivel_actual_estructura.children.Add(nodo);
                        }
                    }
                }
                catch (Exception e)
                {
                    root = null;
                    CLogger.write("3", "EstructuraProyectoDAO.class", e);
                }
            }
            return root;
        }

        public static Nodo getEstructuraPrestamoProyectoArbolProyectosComponentesProductos(int id, String lineaBase, String usuario)
        {
            Nodo root = null;
            Prestamo prestamo = PrestamoDAO.getPrestamoById(id);
            if (prestamo != null)
            {
                int id_ = prestamo.id;
                int objeto_tipo = -1;
                String nombre = prestamo.proyectoPrograma;
                int nivel = 0;
                bool estado = checkPermiso(id, objeto_tipo, usuario);
                root = new Nodo();
                root.id = id_;
                root.objeto_tipo = objeto_tipo;
                root.nombre = nombre;
                root.nivel = nivel;
                root.children = new List<Nodo>();
                root.parent = null;
                root.estado = estado;

                List<Proyecto> proyectos = ProyectoDAO.getProyectosByIdPrestamo(prestamo.id);
                if (proyectos != null && proyectos.Count > 0)
                {
                    foreach (Proyecto proyecto in proyectos)
                    {
                        List<EstructuraProyecto> estructuras = EstructuraProyectoDAO.getEstructuraProyecto(proyecto.id, lineaBase);
                        if (estructuras.Count > 0)
                        {
                            try
                            {
                                EstructuraProyecto dato = estructuras[0];
                                id_ = dato.id;
                                objeto_tipo = dato.objeto_tipo;
                                nombre = dato.nombre;
                                nivel = (dato.treePath != null) ? ((dato.treePath).Length / 8) + 1 : 1;
                                estado = checkPermiso(id_, objeto_tipo, usuario);
                                Nodo nodo = new Nodo();
                                nodo.id = id_;
                                nodo.objeto_tipo = objeto_tipo;
                                nodo.nombre = nombre;
                                nodo.nivel = nivel;
                                nodo.children = new List<Nodo>();
                                nodo.parent = null;
                                nodo.estado = estado;

                                nodo.parent = root;
                                root.children.Add(nodo);

                                Nodo nivel_actual_estructura = root;
                                for (int i = 1; i < estructuras.Count; i++)
                                {
                                    dato = estructuras[i];
                                    id_ = dato.id;
                                    objeto_tipo = dato.objeto_tipo;
                                    nombre = dato.nombre;
                                    nivel = (dato.treePath != null) ? ((dato.treePath).Length / 8) + 1 : 1;
                                    estado = checkPermiso(id_, objeto_tipo, usuario);
                                    if (objeto_tipo < 4)
                                    {
                                        nodo = new Nodo();
                                        nodo.id = id_;
                                        nodo.objeto_tipo = objeto_tipo;
                                        nodo.nombre = nombre;
                                        nodo.nivel = nivel;
                                        nodo.children = new List<Nodo>();
                                        nodo.parent = null;
                                        nodo.estado = estado;

                                        if (nodo.nivel != nivel_actual_estructura.nivel + 1)
                                        {
                                            if (nodo.nivel > nivel_actual_estructura.nivel)
                                            {
                                                nivel_actual_estructura = nivel_actual_estructura.children[nivel_actual_estructura.children.Count - 1];
                                            }
                                            else
                                            {
                                                int retornar = nivel_actual_estructura.nivel - nodo.nivel + 1;
                                                for (int j = 0; j < (retornar); j++)
                                                    nivel_actual_estructura = nivel_actual_estructura.parent;
                                            }
                                        }
                                        nodo.parent = nivel_actual_estructura;
                                        nivel_actual_estructura.children.Add(nodo);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                root = null;
                                CLogger.write("3", "EstructuraProyectoDAO.class", e);
                            }
                        }
                    }

                }
            }

            return root;
        }

        public static List<dynamic> getActividadesProyecto(int prestamoId, String lineaBase)
        {
            List<dynamic> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String str_Query = String.Join(" ", "SELECT a.id, a.nombre, 5 objeto_tipo,  a.treePath, a.nivel, a.fecha_inicio,",
                        "a.fecha_fin , a.duracion, a.duracion_dimension,a.costo,a.pred_objeto_id,a.acumulacion_costo acumulacion_costoid,",
                        "a.porcentaje_avance, a.fecha_inicio_real, a.fecha_fin_real, a.descripcion",
                        "FROM sipro_history.actividad a",
                        "WHERE a.estado=1 AND a.treepath like '" + (10000000 + prestamoId) + "%'",
                        lineaBase != null ? "AND a.linea_base like '%" + lineaBase + "%'" : "AND a.actual=1");

                    ret = db.Query<dynamic>(str_Query).AsList<dynamic>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "EstructuraProyectoDAO.class", e);
            }
            return ret;
        }

        public static List<Object[]> getActividadesByTreePath(String treePath, int idPrestamo, String lineaBase){
            List<Object[]> ret = new List<Object[]>();
            try
            {
                List<dynamic> lstActividadesPrestamo = getActividadesProyecto(idPrestamo, lineaBase);
                Object[] temp = new Object[5];
                foreach (Object objeto in lstActividadesPrestamo)
                {
                    Object[] obj = (Object[])objeto;
                    String treePathObj = (String)obj[3];
                    if (treePathObj != null && treePath != null && treePathObj.Length >= treePath.Length)
                    {
                        if (treePathObj.Substring(0, treePath.Length).Equals(treePath))
                        {
                            temp = new Object[] { (int)obj[0], (String)obj[1], (DateTime)obj[5], (DateTime)obj[6], (int)obj[12], (DateTime)obj[13], (DateTime)obj[14], (String)obj[15] };
                            ret.Add(temp);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "EstructuraProyectoDAO.class", e);
            }

            return ret;
        }

        public static List<Nodo> getEstructuraProyectosArbol(String usuario, String lineaBase)
        {
            List<Nodo> ret = new List<Nodo>();
            List<Proyecto> proyectos = ProyectoDAO.getTodosProyectos();
            if (proyectos != null)
            {
                for (int i = 0; i < proyectos.Count; i++)
                {
                    Nodo proyecto = getEstructuraProyectoArbolProyectosComponentesProductos(proyectos[i].id, lineaBase, usuario);
                    if (proyecto != null)
                        ret.Add(proyecto);
                }
            }
            return (ret.Count > 0 ? ret : null);
        }

        public static List<Nodo> getEstructuraPrestamosArbol(String usuario, String lineaBase)
        {
            List<Nodo> ret = new List<Nodo>();
            List<Prestamo> prestamos = PrestamoDAO.getPrestamos(null);
            if (prestamos != null)
            {
                for (int i = 0; i < prestamos.Count; i++)
                {
                    Nodo prestamo = getEstructuraPrestamoProyectoArbolProyectosComponentesProductos(prestamos[i].id, lineaBase, usuario);
                    if (prestamo != null)
                        ret.Add(prestamo);
                }
            }
            return (ret.Count > 0 ? ret : null);
        }

        public static bool checkPermiso(int id, int objeto_tipo, String usuario)
        {
            bool ret = false;
            switch (objeto_tipo)
            {
                case -1: ret = UsuarioDAO.checkUsuarioPrestamo(usuario, id); break;
                case 0: ret = UsuarioDAO.checkUsuarioProyecto(usuario, id); break;
                case 1: ret = UsuarioDAO.checkUsuarioComponente(usuario, id); break;
                case 2: ret = UsuarioDAO.checkUsuarioSubComponente(usuario, id); break;
                case 3: ret = UsuarioDAO.checkUsuarioProducto(usuario, id); break;
            }
            return ret;
        }

        public static List<ObjetoCosto> getHijosCompleto(String treePathPadre, List<ObjetoCosto> estruturaProyecto)
        {
            List<ObjetoCosto> ret = new List<ObjetoCosto>();
            foreach (ObjetoCosto objeto in estruturaProyecto)
            {
                String treePath = objeto.treePath;
                if (treePath != null)
                {
                    if (treePath.Length >= treePathPadre.Length + 6)
                    {
                        String path = treePath.Substring(0, treePathPadre.Length);
                        if (path.Equals(treePathPadre))
                        {
                            ret.Add(objeto);
                        }
                    }
                }
            }

            return ret;
        }

        public static List<String> getHijos(String treePathPadre, List<dynamic> estruturaProyecto)
        {
            List<String> ret = new List<String>();
            foreach (Object objeto in estruturaProyecto)
            {
                Object[] obj = (Object[])objeto;
                String treePath = (String)obj[3];
                if (treePath != null)
                {
                    if (treePath.Length == treePathPadre.Length + 6)
                    {
                        String path = treePath.Substring(0, treePathPadre.Length);
                        if (path.Equals(treePathPadre))
                        {
                            ret.Add((int)obj[0] + "," + Convert.ToInt32((BigInteger)obj[2]));
                        }
                    }
                }
            }

            return ret;
        }

        public static List<List<Nodo>> getEstructuraProyectoArbolCalculos(int id, String lineaBase)
        {
            List<List<Nodo>> ret = new List<List<Nodo>>();
            Nodo root = null;
            List<EstructuraProyecto> estructuras = EstructuraProyectoDAO.getEstructuraProyecto(id, lineaBase);
            if (estructuras.Count > 0)
            {
                try
                {
                    int nivel_maximo = 0;
                    EstructuraProyecto dato = estructuras[0];
                    int id_ = dato.id;
                    int objeto_tipo = dato.objeto_tipo;
                    String nombre = dato.nombre;
                    int nivel = (dato.treePath != null) ? (dato.treePath).Length / 8 : 0;
                    DateTime fecha_inicio = dato.fecha_inicio ?? default(DateTime);
                    DateTime fecha_fin = dato.fecha_fin ?? default(DateTime);
                    decimal costo = dato.costo ?? default(decimal);
                    DateTime fecha_inicio_real = dato.fecha_inicio_real ?? default(DateTime);
                    DateTime fecha_fin_real = dato.fecha_fin_real ?? default(DateTime);
                    root = new Nodo();
                    root.id = id_;
                    root.objeto_tipo = objeto_tipo;
                    root.nombre = nombre;
                    root.nivel = nivel;
                    root.children = new List<Nodo>();
                    root.parent = null;
                    root.estado = false;
                    root.fecha_inicio = fecha_inicio;
                    root.fecha_fin = fecha_fin;
                    root.costo = costo;
                    root.duracion = 0;
                    root.fecha_inicio_real = fecha_inicio_real;
                    root.fecha_fin_real = fecha_fin_real;
                    Nodo nivel_actual_estructura = root;

                    ret.Add(new List<Nodo>());
                    ret[0].Add(root);
                    for (int i = 1; i < estructuras.Count; i++)
                    {
                        dato = estructuras[i];
                        id_ = dato.id;
                        objeto_tipo = dato.objeto_tipo;
                        nombre = dato.nombre;
                        nivel = (dato.treePath != null) ? (dato.treePath).Length / 8 : 0;
                        fecha_inicio = dato.fecha_inicio ?? default(DateTime);
                        fecha_fin = dato.fecha_fin ?? default(DateTime);
                        costo = dato.costo ?? default(decimal);
                        nivel_maximo = nivel_maximo < nivel ? nivel : nivel_maximo;
                        fecha_inicio_real = dato.fecha_inicio_real ?? default(DateTime);
                        fecha_fin_real = dato.fecha_fin_real ?? default(DateTime);

                        Nodo nodo = new Nodo();
                        nodo.id = id_;
                        nodo.objeto_tipo = objeto_tipo;
                        nodo.nombre = nombre;
                        nodo.nivel = nivel;
                        nodo.children = new List<Nodo>();
                        nodo.parent = null;
                        nodo.estado = false;
                        nodo.fecha_inicio = fecha_inicio;
                        nodo.fecha_fin = fecha_fin;
                        nodo.costo = costo;
                        nodo.duracion = 0;
                        nodo.fecha_inicio_real = fecha_inicio_real;
                        nodo.fecha_fin_real = fecha_fin_real;

                        if (nodo.nivel != nivel_actual_estructura.nivel + 1)
                        {
                            if (nodo.nivel > nivel_actual_estructura.nivel)
                            {
                                nivel_actual_estructura = nivel_actual_estructura.children[nivel_actual_estructura.children.Count - 1];
                            }
                            else
                            {
                                int retornar = nivel_actual_estructura.nivel - nodo.nivel + 1;
                                for (int j = 0; j < retornar; j++)
                                    nivel_actual_estructura = nivel_actual_estructura.parent;
                            }
                        }
                        nodo.parent = nivel_actual_estructura;
                        nivel_actual_estructura.children.Add(nodo);
                        if (ret.Count < nivel)
                        {
                            ret.Add(new List<Nodo>());
                            ret[ret.Count - 1].Add(nodo);
                        }
                        else
                        {
                            ret[nivel - 1].Add(nodo);
                        }
                    }
                }
                catch (Exception e)
                {
                    root = null;
                    CLogger.write("6", "EstructuraProyectoDAO.class", e);
                }
            }
            return ret;
        }

        /*public static List<?> getEstructuraObjeto(Integer objetoId, Integer objetoTipo){
            List<?> ret = null;
            Session session = CHibernateSession.getSessionFactory().openSession();
            String treePath_inicio="";
            switch(objetoTipo){
                case 0: Proyecto proyecto = ProyectoDAO.getProyecto(objetoId); treePath_inicio = (proyecto!=null) ? proyecto.getTreePath() : null; break;
                case 1: Componente componente = ComponenteDAO.getComponente(objetoId); treePath_inicio = (componente!=null) ? componente.getTreePath() : null; break;
                case 2: Subcomponente subcomponente = SubComponenteDAO.getSubComponente(objetoId); treePath_inicio = (subcomponente!=null) ? subcomponente.getTreePath() : null; break;
                case 3: Producto producto = ProductoDAO.getProductoPorId(objetoId); treePath_inicio = (producto!=null) ? producto.getTreePath() : null; break;
                case 4: Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId); treePath_inicio = (subproducto!=null) ? subproducto.getTreePath() : null; break;
                case 5: Actividad actividad = ActividadDAO.getActividadPorId(objetoId); treePath_inicio = (actividad!=null) ? actividad.getTreePath() : null; break;
            }
            try{

                String query =
                        "select * from ( "+
                        ( objetoTipo<=0 ? 
                        "select p.id, p.nombre, 0 objeto_tipo,  p.treePath, p.fecha_inicio, "+
                        "p.fecha_fin, p.duracion, p.duracion_dimension,p.costo,0, p.acumulacion_costoid,  "+
                        "p.programa, p.subprograma, p.proyecto, p.actividad, p.obra, p.fecha_inicio_real, p.fecha_fin_real "+
                        "from proyecto p "+
                        "where p.id=:id and p.estado=1  "+
                        "union " : "" ) +
                        ( objetoTipo<=1 ? 
                        "select c.id, c.nombre, 1 objeto_tipo,  c.treePath, c.fecha_inicio, "+
                        "c.fecha_fin , c.duracion, c.duracion_dimension,c.costo,0,c.acumulacion_costoid, "+
                        "c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.fecha_inicio_real, c.fecha_fin_real  "+
                        "from componente c "+
                        "where c.proyectoid=:id and c.estado=1  "+
                        "union " : "" ) +
                        ( objetoTipo<=2 ? 
                        "select s.id, s.nombre, 2 objeto_tipo,  s.treePath, s.fecha_inicio, "+
                        "s.fecha_fin , s.duracion, s.duracion_dimension,s.costo,0,s.acumulacion_costoid, "+
                        "s.programa, s.subprograma, s.proyecto, s.actividad, s.obra, s.fecha_inicio_real, s.fecha_fin_real  "+
                        "from subcomponente s "+
                        "left outer join componente c on c.id=s.componenteid "+
                        "left outer join proyecto p on p.id=c.proyectoid "+
                        "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1   "+
                        "union " : "" ) +
                        ( objetoTipo<=3 ? "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, "+
                        "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid, "+
                        "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.fecha_inicio_real, pr.fecha_fin_real  "+
                        "from producto pr "+
                        "left outer join componente c on c.id=pr.componenteid "+
                        "left outer join proyecto p on p.id=c.proyectoid "+
                        "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1   "+
                        "union "+
                        "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, "+
                        "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid, "+
                        "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.fecha_inicio_real, pr.fecha_fin_real  "+
                        "from producto pr "+
                        "left outer join subcomponente s on s.id=pr.subcomponenteid "+
                        "left outer join componente c on c.id=s.componenteid "+
                        "left outer join proyecto p on p.id=c.proyectoid "+
                        "where p.id=:id and p.estado=1 and c.estado=1 and s.estado=1 and pr.estado=1   "+
                        "union " : "")+
                        ( objetoTipo<=4 ? "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, "+
                        "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, "+
                        "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.fecha_inicio_real, sp.fecha_fin_real  "+
                        "from subproducto sp "+
                        "left outer join producto pr on pr.id=sp.productoid "+
                        "left outer join componente c on c.id=pr.componenteid "+
                        "left outer join proyecto p on p.id=c.proyectoid "+
                        "where p.id=:id and p.estado=1 and c.estado=1 and pr.estado=1 and sp.estado=1 and sp.id  "+
                        "union "+
                        "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, "+
                        "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, "+
                        "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.fecha_inicio_real, sp.fecha_fin_real  "+
                        "from subproducto sp "+
                        "left outer join producto pr on pr.id=sp.productoid "+
                        "left outer join subcomponente sc on sc.id=pr.subcomponenteid "+
                        "left outer join componente c on c.id=sc.componenteid "+
                        "left outer join proyecto p on p.id=c.proyectoid "+
                        "where p.id=:id and p.estado=1 and c.estado=1 and sc.estado=1 and pr.estado=1 and sp.estado=1 and sp.id  "+
                        "union " : "") +
                        "select a.id, a.nombre, 5 objeto_tipo,  a.treePath, a.fecha_inicio, "+
                        "a.fecha_fin , a.duracion, a.duracion_dimension,a.costo,a.pred_objeto_id,a.acumulacion_costo acumulacion_costoid, "+
                        "a.programa, a.subprograma, a.proyecto, a.actividad, a.obra, a.fecha_inicio_real, a.fecha_fin_real  "+
                        "from actividad a "+
                        "where a.estado=1 and  a.treepath like '"+treePath_inicio+"%'"+
                        ") arbol "+
                        "order by treePath ";			

                Query<?> criteria = session.createNativeQuery(query);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("7", EstructuraProyectoDAO.class, e);
            }
            finally{
                session.close();
            }
            return ret;
        }

        public static ArrayList<ArrayList<Nodo>> getEstructuraObjetoArbolCalculos(Integer objetoId,Integer objetoTipo){
            ArrayList<ArrayList<Nodo>> ret = new ArrayList<ArrayList<Nodo>>();
            Nodo root = null;
            List<?> estructuras = EstructuraProyectoDAO.getEstructuraObjeto(objetoId,objetoTipo);
            if(estructuras.size()>0){
                try{
                    int nivel_maximo = 0;
                    Object[] dato = (Object[]) estructuras.get(0);
                    int id_ = dato[0]!=null ? (Integer)dato[0] : 0;
                    int objeto_tipo = dato[2]!=null ? ((BigInteger)dato[2]).intValue() : 0;
                    String nombre = dato[1]!=null ? (String)dato[1] : null;
                    int nivel = (dato[3]!=null) ? ((String)dato[3]).length()/8 : 0;
                    Timestamp fecha_inicio = (dato[4]!=null) ? new Timestamp(((Date)dato[4]).getTime()) : null;
                    Timestamp fecha_fin = (dato[5]!=null) ? new Timestamp(((Date)dato[5]).getTime()) : null;
                    Double costo = (dato[8]!=null) ? ((BigDecimal)dato[8]).doubleValue() : 0;
                    Timestamp fecha_inicio_real = (dato[16] != null) ? new Timestamp(((Date)dato[16]).getTime()) : null;
                    Timestamp fecha_fin_real = (dato[17] != null) ? new Timestamp(((Date)dato[17]).getTime()) : null;
                    root = new Nodo(id_, objeto_tipo, nombre, nivel, new ArrayList<Nodo>(), null, false, fecha_inicio, fecha_fin, costo,0, fecha_inicio_real, fecha_fin_real);
                    Nodo nivel_actual_estructura = root;
                    ret.add(new ArrayList<Nodo>());
                    ret.get(0).add(root);
                    for(int i=1; i<estructuras.size(); i++){
                        dato = (Object[]) estructuras.get(i);
                        id_ = dato[0]!=null ? (Integer)dato[0] : 0;
                        objeto_tipo = dato[2]!=null ? ((BigInteger)dato[2]).intValue() : 0;
                        nombre = dato[1]!=null ? (String)dato[1] : null;
                        nivel = (dato[3]!=null) ? ((String)dato[3]).length()/8 : 0;
                        fecha_inicio = (dato[4]!=null) ? new Timestamp(((Date)dato[4]).getTime()) : null;
                        fecha_fin = (dato[5]!=null) ? new Timestamp(((Date)dato[5]).getTime()) : null;
                        costo = (dato[8]!=null) ? ((BigDecimal)dato[8]).doubleValue() : 0;
                        nivel_maximo = nivel_maximo <  nivel ? nivel : nivel_maximo;
                        fecha_inicio_real = (dato[16] != null) ? new Timestamp(((Date)dato[16]).getTime()) : null;
                        fecha_fin_real = (dato[17] != null) ? new Timestamp(((Date)dato[17]).getTime()) : null;
                        Nodo nodo = new Nodo(id_, objeto_tipo, nombre, nivel, new ArrayList<Nodo>(), null, false, fecha_inicio, fecha_fin, costo,0, fecha_inicio_real, fecha_fin_real);
                        if(nodo.nivel!=nivel_actual_estructura.nivel+1){
                            if(nodo.nivel>nivel_actual_estructura.nivel){
                                nivel_actual_estructura = nivel_actual_estructura.children.get(nivel_actual_estructura.children.size()-1);
                            }
                            else{
                                int retornar = nivel_actual_estructura.nivel-nodo.nivel+1;
                                for(int j=0; j<retornar; j++)
                                    nivel_actual_estructura=nivel_actual_estructura.parent;
                            }
                        }
                        nodo.parent = nivel_actual_estructura;
                        nivel_actual_estructura.children.add(nodo);
                        if(ret.size()<nivel){
                            ret.add(new ArrayList<Nodo>());
                            ret.get(ret.size()-1).add(nodo);
                        }
                        else{
                            ret.get(nivel-1).add(nodo);
                        }
                    }
                }
                catch(Throwable e){
                    root = null;
                    CLogger.write("8", EstructuraProyectoDAO.class, e);
                }
            }
            return ret;
        }
             */
    }
}
