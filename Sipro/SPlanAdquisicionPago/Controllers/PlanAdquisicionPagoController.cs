using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace SPlanAdquisicionPago.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class PlanAdquisicionPagoController : Controller
    {
        private class stPago
        {
            public int id;
            public String fecha;
            public String fechaReal;
            public decimal pago;
            public String descripcion;
        }

        // GET api/PlanAdquisicionPago/Pagos/1
        [HttpGet("{planId}")]
        [Authorize("Plan Adquisición Pago - Visualizar")]
        public IActionResult Pagos(int planId)
        {
            try
            {
                List<PlanAdquisicionPago> Pagos = PlanAdquisicionPagoDAO.getPagosByPlan(planId);

                List<stPago> resultado = new List<stPago>();
                foreach (PlanAdquisicionPago pago in Pagos)
                {
                    stPago temp = new stPago();
                    temp.id = Convert.ToInt32(pago.id);
                    temp.fecha = "";
                    temp.fechaReal = pago.fechaPago.ToString("dd/MM/yyyy H:mm:ss");
                    temp.pago = pago.pago ?? default(decimal);
                    temp.descripcion = pago.descripcion;
                    resultado.Add(temp);
                }

                return Ok(new { success = true, pagos = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("1", "PlanAdquisicionPagoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PlanAdquisicionPago/Pagos
        [HttpPost]
        [Authorize("Plan Adquisición Pago - Crear")]
        public IActionResult Pagos([FromBody]dynamic value)
        {
            try
            {
                int planId = (int)value.planId;
                bool result = false;

                List<stPago> pagos = JsonConvert.DeserializeObject<List<stPago>>((string)value.pagos);

                foreach (stPago pago in pagos)
                {
                    if (pago.id == 0)
                    {
                        PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionById(planId);
                        PlanAdquisicionPago nuevoPago = new PlanAdquisicionPago();
                        nuevoPago.planAdquisicionid = pa.id;
                        nuevoPago.fechaPago = Convert.ToDateTime(pago.fechaReal);
                        nuevoPago.pago = pago.pago;
                        nuevoPago.descripcion = pago.descripcion;
                        nuevoPago.usuarioCreo = User.Identity.Name;
                        nuevoPago.fechaCreacion = DateTime.Now;
                        nuevoPago.estado = 1;

                        result = PlanAdquisicionPagoDAO.guardarPago(nuevoPago);
                    }
                    else
                        result = true;
                }

                List<PlanAdquisicionPago> Pagos = PlanAdquisicionPagoDAO.getPagosByPlan(planId);

                List<stPago> resultado = new List<stPago>();
                foreach (PlanAdquisicionPago pago in Pagos)
                {
                    stPago temp = new stPago();
                    temp.id = Convert.ToInt32(pago.id);
                    temp.fecha = pago.fechaPago.ToString("dd/MM/yyyy H:mm:ss");
                    temp.pago = pago.pago ?? default(decimal);
                    temp.descripcion = pago.descripcion;
                    resultado.Add(temp);
                }

                return Ok(new { success = true, pagos = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("2", "PlanAdquisicionPagoController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/PlanAdquisicionPago/Pago/1
        [HttpDelete("{idPago}")]
        [Authorize("Plan Adquisición Pago - Eliminar")]
        public IActionResult Pago(int idPago)
        {
            try
            {
                PlanAdquisicionPago pago = PlanAdquisicionPagoDAO.getPagobyId(idPago);
                bool eliminado = PlanAdquisicionPagoDAO.eliminarPago(pago);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("3", "PlanAdquisicionPagoController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/PlanAdquisicionPago/EPagos/1
        [HttpDelete("{idObjeto}")]
        [Authorize("Plan Adquisición Pago - Eliminar")]
        public IActionResult EPagos(int idObjeto)
        {
            try
            {
                bool eliminado = PlanAdquisicionPagoDAO.eliminarPagos(idObjeto);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "PlanAdquisicionPagoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
