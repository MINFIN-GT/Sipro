using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;

namespace SPrestamo.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    public class PrestamoController : Controller
    {
        private class stprestamo
        {
            public int id;
            public String fechaCorte;
            public long codigoPresupuestario;
            public String numeroPrestamo;
            public String destino;
            public String sectorEconomico;
            public int unidadEjecutora;
            public String unidadEjecutoraNombre;
            public String fechaFirma;
            public int tipoAutorizacionId;
            public String tipoAutorizacionNombre;
            public String numeroAutorizacion;
            public String fechaAutorizacion;
            public int aniosPlazo;
            public int aniosGracia;
            public String fechaFinEjecucion;
            public int periodoEjecucion;
            public int tipoInteresId;
            public String tipoInteresNombre;
            public decimal porcentajeInteres;
            public decimal porcentajeComisionCompra;
            public int tipoMonedaId;
            public String tipoMonedaNombre;
            public decimal montoContratado;
            public decimal amortizado;
            public decimal porAmortizar;
            public decimal principalAnio;
            public decimal interesesAnio;
            public decimal comisionCompromisoAnio;
            public decimal otrosGastos;
            public decimal principalAcumulado;
            public decimal interesesAcumulados;
            public decimal comisionCompromisoAcumulado;
            public decimal otrosCargosAcumulados;
            public decimal presupuestoAsignadoFuncionamiento;
            public decimal presupuestoAsignadoInversion;
            public decimal presupuestoModificadoFun;
            public decimal presupuestoModificadoInv;
            public decimal presupuestoVigenteFun;
            public decimal presupuestoVigenteInv;
            public decimal presupuestoDevengadoFun;
            public decimal presupuestoDevengadoInv;
            public decimal presupuestoPagadoFun;
            public decimal presupuestoPagadoInv;
            public decimal saldoCuentas;
            public decimal desembolsoReal;
            public int ejecucionEstadoId;
            public String ejecucionEstadoNombre;
            public String proyectoPrograma;
            public String fechaDecreto;
            public String fechaSuscripcion;
            public String fechaElegibilidadUe;
            public String fechaCierreOrigianlUe;
            public String fechaCierreActualUe;
            public int mesesProrrogaUe;
            public int plazoEjecucionUe;
            public decimal montoAsignadoUe;
            public decimal desembolsoAFechaUe;
            public decimal montoPorDesembolsarUe;
            public String fechaVigencia;
            public decimal montoContratadoUsd;
            public decimal montoContratadoQtz;
            public decimal desembolsoAFechaUsd;
            public decimal montoPorDesembolsarUsd;
            public decimal montoAsignadoUeUsd;
            public decimal montoAsignadoUeQtz;
            public decimal desembolsoAFechaUeUsd;
            public decimal montoPorDesembolsarUeUsd;
            public String nombreEntidadEjecutora;
            public int cooperanteid;
            public String cooperantenombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public String objetivo;
            public String objetivoEspecifico;
            public decimal montoContratadoEntidadUsd;
            public decimal desembolsadoAFecha;
            public Double plazoEjecucionPEP;
            public int ejecucionFisicaRealPEP;
            public int porcentajeAvance;
        }

        private class sttiposprestamo
        {
            public int id;
            public String nombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
        }

        private class stunidadejecutora
        {
            public int id;
            public String nombre;
            public String entidad;
            public int entidadId;
            public int ejercicio;
            public Double prestamo;
            public Double donacion;
            public Double nacional;
            public int esCoordinador;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int techo;
            public int prestamoId;
            public int componenteId;
            public int componenteSigadeId;
        }

        private class stcomponentessigade
        {
            public int id;
            public String nombre;
            public String tipoMoneda;
            public decimal techo;
            public int orden;
            public String descripcion;
            public List<stunidadejecutora> unidadesEjecutoras;
            public long fechaActualizacion;
        }

        // GET api/Prestamos/Prestamos
        [HttpGet]
        public IActionResult Prestamos()
        {
            try
            {
                List<Prestamo> prestamos = PrestamoDAO.getPrestamos(User.Identity.Name);
                if (prestamos != null && prestamos.Count > 0)
                {
                    List<stprestamo> lstprestamo = new List<stprestamo>();

                    foreach (Prestamo prestamo in prestamos)
                    {
                        stprestamo temp = new stprestamo();
                        temp.id = prestamo.id;
                        temp.fechaCorte = prestamo.fechaCorte != null ? prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                        temp.numeroPrestamo = prestamo.numeroPrestamo;
                        temp.destino = prestamo.destino;
                        temp.sectorEconomico = prestamo.sectorEconomico;
                        temp.fechaFirma = prestamo.fechaFirma != null ? prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.tipoAutorizacionId = prestamo.autorizacionTipoid ?? default(int);

                        if (prestamo.autorizacionTipoid != null)
                        {
                            AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));
                            temp.tipoAutorizacionNombre = autorizacionTipo.nombre;
                        }

                        temp.numeroAutorizacion = prestamo.numeroAutorizacion;
                        temp.fechaAutorizacion = prestamo.fechaAutorizacion != null ? prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                        temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                        temp.fechaFinEjecucion = prestamo.fechaFinEjecucion != null ? prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.periodoEjecucion = prestamo.peridoEjecucion ?? default(int);
                        temp.tipoInteresId = prestamo.interesTipoid ?? default(int);

                        //temp.tipoInteresNombre = (prestamo.getInteresTipo() == null ? null : prestamo.getInteresTipo().getNombre());

                        temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                        temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                        prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);

                        temp.tipoMonedaId = prestamo.tipoMonedas.id;
                        temp.tipoMonedaNombre = prestamo.tipoMonedas.nombre;
                        temp.montoContratado = prestamo.montoContratado;
                        temp.amortizado = prestamo.amortizado ?? default(decimal);
                        temp.porAmortizar = prestamo.porAmortizar ?? default(decimal);
                        temp.principalAnio = prestamo.principalAnio ?? default(decimal);
                        temp.interesesAnio = prestamo.interesesAnio ?? default(decimal);
                        temp.comisionCompromisoAnio = prestamo.comisionCompromisoAnio ?? default(decimal);
                        temp.otrosGastos = prestamo.otrosGastos ?? default(decimal);
                        temp.principalAcumulado = prestamo.principalAcumulado ?? default(decimal);
                        temp.interesesAcumulados = prestamo.interesesAcumulados ?? default(decimal);
                        temp.comisionCompromisoAcumulado = prestamo.comisionCompromisoAcumulado ?? default(decimal);
                        temp.otrosCargosAcumulados = prestamo.otrosCargosAcumulados ?? default(decimal);
                        temp.presupuestoAsignadoFuncionamiento = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                        temp.presupuestoAsignadoInversion = prestamo.presupuestoAsignadoInv ?? default(decimal);
                        temp.presupuestoModificadoFun = prestamo.presupuestoModificadoFunc ?? default(decimal);
                        temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                        temp.presupuestoVigenteFun = prestamo.presupuestoVigenteFunc ?? default(decimal);
                        temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                        temp.presupuestoDevengadoFun = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                        temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                        temp.presupuestoPagadoFun = prestamo.presupuestoPagadoFunc ?? default(decimal);
                        temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                        temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal);
                        temp.desembolsoReal = prestamo.desembolsadoReal ?? default(decimal);

                        EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));

                        temp.ejecucionEstadoId = Convert.ToInt32(ejecucionEstado.id);
                        temp.ejecucionEstadoNombre = ejecucionEstado.nombre;
                        temp.proyectoPrograma = prestamo.proyectoPrograma;
                        temp.fechaDecreto = prestamo.fechaDecreto.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaSuscripcion = prestamo.fechaSuscripcion.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaElegibilidadUe = prestamo.fechaElegibilidadUe.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaCierreOrigianlUe = prestamo.fechaCierreOrigianlUe.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaCierreActualUe = prestamo.fechaCierreActualUe.ToString("dd/MM/yyyy H:mm:ss");
                        temp.mesesProrrogaUe = prestamo.mesesProrrogaUe;
                        temp.montoAsignadoUe = prestamo.montoAsignadoUe ?? default(decimal);
                        temp.desembolsoAFechaUe = prestamo.desembolsoAFechaUe ?? default(decimal);
                        temp.montoPorDesembolsarUe = prestamo.montoPorDesembolsarUe ?? default(decimal);
                        temp.fechaVigencia = prestamo.fechaVigencia.ToString("dd/MM/yyyy H:mm:ss");
                        temp.montoContratadoUsd = prestamo.montoContratadoUsd;
                        temp.montoContratadoQtz = prestamo.montoContratadoQtz;
                        temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUe ?? default(decimal);
                        temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUsd;
                        temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                        temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                        temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal);
                        temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                        prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));
                        temp.cooperanteid = prestamo.cooperantes.codigo;
                        temp.cooperantenombre = prestamo.cooperantes.siglas != null ? prestamo.cooperantes.siglas + " - " + prestamo.cooperantes.nombre : prestamo.cooperantes.nombre;

                        prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);

                        if (prestamo.unidadEjecutoras != null)
                        {
                            prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                            temp.unidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
                            temp.unidadEjecutoraNombre = prestamo.unidadEjecutoras.nombre;
                            temp.nombreEntidadEjecutora = prestamo.unidadEjecutoras.entidads.nombre;
                        }

                        temp.usuarioCreo = prestamo.usuarioCreo;
                        temp.usuarioActualizo = prestamo.usuarioActualizo;
                        temp.fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.objetivo = prestamo.objetivo;
                        temp.objetivoEspecifico = prestamo.objetivoEspecifico;
                        temp.porcentajeAvance = prestamo.porcentajeAvance;
                        lstprestamo.Add(temp);
                    }

                    return Ok(new { success = true, prestamos = lstprestamo });
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values/5
        [HttpPost]
        public IActionResult Prestamo([FromBody]dynamic value)
        {
            try
            {
                Prestamo prestamo = new Prestamo();
                prestamo.amortizado = (decimal)value.amortizado;
                prestamo.aniosGracia = (int)value.aniosGracia;
                prestamo.aniosPlazo = (int)value.aniosPlazo;
                prestamo.autorizacionTipoid = (int)value.autorizacionTipoid;
                prestamo.codigoPresupuestario = (long)value.codigoPresupuestario;
                prestamo.comisionCompromisoAcumulado = (decimal)value.comisionCompromisoAcumulado;
                prestamo.comisionCompromisoAnio = (decimal)value.comisionCompromisoAnio;
                prestamo.cooperantecodigo = (int)value.cooperantecodigo;
                prestamo.cooperanteejercicio = (int)value.ejercicio;
                prestamo.desembolsadoReal = (decimal)value.desembolsoReal;
                prestamo.desembolsoAFechaUe = (decimal)value.desembolsoAFechaUe;
                prestamo.desembolsoAFechaUeUsd = (decimal)value.desembolsoAFechaUeUsd;
                prestamo.desembolsoAFechaUsd = (decimal)value.desembolsoAFechaUsd;
                prestamo.destino = (string)value.destino;
                prestamo.ejecucionEstadoid = (int)value.ejecucionEstadoId;
                prestamo.ejercicio = (int)value.ejercicio;
                prestamo.entidad = (int)value.entidad;
                prestamo.estado = 1;
                prestamo.fechaAutorizacion = (DateTime)value.fechaAutorizacion;
                prestamo.fechaCierreActualUe = (DateTime)value.fechaCierreActual;
                prestamo.fechaCierreOrigianlUe = (DateTime)value.fechaCierreOriginal;
                prestamo.fechaCorte = (DateTime)value.fechaCorte;
                prestamo.fechaCreacion = DateTime.Now;
                prestamo.fechaDecreto = (DateTime)value.fechaDecreto;
                prestamo.fechaElegibilidadUe = (DateTime)value.fechaElegibilidad;
                prestamo.fechaFinEjecucion = (DateTime)value.fechaFinEjecucion;
                prestamo.fechaFirma = (DateTime)value.fechaFirma;
                prestamo.fechaSuscripcion = (DateTime)value.fechaSuscripcion;
                prestamo.fechaVigencia = (DateTime)value.fechaVigencia;
                prestamo.interesesAcumulados = (int)value.interesesAcumulados;
                prestamo.interesesAnio = (int)value.interesesAnio;
                prestamo.interesTipoid = (int)value.tipoInteresId;
                prestamo.mesesProrrogaUe = (int)value.mesesProrroga;
                prestamo.montoAsignadoUe = (decimal)value.montoAisignadoUe;
                prestamo.montoAsignadoUeQtz = (decimal)value.montoAsignadoUeQtz;
                prestamo.montoAsignadoUeUsd = (decimal)value.montoAsignadoUeUsd;
                prestamo.montoContratado = (decimal)value.montoContratado;
                prestamo.montoContratadoQtz = (decimal)value.montoContratadoQtz;
                prestamo.montoContratadoUsd = (decimal)value.montoContratadoUsd;
                prestamo.montoPorDesembolsarUe = (decimal)value.montoPorDesembolsarUe;
                prestamo.montoPorDesembolsarUeUsd = (decimal)value.montoPorDesembolsarUeUsd;
                prestamo.montoPorDesembolsarUsd = (decimal)value.montoPorDesembolsarUsd;
                prestamo.numeroAutorizacion = (string)value.numeroAutorizacion;
                prestamo.numeroPrestamo = (string)value.numeroPrestamo;
                prestamo.objetivo = (string)value.objetivo;
                prestamo.objetivoEspecifico = (string)value.objetivoEspecifico;
                prestamo.otrosCargosAcumulados = (decimal)value.otrosCargosAcumulados;
                prestamo.otrosGastos = (decimal)value.otrosGastos;
                prestamo.peridoEjecucion = (int)value.periodoEjecucion;
                prestamo.porAmortizar = (decimal)value.porAmortizar;
                prestamo.porcentajeAvance = (int)value.porcentajeAvance;
                prestamo.porcentajeComisionCompra = (decimal)value.porcentajeComisionCompra;
                prestamo.porcentajeInteres = (decimal)value.porcentajeInteres;
                prestamo.presupuestoAsignadoFunc = (decimal)value.presupuestoAsignadoFuncionamiento;
                prestamo.presupuestoAsignadoInv = (decimal)value.presupuestoAsignadoInversion;
                prestamo.presupuestoDevengadoFunc = (decimal)value.presupuestoDevengadoFunconamiento;
                prestamo.presupuestoDevengadoInv = (decimal)value.presupuestoDevengadoInversion;
                prestamo.presupuestoModificadoFunc = (decimal)value.presupuestoModificadoFuncionamiento;
                prestamo.presupuestoModificadoInv = (decimal)value.presupuestoModificadoInversion;
                prestamo.presupuestoPagadoFunc = (decimal)value.presupuestoPagadoFuncionamiento;
                prestamo.presupuestoPagadoInv = (decimal)value.presupuestoPagadoInversion;
                prestamo.presupuestoVigenteFunc = (decimal)value.presupuestoVigenteFuncionamiento;
                prestamo.presupuestoVigenteInv = (decimal)value.presupuestoVigenteInversion;
                prestamo.principalAcumulado = (decimal)value.principalAcumulado;
                prestamo.principalAnio = (decimal)value.principalAnio;
                prestamo.proyectoPrograma = (string)value.proyetoPrograma;
                prestamo.saldoCuentas = (decimal)value.saldoCuentas;
                prestamo.sectorEconomico = (string)value.sectorEconomico;
                prestamo.tipoMonedaid = (int)value.tipoMonedaId;
                prestamo.ueunidadEjecutora = (int)value.unidadEjecutora;
                prestamo.usuarioCreo = User.Identity.Name;

                bool guardado = PrestamoDAO.guardarPrestamo(prestamo);
                return Ok(new {
                    success = guardado,
                    id = prestamo.id,
                    usuarioCreo = prestamo.usuarioCreo,
                    fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                    usuarioActualizo = prestamo.usuarioActualizo,
                    fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                });
            }
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
