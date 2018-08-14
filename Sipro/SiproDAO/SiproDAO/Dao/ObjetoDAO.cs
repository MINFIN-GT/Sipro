using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using System.Numerics;
using SiproModelAnalyticCore.Models;

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

        /*public static List<ObjetoCosto> getEstructuraConCosto(int idProyecto, int anioInicial, int anioFinal, bool obtenerPlanificado, bool obtenerReal, int mesPresupuestos,  String lineaBase, String usuario)
            {
            List<ObjetoCosto> lstPrestamo = new List<ObjetoCosto>();
            ObjetoCosto root = null;
            int fuente=0;
                int organismo =0;
                int correlativo =0;

                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    List <dynamic> estructuras = getConsultaEstructuraConCosto(idProyecto, lineaBase);

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
                            int programa = dato[11] != null ? (int)dato[11] : default(int);
                            int subprograma = dato[12] != null ? (int)dato[12] : default(int);
                            int proyecto = dato[13] != null ? (int)dato[13] : default(int);
                            int actividad = dato[14] != null ? (int)dato[14] : default(int);
                            int obra = dato[15] != null ? (int)dato[15] : default(int);
                            int reglon = dato[16] != null ? (int)dato[16] : default(int);
                            int geografico = dato[17] != null ? (int)dato[17] : default(int);
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
                            root.programa = programa;
                            root.subprograma = subprograma;
                            root.proyecto = proyecto;
                            root.actividad = actividad;
                            root.obra = obra;
                            root.renglon = reglon;
                            root.geografico = geografico;
                            root.treePath = treePath;

                            root.inicializarStanio(anioInicial, anioFinal);

                            if (obtenerReal || mesPresupuestos != default(int))
                            { //datos de Prestamo para costos reales o presupuestos
                                Proyecto proy = ProyectoDAO.getProyecto(root.getObjeto_id());
                                if (proy.getPrestamo() != null)
                                {
                                    String codigoPresupuestario = Long.toString(proy.getPrestamo().getCodigoPresupuestario());
                                    if (codigoPresupuestario != null && !codigoPresupuestario.isEmpty())
                                    {
                                        fuente = Utils.String2Int(codigoPresupuestario.substring(0, 2));
                                        organismo = Utils.String2Int(codigoPresupuestario.substring(2, 6));
                                        correlativo = Utils.String2Int(codigoPresupuestario.substring(6, 10));
                                        root = getCostoReal(root, fuente, organismo, correlativo, anioInicial, anioFinal, conn_analytic, usuario);
                                    }
                                }
                            }

                            ObjetoCosto nivel_actual_estructura = root;
                            for (int i = 0; i < estructuras.size(); i++)
                            {
                                dato = (Object[])estructuras.get(i);
                                objeto_id = dato[0] != null ? (Integer)dato[0] : 0;
                                nombre = dato[1] != null ? (String)dato[1] : null;
                                objeto_tipo = dato[2] != null ? ((BigInteger)dato[2]).intValue() : 0;
                                nivel = (dato[3] != null) ? ((String)dato[3]).length() / 8 : 0;
                                treePath = dato[3] != null ? ((String)dato[3]) : null;
                                fecha_inicial = dato[4] != null ? new DateTime((Timestamp)dato[4]) : null;
                                fecha_final = dato[5] != null ? new DateTime((Timestamp)dato[5]) : null;
                                duracion = dato[6] != null ? (Integer)dato[6] : null;
                                costo = dato[8] != null ? (BigDecimal)dato[8] : null;
                                acumulacion_costoid = dato[10] != null ? Integer.valueOf(dato[10].toString()) : null;
                                programa = dato[11] != null ? (Integer)dato[11] : null;
                                subprograma = dato[12] != null ? (Integer)dato[12] : null;
                                proyecto = dato[13] != null ? (Integer)dato[13] : null;
                                actividad = dato[14] != null ? (Integer)dato[14] : null;
                                obra = dato[15] != null ? (Integer)dato[15] : null;
                                reglon = dato[16] != null ? (Integer)dato[16] : null;
                                geografico = dato[17] != null ? (Integer)dato[17] : null;
                                fecha_inicial_real = dato[18] != null ? new DateTime((Timestamp)dato[18]) : null;
                                fecha_final_real = dato[19] != null ? new DateTime((Timestamp)dato[19]) : null;
                                unidad_ejecutora = dato[20] != null ? (Integer)dato[20] : null;
                                entidad = dato[21] != null ? (Integer)dato[21] : null;
                                avance_fisico = dato[22] != null ? Integer.valueOf(((BigInteger)dato[22]).toString()) : null;
                                inversion_nueva = dato[23] != null ? (Integer)dato[23] : null;
                                totalPagos = dato[25] != null ? (BigDecimal)dato[25] : null;

                                ObjetoCosto nodo = new ObjetoCosto(nombre, objeto_id, objeto_tipo, nivel, fecha_inicial, fecha_final,
                                        fecha_inicial_real, fecha_final_real, duracion, null,
                                        acumulacion_costoid, costo, totalPagos,
                                        unidad_ejecutora, entidad, avance_fisico, inversion_nueva,
                                        programa, subprograma, proyecto, actividad, obra, reglon, geografico, treePath);
                                nodo.inicializarStanio(anioInicial, anioFinal);

                                if (obtenerReal)
                                {
                                    nodo = getCostoReal(nodo, fuente, organismo, correlativo, anioInicial, anioFinal, conn_analytic, usuario);
                                }

                                if (mesPresupuestos != null)
                                {
                                    nodo = getPresupuestos(nodo, fuente, organismo, correlativo, anioInicial, mesPresupuestos, conn_analytic, usuario);
                                }

                                if (nodo.objeto_tipo > 0 && nodo.nivel != nivel_actual_estructura.nivel + 1)
                                {
                                    if (nodo.nivel > nivel_actual_estructura.nivel)
                                    {
                                        nivel_actual_estructura = nivel_actual_estructura.children.get(nivel_actual_estructura.children.size() - 1);
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
                                    nivel_actual_estructura.children.add(nodo);
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
                    if (CMariaDB.connectAnalytic()){


                    if(obtenerPlanificado){ //sube montos por hijos en arbol
                        root = obtenerPlanificado(root, anioInicial, anioFinal, lineaBase, conn_analytic);
                    }		
                    lstPrestamo = root.getListado(root);
                conn_analytic.close();
            }
            return lstPrestamo;
        }


        /*public static List<ObjetoCostoJasper> getEstructuraConCostoJasper(Integer proyectoId, int anioInicial, int anioFinal, Integer mesPresupuestos, String lineaBase, String usuario) throws SQLException{
            List<ObjetoCosto> listadoObjetos = getEstructuraConCosto(proyectoId, anioInicial, anioFinal, true, true, mesPresupuestos, lineaBase, usuario);
            List<ObjetoCostoJasper> listadoCostos = new ArrayList<ObjetoCostoJasper>(); 

            for (int i=0; i<listadoObjetos.size(); i++){
                ObjetoCosto temp = listadoObjetos.get(i);
                ObjetoCostoJasper elemento = new ObjetoCostoJasper(temp.nombre, temp.objeto_id, temp.objeto_tipo, temp.nivel,
                        temp.fecha_inicial!=null?temp.fecha_inicial.toDate():null, temp.fecha_final!=null?temp.fecha_final.toDate():null, 
                        temp.fecha_inicial_real!=null?temp.fecha_inicial_real.toDate():null, temp.fecha_final_real!=null?temp.fecha_final_real.toDate():null, 
                        temp.duracion, temp.acumulacion_costoid, temp.costo, temp.totalPagos, temp.programa,
                        temp.subprograma, temp.proyecto, temp.actividad, temp.obra, temp.renglon, temp.geografico, temp.treePath, 
                        temp.anios[0].mes[0].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[1].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[2].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[3].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[4].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[5].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[6].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[7].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[8].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[9].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[10].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[11].planificado.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[0].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[1].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[2].real.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[3].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[4].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[5].real.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[6].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[7].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[8].real.setScale(2, BigDecimal.ROUND_HALF_UP), 
                        temp.anios[0].mes[9].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[10].real.setScale(2, BigDecimal.ROUND_HALF_UP), temp.anios[0].mes[11].real.setScale(2, BigDecimal.ROUND_HALF_UP),
                        temp.ejecutado, temp.asignado, temp.modificaciones, temp.avance_fisico, temp.inversion_nueva);
                listadoCostos.add(elemento);
            }
            return listadoCostos;
        }

        private static ObjetoCosto obtenerPlanificado(ObjetoCosto objetoCosto, Integer anioInicial, Integer anioFinal, String lineaBase, Connection conn){
            if(objetoCosto.getChildren()!=null && !objetoCosto.getChildren().isEmpty()){ //tiene hijos
                for(int a=0; a<(anioFinal-anioInicial+1);a++){
                    for (int m=0; m<12; m++){
                        objetoCosto.anios[a].mes[m].planificado = new BigDecimal(0);
                    }
                }
                List<ObjetoCosto> hijos = objetoCosto.getChildren();
                for(int h=0; h<hijos.size(); h++){
                    ObjetoCosto hijo = hijos.get(h); 
                    hijo.anios = obtenerPlanificado(hijo, anioInicial, anioFinal, lineaBase, conn).anios;
                    for(int a=0; a<(anioFinal-anioInicial+1);a++){
                        for (int m=0; m<12; m++){
                            objetoCosto.anios[a].mes[m].planificado = objetoCosto.anios[a].mes[m].planificado!=null ? objetoCosto.anios[a].mes[m].planificado : new BigDecimal(0);
                            hijo.anios[a].mes[m].planificado = hijo.anios[a].mes[m].planificado!=null ? hijo.anios[a].mes[m].planificado : new BigDecimal(0);
                            objetoCosto.anios[a].mes[m].planificado = objetoCosto.anios[a].mes[m].planificado.add(hijo.anios[a].mes[m].planificado);
                            objetoCosto.anios[a].mes[m].real = objetoCosto.anios[a].mes[m].real!=null ? objetoCosto.anios[a].mes[m].real : new BigDecimal(0);
                            hijo.anios[a].mes[m].real = hijo.anios[a].mes[m].real!=null ? hijo.anios[a].mes[m].real : new BigDecimal(0);
                            objetoCosto.anios[a].mes[m].real = objetoCosto.anios[a].mes[m].real.add(hijo.anios[a].mes[m].real);
                        }
                    }
                }
            }else{ //es hijo
                if(objetoCosto.totalPagos!=null && objetoCosto.totalPagos.compareTo(BigDecimal.ZERO)!=0){
                    //obtener pagos
                    List<?> estructuraPagos = getConsultaPagos(objetoCosto.objeto_id, objetoCosto.objeto_tipo, anioInicial, anioFinal, lineaBase);

                    Iterator<?> iteradorPagos = estructuraPagos.iterator();
                    while (iteradorPagos.hasNext()) {
                        Object objetoPago = iteradorPagos.next();
                        Object[] objPago = (Object[]) objetoPago;
                        Integer ejercicio = objPago[0]!=null ? (Integer)objPago[0] : null;
                        objetoCosto.anios[ejercicio-anioInicial].anio = ejercicio;
                        for(int m=0; m<12; m++){
                            objetoCosto.anios[ejercicio-anioInicial].mes[m].planificado = objPago[3+m]!=null ? (BigDecimal)objPago[3+m] : null;
                        }
                    }	
                }else{
                    //utilizar costo del objeto
                    for(int a=0; a<(anioFinal-anioInicial+1); a++){
                        objetoCosto.anios[a].anio=anioInicial+a;
                        ObjetoCosto.stanio anioObj = objetoCosto.anios[a];
                        if(objetoCosto.getFecha_inicial()!=null && objetoCosto.getFecha_final()!=null){
                            int mesInicial = objetoCosto.getFecha_inicial().getMonthOfYear() -1;
                            int anioInicialObj = objetoCosto.getFecha_inicial().getYear();
                            int mesFinal = objetoCosto.getFecha_final().getMonthOfYear() -1;
                            int anioFinalObj = objetoCosto.getFecha_final().getYear();
                            if((anioInicial+a) >= anioInicialObj && (anioInicial+a)<=anioFinalObj){
                                Integer acumulacionCostoId = objetoCosto.getAcumulacion_costoid()!=null ? objetoCosto.getAcumulacion_costoid() : 3;
                                if(acumulacionCostoId.compareTo(1)==0){						
                                    if(anioInicialObj == (anioInicial+a)){
                                        anioObj.mes[mesInicial].planificado =  objetoCosto.getCosto() != null ? objetoCosto.getCosto() : new BigDecimal(0);
                                    }
                                }else if(acumulacionCostoId.compareTo(2)==0){
                                    List<PagoPlanificado> lstPagosPlanificados = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objetoCosto.objeto_id,objetoCosto.objeto_tipo);
                                    Calendar cal = Calendar.getInstance();
                                    for(PagoPlanificado pago : lstPagosPlanificados){
                                        cal.setTime(pago.getFechaPago());
                                        int mesPago = cal.get(Calendar.MONTH);
                                        int anioPago = cal.get(Calendar.YEAR);
                                        if(anioObj.anio == anioPago){
                                            anioObj.mes[mesPago].planificado = anioObj.mes[mesPago].planificado.add(pago.getPago());	
                                        }
                                    }								
                                }else{
                                    if(anioFinalObj == anioObj.anio){
                                        anioObj.mes[mesFinal].planificado =  objetoCosto.getCosto() != null ? objetoCosto.getCosto() : new BigDecimal(0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return objetoCosto;
        }

        private static ObjetoCosto getCostoReal(ObjetoCosto objetoCosto, Integer fuente, Integer organismo, Integer correlativo, Integer anioInicial, Integer anioFinal, Connection conn, String usuario){
            ArrayList<ArrayList<BigDecimal>> presupuestoPrestamo = new ArrayList<ArrayList<BigDecimal>>();

                if(objetoCosto.getObjeto_tipo() == 0){
                    presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestoProyecto(fuente, organismo, correlativo,anioInicial,anioFinal, conn);
                }else{
                    presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestoPorObjeto(fuente, organismo, correlativo, 
                            anioInicial, anioFinal, objetoCosto.getPrograma(), objetoCosto.getSubprograma(), objetoCosto.getProyecto(), 
                            objetoCosto.getActividad(), objetoCosto.getObra(), objetoCosto.getRenglon(), objetoCosto.getGeografico() ,conn);
                }

            if(presupuestoPrestamo.size() > 0){
                int pos = 0;
                for(ArrayList<BigDecimal> objprestamopresupuesto : presupuestoPrestamo){
                    for (int m=0; m<12; m++){
                        objetoCosto.getAnios()[pos].mes[m].real = objprestamopresupuesto.get(m) != null ? objprestamopresupuesto.get(m) : new BigDecimal(0);
                    }
                    objetoCosto.getAnios()[pos].anio = objprestamopresupuesto.get(12) != null ? objprestamopresupuesto.get(12).intValueExact() : 0;
                    pos = pos + 1;
                }
            }
            return objetoCosto;
        }

        private static ObjetoCosto getPresupuestos(ObjetoCosto objetoCosto, Integer fuente, Integer organismo, Integer correlativo, Integer ejercicio, Integer mes, Connection conn, String usuario){
            if(fuente!=null && organismo!=null && correlativo!=null && ejercicio!=null && ejercicio>0 && mes!=null && mes>0
                    && objetoCosto.getPrograma()!=null && objetoCosto.getPrograma()>=0){
                ArrayList<BigDecimal> presupuestoPrestamo = new ArrayList<BigDecimal>();
                presupuestoPrestamo = InformacionPresupuestariaDAO.getPresupuestosPorObjeto(fuente, organismo, correlativo, ejercicio, mes, 
                    objetoCosto.getEntidad(), 
                    objetoCosto.getPrograma(), objetoCosto.getSubprograma(), objetoCosto.getProyecto(), objetoCosto.getActividad(), objetoCosto.getObra(), 
                    objetoCosto.getRenglon(), objetoCosto.getGeografico(), conn);

                if(presupuestoPrestamo.size() > 0){
                    objetoCosto.setAsignado(presupuestoPrestamo.get(0));
                    objetoCosto.setEjecutado(presupuestoPrestamo.get(1));
                    objetoCosto.setModificaciones(presupuestoPrestamo.get(2));
                }
            }
            return objetoCosto;
        }*/

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

        /*public static List<ObjetoHoja> getHojas(Integer proyectoId){
            Session session = CHibernateSession.getSessionFactory().openSession();
            ArrayList<ObjetoHoja> hojas = new ArrayList<ObjetoHoja>();
            try{
                ObjetoHoja temp = null;
                List<?> resultados = new ArrayList<Object>();
                String query = String.join(" ", "SELECT c FROM Componente c",
                        "WHERE not exists (FROM Producto pr where pr.componente.id=c.id and pr.estado=1)",
                        "and not exists (FROM Subcomponente s where s.componente.id=c.id and s.estado=1)",
                        "and not exists (FROM Actividad a where a.objetoId=c.id and a.objetoTipo=1 and a.estado=1)",
                        "and c.proyecto.id=:proyectoId");
                Query<?> criteria = session.createQuery(query);
                criteria.setParameter("proyectoId", proyectoId);
                resultados = criteria.getResultList();

                Proyecto proyecto = null;
                for(Object obj : resultados){
                    if(proyecto == null)
                    {
                        Componente componente = (Componente)obj;
                        proyecto = ProyectoDAO.getProyecto(componente.getProyecto().getId());
                    }

                    temp = new ObjetoHoja(1, obj, 0, proyecto);
                    hojas.add(temp);
                }

                query = String.join(" ", "SELECT s FROM Subcomponente s",
                        "WHERE not exists (FROM Producto p where p.producto.id=pr.id and sp.estado=1)",
                        "and not exists (FROM Actividad a where a.objetoId=pr.id and a.objetoTipo=2 and a.estado=1)",
                        "and pr.componente.proyecto.id=:proyectoId");

                criteria = session.createQuery(query);
                criteria.setParameter("proyectoId", proyectoId);
                resultados = criteria.getResultList();

                Componente componente = null;
                for(Object obj : resultados){
                    Subcomponente subcomponente = (Subcomponente)obj;
                    componente = ComponenteDAO.getComponente(subcomponente.getComponente().getId());
                    temp = new ObjetoHoja(2, obj, 1, componente);
                    hojas.add(temp);
                }

                query = String.join(" ", "SELECT pr FROM Producto pr",
                        "WHERE not exists (FROM Subproducto sp where sp.producto.id=pr.id and sp.estado=1)",
                        "and not exists (FROM Actividad a where a.objetoId=pr.id and a.objetoTipo=3 and a.estado=1)",
                        "and (pr.componente.proyecto.id=:proyectoId or pr.subcomponente.componente.componente.proyecto.id=:proyectoId)");

                criteria = session.createQuery(query);
                criteria.setParameter("proyectoId", proyectoId);
                resultados = criteria.getResultList();

                Subcomponente subcomponente = null;
                componente = null;
                for(Object obj : resultados){
                    Producto producto = (Producto)obj;
                    if(producto.getComponente()!=null){
                        componente = ComponenteDAO.getComponente(producto.getComponente().getId());
                        temp = new ObjetoHoja(3, obj, 1, componente);
                        hojas.add(temp);
                    }
                    if(producto.getSubcomponente()!=null){
                        subcomponente = SubComponenteDAO.getSubComponente(producto.getSubcomponente().getId());
                        temp = new ObjetoHoja(3, obj, 2, subcomponente);
                        hojas.add(temp);
                    }
                }

                query = String.join(" ", "SELECT sp FROM Subproducto sp",
                        "WHERE not exists (FROM Actividad a where a.objetoId=sp.id and a.objetoTipo=4 and a.estado=1)",
                        "and (sp.producto.componente.proyecto.id=:proyectoId or sp.producto.subcomponente.componente.proyecto.id=:proyectoId)");

                criteria = session.createQuery(query);
                criteria.setParameter("proyectoId", proyectoId);
                resultados = criteria.getResultList();

                Producto producto = null;
                for(Object obj : resultados){
                    Subproducto subproducto = (Subproducto)obj;
                    producto = ProductoDAO.getProductoPorId(subproducto.getProducto().getId());
                    temp = new ObjetoHoja(4, obj, 3, producto);
                    hojas.add(temp);
                }

                query = String.join(" ", "SELECT a FROM Actividad a",
                        "WHERE not exists (FROM Actividad a2 where a2.objetoId=a.id and a2.objetoTipo=5 and a2.estado=1 and a2.treePath like '"+(10000000+proyectoId)+"%')",
                        "and a.treePath like '"+(10000000+proyectoId)+"%'");

                criteria = session.createQuery(query);
                resultados = criteria.getResultList();

                Actividad actividadP = null;
                Subproducto subproducto = null;
                for(Object obj : resultados){
                    Actividad actividad = (Actividad)obj;
                    switch(actividad.getObjetoTipo()){
                    case 0:
                        proyecto = ProyectoDAO.getProyecto(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 0, proyecto);
                        break;
                    case 1:
                        componente = ComponenteDAO.getComponente(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 1, componente);
                        break;
                    case 2:
                        subcomponente = SubComponenteDAO.getSubComponente(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 2, componente);
                        break;
                    case 3:
                        producto = ProductoDAO.getProductoPorId(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 3, producto);
                        break;
                    case 4:
                        subproducto = SubproductoDAO.getSubproductoPorId(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 4, subproducto);
                        break;
                    case 5:
                        actividadP = ActividadDAO.getActividadPorId(actividad.getObjetoId());
                        temp = new ObjetoHoja(5, obj, 5, actividadP);
                        break;
                    }
                    hojas.add(temp);
                }
            }catch(Exception e){
                CLogger.write("4", ObjetoDAO.class, e);
            }finally {
                session.close();
            }

            return hojas;
        }*/

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

        /*public static List<?> getVigente(Integer fuente, Integer organismo, Integer correlativo,
                int ejercicio, int mesMaximo, int entidad){
            List<?> ret = null;
            Session session = CHibernateSession.getSessionFactory().openSession();

            try{
                String query =String.join(" ","select ",
                "sum(case when t.mes = 1 then t.vigente else null end) enero,",
                "sum(case when t.mes = 2 then t.vigente else null end) febrero,",
                "sum(case when t.mes = 3 then t.vigente else null end) marzo,",
                "sum(case when t.mes = 4 then t.vigente else null end) abril,",
                "sum(case when t.mes = 5 then t.vigente else null end) mayo,",
                "sum(case when t.mes = 6 then t.vigente else null end) junio,",
                "sum(case when t.mes = 7 then t.vigente else null end) julio,",
                "sum(case when t.mes = 8 then t.vigente else null end) agosto,",
                "sum(case when t.mes = 9 then t.vigente else null end) septiembre,",
                "sum(case when t.mes = 10 then t.vigente else null end) octubre,",
                "sum(case when t.mes = 11 then t.vigente else null end) noviembre,",
                "sum(case when t.mes = 12 then t.vigente else null end) diciembre",
                "from",
                "(",
                    "select  v.ejercicio,v.mes, sum(v.asignado + v.modificaciones) vigente",
                    "from sipro_analytic.mv_ep_ejec_asig_vige  v",
                    "where organismo =:proyectoId",
                    "and fuente = ?2",
                    "and correlativo = ?3",
                    "and entidad = ?4",
    //				"and unidad_ejecutra = ?5",
                    "and ejercicio = ?6",
                    "and mes between 1 and ?7",
                    "group by  v.ejercicio,v.mes",
                ") as t");


                Query<?> criteria = session.createNativeQuery(query);
                criteria.setParameter("1", organismo);
                criteria.setParameter("2", fuente);
                criteria.setParameter("3", correlativo);
                criteria.setParameter("4", entidad);
    //			criteria.setParameter("5", unidad_ejecutora);
                criteria.setParameter("6", ejercicio);
                criteria.setParameter("7", mesMaximo);
                ret = criteria.getResultList();
            }
            catch(Throwable e){
                CLogger.write("7", ObjetoDAO.class, e);
            }
            finally{
                session.close();
            }
            return  ret;
        }*/

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
