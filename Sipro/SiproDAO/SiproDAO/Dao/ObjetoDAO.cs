using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using System.Numerics;

namespace SiproDAO.Dao
{
    public class ObjetoDAO
    {

        public static List<dynamic> getConsultaEstructuraConCosto(int proyectoId, String lineaBase)
        {
            List<dynamic> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String queryVersionP = "";
                    String queryVersionC = "";
                    String queryVersionSc = "";
                    String queryVersionPr = "";
                    String queryVersionSp = "";
                    String queryVersionA = "";
                    String queryVersionPad = "";
                    String queryVersionPadP = "";
                    if (lineaBase == null)
                    {
                        queryVersionP = " and p.actual = 1 ";
                        queryVersionC = " and c.actual = 1 ";
                        queryVersionSc = " and sc.actual = 1 ";
                        queryVersionPr = " and pr.actual = 1 ";
                        queryVersionSp = " and sp.actual = 1 ";
                        queryVersionA = " and a.actual = 1 ";
                        queryVersionPad = " where pa.actual = 1 ";
                        queryVersionPadP = " where actual = 1 ";
                    }
                    else
                    {
                        queryVersionP = " and p.linea_base like '%" + lineaBase + "%' ";
                        queryVersionC = " and c.linea_base like '%" + lineaBase + "%' ";
                        queryVersionSc = " and sc.linea_base like '%" + lineaBase + "%' ";
                        queryVersionPr = " and pr.linea_base like '%" + lineaBase + "%' ";
                        queryVersionSp = " and sp.linea_base like '%" + lineaBase + "%' ";
                        queryVersionA = " and a.linea_base like '%" + lineaBase + "%' ";
                        queryVersionPad = " where pa.linea_base like '%" + lineaBase + "%' ";
                        queryVersionPadP = " where linea_base like '%" + lineaBase + "%' ";
                    }
                    String query =
                            "select arbol.*, costo.total, costo.pago from ( " +
                            "select p.id, p.nombre, 0 objeto_tipo,  p.treePath, p.fecha_inicio, " +
                            "p.fecha_fin, p.duracion, p.duracion_dimension,p.costo,0, p.acumulacion_costoid,  " +
                            "p.programa, p.subprograma, p.proyecto, p.actividad, p.obra, p.renglon, p.ubicacion_geografica geografico, " +
                            "p.fecha_inicio_real, p.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, p.ejecucion_fisica_real ejecucion_fisica, " +
                            "NULL inversion_nueva " +
                            "from sipro_history.proyecto p " +
                            "where p.id=:proyectoId and p.estado=1  " +
                            queryVersionP +
                            "union " +
                            "select c.id, c.nombre, 1 objeto_tipo,  c.treePath, c.fecha_inicio, " +
                            "c.fecha_fin , c.duracion, c.duracion_dimension,c.costo,0,c.acumulacion_costoid, " +
                            "c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.renglon, c.ubicacion_geografica geografico, " +
                            "c.fecha_inicio_real, c.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "c.inversion_nueva " +
                            "from sipro_history.componente c " +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where c.proyectoid=:proyectoId and c.estado=1 and p.estado=1 " +
                            queryVersionC +
                            "union    " +
                            "select sc.id, sc.nombre, 2 objeto_tipo,  sc.treePath, sc.fecha_inicio,  " +
                            "sc.fecha_fin , sc.duracion, sc.duracion_dimension,sc.costo,0,sc.acumulacion_costoid,  " +
                            "sc.programa, sc.subprograma, sc.proyecto, sc.actividad, sc.obra, sc.renglon, sc.ubicacion_geografica geografico,  " +
                            "sc.fecha_inicio_real, sc.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "sc.inversion_nueva " +
                            "from sipro_history.subcomponente sc  " +
                            "left outer join sipro_history.componente c on c.id = sc.componenteid  " + queryVersionC +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where c.proyectoid=:proyectoId and sc.estado=1 and c.estado=1   " +
                            queryVersionSc +
                            "union " +
                            "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio, " +
                            "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid, " +
                            "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.renglon, pr.ubicacion_geografica geografico, " +
                            "pr.fecha_inicio_real, pr.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "pr.inversion_nueva " +
                            "from sipro_history.producto pr " +
                            "left outer join sipro_history.componente c on c.id=pr.componenteid " + queryVersionC +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:proyectoId and p.estado=1 and c.estado=1 and pr.estado=1   " +
                            queryVersionPr +
                            "union     " +
                            "select pr.id, pr.nombre, 3 objeto_tipo , pr.treePath, pr.fecha_inicio,   " +
                            "pr.fecha_fin, pr.duracion, pr.duracion_dimension,pr.costo,0,pr.acumulacion_costoid,   " +
                            "pr.programa, pr.subprograma, pr.proyecto, pr.actividad, pr.obra, pr.renglon, pr.ubicacion_geografica geografico,   " +
                            "pr.fecha_inicio_real, pr.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "pr.inversion_nueva " +
                            "from sipro_history.producto pr " +
                            "left outer join sipro_history.subcomponente sc on sc.id=pr.subcomponenteid   " + queryVersionSc +
                            "left outer join sipro_history.componente c on c.id = sc.componenteid   " + queryVersionC +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid   " + queryVersionP +
                            "where p.id=:proyectoId and p.estado=1 and c.estado=1 and sc.estado=1 and pr.estado=1   " +
                            queryVersionPr +
                            "union   " +
                            "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                            "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, " +
                            "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.renglon, sp.ubicacion_geografica geografico, " +
                            "sp.fecha_inicio_real, sp.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "sp.inversion_nueva " +
                            "from sipro_history.subproducto sp " +
                            "left outer join sipro_history.producto pr on pr.id=sp.productoid " + queryVersionPr +
                            "left outer join sipro_history.componente c on c.id=pr.componenteid " + queryVersionC +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:proyectoId and p.estado=1 and c.estado=1 and pr.estado=1 and sp.estado=1 and sp.id  " +
                            queryVersionSp +
                            "union   " +
                            "select sp.id, sp.nombre, 4 objeto_tipo,  sp.treePath, sp.fecha_inicio, " +
                            "sp.fecha_fin , sp.duracion, sp.duracion_dimension,sp.costo,0,sp.acumulacion_costoid, " +
                            "sp.programa, sp.subprograma, sp.proyecto, sp.actividad, sp.obra, sp.renglon, sp.ubicacion_geografica geografico, " +
                            "sp.fecha_inicio_real, sp.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, 0 ejecucion_fisica, " +
                            "sp.inversion_nueva " +
                            "from sipro_history.subproducto sp " +
                            "left outer join sipro_history.producto pr on pr.id=sp.productoid " + queryVersionPr +
                            "left outer join sipro_history.subcomponente sc on sc.id=pr.subcomponenteid " + queryVersionSc +
                            "left outer join sipro_history.componente c on c.id=sc.componenteid " + queryVersionC +
                            "left outer join sipro_history.proyecto p on p.id=c.proyectoid " + queryVersionP +
                            "where p.id=:proyectoId and p.estado=1 and c.estado=1 and sc.estado=1 and pr.estado=1 and sp.estado=1 and sp.id  " +
                            queryVersionSp +
                            "union " +
                            "select a.id, a.nombre, 5 objeto_tipo,  a.treePath, a.fecha_inicio, " +
                            "a.fecha_fin , a.duracion, a.duracion_dimension,a.costo,a.pred_objeto_id,a.acumulacion_costo acumulacion_costoid, " +
                            "a.programa, a.subprograma, a.proyecto, a.actividad, a.obra, a.renglon, a.ubicacion_geografica geografico, " +
                            "a.fecha_inicio_real, a.fecha_fin_real, " +
                            "p.unidad_ejecutoraunidad_ejecutora unidad_ejecutora, p.entidad entidad, a.porcentaje_avance ejecucion_fisica, " +
                            "a.inversion_nueva " +
                            "from sipro_history.actividad a " +
                            "JOIN sipro_history.proyecto p on p.id =:proyectoId " + queryVersionP +
                            "where a.estado=1 and  a.treepath like '" + (10000000 + proyectoId) + "%'" +
                            queryVersionA +
                            ") arbol " +
                            "left outer join ( " +
                            "select pa.id, pa.objeto_id, pa.objeto_tipo, SUM(pa.total) total, pp.pago pago "
                            + "from sipro_history.plan_adquisicion pa " +
                            "left outer join (select plan_adquisicionid id, SUM(pago) pago " +
                            "from sipro_history.plan_adquisicion_pago" +
                            queryVersionPadP +
                            " group by plan_adquisicionid) pp on pp.id = pa.id " +
                            queryVersionPad +
                            "group by pa.objeto_id, pa.objeto_tipo"
                            + ") costo on costo.objeto_id = arbol.id and costo.objeto_tipo = arbol.objeto_tipo " +
                            "order by treePath  ";

                    ret = db.Query<dynamic>(query, new { id = proyectoId }).AsList<dynamic>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ObjetoDAO.class", e);
            }
            return ret;
        }

        public static List<dynamic> getConsultaPagos(int objetoId, int objetoTipo, int anioInicial, int anioFinal, String lineaBase)
        {
            List<dynamic> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String queryVersionPa = "";
                    if (lineaBase == null)
                    {
                        queryVersionPa = " where pa.actual = 1 and pp.actual = 1 ";
                    }
                    else
                    {
                        queryVersionPa = " where pa.linea_base like '%" + lineaBase + "%' and pp.linea_base like '%" + lineaBase + "%' ";
                    }
                    String query =
                            "select t1.ejercicio, t1.objeto_id objeto_id_pago, t1.objeto_tipo objeto_tipo_pago, " +
                            "SUM(case when t1.mes = 1 then t1.pago end) enero, " +
                            "SUM(case when t1.mes = 2 then t1.pago end) febrero, " +
                            "SUM(case when t1.mes = 3 then t1.pago end) marzo, " +
                            "SUM(case when t1.mes = 4 then t1.pago end) abril, " +
                            "SUM(case when t1.mes = 5 then t1.pago end) mayo, " +
                            "SUM(case when t1.mes = 6 then t1.pago end) junio, " +
                            "SUM(case when t1.mes = 7 then t1.pago end) julio, " +
                            "SUM(case when t1.mes = 8 then t1.pago end) agosto, " +
                            "SUM(case when t1.mes = 9 then t1.pago end) septiembre, " +
                            "SUM(case when t1.mes = 10 then t1.pago end) octubre, " +
                            "SUM(case when t1.mes = 11 then t1.pago end) noviembre, " +
                            "SUM(case when t1.mes = 12 then t1.pago end) diciembre " +
                            "from " +
                            "( " +
                            "select pa.objeto_id, pa.objeto_tipo, year(pp.fecha_pago) ejercicio, month(pp.fecha_pago) mes, pp.pago " +
                            "from sipro_history.plan_adquisicion_pago pp " +
                            "join sipro_history.plan_adquisicion pa on pp.plan_adquisicionid = pa.id " +
                            queryVersionPa +
                            ") t1 " +
                            "where t1.objeto_id =:objetoId and t1.objeto_tipo = :objetoTipo " +
                            "and t1.ejercicio between :anioInicial and :anioFinal " +
                            "group by t1.ejercicio  ";

                    ret = db.Query<dynamic>(query, new { objetoId = objetoId, objetoTipo = objetoTipo, anioInicial = anioInicial, anioFinal = anioFinal }).AsList<dynamic>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ObjetoDAO.class", e);
            }
            return ret;
        }

        public static List<ObjetoCosto> getEstructuraConCosto(int idProyecto, int anioInicial, int anioFinal, bool obtenerPlanificado, bool obtenerReal, int? mesPresupuestos, String lineaBase, String usuario)
        {
            List<ObjetoCosto> lstPrestamo = new List<ObjetoCosto>();
            ObjetoCosto root = null;
            int fuente = 0;
            int organismo = 0;
            int correlativo = 0;

            using (DbConnection db = new OracleContext().getConnectionAnalytic())
            {
                List<dynamic> estructuras = getConsultaEstructuraConCosto(idProyecto, lineaBase);

                if (estructuras != null && estructuras.Count > 0)
                {
                    try
                    {
                        Object[] dato = (Object[])estructuras[0];
                        int objeto_id = dato[0] != null ? (int)dato[0] : 0;
                        String nombre = dato[1] != null ? (String)dato[1] : null;
                        int objeto_tipo = dato[2] != null ? Convert.ToInt32((BigInteger)dato[2]) : 0;
                        int nivel = (dato[3] != null) ? ((String)dato[3]).Length / 8 : 0;
                        String treePath = dato[3] != null ? ((String)dato[3]) : null;
                        DateTime fecha_inicial = dato[4] != null ? (DateTime)dato[4] : default(DateTime);
                        DateTime fecha_final = dato[5] != null ? (DateTime)dato[5] : default(DateTime);
                        int duracion = dato[6] != null ? (int)dato[6] : default(int);
                        decimal costo = dato[8] != null ? (decimal)dato[8] : default(decimal);
                        int acumulacion_costoid = dato[10] != null ? Convert.ToInt32(dato[10].ToString()) : default(int);
                        int? programa = dato[11] != null ? (int)dato[11] : default(int);
                        int? subprograma = dato[12] != null ? (int)dato[12] : default(int);
                        int? proyecto = dato[13] != null ? (int)dato[13] : default(int);
                        int? actividad = dato[14] != null ? (int)dato[14] : default(int);
                        int? obra = dato[15] != null ? (int)dato[15] : default(int);
                        int? reglon = dato[16] != null ? (int)dato[16] : default(int);
                        int? geografico = dato[17] != null ? (int)dato[17] : default(int);
                        DateTime fecha_inicial_real = dato[18] != null ? (DateTime)dato[18] : default(DateTime);
                        DateTime fecha_final_real = dato[19] != null ? (DateTime)dato[19] : default(DateTime);
                        int unidad_ejecutora = dato[20] != null ? (int)dato[20] : default(int);
                        int entidad = dato[21] != null ? (int)dato[21] : default(int);
                        int avance_fisico = dato[22] != null ? Convert.ToInt32((BigInteger)dato[22]) : default(int);
                        int inversion_nueva = dato[23] != null ? (int)dato[23] : default(int);
                        decimal totalPagos = dato[25] != null ? (decimal)dato[25] : default(decimal);

                        root.nombre = nombre;
                        root.objeto_id = objeto_id;
                        root.objeto_tipo = objeto_tipo;
                        root.nivel = nivel;
                        root.fecha_inicial = fecha_inicial;
                        root.fecha_final = fecha_final;
                        root.fecha_inicial_real = fecha_inicial_real;
                        root.fecha_final_real = fecha_final_real;
                        root.duracion = duracion;
                        root.acumulacion_costoid = acumulacion_costoid;
                        root.costo = costo;
                        root.totalPagos = totalPagos;
                        root.unidad_ejecutora = unidad_ejecutora;
                        root.entidad = entidad;
                        root.avance_fisico = avance_fisico;
                        root.inversion_nueva = inversion_nueva;
                        root.programa = programa ?? default(int);
                        root.subprograma = subprograma ?? default(int);
                        root.proyecto = proyecto ?? default(int);
                        root.actividad = actividad ?? default(int);
                        root.obra = obra ?? default(int);
                        root.renglon = reglon ?? default(int);
                        root.geografico = geografico ?? default(int);
                        root.treePath = treePath;

                        root.inicializarStanio(anioInicial, anioFinal);

                        if (obtenerReal || mesPresupuestos != default(int))
                        { //datos de Prestamo para costos reales o presupuestos
                            Proyecto proy = ProyectoDAO.getProyecto(root.objeto_id);
                            proy.prestamos = PrestamoDAO.getPrestamoById(proy.prestamoid ?? default(int));
                            if (proy.prestamos != null)
                            {
                                String codigoPresupuestario = proy.prestamos.codigoPresupuestario.ToString();
                                if (codigoPresupuestario != null && codigoPresupuestario.Length > 0)
                                {
                                    fuente = Convert.ToInt32(codigoPresupuestario.Substring(0, 2));
                                    organismo = Convert.ToInt32(codigoPresupuestario.Substring(2, 6));
                                    correlativo = Convert.ToInt32(codigoPresupuestario.Substring(6, 10));
                                    root = getCostoReal(root, fuente, organismo, correlativo, anioInicial, anioFinal, usuario);
                                }
                            }
                        }

                        ObjetoCosto nivel_actual_estructura = root;
                        for (int i = 0; i < estructuras.Count; i++)
                        {
                            dato = (Object[])estructuras[i];
                            objeto_id = dato[0] != null ? (int)dato[0] : 0;
                            nombre = dato[1] != null ? (String)dato[1] : null;
                            objeto_tipo = dato[2] != null ? (int)dato[2] : 0;
                            nivel = (dato[3] != null) ? ((String)dato[3]).Length / 8 : 0;
                            treePath = dato[3] != null ? ((String)dato[3]) : null;
                            fecha_inicial = dato[4] != null ? (DateTime)dato[4] : default(DateTime);
                            fecha_final = dato[5] != null ? (DateTime)dato[5] : default(DateTime);
                            duracion = dato[6] != null ? (int)dato[6] : default(int);
                            costo = dato[8] != null ? (decimal)dato[8] : default(int);
                            acumulacion_costoid = dato[10] != null ? (int)dato[10] : default(int);
                            programa = dato[11] != null ? (int)dato[11] : default(int);
                            subprograma = dato[12] != null ? (int)dato[12] : default(int);
                            proyecto = dato[13] != null ? (int)dato[13] : default(int);
                            actividad = dato[14] != null ? (int)dato[14] : default(int);
                            obra = dato[15] != null ? (int)dato[15] : default(int);
                            reglon = dato[16] != null ? (int)dato[16] : default(int);
                            geografico = dato[17] != null ? (int)dato[17] : default(int);
                            fecha_inicial_real = dato[18] != null ? (DateTime)dato[18] : default(DateTime);
                            fecha_final_real = dato[19] != null ? (DateTime)dato[19] : default(DateTime);
                            unidad_ejecutora = dato[20] != null ? (int)dato[20] : default(int);
                            entidad = dato[21] != null ? (int)dato[21] : default(int);
                            avance_fisico = dato[22] != null ? (int)dato[22] : default(int);
                            inversion_nueva = dato[23] != null ? (int)dato[23] : default(int);
                            totalPagos = dato[25] != null ? (decimal)dato[25] : default(decimal);

                            ObjetoCosto nodo = new ObjetoCosto();

                            nodo.nombre = nombre;
                            nodo.objeto_id = objeto_id;
                            nodo.objeto_tipo = objeto_tipo;
                            nodo.nivel = nivel;
                            nodo.fecha_inicial = fecha_inicial;
                            nodo.fecha_final = fecha_final;
                            nodo.fecha_inicial_real = fecha_inicial_real;
                            nodo.fecha_final_real = fecha_final_real;
                            nodo.duracion = duracion;
                            nodo.anios = null;
                            nodo.acumulacion_costoid = acumulacion_costoid;
                            nodo.costo = costo;
                            nodo.totalPagos = totalPagos;
                            nodo.unidad_ejecutora = unidad_ejecutora;
                            nodo.entidad = entidad;
                            nodo.avance_fisico = avance_fisico;
                            nodo.inversion_nueva = inversion_nueva;
                            nodo.programa = programa ?? default(int);
                            nodo.subprograma = subprograma ?? default(int);
                            nodo.proyecto = proyecto ?? default(int);
                            nodo.actividad = actividad ?? default(int);
                            nodo.obra = obra ?? default(int);
                            nodo.renglon = reglon ?? default(int);
                            nodo.geografico = geografico ?? default(int);
                            nodo.treePath = treePath;
                            nodo.inicializarStanio(anioInicial, anioFinal);

                            if (obtenerReal)
                            {
                                nodo = getCostoReal(nodo, fuente, organismo, correlativo, anioInicial, anioFinal, usuario);
                            }

                            if (mesPresupuestos != null)
                            {
                                nodo = getPresupuestos(nodo, fuente, organismo, correlativo, anioInicial, mesPresupuestos, usuario);
                            }

                            if (nodo.objeto_tipo > 0 && nodo.nivel != nivel_actual_estructura.nivel + 1)
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

                            if (nodo.objeto_tipo > 0)
                            {
                                nodo.parent = nivel_actual_estructura;
                                nivel_actual_estructura.children.Add(nodo);
                            }
                            else
                                nodo.parent = null;
                        }
                    }
                    catch (Exception e)
                    {
                        root = null;
                        CLogger.write("3", "ObjetoDAO.class", e);
                    }

                }
            }
            if (obtenerPlanificado)
            { //sube montos por hijos en arbol
                root = obtenerCostoPlanificado(root, anioInicial, anioFinal, lineaBase);
            }
            lstPrestamo = root.getListado(root);
            return lstPrestamo;
        }


        public static List<ObjetoCostoJasper> getEstructuraConCostoJasper(int proyectoId, int anioInicial, int anioFinal, int mesPresupuestos, String lineaBase, String usuario)
        {
            List<ObjetoCosto> listadoObjetos = getEstructuraConCosto(proyectoId, anioInicial, anioFinal, true, true, mesPresupuestos, lineaBase, usuario);
            List<ObjetoCostoJasper> listadoCostos = new List<ObjetoCostoJasper>();

            for (int i = 0; i < listadoObjetos.Count; i++)
            {
                ObjetoCosto temp = listadoObjetos[i];
                ObjetoCostoJasper elemento = new ObjetoCostoJasper(temp.nombre, temp.objeto_id, temp.objeto_tipo, temp.nivel,
                        temp.fecha_inicial, temp.fecha_final, temp.fecha_inicial_real, temp.fecha_final_real,
                        temp.duracion, temp.acumulacion_costoid, temp.costo, temp.totalPagos, temp.programa,
                        temp.subprograma, temp.proyecto, temp.actividad, temp.obra, temp.renglon, temp.geografico, temp.treePath,
                        Math.Round(temp.anios[0].mes[0].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[1].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[2].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[3].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[4].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[5].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[6].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[7].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[8].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[9].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[10].planificado, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[11].planificado, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[0].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[1].real, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[2].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[3].real, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[4].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[5].real, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[6].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[7].real, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[8].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[9].real, 2, MidpointRounding.AwayFromZero),
                        Math.Round(temp.anios[0].mes[10].real, 2, MidpointRounding.AwayFromZero), Math.Round(temp.anios[0].mes[11].real, 2, MidpointRounding.AwayFromZero),
                        temp.ejecutado, temp.asignado, temp.modificaciones, temp.avance_fisico, temp.inversion_nueva);
                listadoCostos.Add(elemento);
            }
            return listadoCostos;
        }

        private static ObjetoCosto obtenerCostoPlanificado(ObjetoCosto objetoCosto, int anioInicial, int anioFinal, String lineaBase)
        {
            if (objetoCosto.children != null && objetoCosto.children.Count != 0)
            { //tiene hijos
                for (int a = 0; a < (anioFinal - anioInicial + 1); a++)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        objetoCosto.anios[a].mes[m].planificado = decimal.Zero;
                    }
                }
                List<ObjetoCosto> hijos = objetoCosto.children;
                for (int h = 0; h < hijos.Count; h++)
                {
                    ObjetoCosto hijo = hijos[h];
                    hijo.anios = obtenerCostoPlanificado(hijo, anioInicial, anioFinal, lineaBase).anios;
                    for (int a = 0; a < (anioFinal - anioInicial + 1); a++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            objetoCosto.anios[a].mes[m].planificado = objetoCosto.anios[a].mes[m].planificado;
                            hijo.anios[a].mes[m].planificado = hijo.anios[a].mes[m].planificado;
                            objetoCosto.anios[a].mes[m].planificado = objetoCosto.anios[a].mes[m].planificado + hijo.anios[a].mes[m].planificado;
                            objetoCosto.anios[a].mes[m].real = objetoCosto.anios[a].mes[m].real;
                            hijo.anios[a].mes[m].real = hijo.anios[a].mes[m].real;
                            objetoCosto.anios[a].mes[m].real = objetoCosto.anios[a].mes[m].real + hijo.anios[a].mes[m].real;
                        }
                    }
                }
            }
            else
            { //es hijo
                if (objetoCosto.totalPagos.CompareTo(decimal.Zero) != 0)
                {
                    //obtener pagos
                    List<dynamic> estructuraPagos = getConsultaPagos(objetoCosto.objeto_id, objetoCosto.objeto_tipo, anioInicial, anioFinal, lineaBase);

                    foreach (dynamic objPago in estructuraPagos)
                    {
                        int ejercicio = (int)objPago[0];
                        objetoCosto.anios[ejercicio - anioInicial].anio = ejercicio;
                        for (int m = 0; m < 12; m++)
                        {
                            objetoCosto.anios[ejercicio - anioInicial].mes[m].planificado = (decimal)objPago[3 + m];
                        }
                    }
                }
                else
                {
                    //utilizar costo del objeto
                    for (int a = 0; a < (anioFinal - anioInicial + 1); a++)
                    {
                        objetoCosto.anios[a].anio = anioInicial + a;
                        ObjetoCosto.stanio anioObj = objetoCosto.anios[a];
                        if (objetoCosto.fecha_inicial != null && objetoCosto.fecha_final != null)
                        {
                            int mesInicial = objetoCosto.fecha_inicial.Month - 1;
                            int anioInicialObj = objetoCosto.fecha_inicial.Year;
                            int mesFinal = objetoCosto.fecha_final.Month - 1;
                            int anioFinalObj = objetoCosto.fecha_final.Year;
                            if ((anioInicial + a) >= anioInicialObj && (anioInicial + a) <= anioFinalObj)
                            {
                                int acumulacionCostoId = objetoCosto.acumulacion_costoid;
                                if (acumulacionCostoId.CompareTo(1) == 0)
                                {
                                    if (anioInicialObj == (anioInicial + a))
                                    {
                                        anioObj.mes[mesInicial].planificado = objetoCosto.costo;
                                    }
                                }
                                else if (acumulacionCostoId.CompareTo(2) == 0)
                                {
                                    List<PagoPlanificado> lstPagosPlanificados = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objetoCosto.objeto_id, objetoCosto.objeto_tipo);
                                    DateTime cal = new DateTime();
                                    foreach (PagoPlanificado pago in lstPagosPlanificados)
                                    {
                                        cal = pago.fechaPago;
                                        int mesPago = cal.Month;
                                        int anioPago = cal.Year;
                                        if (anioObj.anio == anioPago)
                                        {
                                            anioObj.mes[mesPago].planificado += pago.pago;
                                        }
                                    }
                                }
                                else
                                {
                                    if (anioFinalObj == anioObj.anio)
                                    {
                                        anioObj.mes[mesFinal].planificado += objetoCosto.costo;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return objetoCosto;
        }

        private static ObjetoCosto getCostoReal(ObjetoCosto objetoCosto, int fuente, int organismo, int correlativo, int anioInicial, int anioFinal, String usuario)
        {
            List<List<decimal>> presupuestoPrestamo = new List<List<decimal>>();

            if (objetoCosto.objeto_tipo == 0)
            {
                presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestoProyecto(fuente, organismo, correlativo, anioInicial, anioFinal);
            }
            else
            {
                presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestoPorObjeto(fuente, organismo, correlativo,
                        anioInicial, anioFinal, objetoCosto.programa, objetoCosto.subprograma, objetoCosto.proyecto,
                        objetoCosto.actividad, objetoCosto.obra, objetoCosto.renglon, objetoCosto.geografico);
            }

            if (presupuestoPrestamo.Count > 0)
            {
                int pos = 0;
                foreach (List<decimal> objprestamopresupuesto in presupuestoPrestamo)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        objetoCosto.anios[pos].mes[m].real = objprestamopresupuesto[m];
                    }
                    objetoCosto.anios[pos].anio = Convert.ToInt32(objprestamopresupuesto[12]);
                    pos = pos + 1;
                }
            }
            return objetoCosto;
        }

        private static ObjetoCosto getPresupuestos(ObjetoCosto objetoCosto, int? fuente, int? organismo, int? correlativo, int? ejercicio, int? mes, String usuario)
        {
            if (fuente != null && organismo != null && correlativo != null && ejercicio != null && ejercicio > 0 && mes != null && mes > 0
                    && objetoCosto.programa >= 0)
            {
                List<decimal> presupuestoPrestamo = new List<decimal>();
                presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestosPorObjeto(fuente, organismo, correlativo, ejercicio ?? default(int), mes ?? default(int),
                    objetoCosto.entidad,
                    objetoCosto.programa, objetoCosto.subprograma, objetoCosto.proyecto, objetoCosto.actividad, objetoCosto.obra,
                    objetoCosto.renglon, objetoCosto.geografico);

                if (presupuestoPrestamo.Count > 0)
                {
                    objetoCosto.asignado = presupuestoPrestamo[0];
                    objetoCosto.ejecutado = presupuestoPrestamo[1];
                    objetoCosto.modificaciones = presupuestoPrestamo[2];
                }
            }
            return objetoCosto;
        }

        public static bool tieneHijos(int objetoId, int objetoTipo)
        {
            if (ActividadDAO.getActividadesPorObjeto(objetoId, objetoTipo) != null && ActividadDAO.getActividadesPorObjeto(objetoId, objetoTipo).Count > 0)
            {
                return true;
            }
            switch (objetoTipo)
            {
                case 0:
                    List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(objetoId);
                    if (componentes != null && componentes.Count > 0)
                    {
                        return true;
                    }
                    return false;
                case 1:
                    List<Producto> productos = ProductoDAO.getProductosByComponente(objetoId);
                    Componente componente = ComponenteDAO.getComponente(objetoId);
                    if (productos != null && productos.Count > 0)
                    {
                        return true;
                    }

                    List<Subcomponente> subcomponentes = SubComponenteDAO.getSubComponentesPorComponente(objetoId);
                    if (subcomponentes != null && subcomponentes.Count > 0)
                    {
                        return true;
                    }
                    return false;
                case 2:
                    productos = ProductoDAO.getProductosBySubComponente(objetoId);
                    if (productos != null && productos.Count > 0)
                    {
                        return true;
                    }
                    return false;
                case 3:
                    List<Subproducto> subproductos = SubproductoDAO.getSubproductosByProductoid(objetoId);
                    if (subproductos != null && subproductos.Count > 0)
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static List<ObjetoHoja> getHojas(int proyectoId){
            List<ObjetoHoja> hojas = new List<ObjetoHoja>();
            try{
                ObjetoHoja temp = null;
                List<dynamic> resultados = new List<Object>();
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT c.* FROM componente c",
                        "WHERE NOT EXISTS (SELECT * FROM producto pr WHERE pr.componenteid=c.id AND pr.estado=1)",
                        "AND NOT EXISTS (SELECT * FROM subcomponente s WHERE s.componenteid=c.id AND s.estado=1)",
                        "AND NOT EXISTS (SELECT * FROM actividad a WHERE a.objeto_id=c.id and a.objeto_tipo=1 AND a.estado=1)",
                        "AND c.proyectoid=:proyectoId");

                    resultados = db.Query<dynamic>(query, new { proyectoId = proyectoId }).AsList<dynamic>();

                    Proyecto proyecto = null;
                    Componente componente = null;
                    Subcomponente subcomponente = null;
                    Producto producto = null;
                    Subproducto subproducto = null;
                    Actividad actividadP = null;

                    foreach (Object obj in resultados)
                    {
                        if (proyecto == null)
                        {
                            componente = (Componente)obj;
                            proyecto = ProyectoDAO.getProyecto(componente.proyectoid);
                        }

                        temp = new ObjetoHoja(1, obj, 0, proyecto);
                        hojas.Add(temp);
                    }

                    query = String.Join(" ", "SELECT s FROM Subcomponente s",
                        "WHERE not exists (FROM Producto p where p.producto.id=pr.id and sp.estado=1)",
                        "and not exists (FROM Actividad a where a.objetoId=pr.id and a.objetoTipo=2 and a.estado=1)",
                        "and pr.componente.proyecto.id=:proyectoId");

                    resultados = db.Query<dynamic>(query, new { proyectoId = proyectoId }).AsList<dynamic>();

                    foreach (Object obj in resultados)
                    {
                        subcomponente = (Subcomponente)obj;
                        componente = ComponenteDAO.getComponente(subcomponente.componenteid);
                        temp = new ObjetoHoja(2, obj, 1, componente);
                        hojas.Add(temp);
                    }

                    query = String.Join(" ", "SELECT pr FROM Producto pr",
                            "WHERE not exists (FROM Subproducto sp where sp.producto.id=pr.id and sp.estado=1)",
                            "and not exists (FROM Actividad a where a.objetoId=pr.id and a.objetoTipo=3 and a.estado=1)",
                            "and (pr.componente.proyecto.id=:proyectoId or pr.subcomponente.componente.componente.proyecto.id=:proyectoId)");

                    resultados = db.Query<dynamic>(query, new { proyectoId = proyectoId }).AsList<dynamic>();

                    foreach (Object obj in resultados)
                    {
                        producto = (Producto)obj;
                        producto.componentes = ComponenteDAO.getComponentePorId(producto.componenteid ?? default(int), null);
                        producto.subcomponentes = SubComponenteDAO.getSubComponentePorId(producto.subcomponenteid ?? default(int), null);
                        if (producto.componentes != null)
                        {
                            componente = ComponenteDAO.getComponente(producto.componenteid ?? default(int));
                            temp = new ObjetoHoja(3, obj, 1, componente);
                            hojas.Add(temp);
                        }
                        if (producto.subcomponentes != null)
                        {
                            subcomponente = SubComponenteDAO.getSubComponente(producto.subcomponenteid ?? default(int));
                            temp = new ObjetoHoja(3, obj, 2, subcomponente);
                            hojas.Add(temp);
                        }
                    }

                    query = String.Join(" ", "SELECT sp FROM Subproducto sp",
                        "WHERE not exists (FROM Actividad a where a.objetoId=sp.id and a.objetoTipo=4 and a.estado=1)",
                        "and (sp.producto.componente.proyecto.id=:proyectoId or sp.producto.subcomponente.componente.proyecto.id=:proyectoId)");

                    resultados = db.Query<dynamic>(query, new { proyectoId = proyectoId }).AsList<dynamic>();

                    foreach (Object obj in resultados)
                    {
                        subproducto = (Subproducto)obj;
                        producto = ProductoDAO.getProductoPorId(subproducto.productoid);
                        temp = new ObjetoHoja(4, obj, 3, producto);
                        hojas.Add(temp);
                    }

                    query = String.Join(" ", "SELECT a FROM Actividad a",
                        "WHERE not exists (FROM Actividad a2 where a2.objetoId=a.id and a2.objetoTipo=5 and a2.estado=1 and a2.treePath like '" + (10000000 + proyectoId) + "%')",
                        "and a.treePath like '" + (10000000 + proyectoId) + "%'");

                    resultados = db.Query<dynamic>(query, new { proyectoId = proyectoId }).AsList<dynamic>();

                    foreach (Object obj in resultados)
                    {
                        Actividad actividad = (Actividad)obj;
                        switch (actividad.objetoTipo)
                        {
                            case 0:
                                proyecto = ProyectoDAO.getProyecto(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 0, proyecto);
                                break;
                            case 1:
                                componente = ComponenteDAO.getComponente(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 1, componente);
                                break;
                            case 2:
                                subcomponente = SubComponenteDAO.getSubComponente(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 2, componente);
                                break;
                            case 3:
                                producto = ProductoDAO.getProductoPorId(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 3, producto);
                                break;
                            case 4:
                                subproducto = SubproductoDAO.getSubproductoPorId(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 4, subproducto);
                                break;
                            case 5:
                                actividadP = ActividadDAO.getActividadPorId(Convert.ToInt32(actividad.objetoId));
                                temp = new ObjetoHoja(5, obj, 5, actividadP);
                                break;
                        }
                        hojas.Add(temp);
                    }
                }
            }catch(Exception e){
                CLogger.write("4", "ObjetoDAO.class", e);
            }

            return hojas;
        }

        public static Object getObjetoPorIdyTipo(int id, int tipo) {
            Object ret = null;
            switch (tipo) {
                case 0: ret = (Object)ProyectoDAO.getProyecto(id); break;
                case 1: ret = (Object)ComponenteDAO.getComponente(id); break;
                case 2: ret = (Object)SubComponenteDAO.getSubComponente(id); break;
                case 3: ret = (Object)ProductoDAO.getProductoPorId(id); break;
                case 4: ret = (Object)SubproductoDAO.getSubproductoPorId(id); break;
                case 5: ret = (Object)ActividadDAO.getActividadPorId(id); break;
            }
            return ret;
        }

        public static decimal calcularCostoPlan(Object objeto, int objetoTipo)
        {
            decimal costo = decimal.Zero;
            try
            {
                Type objetoType = objeto.GetType();
                var getId = objetoType.GetProperty("id");
                var getCosto = objetoType.GetProperty("costo");

                PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionByObjeto(objetoTipo, (int)getId.GetValue(objeto));
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
                        costo = pa.total ?? default(decimal);
                }
                else
                    costo = (decimal)getCosto.GetValue(objeto);
            }
            catch (Exception e)
            {
                CLogger.write("5", "ObjetoDAO.class", e);
            }
            return costo;
        }

        public static bool borrarHijos(String treePathPadre, int objetoTipo, String usuarioActualiza)
        {
            bool ret = false;
            List<Object> objetos = new List<Object>();
            List<dynamic> objetos_nuevos = new List<Object>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "";

                    query = "SELECT * FROM ACTIVIDAD WHERE treepath LIKE '" + treePathPadre + "%' AND estado=1";
                    objetos.AddRange(db.Query<Actividad>(query).AsList<Actividad>());

                    if (objetoTipo < 5)
                    {
                        query = "SELECT * FROM SUBPRODUCTO WHERE treepath LIKE '" + treePathPadre + "%' AND estado=1";
                        objetos.AddRange(db.Query<Subproducto>(query).AsList<Subproducto>());
                    }
                    if (objetoTipo < 4)
                    {
                        query = "SELECT * FROM PRODUCTO WHERE treepath LIKE '" + treePathPadre + "%' AND estado=1";
                        objetos.AddRange(db.Query<Producto>(query).AsList<Producto>());
                    }
                    if (objetoTipo < 3)
                    {
                        query = "SELECT * FROM SUBCOMPONENTE WHERE treePath LIKE '" + treePathPadre + "%' AND estado=1";
                        objetos.AddRange(db.Query<Subcomponente>(query).AsList<Subcomponente>());
                    }
                    if (objetoTipo < 2)
                    {
                        query = "SELECT * FROM COMPONENTE WHERE treepath LIKE '" + treePathPadre + "%' AND estado=1";
                        objetos.AddRange(db.Query<Componente>(query).AsList<Componente>());
                    }
                    if (objetoTipo < 1)
                    {
                        query = "SELECT * FROM PROYECTO WHERE treepath = '" + treePathPadre + "' AND estado=1";
                        objetos.AddRange(db.Query<Proyecto>(query).AsList<Proyecto>());
                    }

                    foreach (Object objeto in objetos)
                    {
                        Type objetoType = objeto.GetType();
                        var setEstado = objetoType.GetProperty("estado");
                        var setUsuarioActualiza = objetoType.GetProperty("usuarioActualizo");
                        var setFechaActualizacion = objetoType.GetProperty("fechaActualizacion");
                        setEstado.SetValue(objeto, 0);
                        setUsuarioActualiza.SetValue(objeto, usuarioActualiza);
                        setFechaActualizacion.SetValue(objeto, DateTime.Now);
                    }

                    bool guardado = true;
                    for (int i = 0; i < objetos.Count; i++)
                    {
                        Type obj = objetos[i].GetType();
                        if (obj == typeof(Proyecto))
                        {
                            guardado = guardado & ProyectoDAO.guardarProyecto((Proyecto)objetos[i], false);
                        }
                        else if (obj == typeof(Componente))
                        {
                            guardado = guardado & ComponenteDAO.guardarComponente((Componente)objetos[i], false);
                        }
                        else if (obj == typeof(Subcomponente))
                        {
                            guardado = guardado & SubComponenteDAO.guardarSubComponente((Subcomponente)objetos[i], false);
                        }
                        else if (obj == typeof(Producto))
                        {
                            guardado = guardado & ProductoDAO.guardarProducto((Producto)objetos[i], false);
                        }
                        else if (obj == typeof(Subproducto))
                        {
                            guardado = guardado & SubproductoDAO.guardarSubproducto((Subproducto)objetos[i], false);
                        }
                        else if (obj == typeof(Actividad))
                        {
                            guardado = guardado & ActividadDAO.guardarActividad((Actividad)objetos[i], false);
                        }
                    }

                    ret = guardado;
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ObjetoDAO.class", e);
                ret = false;
            }

            return ret;
        }

        public static List<Vigente> getVigente(int fuente, int organismo, int correlativo,
                int ejercicio, int mesMaximo, int entidad)
        {
            List<Vigente> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    String query = String.Join(" ", "SELECT ",
                "SUM(CASE WHEN t.mes = 1 THEN t.vigente ELSE null END) enero,",
                "SUM(CASE WHEN t.mes = 2 THEN t.vigente ELSE null END) febrero,",
                "SUM(CASE WHEN t.mes = 3 THEN t.vigente ELSE null END) marzo,",
                "SUM(CASE WHEN t.mes = 4 THEN t.vigente ELSE null END) abril,",
                "SUM(CASE WHEN t.mes = 5 THEN t.vigente ELSE null END) mayo,",
                "SUM(CASE WHEN t.mes = 6 THEN t.vigente ELSE null END) junio,",
                "SUM(CASE WHEN t.mes = 7 THEN t.vigente ELSE null END) julio,",
                "SUM(CASE WHEN t.mes = 8 THEN t.vigente ELSE null END) agosto,",
                "SUM(CASE WHEN t.mes = 9 THEN t.vigente ELSE null END) septiembre,",
                "SUM(CASE WHEN t.mes = 10 THEN t.vigente ELSE null END) octubre,",
                "SUM(CASE WHEN t.mes = 11 THEN t.vigente ELSE null END) noviembre,",
                "SUM(CASE WHEN t.mes = 12 THEN t.vigente ELSE null END) diciembre",
                "FROM",
                "(",
                    "SELECT v.ejercicio,v.mes, SUM(v.asignado + v.modificaciones) vigente",
                    "FROM mv_ep_ejec_asig_vige v",
                    "WHERE organismo=:organismo",
                    "AND fuente=:fuente",
                    "AND correlativo=:correlativo",
                    "AND entidad=:entidad",
                    "AND ejercicio=:ejercicio",
                    "AND mes BETWEEN 1 AND :mesFinal",
                    "GROUP BY v.ejercicio,v.mes",
                ") AS t");

                    ret = db.Query<Vigente>(query, new
                    {
                        organismo = organismo,
                        fuente = fuente,
                        correlativo = correlativo,
                        entidad = entidad,
                        ejercicio = ejercicio,
                        mesFinal = mesMaximo
                    }).AsList<Vigente>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ObjetoDAO.class", e);
            }
            return ret;
        }

        public static decimal getAsignadoPorLineaPresupuestaria(int ejercicio, int entidad, int programa, int subprograma, int proyecto, int actividad, int obra, int renglon,
            int geografico)
        {
            Decimal ret = decimal.Zero;

            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    String query = String.Join(" ", "SELECT SUM(asignado) AS asignado FROM mv_ep_ejec_asig_vige c WHERE",
                        "programa=:programa",
                        "AND subprograma=:subprograma",
                        "AND proyecto=:proyecto",
                        "AND actividad=:actividad",
                        "AND obra=:obra",
                        "AND renglon=:renglon",
                        "AND geografico=:geografico",
                        "AND ejercicio=:ejercicio",
                        "AND entidad=:entidad");

                    ret = db.ExecuteScalar<decimal>(query, new {
                        programa = programa,
                        subprograma = subprograma,
                        proyecto = proyecto,
                        actividad = actividad,
                        obra = obra,
                        renglon = renglon,
                        geografico = geografico,
                        ejercicio = ejercicio,
                        entidad = entidad
                    });
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "ObjetoDAO.class", e);
            }

            return ret;
        }
    }
}
