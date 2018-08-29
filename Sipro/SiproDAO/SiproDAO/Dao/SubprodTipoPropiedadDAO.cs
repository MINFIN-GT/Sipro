using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using Dapper;
using System.Data.Common;

namespace SiproDAO.Dao
{
    public class SubprodTipoPropiedadDAO
    {
        public static SubprodtipoPropiedad getSubprodtipoPropiedad(int subproductoTipoId, int propiedadId)
        {
            SubprodtipoPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubprodtipoPropiedad>("SELECT * FROM subprodtipo_propiedad WHERE subproducto_tipoid=:subproductoTipoId AND " +
                        "subproducto_propiedadid=:subproductoPropiedadid", new { subproductoTipoId = subproductoTipoId, subproductoPropiedadid = propiedadId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubprodTipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubproductoTipoPropiedad(SubprodtipoPropiedad subprodtipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subprodtipo_propiedad WHERE subproducto_tipoid=:subproductoTipoId AND " +
                        "subproducto_propiedadid=:subproductoPropiedadid", new { subproductoTipoId = subprodtipoPropiedad.subproductoTipoid,
                            subproductoPropiedadid = subprodtipoPropiedad.subproductoPropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subprodtipo_propiedad SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion WHERE subproducto_tipoid=:subproductoTipoid AND subproducto_propiedadid=:subproductoPropiedadid", subprodtipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO subprodtipo_propiedad VALUES (:subproductoTipoid, :subproductoPropiedadid, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion)", subprodtipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubprodTipoPropiedadDAO.class", e);
            }

            return ret;
        }

        public static bool eliminarSubproductoTipoPropiedad(SubprodtipoPropiedad subprodtipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM subprodtipo_propiedad WHERE subproducto_tipoid=:subproductoTipoId AND " +
                        "subproducto_propiedadid=:subproductoPropiedadid", new
                        {
                            subproductoTipoId = subprodtipoPropiedad.subproductoTipoid,
                            subproductoPropiedadid = subprodtipoPropiedad.subproductoPropiedadid
                        });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubprodTipoPropiedadDAO.class", e);
            }

            return ret;
        }

        public static List<SubprodtipoPropiedad> getPagina(int pagina, int registros, int subprodutoTipo)
        {
            List<SubprodtipoPropiedad> ret = new List<SubprodtipoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subprodtipo_propiedad WHERE subproducto_tipoid=:subproductoTipoid");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<SubprodtipoPropiedad>(query, new { subproductoTipoid = subprodutoTipo }).AsList<SubprodtipoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubprodTipoPropiedadDAO.class", e);
            }
            return ret;
        }

        /*public static String getJson(int pagina, int registros, Integer codigoTipo) {
            String jsonEntidades = "";

            List<SubprodtipoPropiedad> pojos = getPagina(pagina, registros, codigoTipo);

            List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

            for (SubprodtipoPropiedad pojo : pojos) {
                EstructuraPojo estructuraPojo = new EstructuraPojo();
                estructuraPojo.idTipo = pojo.getSubproductoTipo().getId();
                estructuraPojo.tipo = pojo.getSubproductoTipo().getNombre();
                estructuraPojo.idPropiedad = pojo.getSubproductoPropiedad().getId();
                estructuraPojo.propiedad = pojo.getSubproductoPropiedad().getNombre();
                estructuraPojo.idPropiedadTipo = pojo.getSubproductoPropiedad().getDatoTipo().getId();
                estructuraPojo.propiedadTipo = pojo.getSubproductoPropiedad().getDatoTipo().getNombre();
                estructuraPojo.estado = "C";

                listaEstructuraPojos.add(estructuraPojo);
            }

            jsonEntidades = Utils.getJSonString("productoTipos", listaEstructuraPojos);

            return jsonEntidades;
        }*/

        public static long getTotal()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM subprodtipo_propiedad");
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubprodTipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<SubprodtipoPropiedad> getTipoPropiedades(int subproductoTipoid)
        {
            List<SubprodtipoPropiedad> ret = new List<SubprodtipoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<SubprodtipoPropiedad>("SELECT * FROM subprodtipo_propiedad WHERE subproducto_tipoid=:subproductoTipoid", 
                        new { subproductoTipoid = subproductoTipoid }).AsList<SubprodtipoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubprodTipoPropiedadDAO.class", e);
            }
            return ret;
        }

	/*public static String getJson(Integer codigoTipo) {
		String jsonEntidades = "";

		List<SubprodtipoPropiedad> pojos = getTipoPropiedades(codigoTipo);

		List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

		for (SubprodtipoPropiedad pojo : pojos) {
			EstructuraPojo estructuraPojo = new EstructuraPojo();
			estructuraPojo.idTipo = pojo.getSubproductoTipo().getId();
			estructuraPojo.tipo = pojo.getSubproductoTipo().getNombre();
			estructuraPojo.idPropiedad = pojo.getSubproductoPropiedad().getId();
			estructuraPojo.propiedad = pojo.getSubproductoPropiedad().getNombre();
			estructuraPojo.idPropiedadTipo = pojo.getSubproductoPropiedad().getDatoTipo().getId();
			estructuraPojo.propiedadTipo = pojo.getSubproductoPropiedad().getDatoTipo().getNombre();
			estructuraPojo.estado = "C";

			listaEstructuraPojos.add(estructuraPojo);
		}

		jsonEntidades = Utils.getJSonString("subproductoTipos", listaEstructuraPojos);

		return jsonEntidades;
	}

	public static boolean persistirPropiedades(Integer idTipo, String propiedades, String usuario) {
		boolean ret = true;

		Gson gson = new Gson();

		List<EstructuraPojo> pojos = gson.fromJson(propiedades, new TypeToken<List<EstructuraPojo>>() {
		}.getType());

		if (!pojos.isEmpty()){
			for (EstructuraPojo pojo : pojos) {
				
				if(pojo.estado.equalsIgnoreCase("N")){
					ret = guardar(idTipo, pojo.idPropiedad, usuario);
				}else if(pojo.estado.equalsIgnoreCase("E")){
					ret = eliminar(pojo.idTipo, pojo.idPropiedad, usuario);
				}
			}
		}else{
			ret = true;
		}

		return ret;
	}
         
         
         */
    }
}
