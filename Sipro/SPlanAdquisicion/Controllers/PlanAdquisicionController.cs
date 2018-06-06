using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproModelAnalyticCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

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

        // POST api/PlanAdquisicion/Adquisicion
        [HttpPost]
        [Authorize("Plan Adquisición - Crear")]
        public IActionResult Adquisicion([FromBody]dynamic value)
        {
            try
            {
                PlanAdquisicionValidator validator = new PlanAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    PlanAdquisicion pa = new PlanAdquisicion();
                    pa.categoriaAdquisicions = CategoriaAdquisicionDAO.getCategoriaPorId(value.categoriaAdquisicion);
                    pa.categoriaAdquisicion = pa.categoriaAdquisicions.id;
                    pa.tipoAdquisicions = TipoAdquisicionDAO.getTipoAdquisicionPorId(value.tipoId);
                    pa.unidadMedida = value.medidaNombre;
                    pa.cantidad = value.cantidad;
                    pa.total = value.tota;
                    pa.precioUnitario = value.precioUnitario;
                    pa.preparacionDocPlanificado = value.preparacionDocumentosPlanificada;
                    pa.preparacionDocReal = value.preparacionDocReal;
                    pa.lanzamientoEventoPlanificado = value.lanzamientoEventoPlanificada;
                    pa.lanzamientoEventoReal = value.lanzamientoEventoReal;
                    pa.recepcionOfertasPlanificado = value.recepcionOfertasPlanificada;
                    pa.recepcionOfertasReal = value.recepcionOfertasReal;
                    pa.adjudicacionPlanificado = value.adjudicacionPlanificada;
                    pa.adjudicacionReal = value.adjudicacionReal;
                    pa.firmaContratoPlanificado = value.firmaContratoPlanificada;
                    pa.firmaContratoReal = value.firmaContratoReal;
                    pa.objetoId = value.objetoId;
                    pa.objetoTipo = value.objetoTipo;
                    pa.usuarioCreo = User.Identity.Name;
                    pa.fechaCreacion = DateTime.Now;
                    pa.estado = 1;
                    pa.bloqueado = 0;
                    pa.numeroContrato = value.numeroContrato;
                    pa.montoContrato = value.montoContrato;
                    pa.nog = value.nog;
                    pa.tipoRevision = value.tipoRevision;

                    PlanAdquisicionDAO.guardarPlanAdquisicion(pa);
                    string pagos = value.pagos;
                    int objetoId = value.objetoId;
                    int objetoTipo = value.objetoTipo;

                    bool guardado = PlanAdquisicionDAO.actualizarNivelesPagos(pagos, pa, User.Identity.Name, objetoId, objetoTipo);

                    return Ok(new { success = guardado, id = pa.id });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("1", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/PlanAdquisicion/Adquisicion/5
        [HttpPut("{id}")]
        [Authorize("Plan Adquisición - Editar")]
        public IActionResult Adquisicion(int id, [FromBody]dynamic value)
        {
            try
            {
                PlanAdquisicionValidator validator = new PlanAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    PlanAdquisicion pa = PlanAdquisicionDAO.getPlanAdquisicionById(id);
                    pa.categoriaAdquisicions = CategoriaAdquisicionDAO.getCategoriaPorId(value.categoriaAdquisicion);
                    pa.categoriaAdquisicion = pa.categoriaAdquisicions.id;
                    pa.tipoAdquisicions = TipoAdquisicionDAO.getTipoAdquisicionPorId(value.tipoId);
                    pa.unidadMedida = value.unidadMedida;
                    pa.cantidad = value.cantidad;
                    pa.total = value.total;
                    pa.precioUnitario = value.precioUnitario;
                    pa.preparacionDocPlanificado = value.preparacionDocumentosPlanificada;
                    pa.preparacionDocReal = value.preparacionDocReal;
                    pa.lanzamientoEventoPlanificado = value.lanzamientoEventoPlanificada;
                    pa.lanzamientoEventoReal = value.lanzamientoEventoReal;
                    pa.recepcionOfertasPlanificado = value.recepcionOfertasPlanificada;
                    pa.recepcionOfertasReal = value.recepcionOfertasReal;
                    pa.adjudicacionPlanificado = value.adjudicacionPlanificada;
                    pa.adjudicacionReal = value.adjudicacionReal;
                    pa.firmaContratoPlanificado = value.firmaContratoPlanificada;
                    pa.firmaContratoReal = value.firmaContratoReal;
                    pa.objetoId = value.objetoId;
                    pa.objetoTipo = value.objetoTipo;
                    pa.usuarioActualizo = User.Identity.Name;
                    pa.fechaActualizacion = DateTime.Now;
                    pa.estado = 1;
                    pa.bloqueado = 0;
                    pa.numeroContrato = value.numeroContrato;
                    pa.montoContrato = value.montoContrato;
                    pa.nog = value.nog;
                    pa.tipoRevision = value.tipoRevision;

                    PlanAdquisicionPagoDAO.eliminarPagos(PlanAdquisicionPagoDAO.getPagosByPlan(Convert.ToInt32(pa.id)));

                    PlanAdquisicionDAO.guardarPlanAdquisicion(pa);

                    bool guardado = PlanAdquisicionDAO.actualizarNivelesPagos(value.pagos, pa, User.Identity.Name, value.objetoId, value.objetoTipo);

                    return Ok(new { success = guardado, id = pa.id });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PlanAdquisicion/PlanAdquisicionPorObjeto
        [HttpPost]
        [Authorize("Plan Adquisición - Visualizar")]
        public IActionResult PlanAdquisicionPorObjeto([FromBody]dynamic value)
        {
            try
            {
                PlanAdquisicion adquisicion = PlanAdquisicionDAO.getPlanAdquisicionByObjeto((int)value.objetoTipo, (int)value.objetoId);
                if (adquisicion != null)
                {
                    stadquisicion temp = new stadquisicion();
                    temp.id = Convert.ToInt32(adquisicion.id);
                    temp.adjudicacionPlanificada = adquisicion.adjudicacionPlanificado != null ? adquisicion.adjudicacionPlanificado.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.adjudicacionReal = adquisicion.adjudicacionReal != null ? adquisicion.adjudicacionReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.cantidad = Convert.ToInt32(adquisicion.cantidad);
                    temp.categoriaId = Convert.ToInt32(adquisicion.categoriaAdquisicions.id);
                    temp.categoriaNombre = adquisicion.categoriaAdquisicions.nombre;
                    temp.firmaContratoPlanificada = adquisicion.firmaContratoPlanificado != null ? adquisicion.firmaContratoPlanificado.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.firmaContratoReal = adquisicion.firmaContratoReal != null ? adquisicion.firmaContratoReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.lanzamientoEventoPlanificada = adquisicion.lanzamientoEventoPlanificado != null ? adquisicion.lanzamientoEventoPlanificado.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.lanzamientoEventoReal = adquisicion.lanzamientoEventoReal != null ? adquisicion.lanzamientoEventoReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.medidaNombre = adquisicion.unidadMedida;
                    temp.montoContrato = adquisicion.montoContrato;
                    temp.nog = Convert.ToInt64(adquisicion.nog);
                    temp.numeroContrato = adquisicion.numeroContrato;
                    temp.precioUnitario = adquisicion.precioUnitario ?? default(decimal);
                    temp.preparacionDocumentoPlanificada = adquisicion.preparacionDocPlanificado != null ? adquisicion.preparacionDocPlanificado.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.preparacionDocumentoReal = adquisicion.preparacionDocReal != null ? adquisicion.preparacionDocReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.recepcionOfertasPlanificada = adquisicion.recepcionOfertasPlanificado != null ? adquisicion.recepcionOfertasPlanificado.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.recepcionOfertasReal = adquisicion.recepcionOfertasReal != null ? adquisicion.recepcionOfertasReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.tipoId = Convert.ToInt32(adquisicion.tipoAdquisicions.id);
                    temp.tipoNombre = adquisicion.tipoAdquisicions.nombre;
                    temp.total = adquisicion.total ?? default(decimal);
                    temp.tipoRevision = adquisicion.tipoRevision ?? default(int);
                    temp.tipoRevisionNombre = temp.tipoRevision == 1 ? "Ex-ante" : temp.tipoRevision == 2 ? "Ex-Post" : null;


                    List<PlanAdquisicionPago> lstpagos = PlanAdquisicionDAO.getPagos(Convert.ToInt32(adquisicion.id));
                    if (lstpagos != null && lstpagos.Count > 0)
                    {
                        List<stpago> pagos = new List<stpago>();
                        stpago pago = null;

                        for (int i = 0; i < lstpagos.Count; i++)
                        {
                            if (lstpagos[i].estado == 1)
                            {
                                pago = new stpago();
                                pago.fechaPago = lstpagos[i].fechaPago.ToString("dd/MM/yyyy H:mm:ss");
                                pago.pago = lstpagos[i].pago ?? default(decimal);
                                pagos.Add(pago);
                            }
                        }
                    }

                    return Ok(new { success = true, adquisicion = temp });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/PlanAdquisicion/TodasAdquisiciones
        [HttpDelete]
        [Authorize("Plan Adquisición - Eliminar")]
        public IActionResult TodasAdquisiciones([FromBody]dynamic value)
        {
            try
            {
                int objetoId = value.objetoId != null ? (int)value.objetoId : default(int);
                int objetoTipo = value.objetoTipo != null ? (int)value.objetoTipo : default(int);

                bool eliminado = PlanAdquisicionDAO.borrarTodosPlan(objetoId, objetoTipo);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/PlanAdquisicion/InfoNog/355244
        [HttpGet("{nog}")]
        [Authorize("Plan Adquisición - Visualizar")]
        public IActionResult InfoNog(int nog)
        {
            try
            {
                List<MvGcAdquisiciones> infoNogObj = PlanAdquisicionDAO.getInfoNog(nog);
                List<stnog> lstnog = new List<stnog>();
                if (infoNogObj != null && infoNogObj.Count > 0)
                {
                    stnog temp = new stnog();
                    foreach (MvGcAdquisiciones objetoNog in infoNogObj)
                    {
                        temp = new stnog();
                        temp.nog = objetoNog.nog ?? default(int);
                        temp.numeroContrato = objetoNog.numeroContrato;
                        temp.montoContrato = objetoNog.montoContrato ?? default(decimal);
                        temp.preparacionDocumentosReal = objetoNog.preparacionDocumentos != null ? objetoNog.preparacionDocumentos.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.lanzamientoEventoReal = objetoNog.lanzamientoEvento != null ? objetoNog.lanzamientoEvento.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.recepcionOfertasReal = objetoNog.recepcionOfertas != null ? objetoNog.recepcionOfertas.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.adjudicacionReal = objetoNog.adjudicacion != null ? objetoNog.adjudicacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.firmaContratoReal = objetoNog.adjudicacion != null ? objetoNog.firmaContrato.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        lstnog.Add(temp);
                    }
                }

                return Ok(new { success = lstnog.Count > 0 ? true : false, nogInfo = lstnog });
            }
            catch (Exception e)
            {
                CLogger.write("5", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PlanAdquisicion/CantidadHistoria
        [HttpPost]
        [Authorize("Plan Adquisición - Visualizar")]
        public IActionResult CantidadHistoria([FromBody]dynamic value)
        {
            try
            {
                int id = value.id != null ? (int)value.id : default(int);
                int objetoTipo = value.objetoTipo != null ? (int)value.objetoTipo : default(int);

                String resultado = PlanAdquisicionDAO.getVersiones(id, objetoTipo);
                return Ok(new { success = true, versiones = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("6", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PlanAdquisicion/Historia
        [HttpPost]
        [Authorize("Plan Adquisición - Visualizar")]
        public IActionResult Historia([FromBody]dynamic value)
        {
            try
            {
                int objetoId = value.objetoId != null ? (int)value.objetoId : default(int);
                int objetoTipo = value.objetoTipo != null ? (int)value.objetoTipo : default(int);
                int version = value.version != null ? (int)value.version : default(int);

                String resultado = PlanAdquisicionDAO.getHistoria(objetoId, objetoTipo, version);
                return Ok(new { success = true, historia = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("7", "PlanAdquisicionController.class", e);
                return BadRequest(500);
            }
        }
    }
}
