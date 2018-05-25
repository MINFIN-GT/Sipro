using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class PlanAdquisicionPagoDAO
    {

        public static List<PlanAdquisicionPago> getPagosByPlan(int planId)
        {
            List<PlanAdquisicionPago> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<PlanAdquisicionPago>("SELECT * FROM PLAN_ADQUISICION_PAGO WHERE plan_adquisicionid=:planId", new { planId = planId }).AsList<PlanAdquisicionPago>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PlanAdquisicionPagoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarPago(PlanAdquisicionPago pago)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int guardado = db.Execute("INSERT INTO PLAN_ADQUISICION_PAGO VALUES (:id, :planAdquisicionid, :fechaPago, :pago, :descripcion, :usuarioCreo, " +
                        ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", pago);

                    ret = guardado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PlanAdquisicionPagoDAO.class", e);
            }

            return ret;
        }

        public static bool eliminarPago(PlanAdquisicionPago pago)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM PLAN_ADQUISICION_PAGO WHERE id=:id", new { id = pago.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "PlanAdquisicionPagoDAO.class", e);
            }

            return ret;
        }
	
	/*public static PlanAdquisicionPago getPagobyId(int idPago){
		PlanAdquisicionPago ret = null;
		List<PlanAdquisicionPago> listRet = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		
		try{
			Query<PlanAdquisicionPago> pago = session.createQuery("FROM PlanAdquisicionPago p where p.id=:id",PlanAdquisicionPago.class);
			pago.setParameter("id", idPago);
			listRet = pago.getResultList();
			
			ret = !listRet.isEmpty() ? listRet.get(0) : null;
		}
		catch(Throwable e){
			CLogger.write("4", PlanAdquisicionPagoDAO.class, e);
		}
		finally{
			session.close();
		}
		
		return ret;
	}*/
	
	public static bool eliminarPagos(int planId){
		bool ret = false;
		List<PlanAdquisicionPago> Pagos = getPagosByPlan(planId);
		try{
			foreach (PlanAdquisicionPago pago in Pagos){
				if(eliminarPago(pago))
					ret = true;
				else
					return false;
			}
			ret = true;
		}catch(Exception e){
			CLogger.write("5", "PlanAdquisicionPagoDAO.class", e);
		}
		
		return ret;
	}
	
	public static bool eliminarPagos(List<PlanAdquisicionPago> pagos){
		bool ret = false;
		try{
			foreach(PlanAdquisicionPago pago in pagos){
				if(eliminarPago(pago))
					ret = true;
				else
					return false;
			}
			ret = true;
		}catch(Exception e){
			CLogger.write("6", "PlanAdquisicionPagoDAO.class", e);
		}
		
		return ret;
	}
	
	/*public static List<PlanAdquisicionPago> getPagosByObjetoTipo(Integer objetoId,Integer objetoTipo){
		List<PlanAdquisicionPago> ret = null;
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<PlanAdquisicionPago> criteria = session.createQuery("FROM PlanAdquisicionPago p where p.planAdquisicion.objetoId=:objetoId AND p.planAdquisicio.objetoTipo=:objetoTipo",PlanAdquisicionPago.class);
			criteria.setParameter("objetoId", objetoId);
			criteria.setParameter("objetoTipo", objetoTipo);
			ret = criteria.getResultList();
		}catch(Throwable e){
			CLogger.write("7", PlanAdquisicionPagoDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	}
         
         */
    }
}
