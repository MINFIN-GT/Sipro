using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class InformacionPresupuestariaDAO
    {
        /*
         public static ArrayList<Date> getEstructuraArbolPrestamoFecha(int idPrestamo, Connection conn){
    	ArrayList<Date> ret = new ArrayList<Date>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select min(fecha_inicio) fecha_inicio, max(fecha_fin) fecha_fin from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente  is not null",
                            "and producto  is not null",
                            "and subproducto  is not null",
                            "group by componente",
                            "order by 2;");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                       ret.add(rs.getDate("fecha_inicio") );
                       ret.add(rs.getDate("fecha_fin"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("1", InformacionPresupuestariaDAO.class, e);
                }
            }
        } catch ( SQLException e) {
            e.printStackTrace();
        }
        return ret;
        
    }
	
	
    public static ArrayList<Integer> getEstructuraArbolComponentes(int idPrestamo, Connection conn){
    	ArrayList<Integer> ret = new ArrayList<Integer>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select componente, min(fecha_inicio) fecha from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente is not null",
                            "group by componente",
                            "order by 2;");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                       ret.add(rs.getInt("componente") );
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("2", InformacionPresupuestariaDAO.class, e);
                }
            }
        } catch ( SQLException e) {
            e.printStackTrace();
        }
        return ret;
        
    }
    
    public static ArrayList<Date> getEstructuraArbolComponentesFecha(int idPrestamo, int idComponente, Connection conn){
    	ArrayList<Date> ret = new ArrayList<Date>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select min(fecha_inicio) fecha_inicio, max(fecha_fin) fecha_fin from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ?",
                            "group by componente",
                            "order by 2;");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                       ret.add(rs.getDate("fecha_inicio") );
                       ret.add(rs.getDate("fecha_fin"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("3", InformacionPresupuestariaDAO.class, e);
                }
            }
        } catch ( SQLException e) {
            e.printStackTrace();
        }
        return ret;
        
    }
    
    public static ArrayList<Integer> getEstructuraArbolProducto(int idPrestamo, int idComponente, Connection conn){
    	ArrayList<Integer> ret = new ArrayList<Integer>();
        try {
            if( !conn.isClosed()){
                try{
                    String str_Query = String.join(" ","select producto, min(fecha_inicio) fecha ",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto is not null",
                            "group by producto",
                            "order by 2");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    ResultSet rs = pstm.executeQuery();
                   
                    
                    while(rs!=null && rs.next()){
                        ret.add(rs.getInt("producto"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    e.printStackTrace();
                    CLogger.write("4", InformacionPresupuestariaDAO.class, e);
                }
                
            }
        } catch (SQLException e) {
            CLogger.write("2", InformacionPresupuestariaDAO.class, e);
        }
        return ret;
    }
    
    public static ArrayList<Date> getEstructuraArbolProductoFecha(int idPrestamo, int idComponente, int idProducto, Connection conn){
    	ArrayList<Date> ret = new ArrayList<Date>();
        try {
            if( !conn.isClosed()){
                try{
                    String str_Query = String.join(" ","select producto, min(fecha_inicio) fecha_inicio, max(fecha_fin) fecha_fin ",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto = ?",
                            "group by producto",
                            "order by 2");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    pstm.setInt(3, idProducto);
                    ResultSet rs = pstm.executeQuery();
                   
                    
                    while(rs!=null && rs.next()){
                        ret.add(rs.getDate("fecha_inicio"));
                        ret.add(rs.getDate("fecha_fin"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("5", InformacionPresupuestariaDAO.class, e);
                }
                
            }
        } catch (SQLException e) {
            CLogger.write("6", InformacionPresupuestariaDAO.class, e);
        }
        return ret;
    }
    
    public static ArrayList<Integer> getEstructuraArbolSubProducto(int idPrestamo,int idComponente, int idProducto, Connection conn){
    	ArrayList<Integer> ret = new ArrayList<Integer>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select subproducto, min(fecha_inicio) fecha from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto = ? ",
                            "and subproducto is not null",
                            "group by subproducto",
                            "order by 2;");
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    pstm.setInt(3, idProducto);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                       ret.add(rs.getInt("subproducto"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("7", InformacionPresupuestariaDAO.class, e);
                }            
            }
        } catch (SQLException e) {
            CLogger.write("8", InformacionPresupuestariaDAO.class, e);
        }
        
        
        return ret;
    }
    
    public static ArrayList<Date> getEstructuraArbolSubProductoFecha(int idPrestamo,int idComponente, int idProducto, int idSubProducto, Connection conn){
    	ArrayList<Date> ret = new ArrayList<Date>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select min(fecha_inicio) fecha_inicio, max(fecha_fin) fecha_fin from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto = ? ",
                            "and subproducto = ?",
                            "group by subproducto",
                            "order by 2;");
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    pstm.setInt(3, idProducto);
                    pstm.setInt(4, idSubProducto);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                       ret.add(rs.getDate("fecha_inicio"));
                       ret.add(rs.getDate("fecha_fin"));
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    e.printStackTrace();
                    CLogger.write("9", InformacionPresupuestariaDAO.class, e);
                }            
            }
        } catch (SQLException e) {
            CLogger.write("9", InformacionPresupuestariaDAO.class, e);
        }
        
        
        return ret;
    }
    
    public static ArrayList<ArrayList<Integer>> getEstructuraArbolComponentesActividades(int idPrestamo, int idComponente, Connection conn){
    	ArrayList<ArrayList<Integer>> ret = new ArrayList<ArrayList<Integer>>();
        try {
            if( !conn.isClosed()){
                try{
                    String str_Query = String.join(" ","select prestamo,componente, actividad, treelevel, min(fecha_inicio) fecha",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto is null",
                            "and actividad is not null",
                            "group by prestamo, componente, actividad",
                            "order by 5, treelevel");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setFetchSize(50);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                        ArrayList<Integer> temp = new ArrayList<Integer>();
                        temp.add(rs.getInt("actividad"));
                        temp.add(rs.getInt("treelevel"));
                        ret.add(temp);
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("10", InformacionPresupuestariaDAO.class, e);
                }
                
            }
        } catch (SQLException e) {
            CLogger.write("11", InformacionPresupuestariaDAO.class, e);
        }
        return ret;
    }
    
    public static ArrayList<ArrayList<Integer>> getEstructuraArbolProductoActividades(int idPrestamo, int idComponente, int idProducto, Connection conn){
        ArrayList<ArrayList<Integer>> ret = new ArrayList<ArrayList<Integer>>();
        try {
            if( !conn.isClosed()){
                try{
                    String str_Query = String.join(" ","select prestamo,componente, producto, actividad, treelevel, min(fecha_inicio) fecha",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto = ? ",
                            "and subproducto is null",
                            "and actividad is not null",
                            "group by prestamo, componente, producto,actividad",
                            "order by 6, treelevel");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setFetchSize(50);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    pstm.setInt(3, idProducto);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                    	ArrayList<Integer> temp = new ArrayList<Integer>();
                        temp.add(rs.getInt("actividad"));
                        temp.add(rs.getInt("treelevel"));
                        ret.add(temp);
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("12", InformacionPresupuestariaDAO.class, e);
                }
                
            }
        } catch (SQLException e) {
            CLogger.write("13", InformacionPresupuestariaDAO.class, e);
        }
        return ret;
    }
    
    public static ArrayList<ArrayList<Integer>> getEstructuraArbolSubProductoActividades(int idPrestamo, int idComponente, int idProducto, int idsubProducto,
            Connection conn){
    	ArrayList<ArrayList<Integer>> ret = new ArrayList<ArrayList<Integer>>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select prestamo,componente, producto, subproducto, actividad, treelevel,min(fecha_inicio) fecha",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente = ? ",
                            "and producto = ? ",
                            "and subproducto = ? ",
                            "and actividad is not null",
                            "group by prestamo, componente, producto,subproducto, actividad",
                            "order by 7, treelevel");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setFetchSize(1000);
                    pstm.setInt(1, idPrestamo);
                    pstm.setInt(2, idComponente);
                    pstm.setInt(3, idProducto);
                    pstm.setInt(4, idsubProducto);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                        ArrayList<Integer> temp = new ArrayList<Integer>();
                        temp.add(rs.getInt("actividad"));
                        temp.add(rs.getInt("treelevel"));
                        ret.add(temp);
                    }
                    
                    rs.close();
                    pstm.close();
                }
                catch(Throwable e){
                    CLogger.write("14",InformacionPresupuestariaDAO.class, e);
                }
                
            }
        } catch (SQLException e) {
            CLogger.write("15", InformacionPresupuestariaDAO.class, e);
        }
        
        return ret;
    }
    
    public static ArrayList<ArrayList<Integer>> getEstructuraArbolPrestamoActividades(int idPrestamo, Connection conn){
    	 ArrayList<ArrayList<Integer>> ret = new ArrayList<ArrayList<Integer>>();
        try {
            if( !conn.isClosed() ){
                try{
                    String str_Query = String.join(" ","select prestamo,actividad, treelevel, min(fecha_inicio) fecha",
                            "from estructura_arbol",
                            "where prestamo = ? ",
                            "and componente is null",
                            "and producto is null",
                            "and subproducto is null",
                            "and actividad is not null",
                            "group by prestamo, actividad",
                            "order by 4, treelevel");
                    
                    PreparedStatement pstm  = conn.prepareStatement(str_Query);
                    pstm.setFetchSize(50);
                    pstm.setInt(1, idPrestamo);
                    ResultSet rs = pstm.executeQuery();
                    
                    while(rs!=null && rs.next()){
                        ArrayList<Integer> temp = new ArrayList<Integer>();
                        temp.add(rs.getInt("actividad"));
                        temp.add( rs.getInt("treelevel"));   
                        ret.add(temp);
                    }
                    
                    rs.close();
                    pstm.close();
                    
                }
                catch(Throwable e){
                    CLogger.write("16",InformacionPresupuestariaDAO.class, e);
                }
            }
        } catch (SQLException e) {
            CLogger.write("17", InformacionPresupuestariaDAO.class, e);
        }
        
        return ret;
    }*/

        public static List<List<decimal>> getPresupuestoProyecto(int fuente, int organismo, int correlativo, int anoInicial, int anoFinal)
        {
            List<List<decimal>> result = new List<List<decimal>>();

            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    String str_Query = String.Join(" ", "SELECT enero, febrero, marzo, abril, mayo, junio, julio, agosto, septiembre, octubre, noviembre, diciembre, ejercicio",
                        "FROM mv_ep_prestamo WHERE",
                        "fuente=:fuente and",
                        "organismo=:organismo and",
                        "correlativo=:correlativo and",
                        "ejercicio between :anoInicial and :anoFinal");

                    List<dynamic> ret = db.Query<dynamic>(str_Query, new
                    {
                        fuente = fuente,
                        organismo = organismo,
                        correlativo = correlativo,
                        anoInicial = anoInicial,
                        anoFinal = anoFinal
                    }).AsList<dynamic>();

                    foreach (dynamic obj in ret)
                    {
                        List<decimal> temp = new List<decimal>();
                        temp.Add(obj["enero"]);
                        temp.Add(obj["febrero"]);
                        temp.Add(obj["marzo"]);
                        temp.Add(obj["abril"]);
                        temp.Add(obj["mayo"]);
                        temp.Add(obj["junio"]);
                        temp.Add(obj["julio"]);
                        temp.Add(obj["agosto"]);
                        temp.Add(obj["septiembre"]);
                        temp.Add(obj["octubre"]);
                        temp.Add(obj["noviembre"]);
                        temp.Add(obj["diciembre"]);
                        temp.Add(obj["ejercicio"]);

                        result.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("18", "InformacionPresupuestariaDAO.class", e);
            }

            return result;
        }

        public static List<List<decimal>> getPresupuestoPorObjeto(int? fuente, int? organismo, int? correlativo, int? anoInicial, int? anoFinal, int? programa, int? subprograma,
                int? proyecto, int? actividad, int? obra, int? renglon, int? geografico)
        {
            List<List<decimal>> result = new List<List<decimal>>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    if (programa != null && programa >= 0)
                    {
                        String str_Query = String.Join(" ", "select sum(enero) enero, sum(febrero) febrero, sum(marzo) marzo, sum(abril) abril, sum(mayo) mayo, " +
                            "sum(junio) junio, sum(julio) julio, sum(agosto) agosto, sum(septiembre) septiembre, sum(octubre) octubre, sum(noviembre) noviembre, " +
                            "sum(diciembre) diciembre, ejercicio from mv_ep_estructura",
                            "where ejercicio between :anoInicial and :anoFinal");

                        if (fuente != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and fuente=:fuente");
                        }

                        if (organismo != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and organismo=:organismo");
                        }

                        if (correlativo != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and correlativo=:correlativo");
                        }

                        if (programa != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and programa=:programa");
                        }

                        if (subprograma != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and subprograma=:subprograma");
                        }

                        if (proyecto != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and proyecto=:proyecto");
                        }

                        if (actividad != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and actividad=:actividad");
                        }

                        if (obra != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and obra=:obra");
                        }

                        if (renglon != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and renglon=:renglon");
                        }

                        if (geografico != null)
                        {
                            str_Query = String.Join(" ", str_Query, "and geografico=:geografico");
                        }

                        List<dynamic> ret = db.Query<dynamic>(str_Query, new
                        {
                            anoInicial = anoInicial,
                            anoFinal = anoFinal,
                            fuente = fuente,
                            organismo = organismo,
                            correlativo = correlativo,
                            programa = programa,
                            subprograma = subprograma,
                            proyecto = proyecto,
                            actividad = actividad,
                            obra = obra,
                            renglon = renglon,
                            geografico = geografico
                        }).AsList<dynamic>();

                        foreach (dynamic obj in ret)
                        {
                            List<decimal> temp = new List<decimal>();
                            temp.Add(obj["enero"]);
                            temp.Add(obj["febrero"]);
                            temp.Add(obj["marzo"]);
                            temp.Add(obj["abril"]);
                            temp.Add(obj["mayo"]);
                            temp.Add(obj["junio"]);
                            temp.Add(obj["julio"]);
                            temp.Add(obj["agosto"]);
                            temp.Add(obj["septiembre"]);
                            temp.Add(obj["octubre"]);
                            temp.Add(obj["noviembre"]);
                            temp.Add(obj["diciembre"]);
                            temp.Add(obj["ejercicio"]);

                            result.Add(temp);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "InformacionPresupuestariaDAO.class", e);
            }

            return result;
        }


        public static List<decimal> getPresupuestosPorObjeto(int? fuente, int? organismo, int? correlativo, int ejercicio,
                int mes, int? entidad, int? programa, int? subprograma, int? proyecto, int? actividad, int? obra, int? renglon, int? geografico)
        {
            List<decimal> result = new List<decimal>();
            result.Add(decimal.Zero);
            result.Add(decimal.Zero);
            result.Add(decimal.Zero);
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    String str_Query = "SELECT SUM(asignado) asignado, SUM(devengado) devengado, SUM(modificaciones) modificaciones "
                        + " FROM ( "
                        + " SELECT SUM(asignado) asignado, 0 devengado, 0 modificaciones "
                        + " FROM mv_ep_ejec_asig_vige  "
                        + " WHERE "
                        + " mes = 0 ";
                    str_Query += entidad != null ? " and entidad = '" + entidad + "' " : "";
                    str_Query += programa != null ? " and programa = " + programa : "";
                    str_Query += subprograma != null ? " and subprograma = " + subprograma : "";
                    str_Query += proyecto != null ? " and proyecto = " + proyecto : "";
                    str_Query += actividad != null ? " and actividad = " + actividad : "";
                    str_Query += renglon != null ? " and renglon = " + renglon : "";
                    str_Query += geografico != null ? " and geografico = " + geografico : "";
                    str_Query += correlativo != null ? " and correlativo = " + correlativo : "";
                    str_Query += organismo != null ? " and organismo = " + organismo : "";
                    str_Query += fuente != null ? " and fuente = " + fuente : "";
                    str_Query += " and ejercicio = " + ejercicio
                    + " GROUP BY entidad "
                    + " UNION "
                    + " SELECT 0 asignado, SUM(ejecutado) devengado, 0 modificaciones "
                    + " FROM mv_ep_ejec_asig_vige  "
                    + " WHERE ejercicio = " + ejercicio;
                    str_Query += entidad != null ? " and entidad = '" + entidad + "' " : "";
                    str_Query += programa != null ? " and programa = " + programa : "";
                    str_Query += subprograma != null ? " and subprograma = " + subprograma : "";
                    str_Query += proyecto != null ? " and proyecto = " + proyecto : "";
                    str_Query += actividad != null ? " and actividad = " + actividad : "";
                    str_Query += renglon != null ? " and renglon = " + renglon : "";
                    str_Query += geografico != null ? " and geografico = " + geografico : "";
                    str_Query += correlativo != null ? " and correlativo = " + correlativo : "";
                    str_Query += organismo != null ? " and organismo = " + organismo : "";
                    str_Query += fuente != null ? " and fuente = " + fuente : "";
                    str_Query += " and mes = " + mes
                    + " group by entidad "
                    + " UNION "
                    + " select 0 asignado, 0 devengado, sum(modificaciones) modificaciones "
                    + " from sipro_analytic.mv_ep_ejec_asig_vige  "
                    + " where ejercicio = " + ejercicio;
                    str_Query += entidad != null ? " and entidad = '" + entidad + "' " : "";
                    str_Query += programa != null ? " and programa = " + programa : "";
                    str_Query += subprograma != null ? " and subprograma = " + subprograma : "";
                    str_Query += proyecto != null ? " and proyecto = " + proyecto : "";
                    str_Query += actividad != null ? " and actividad = " + actividad : "";
                    str_Query += renglon != null ? " and renglon = " + renglon : "";
                    str_Query += geografico != null ? " and geografico = " + geografico : "";
                    str_Query += correlativo != null ? " and correlativo = " + correlativo : "";
                    str_Query += organismo != null ? " and organismo = " + organismo : "";
                    str_Query += fuente != null ? " and fuente = " + fuente : "";
                    str_Query += " and mes = " + mes
                    + " GROUP BY entidad "
                    + " ) t1 ";

                    List<dynamic> ret = db.Query<dynamic>(str_Query).AsList<dynamic>();

                    foreach (dynamic obj in ret)
                    {
                        result.Add(obj["asignado"]);
                        result.Add(obj["devengado"]);
                        result.Add(obj["modificaciones"]);
                        //break;
                    }
                }

            }
            catch (Exception e)
            {
                CLogger.write("20", "InformacionPresupuestariaDAO.class", e);
            }

            return result;
        }
    
    /*public static JasperPrint generarJasper(Integer proyectoId, Integer anio, Integer mesPresupuestos, String lineaBase, String usuario) throws JRException, SQLException{
		JasperPrint jasperPrint = null;
		Proyecto proyecto = ProyectoDAO.getProyecto(proyectoId);
		if (proyecto!=null){
			Map<String, Object> parameters = new HashMap<String, Object>();
			parameters.put("proyectoId",proyectoId);
			parameters.put("usuario",usuario);
			
			List<ObjetoCostoJasper> listadoCostos = ObjetoDAO.getEstructuraConCostoJasper(proyectoId, anio, anio, mesPresupuestos, usuario, lineaBase);
			
			parameters.put("costos",listadoCostos);
			jasperPrint = CJasperReport.reporteJasperPrint(CJasperReport.PLANTILLA_EJECUCIONFINANCIERA, parameters);
		}
		return jasperPrint;
	}
         
         */
    }
}
