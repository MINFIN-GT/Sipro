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

namespace SPlanAdquisicion.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class PlanAdquisicionController : Controller
    {
        private class stpago
        {
            public String fechaPago;
            public decimal pago;
        }

        private class stadquisicion
        {
            public int id;
            public String tipoNombre;
            public int tipoId;
            public String categoriaNombre;
            public int categoriaId;
            public String medidaNombre;
            public int cantidad;
            public decimal precioUnitario;
            public decimal total;
            public long nog;
            public String numeroContrato;
            public decimal montoContrato;
            public String preparacionDocumentoPlanificada;
            public String preparacionDocumentoReal;
            public String lanzamientoEventoPlanificada;
            public String lanzamientoEventoReal;
            public String recepcionOfertasPlanificada;
            public String recepcionOfertasReal;
            public String adjudicacionPlanificada;
            public String adjudicacionReal;
            public String firmaContratoPlanificada;
            public String firmaContratoReal;
            public stpago[] pagos;
            public int tipoRevision;
            public String tipoRevisionNombre;
        }

        private class stnog
        {
            public int nog;
            public String numeroContrato;
            public decimal montoContrato;
            public String preparacionDocumentosReal;
            public String lanzamientoEventoReal;
            public String recepcionOfertasReal;
            public String adjudicacionReal;
            public String firmaContratoReal;
        }

        // GET api/values
        [HttpPost]
        public IActionResult Adquisicion([FromBody]dynamic value)
        {
            try
            {
                PlanAdquisicion pa = new PlanAdquisicion();
                pa.categoriaAdquisicions = CategoriaAdquisicionDAO.getCategoriaPorId((int)value.categoriaId);
                pa.categoriaAdquisicion = pa.categoriaAdquisicions.id;
                pa.tipoAdquisicions = TipoAdquisicionDAO.getTipoAdquisicionPorId((int)value.tipoId);
                pa.unidadMedida = (string)value.medidaNombre;
                pa.cantidad = (int)value.cantidad;
                pa.total = (int)value.total;
                pa.precioUnitario = (decimal)value.precioUnitario;
                pa.preparacionDocPlanificado = (DateTime)value.preparacionDocumentosPlanificada;
                pa.preparacionDocReal = (DateTime)value.preparacionDocReal;
                pa.lanzamientoEventoPlanificado = (DateTime)value.lanzamientoEventoPlanificada;
                pa.lanzamientoEventoReal = (DateTime)value.lanzamientoEventoReal;
                pa.recepcionOfertasPlanificado = (DateTime)value.recepcionOfertasPlanificada;
                pa.recepcionOfertasReal = (DateTime)value.recepcionOfertasReal;
                pa.adjudicacionPlanificado = (DateTime)value.adjudicacionPlanificada;
                pa.adjudicacionReal = (DateTime)value.adjudicacionReal;
                pa.firmaContratoPlanificado = (DateTime)value.firmaContratoPlanificada;
                pa.firmaContratoReal = (DateTime)value.firmaContratoReal;
                pa.objetoId = (int)value.objetoId;
                pa.objetoTipo = (int)value.objetoTipo;
                pa.usuarioCreo = User.Identity.Name;
                pa.fechaCreacion = DateTime.Now;
                pa.estado = 1;
                pa.bloqueado = 0;
                pa.numeroContrato = (string)value.numeroContrato;
                pa.montoContrato = (decimal)value.montoContrato;
                pa.nog = (int)value.nog;
                pa.tipoRevision = (int)value.tipoRevision;

                PlanAdquisicionDAO.guardarPlanAdquisicion(pa);

                bool guardado = PlanAdquisicionDAO.actualizarNivelesPagos((string)value.pagos, pa, User.Identity.Name, (int)value.objetoId, (int)value.objetoTipo);

                return Ok(new { success = guardado, id = pa.id });
            }
            catch (Exception e)
            {
                CLogger.write("1", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Adquisicion(int id, [FromBody]dynamic value)
        {
            try
            {                
                PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionById(id);
                pa.categoriaAdquisicions = CategoriaAdquisicionDAO.getCategoriaPorId((int)value.categoriaId);
                pa.categoriaAdquisicion = pa.categoriaAdquisicions.id;
                pa.tipoAdquisicions = TipoAdquisicionDAO.getTipoAdquisicionPorId((int)value.tipoId);
                pa.unidadMedida = (string)value.medidaNombre;
                pa.cantidad = (int)value.cantidad;
                pa.total = (int)value.total;
                pa.precioUnitario = (decimal)value.precioUnitario;
                pa.preparacionDocPlanificado = (DateTime)value.preparacionDocumentosPlanificada;
                pa.preparacionDocReal = (DateTime)value.preparacionDocReal;
                pa.lanzamientoEventoPlanificado = (DateTime)value.lanzamientoEventoPlanificada;
                pa.lanzamientoEventoReal = (DateTime)value.lanzamientoEventoReal;
                pa.recepcionOfertasPlanificado = (DateTime)value.recepcionOfertasPlanificada;
                pa.recepcionOfertasReal = (DateTime)value.recepcionOfertasReal;
                pa.adjudicacionPlanificado = (DateTime)value.adjudicacionPlanificada;
                pa.adjudicacionReal = (DateTime)value.adjudicacionReal;
                pa.firmaContratoPlanificado = (DateTime)value.firmaContratoPlanificada;
                pa.firmaContratoReal = (DateTime)value.firmaContratoReal;
                pa.objetoId = (int)value.objetoId;
                pa.objetoTipo = (int)value.objetoTipo;
                pa.usuarioCreo = User.Identity.Name;
                pa.fechaCreacion = DateTime.Now;
                pa.estado = 1;
                pa.bloqueado = 0;
                pa.numeroContrato = (string)value.numeroContrato;
                pa.montoContrato = (decimal)value.montoContrato;
                pa.nog = (int)value.nog;
                pa.tipoRevision = (int)value.tipoRevision;
                PlanAdquisicionPagoDAO.eliminarPagos(PlanAdquisicionPagoDAO.getPagosByPlan(Convert.ToInt32(pa.id)));

                PlanAdquisicionDAO.guardarPlanAdquisicion(pa);

                bool guardado = PlanAdquisicionDAO.actualizarNivelesPagos((string)value.pagos, pa, User.Identity.Name, (int)value.objetoId, (int)value.objetoTipo);

                return Ok(new { success = guardado, id = pa.id });
            }
            catch (Exception e)
            {
                CLogger.write("2", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        } 
        

    }
}
