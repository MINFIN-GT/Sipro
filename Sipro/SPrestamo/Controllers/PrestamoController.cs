using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using SiproModelAnalyticCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

namespace SPrestamo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
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
            public int ueunidadEjecutora;
            public String unidadEjecutoraNombre;
            public String fechaFirma;
            public int autorizacionTipoid;
            public String tipoAutorizacionNombre;
            public String numeroAutorizacion;
            public String fechaAutorizacion;
            public int aniosPlazo;
            public int aniosGracia;
            public String fechaFinEjecucion;
            public int peridoEjecucion;
            public int interesTipoid;
            public String tipoInteresNombre;
            public decimal porcentajeInteres;
            public decimal porcentajeComisionCompra;
            public int tipoMonedaid;
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
            public decimal presupuestoAsignadoFunc;
            public decimal presupuestoAsignadoInv;
            public decimal presupuestoModificadoFunc;
            public decimal presupuestoModificadoInv;
            public decimal presupuestoVigenteFunc;
            public decimal presupuestoVigenteInv;
            public decimal presupuestoDevengadoFunc;
            public decimal presupuestoDevengadoInv;
            public decimal presupuestoPagadoFunc;
            public decimal presupuestoPagadoInv;
            public decimal saldoCuentas;
            public decimal desembolsadoReal;
            public int ejecucionEstadoid;
            public String ejecucionEstadoNombre;
            public String proyectoPrograma;
            public String fechaDecreto;
            public String fechaSuscripcion;
            public String fechaElegibilidadUe;
            public String fechaCierreOrigianlUe;
            public String fechaCierreActualUe;
            public decimal mesesProrrogaUe;
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
            public int cooperantecodigo;
            public int cooperanteejercicio;
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
            public int ejercicio;
            public int entidad;
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

        public class stunidadejecutora
        {
            public int id;
            public String nombre;
            public String entidad;
            public int entidadId;
            public int ejercicio;
            public decimal prestamo;
            public decimal donacion;
            public decimal nacional;
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
        [Authorize("Préstamos o Proyectos - Visualizar")]
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
                        temp.autorizacionTipoid = prestamo.autorizacionTipoid ?? default(int);

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
                        temp.peridoEjecucion = prestamo.peridoEjecucion ?? default(int);

                        InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));
                        if (interesTipo != null)
                        {
                            temp.interesTipoid = interesTipo.id;
                            temp.tipoInteresNombre = interesTipo.nombre;
                        }

                        temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                        temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                        prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);
                        if (prestamo.tipoMonedas != null)
                        {
                            temp.tipoMonedaid = prestamo.tipoMonedas.id;
                            temp.tipoMonedaNombre = prestamo.tipoMonedas.nombre;
                        }

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
                        temp.presupuestoAsignadoFunc = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                        temp.presupuestoAsignadoInv = prestamo.presupuestoAsignadoInv ?? default(decimal);
                        temp.presupuestoModificadoFunc = prestamo.presupuestoModificadoFunc ?? default(decimal);
                        temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                        temp.presupuestoVigenteFunc = prestamo.presupuestoVigenteFunc ?? default(decimal);
                        temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                        temp.presupuestoDevengadoFunc = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                        temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                        temp.presupuestoPagadoFunc = prestamo.presupuestoPagadoFunc ?? default(decimal);
                        temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                        temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal);
                        temp.desembolsadoReal = prestamo.desembolsadoReal ?? default(decimal);

                        EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));

                        temp.ejecucionEstadoid = Convert.ToInt32(ejecucionEstado.id);
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
                        temp.cooperantecodigo = prestamo.cooperantes.codigo;
                        temp.cooperantenombre = prestamo.cooperantes.siglas != null ? prestamo.cooperantes.siglas + " - " + prestamo.cooperantes.nombre : prestamo.cooperantes.nombre;
                        temp.cooperanteejercicio = prestamo.cooperantes.ejercicio;

                        prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);

                        if (prestamo.unidadEjecutoras != null)
                        {
                            prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                            temp.ueunidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
                            temp.unidadEjecutoraNombre = prestamo.unidadEjecutoras.nombre;
                            temp.nombreEntidadEjecutora = prestamo.unidadEjecutoras.entidads.nombre;
                        }

                        temp.ejercicio = prestamo.ejercicio;
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

        // POST api/Prestamos/Prestamo
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Prestamo([FromBody]dynamic value)
        {
            try
            {
                PrestamoValidator validator = new PrestamoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Prestamo prestamo = new Prestamo();
                    prestamo.amortizado = value.amortizado;
                    prestamo.aniosGracia = value.aniosGracia;
                    prestamo.aniosPlazo = value.aniosPlazo;
                    prestamo.autorizacionTipoid = value.autorizacionTipoid;
                    prestamo.codigoPresupuestario = value.codigoPresupuestario;
                    prestamo.comisionCompromisoAcumulado = value.comisionCompromisoAcumulado;
                    prestamo.comisionCompromisoAnio = value.comisionCompromisoAnio;
                    prestamo.cooperantecodigo = value.cooperantecodigo;
                    prestamo.cooperanteejercicio = value.ejercicio;
                    prestamo.desembolsadoReal = value.desembolsoReal;
                    prestamo.desembolsoAFechaUe = value.desembolsoAFechaUe;
                    prestamo.desembolsoAFechaUeUsd = value.desembolsoAFechaUeUsd;
                    prestamo.desembolsoAFechaUsd = value.desembolsoAFechaUsd;
                    prestamo.destino = value.destino;
                    prestamo.ejecucionEstadoid = value.ejecucionEstadoId;
                    prestamo.ejercicio = value.ejercicio;
                    prestamo.entidad = value.entidad;
                    prestamo.estado = 1;
                    prestamo.fechaCreacion = DateTime.Now;
                    prestamo.fechaAutorizacion = value.fechaAutorizacion;
                    prestamo.fechaCierreActualUe = value.fechaCierreActualUe;
                    prestamo.fechaCierreOrigianlUe = value.fechaCierreOrigianlUe;
                    prestamo.fechaCorte = value.fechaCorte;
                    prestamo.fechaCreacion = prestamo.fechaCreacion;
                    prestamo.fechaDecreto = value.fechaDecreto;
                    prestamo.fechaElegibilidadUe = value.fechaElegibilidadUe;
                    prestamo.fechaFinEjecucion = value.fechaFinEjecucion;
                    prestamo.fechaFirma = value.fechaFirma;
                    prestamo.fechaSuscripcion = value.fechaSuscripcion;
                    prestamo.fechaVigencia = value.fechaVigencia;
                    prestamo.interesesAcumulados = value.interesesAcumulados;
                    prestamo.interesesAnio = value.interesesAnio;
                    prestamo.interesTipoid = value.interesTipoid;
                    prestamo.mesesProrrogaUe = value.mesesProrrogaUe;
                    prestamo.montoAsignadoUe = value.montoAsignadoUe;
                    prestamo.montoAsignadoUeQtz = value.montoAsignadoUeQtz;
                    prestamo.montoAsignadoUeUsd = value.montoAsignadoUeUsd;
                    prestamo.montoContratado = value.montoContratado;
                    prestamo.montoContratadoQtz = value.montoContratadoQtz;
                    prestamo.montoContratadoUsd = value.montoContratadoUsd;
                    prestamo.montoPorDesembolsarUe = value.montoPorDesembolsarUe;
                    prestamo.montoPorDesembolsarUeUsd = value.montoPorDesembolsarUeUsd;
                    prestamo.montoPorDesembolsarUsd = value.montoPorDesembolsarUsd;
                    prestamo.numeroAutorizacion = value.numeroAutorizacion;
                    prestamo.numeroPrestamo = value.numeroPrestamo;
                    prestamo.objetivo = value.objetivo;
                    prestamo.objetivoEspecifico = value.objetivoEspecifico;
                    prestamo.otrosCargosAcumulados = value.otrosCargosAcumulados;
                    prestamo.otrosGastos = value.otrosGastos;
                    prestamo.peridoEjecucion = value.peridoEjecucion;
                    prestamo.porAmortizar = value.porAmortizar;
                    prestamo.porcentajeAvance = value.porcentajeAvance;
                    prestamo.porcentajeComisionCompra = value.porcentajeComisionCompra;
                    prestamo.porcentajeInteres = value.porcentajeInteres;
                    prestamo.presupuestoAsignadoFunc = value.presupuestoAsignadoFunc;
                    prestamo.presupuestoAsignadoInv = value.presupuestoAsignadoInv;
                    prestamo.presupuestoDevengadoFunc = value.presupuestoDevengadoFunc;
                    prestamo.presupuestoDevengadoInv = value.presupuestoDevengadoInv;
                    prestamo.presupuestoModificadoFunc = value.presupuestoModificadoFunc;
                    prestamo.presupuestoModificadoInv = value.presupuestoModificadoInv;
                    prestamo.presupuestoPagadoFunc = value.presupuestoPagadoFunc;
                    prestamo.presupuestoPagadoInv = value.presupuestoPagadoInv;
                    prestamo.presupuestoVigenteFunc = value.presupuestoVigenteFunc;
                    prestamo.presupuestoVigenteInv = value.presupuestoVigenteInv;
                    prestamo.principalAcumulado = value.principalAcumulado;
                    prestamo.principalAnio = value.principalAnio;
                    prestamo.proyectoPrograma = value.proyectoPrograma;
                    prestamo.saldoCuentas = value.saldoCuentas;
                    prestamo.sectorEconomico = value.sectorEconomico;
                    prestamo.tipoMonedaid = value.tipoMonedaid;
                    prestamo.ueunidadEjecutora = value.ueunidadEjecutora;
                    prestamo.usuarioCreo = User.Identity.Name;

                    bool result = PrestamoDAO.guardarPrestamo(prestamo);

                    if (result)
                    {
                        PrestamoTipoDAO.desasignarTiposAPrestamo(prestamo.id);
                        string idPrestamoTipos = (string)value.idPrestamoTipos;
                        if (idPrestamoTipos != null && idPrestamoTipos.Length > 0)
                        {
                            String[] prestamoTipos = idPrestamoTipos.Split(",");
                            List<int> tipos = new List<int>();
                            foreach (String tipo in prestamoTipos)
                            {
                                tipos.Add(Convert.ToInt32(tipo));
                            }
                            result = PrestamoTipoDAO.asignarTiposAPrestamo(tipos, prestamo, User.Identity.Name);
                        }
                    }

                    return Ok(new
                    {
                        success = result,
                        id = prestamo.id,
                        usuarioCreo = prestamo.usuarioCreo,
                        fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = prestamo.usuarioActualizo,
                        fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                {
                    return Ok(new { success = false, errores = results.Errors });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/Prestamos/Prestamo/5
        [HttpPut("{id}")]
        [Authorize("Préstamos o Proyectos - Editar")]
        public IActionResult Prestamo(int id, [FromBody]dynamic value)
        {
            try
            {
                PrestamoValidator validator = new PrestamoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Prestamo prestamo = PrestamoDAO.getPrestamoById(id);
                    prestamo.amortizado = value.amortizado;
                    prestamo.aniosGracia = value.aniosGracia;
                    prestamo.aniosPlazo = value.aniosPlazo;
                    prestamo.autorizacionTipoid = value.autorizacionTipoid;
                    prestamo.codigoPresupuestario = value.codigoPresupuestario;
                    prestamo.comisionCompromisoAcumulado = value.comisionCompromisoAcumulado;
                    prestamo.comisionCompromisoAnio = value.comisionCompromisoAnio;
                    prestamo.cooperantecodigo = value.cooperantecodigo;
                    prestamo.cooperanteejercicio = value.ejercicio;
                    prestamo.desembolsadoReal = value.desembolsoReal;
                    prestamo.desembolsoAFechaUe = value.desembolsoAFechaUe;
                    prestamo.desembolsoAFechaUeUsd = value.desembolsoAFechaUeUsd;
                    prestamo.desembolsoAFechaUsd = value.desembolsoAFechaUsd;
                    prestamo.destino = value.destino;
                    prestamo.ejecucionEstadoid = value.ejecucionEstadoId;
                    prestamo.ejercicio = value.ejercicio;
                    prestamo.entidad = value.entidad;
                    prestamo.estado = 1;
                    prestamo.fechaActualizacion = DateTime.Now;
                    prestamo.fechaAutorizacion = value.fechaAutorizacion;
                    prestamo.fechaCierreActualUe = value.fechaCierreActualUe;
                    prestamo.fechaCierreOrigianlUe = value.fechaCierreOrigianlUe;
                    prestamo.fechaCorte = value.fechaCorte;
                    prestamo.fechaCreacion = prestamo.fechaCreacion;
                    prestamo.fechaDecreto = value.fechaDecreto;
                    prestamo.fechaElegibilidadUe = value.fechaElegibilidadUe;
                    prestamo.fechaFinEjecucion = value.fechaFinEjecucion;
                    prestamo.fechaFirma = value.fechaFirma;
                    prestamo.fechaSuscripcion = value.fechaSuscripcion;
                    prestamo.fechaVigencia = value.fechaVigencia;
                    prestamo.interesesAcumulados = value.interesesAcumulados;
                    prestamo.interesesAnio = value.interesesAnio;
                    prestamo.interesTipoid = value.interesTipoid;
                    prestamo.mesesProrrogaUe = value.mesesProrrogaUe;
                    prestamo.montoAsignadoUe = value.montoAsignadoUe;
                    prestamo.montoAsignadoUeQtz = value.montoAsignadoUeQtz;
                    prestamo.montoAsignadoUeUsd = value.montoAsignadoUeUsd;
                    prestamo.montoContratado = value.montoContratado;
                    prestamo.montoContratadoQtz = value.montoContratadoQtz;
                    prestamo.montoContratadoUsd = value.montoContratadoUsd;
                    prestamo.montoPorDesembolsarUe = value.montoPorDesembolsarUe;
                    prestamo.montoPorDesembolsarUeUsd = value.montoPorDesembolsarUeUsd;
                    prestamo.montoPorDesembolsarUsd = value.montoPorDesembolsarUsd;
                    prestamo.numeroAutorizacion = value.numeroAutorizacion;
                    prestamo.numeroPrestamo = value.numeroPrestamo;
                    prestamo.objetivo = value.objetivo;
                    prestamo.objetivoEspecifico = value.objetivoEspecifico;
                    prestamo.otrosCargosAcumulados = value.otrosCargosAcumulados;
                    prestamo.otrosGastos = value.otrosGastos;
                    prestamo.peridoEjecucion = value.peridoEjecucion;
                    prestamo.porAmortizar = value.porAmortizar;
                    prestamo.porcentajeAvance = value.porcentajeAvance;
                    prestamo.porcentajeComisionCompra = value.porcentajeComisionCompra;
                    prestamo.porcentajeInteres = value.porcentajeInteres;
                    prestamo.presupuestoAsignadoFunc = value.presupuestoAsignadoFunc;
                    prestamo.presupuestoAsignadoInv = value.presupuestoAsignadoInv;
                    prestamo.presupuestoDevengadoFunc = value.presupuestoDevengadoFunc;
                    prestamo.presupuestoDevengadoInv = value.presupuestoDevengadoInv;
                    prestamo.presupuestoModificadoFunc = value.presupuestoModificadoFunc;
                    prestamo.presupuestoModificadoInv = value.presupuestoModificadoInv;
                    prestamo.presupuestoPagadoFunc = value.presupuestoPagadoFunc;
                    prestamo.presupuestoPagadoInv = value.presupuestoPagadoInv;
                    prestamo.presupuestoVigenteFunc = value.presupuestoVigenteFunc;
                    prestamo.presupuestoVigenteInv = value.presupuestoVigenteInv;
                    prestamo.principalAcumulado = value.principalAcumulado;
                    prestamo.principalAnio = value.principalAnio;
                    prestamo.proyectoPrograma = value.proyectoPrograma;
                    prestamo.saldoCuentas = value.saldoCuentas;
                    prestamo.sectorEconomico = value.sectorEconomico;
                    prestamo.tipoMonedaid = value.tipoMonedaid;
                    prestamo.ueunidadEjecutora = value.ueunidadEjecutora;
                    prestamo.usuarioActualizo = User.Identity.Name;

                    bool result = PrestamoDAO.guardarPrestamo(prestamo);

                    if (result)
                    {
                        PrestamoTipoDAO.desasignarTiposAPrestamo(prestamo.id);
                        string idPrestamoTipos = (string)value.idPrestamoTipos;
                        if (idPrestamoTipos != null && idPrestamoTipos.Length > 0)
                        {
                            String[] prestamoTipos = idPrestamoTipos.Split(",");
                            List<int> tipos = new List<int>();
                            foreach (String tipo in prestamoTipos)
                            {
                                tipos.Add(Convert.ToInt32(tipo));
                            }
                            result = PrestamoTipoDAO.asignarTiposAPrestamo(tipos, prestamo, User.Identity.Name);
                        }
                    }

                    return Ok(new
                    {
                        success = result,
                        id = prestamo.id,
                        usuarioCreo = prestamo.usuarioCreo,
                        fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = prestamo.usuarioActualizo,
                        fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });                
            }
            catch (Exception e)
            {
                CLogger.write("3", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/PrestamoPagina
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult PrestamosPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? value.pagina : default(int);
                int elementosPorPagina = value.elementosPorPagina != null ? value.elementosPorPagina : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);                
                string columna_ordenada = value.columna_ordenada != null ? value.columna_ordenada : default(string);
                string orden_direccion = value.orden_direccion != null ? value.orden_direccion : default(string);

                List <Prestamo> lstprestamos = PrestamoDAO.getPrestamosPagina(pagina, elementosPorPagina, filtro_busqueda, columna_ordenada, orden_direccion, User.Identity.Name);

                List<stprestamo> lstprestamo = new List<stprestamo>();
                stprestamo temp = null;
                foreach (Prestamo prestamo in lstprestamos)
                {
                    temp = new stprestamo();
                    temp.id = prestamo.id;
                    temp.fechaCorte = prestamo.fechaCorte != null ? prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss") : default(string);
                    temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                    temp.numeroPrestamo = prestamo.numeroPrestamo != null ? prestamo.numeroPrestamo : default(string);
                    temp.destino = prestamo.destino != null ? prestamo.destino : default(string);
                    temp.sectorEconomico = prestamo.sectorEconomico != null ? prestamo.sectorEconomico : default(string);
                    temp.fechaFirma = prestamo.fechaFirma != null ? prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss") : default(string);

                    AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));
                    if (autorizacionTipo != null)
                    {
                        temp.autorizacionTipoid = autorizacionTipo.id;
                        temp.tipoAutorizacionNombre = autorizacionTipo.nombre != null ? autorizacionTipo.nombre : default(string);
                    }

                    temp.numeroAutorizacion = prestamo.numeroAutorizacion != null ? prestamo.numeroAutorizacion : default(string);
                    temp.fechaAutorizacion = prestamo.fechaAutorizacion != null ? prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : default(string);
                    temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                    temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                    temp.fechaFinEjecucion = prestamo.fechaFinEjecucion != null ? prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss") : default(string);
                    temp.peridoEjecucion = prestamo.peridoEjecucion ?? default(int);

                    InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));
                    if (interesTipo != null)
                    {
                        temp.interesTipoid = interesTipo.id;
                        temp.tipoInteresNombre = interesTipo.nombre != null ? interesTipo.nombre : default(string);
                    }

                    temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                    temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                    prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);

                    if (prestamo.tipoMonedas != null)
                    {
                        temp.tipoMonedaid = prestamo.tipoMonedas.id;
                        temp.tipoMonedaNombre = prestamo.tipoMonedas.nombre != null ? prestamo.tipoMonedas.nombre : default(string);
                    }

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
                    temp.presupuestoAsignadoFunc = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                    temp.presupuestoAsignadoInv = prestamo.presupuestoAsignadoInv ?? default(decimal);
                    temp.presupuestoModificadoFunc = prestamo.presupuestoModificadoFunc ?? default(decimal);
                    temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                    temp.presupuestoVigenteFunc = prestamo.presupuestoVigenteFunc ?? default(decimal);
                    temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                    temp.presupuestoDevengadoFunc = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                    temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                    temp.presupuestoPagadoFunc = prestamo.presupuestoPagadoFunc ?? default(decimal);
                    temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                    temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal); ;
                    temp.desembolsadoReal = prestamo.desembolsadoReal ?? default(decimal);

                    EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));
                    if (ejecucionEstado != null)
                    {
                        temp.ejecucionEstadoid = Convert.ToInt32(ejecucionEstado.id);
                        temp.ejecucionEstadoNombre = ejecucionEstado.nombre != null ? ejecucionEstado.nombre : default(string);
                    }
                    temp.proyectoPrograma = prestamo.proyectoPrograma != null ? prestamo.proyectoPrograma : default(string);
                    temp.fechaDecreto = prestamo.fechaDecreto.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaSuscripcion = prestamo.fechaSuscripcion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaElegibilidadUe = prestamo.fechaElegibilidadUe.ToString("dd/MM/yyyy");
                    temp.fechaCierreOrigianlUe = prestamo.fechaCierreOrigianlUe.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaCierreActualUe = prestamo.fechaCierreActualUe.ToString("dd/MM/yyyy H:mm:ss");
                    temp.mesesProrrogaUe = prestamo.mesesProrrogaUe;
                    temp.montoAsignadoUe = prestamo.montoAsignadoUe ?? default(decimal);
                    temp.desembolsoAFechaUe = prestamo.desembolsoAFechaUe ?? default(decimal);
                    temp.montoPorDesembolsarUe = prestamo.montoPorDesembolsarUe ?? default(decimal);
                    temp.fechaVigencia = prestamo.fechaVigencia.ToString("dd/MM/yyyy H:mm:ss");
                    temp.montoContratadoUsd = prestamo.montoContratadoUsd;
                    temp.montoContratadoQtz = prestamo.montoContratadoQtz;
                    temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal); ;
                    temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);
                    temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                    temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                    temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal); ;
                    temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                    prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));
                    if (prestamo.cooperantes != null)
                    {
                        temp.cooperantecodigo = prestamo.cooperantes.codigo;
                        temp.cooperantenombre = (prestamo.cooperantes.siglas != null ?
                                prestamo.cooperantes.siglas + " - " : default(string)) + prestamo.cooperantes.nombre;
                        temp.cooperanteejercicio = prestamo.cooperantes.ejercicio;
                    }

                    prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);
                    if (prestamo.unidadEjecutoras != null)
                    {
                        prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                        temp.ueunidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
                        temp.unidadEjecutoraNombre = prestamo.unidadEjecutoras.nombre != null ? prestamo.unidadEjecutoras.nombre : default(string);
                        temp.nombreEntidadEjecutora = prestamo.unidadEjecutoras.entidads.nombre;                        
                    }

                    temp.entidad = prestamo.entidad;
                    temp.ejercicio = prestamo.ejercicio;
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
            catch (Exception e)
            {
                CLogger.write("4", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/numeroPrestamos
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult NumeroPrestamos([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);                

                long total = PrestamoDAO.getTotalPrestamos(filtro_busqueda, User.Identity.Name);

                return Ok(new { success = true, totalprestamos = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/numeroPrestamos
        [HttpDelete("{prestamoId}")]
        [Authorize("Préstamos o Proyectos - Eliminar")]
        public IActionResult BorrarPrestamo(int prestamoId)
        {
            try
            {
                Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamoId);
                prestamo.usuarioActualizo = User.Identity.Name;
                prestamo.fechaActualizacion = DateTime.Now;

                bool eliminado = PrestamoDAO.borrarPrestamo(prestamo);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("6", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/ComponentesSigade
        [HttpGet("{codigoPresupuestario}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ComponentesSigade(string codigoPresupuestario)
        {
            try
            {
                List<DtmAvanceFisfinanCmp> componentesSigade = DataSigadeDAO.getComponentes(codigoPresupuestario);
                List<stcomponentessigade> lstcomponentes = new List<stcomponentessigade>();
                stcomponentessigade temp = null;
                foreach (DtmAvanceFisfinanCmp objComponente in componentesSigade)
                {
                    temp = new stcomponentessigade();
                    temp.id = objComponente.numeroComponente ?? default(int);
                    temp.nombre = objComponente.nombreComponente;
                    temp.tipoMoneda = objComponente.monedaComponente;
                    temp.techo = objComponente.montoComponente ?? default(decimal);
                    lstcomponentes.Add(temp);
                }

                return Ok(new { success = true, componentes = lstcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("7", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/UnidadesEjecutoras
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult UnidadesEjecutoras([FromBody]dynamic value)
        {
            try
            {
                int ejercicio = 2017;//DateTime.Now.Year;
                int prestamoId = value.prestamoId != null ? (int)value.prestamoId : default(int);
                string codigoPresupuestario = value.codigoPresupuestario != null ? value.codigoPresupuestario : null;
                List <DtmAvanceFisfinanEnp> unidadesEjecutoras = DataSigadeDAO.getUnidadesEjecutoras((string)value.codigoPresupuestario, ejercicio);
                List<stunidadejecutora> lstunidadesejecutoras = new List<stunidadejecutora>();
                stunidadejecutora temp = null;
                foreach (DtmAvanceFisfinanEnp unidadEjecutora in unidadesEjecutoras)
                {
                    temp = new stunidadejecutora();
                    UnidadEjecutora EU = UnidadEjecutoraDAO.getUnidadEjecutora(unidadEjecutora.ejercicioFiscal ?? default(int), unidadEjecutora.entidadPresupuestaria ?? default(int), unidadEjecutora.unidadEjecutora ?? default(int));
                    if (EU != null)
                    {
                        EU.entidads = EntidadDAO.getEntidad(EU.entidadentidad, EU.ejercicio);
                        temp.id = EU.unidadEjecutora;
                        temp.ejercicio = EU.ejercicio;
                        temp.entidad = EU.entidads.nombre;
                        temp.entidadId = EU.entidads.entidad;
                        temp.nombre = EU.nombre;

                        if (prestamoId > 0)
                        {
                            Proyecto proyecto = ProyectoDAO.getProyectoPorUnidadEjecutora(temp.id, prestamoId, temp.entidadId);
                            if (proyecto != null)
                            {
                                temp.esCoordinador = proyecto.coordinador ?? default(int);
                                temp.fechaElegibilidad = proyecto.fechaElegibilidad != null ? proyecto.fechaElegibilidad.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                                temp.fechaCierre = proyecto.fechaCierre != null ? proyecto.fechaCierre.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                            }
                        }

                        lstunidadesejecutoras.Add(temp);
                    }
                }

                return Ok(new
                {
                    success = true,
                    unidadesEjecutoras = lstunidadesejecutoras
                });
            }
            catch (Exception e)
            {
                CLogger.write("8", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/UnidadesEjecutoras
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ObtenerMatriz([FromBody]dynamic value)
        {
            try
            {
                string codigoPresupuestario = value.codigoPresupuestario != null ? (string)value.codigoPresupuestario : null;
                int prestamoId = value.prestamoId != null ? (int)value.prestamoId : default(int);
                bool existenDatos = false;
                int anio_actual = 2017;//DateTime.Now.Year;
                List<DtmAvanceFisfinanEnp> unidadesEjecutorasSigade = DataSigadeDAO.getUnidadesEjecutoras(codigoPresupuestario, anio_actual);
                List<stunidadejecutora> unidadesEjecutroas = new List<stunidadejecutora>();


                List<DtmAvanceFisfinanCmp> componentesSigade = DataSigadeDAO.getComponentes(codigoPresupuestario);
                List<stcomponentessigade> stcomponentes = new List<stcomponentessigade>();
                if (componentesSigade != null && componentesSigade.Count > 0)
                {
                    for (int i = 0; i < componentesSigade.Count; i++)
                    {
                        DtmAvanceFisfinanCmp componenteSigade = componentesSigade[i];
                        stcomponentessigade temp = new stcomponentessigade();
                        temp.nombre = componenteSigade.nombreComponente;
                        temp.techo = componenteSigade.montoComponente ?? default(decimal);
                        temp.orden = componenteSigade.numeroComponente ?? default(int);

                        unidadesEjecutroas = new List<stunidadejecutora>();
                        if (unidadesEjecutorasSigade != null && unidadesEjecutorasSigade.Count > 0)
                        {
                            for (int j = 0; j < unidadesEjecutorasSigade.Count; j++)
                            {
                                DtmAvanceFisfinanEnp unidadEjecutora = unidadesEjecutorasSigade[j];
                                UnidadEjecutora unidade = UnidadEjecutoraDAO.getUnidadEjecutora(unidadEjecutora.ejercicioFiscal ?? default(int), unidadEjecutora.entidadPresupuestaria ?? default(int), unidadEjecutora.unidadEjecutora ?? default(int));
                                stunidadejecutora temp_ = new stunidadejecutora();
                                temp_.id = unidade.unidadEjecutora;
                                temp_.entidad = unidade.entidadentidad + "";
                                temp_.entidadId = unidade.entidadentidad;
                                temp_.ejercicio = unidade.ejercicio;
                                temp_.nombre = unidade.nombre;
                                temp_.techo = Convert.ToInt32(temp.techo);
                                temp_.prestamoId = prestamoId;

                                Componente compTemp = ComponenteDAO.obtenerComponentePorEntidad(codigoPresupuestario, temp_.ejercicio,
                                        Convert.ToInt32(temp_.entidad), temp_.id, temp.orden, prestamoId);
                                if (compTemp != null)
                                {
                                    temp_.componenteId = compTemp.id;
                                    temp_.componenteSigadeId = compTemp.componenteSigadeid;
                                    temp_.prestamo = compTemp.fuentePrestamo ?? default(decimal);
                                    temp_.donacion = compTemp.fuenteDonacion ?? default(decimal);
                                    temp_.nacional = compTemp.fuenteNacional ?? default(decimal);
                                    temp.descripcion = compTemp.descripcion;
                                    existenDatos = true;
                                }

                                unidadesEjecutroas.Add(temp_);
                            }
                        }

                        temp.unidadesEjecutoras = unidadesEjecutroas;
                        stcomponentes.Add(temp);
                    }
                }

                decimal diferencia = DataSigadeDAO.getDiferenciaMontos(codigoPresupuestario);

                return Ok(new { success = true, unidadesEjecutoras = unidadesEjecutroas, componentes = stcomponentes, diferencia = diferencia, existenDatos = existenDatos });
            }
            catch (Exception e)
            {
                CLogger.write("9", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/UnidadesEjecutoras
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult GuardarMatriz([FromBody]dynamic value)
        {
            try
            {
                int prestamoId = (int)value.prestamoId;
                Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamoId);
                String data = (string)value.estructura;
                String unidadesEjecutoras = (string)value.unidadesEjecutoras;
                int existenDatos = (bool)value.existenDatos == true ? 1 : 0;
                bool ret = false;

                PrestamoDAO.guardarComponentesSigade(prestamo.codigoPresupuestario + "", User.Identity.Name, existenDatos);

                dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(data);
                JArray est_unidadesEjecutoras_ = JArray.Parse(unidadesEjecutoras);

                List<Proyecto> proyectos = new List<Proyecto>();
                List<PrestamoDAO.stunidadejecutora> unidadesEjecutorasMatriz = new List<PrestamoDAO.stunidadejecutora>();
                foreach (JObject estructura in jsonData)
                {
                    JArray unidades = JArray.Parse(estructura["unidadesEjecutoras"].ToString());

                    for (int j = 0; j < unidades.Count; j++)
                    {
                        JObject unidad = (JObject)unidades[j];
                        int posicion = PrestamoDAO.existeUnidad(proyectos, unidad);

                        decimal fuentePrestamo = unidad["prestamo"] != null ? Convert.ToDecimal(unidad["prestamo"].ToString()) : default(decimal);
                        decimal fuenteDonacion = unidad["donacion"] != null ? Convert.ToDecimal(unidad["donacion"].ToString()) : default(decimal);
                        decimal fuenteNacional = unidad["nacional"] != null ? Convert.ToDecimal(unidad["nacional"].ToString()) : default(decimal);

                        if (posicion == -1)
                        {
                            proyectos.Add(PrestamoDAO.crearEditarProyecto(unidad, prestamo, User.Identity.Name, est_unidadesEjecutoras_, existenDatos));
                            ret = proyectos.Count > 0;
                            if (fuentePrestamo.CompareTo(decimal.Zero) > 0 ||
                                    fuenteDonacion.CompareTo(decimal.Zero) > 0 ||
                                    fuenteNacional.CompareTo(decimal.Zero) > 0)
                            {
                                ret = ret && PrestamoDAO.crearEditarComponente(proyectos[proyectos.Count - 1], (String)estructura["nombre"], (String)estructura["descripcion"],
                                        fuentePrestamo, fuenteDonacion, fuenteNacional, User.Identity.Name,
                                        prestamo.codigoPresupuestario, Convert.ToInt32((Double)estructura["orden"]));
                            }
                        }
                        else
                        {
                            if (fuentePrestamo.CompareTo(decimal.Zero) > 0 ||
                                    fuenteDonacion.CompareTo(decimal.Zero) > 0 ||
                                    fuenteNacional.CompareTo(decimal.Zero) > 0)
                            {
                                ret = ret && PrestamoDAO.crearEditarComponente(proyectos[posicion], (String)estructura["nombre"], (String)estructura["descripcion"],
                                        fuentePrestamo, fuenteDonacion, fuenteNacional, User.Identity.Name,
                                        prestamo.codigoPresupuestario, Convert.ToInt32((Double)estructura["orden"]));
                            }
                        }
                        ComponenteSigade componenteSigade = ComponenteSigadeDAO.getComponenteSigadePorCodigoNumero(prestamo.codigoPresupuestario + "", Convert.ToInt32((Double)estructura["orden"]));
                        PrestamoDAO.stunidadejecutora unidadMatriz = new PrestamoDAO.stunidadejecutora();
                        unidadMatriz.componenteSigadeId = componenteSigade.id;
                        unidadMatriz.id = Convert.ToInt32(unidad["id"].ToString());
                        unidadMatriz.prestamoId = prestamo.id;
                        unidadMatriz.entidadId = Convert.ToInt32(unidad["entidad"].ToString());
                        unidadMatriz.ejercicio = Convert.ToInt32(unidad["ejercicio"].ToString());
                        unidadMatriz.prestamo = fuentePrestamo;
                        unidadMatriz.donacion = fuenteDonacion;
                        unidadMatriz.nacional = fuenteNacional;
                        unidadMatriz.techo = Convert.ToInt32((decimal)estructura["techo"]);
                        unidadesEjecutorasMatriz.Add(unidadMatriz);
                    }                  
                }
                PrestamoDAO.actualizarMatriz(prestamo.id, unidadesEjecutorasMatriz);
                decimal diferencia = DataSigadeDAO.getDiferenciaMontos(prestamo.codigoPresupuestario + "");

                return Ok(new { success = ret ? true : false, diferencia = diferencia });
            }
            catch (Exception e)
            {
                CLogger.write("9", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/ComponentesSigade
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult ComponentesSigade([FromBody]dynamic value)
        {
            try
            {
                int prestamoId = value.prestamoId != null ? (int)value.prestamoId : default(int);
                Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamoId);

                bool ret = PrestamoDAO.guardarComponentes(prestamo.codigoPresupuestario + "", prestamoId, User.Identity.Name, prestamo.fechaSuscripcion);
                return Ok(new { success = ret });
            }
            catch (Exception e)
            {
                CLogger.write("10", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/ComponentesSigade/id
        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult PrestamoPorId(int id)
        {
            try
            {
                Prestamo prestamo = PrestamoDAO.getPrestamoById(id);
                return Ok(new
                {
                    success = prestamo != null ? true : false,
                    codigoPresupuestario = prestamo != null ? prestamo.codigoPresupuestario : default(long),
                    montoPorDesembolsar = prestamo != null ? prestamo.montoPorDesembolsarUsd : default(decimal),
                    desembolsoAFechaUsd = prestamo != null ? prestamo.desembolsoAFechaUsd : default(decimal),
                    fechaEligibilidadUe = prestamo != null ? prestamo.fechaElegibilidadUe.ToString("dd/MM/yyyy H:mm:ss") : null,
                    fechaCierreActualUe = prestamo != null ? prestamo.fechaCierreActualUe.ToString("dd/MM/yyyy H:mm:ss") : null,
                    nombre = prestamo != null ? prestamo.proyectoPrograma : null
                });
            }
            catch (Exception e)
            {
                CLogger.write("10", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/Tipos/prestamoId
        [HttpGet("{prestamoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Tipos(int prestamoId)
        {
            try
            {
                List<PrestamoTipoPrestamo> tipos = PrestamoTipoDAO.getPrestamoTiposPrestamo(prestamoId);
                List<sttiposprestamo> lsttipos = new List<sttiposprestamo>();
                if (tipos != null && tipos.Count > 0)
                {
                    foreach (PrestamoTipoPrestamo tipo in tipos)
                    {
                        sttiposprestamo temp = new sttiposprestamo();
                        tipo.prestamoTipos = PrestamoTipoDAO.getPrestamoTipoPorId(Convert.ToInt32(tipo.tipoprestamoid));
                        temp.id = Convert.ToInt32(tipo.prestamoTipos.id);
                        temp.nombre = tipo.prestamoTipos.nombre;
                        temp.usuarioCreo = tipo.usuarioCreo;
                        temp.usuarioActualizo = tipo.usuarioActualizo;
                        temp.fechaCreacion = tipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaActualizacion = tipo.fechaActualizacion != null ? tipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        lsttipos.Add(temp);
                    }
                }

                return Ok(new { success = true, prestamoTipos = lsttipos });
            }
            catch (Exception e)
            {
                CLogger.write("10", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/Prestamo/prestamoId
        [HttpGet("{prestamoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Prestamo(int prestamoId)
        {
            try
            {
                Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamoId);
                stprestamo temp = null;
                if (prestamo != null)
                {
                    temp = new stprestamo();
                    temp.id = prestamo.id;
                    temp.fechaCorte = prestamo.fechaCorte == null ? null : prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                    temp.numeroPrestamo = prestamo.numeroPrestamo;
                    temp.destino = prestamo.destino;
                    temp.sectorEconomico = prestamo.sectorEconomico;
                    temp.fechaFirma = prestamo.fechaFirma == null ? null : prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss");

                    AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));

                    temp.autorizacionTipoid = autorizacionTipo.id;
                    temp.tipoAutorizacionNombre = autorizacionTipo.nombre;
                    temp.numeroAutorizacion = prestamo.numeroAutorizacion;
                    temp.fechaAutorizacion = prestamo.fechaAutorizacion == null ? null : prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                    temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                    temp.fechaFinEjecucion = prestamo.fechaFinEjecucion == null ? null : prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.peridoEjecucion = prestamo.peridoEjecucion ?? default(int);

                    InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));

                    temp.interesTipoid = interesTipo.id;
                    temp.tipoInteresNombre = interesTipo.nombre;
                    temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                    temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                    prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);
                    temp.tipoMonedaid = prestamo.tipoMonedas.id;
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
                    temp.presupuestoAsignadoFunc = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                    temp.presupuestoAsignadoInv = prestamo.presupuestoAsignadoInv ?? default(decimal);
                    temp.presupuestoModificadoFunc = prestamo.presupuestoModificadoFunc ?? default(decimal);
                    temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                    temp.presupuestoVigenteFunc = prestamo.presupuestoVigenteFunc ?? default(decimal);
                    temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                    temp.presupuestoDevengadoFunc = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                    temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                    temp.presupuestoPagadoFunc = prestamo.presupuestoPagadoFunc ?? default(decimal);
                    temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                    temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal);
                    temp.desembolsadoReal = prestamo.desembolsadoReal ?? default(decimal);

                    EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));

                    temp.ejecucionEstadoid = Convert.ToInt32(ejecucionEstado.id);
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
                    temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUsd ?? default(decimal);
                    temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUsd;
                    temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                    temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                    temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal);
                    temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                    prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));

                    temp.cooperantecodigo = prestamo.cooperantes.codigo;
                    temp.cooperantenombre = (prestamo.cooperantes.siglas != null ? prestamo.cooperantes.siglas + " - " : default(string)) + prestamo.cooperantes.nombre;
                    temp.cooperanteejercicio = prestamo.cooperantes.ejercicio;

                    prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);

                    if (prestamo.unidadEjecutoras != null)
                    {
                        prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                        temp.ueunidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
                        temp.unidadEjecutoraNombre = prestamo.unidadEjecutoras.nombre != null ? prestamo.unidadEjecutoras.nombre : default(string);
                        temp.nombreEntidadEjecutora = prestamo.unidadEjecutoras.entidads.nombre != null ? prestamo.unidadEjecutoras.entidads.nombre : default(string);
                    }

                    temp.ejercicio = prestamo.ejercicio;
                    temp.usuarioCreo = prestamo.usuarioCreo;
                    temp.usuarioActualizo = prestamo.usuarioActualizo;
                    temp.fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : default(string);
                    temp.objetivo = prestamo.objetivo != null ? prestamo.objetivo : default(string);
                    temp.objetivoEspecifico = prestamo.objetivoEspecifico != null ? prestamo.objetivoEspecifico : default(string);
                    temp.porcentajeAvance = prestamo.porcentajeAvance;
                    temp.entidad = prestamo.entidad;
                }

                return Ok(new { success = true, prestamo = temp });
            }
            catch (Exception e)
            {
                CLogger.write("11", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/PrestamoPorPep
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult PrestamoPorPep([FromBody]dynamic value)
        {
            try
            {
                Proyecto proyecto = ProyectoDAO.getProyectoHistory((int)value.proyectoId, (string)value.lineaBase);
                if (proyecto != null)
                {
                    Prestamo prestamo = PrestamoDAO.getPrestamoById(proyecto.prestamoid ?? default(int));
                    if (prestamo != null)
                    {
                        stprestamo temp = new stprestamo();
                        temp = new stprestamo();
                        temp.id = prestamo.id;
                        temp.fechaCorte = prestamo.fechaCorte == null ? null : prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss");
                        temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                        temp.numeroPrestamo = prestamo.numeroPrestamo;
                        temp.destino = prestamo.destino;
                        temp.sectorEconomico = prestamo.sectorEconomico;
                        temp.fechaFirma = prestamo.fechaFirma == null ? null : prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss");

                        AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));

                        temp.autorizacionTipoid = autorizacionTipo.id;
                        temp.tipoAutorizacionNombre = autorizacionTipo.nombre;
                        temp.numeroAutorizacion = prestamo.numeroAutorizacion;
                        temp.fechaAutorizacion = prestamo.fechaAutorizacion == null ? null : prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss");
                        temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                        temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                        temp.fechaFinEjecucion = prestamo.fechaFinEjecucion == null ? null : prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss");
                        temp.peridoEjecucion = prestamo.peridoEjecucion ?? default(int);

                        InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));

                        temp.interesTipoid = interesTipo.id;
                        temp.tipoInteresNombre = interesTipo.nombre;
                        temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                        temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                        prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);
                        temp.tipoMonedaid = prestamo.tipoMonedas.id;
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
                        temp.presupuestoAsignadoFunc = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                        temp.presupuestoAsignadoInv = prestamo.presupuestoAsignadoInv ?? default(decimal);
                        temp.presupuestoModificadoFunc = prestamo.presupuestoModificadoFunc ?? default(decimal);
                        temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                        temp.presupuestoVigenteFunc = prestamo.presupuestoVigenteFunc ?? default(decimal);
                        temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                        temp.presupuestoDevengadoFunc = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                        temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                        temp.presupuestoPagadoFunc = prestamo.presupuestoPagadoFunc ?? default(decimal);
                        temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                        temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal);
                        temp.desembolsadoReal = prestamo.desembolsadoReal ?? default(decimal);

                        EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));

                        temp.ejecucionEstadoid = Convert.ToInt32(ejecucionEstado.id);
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
                        temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUsd ?? default(decimal);
                        temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUsd;
                        temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                        temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                        temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal);
                        temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                        prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));

                        temp.cooperantecodigo = prestamo.cooperantes.codigo;
                        temp.cooperantenombre = (prestamo.cooperantes.siglas != null ? prestamo.cooperantes.siglas + " - " : default(string)) + prestamo.cooperantes.nombre;
                        temp.cooperanteejercicio = prestamo.cooperantes.ejercicio;

                        prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);

                        if (prestamo.unidadEjecutoras != null)
                        {
                            prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                            temp.ueunidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
                            temp.unidadEjecutoraNombre = prestamo.unidadEjecutoras.nombre;
                            temp.nombreEntidadEjecutora = prestamo.unidadEjecutoras.entidads.nombre;
                        }

                        temp.montoContratadoEntidadUsd = decimal.Zero;
                        List<Componente> componentes = (List<Componente>)ComponenteDAO.getComponentesPorProyectoHistory(proyecto.id, null);
                        if (componentes != null)
                        {
                            foreach (Componente c in componentes)
                                temp.montoContratadoEntidadUsd += c.fuentePrestamo ?? default(decimal);
                        }

                        int anio = DateTime.Now.Year;
                        int mes = DateTime.Now.Month;

                        temp.desembolsadoAFecha = DataSigadeDAO.totalDesembolsadoAFechaRealDolaresPorEntidad(prestamo.codigoPresupuestario + "",
                                anio, mes, proyecto.unidadEjecutoras.entidadentidad);

                        temp.ejecucionFisicaRealPEP = proyecto.ejecucionFisicaReal ?? default(int);
                        temp.usuarioCreo = prestamo.usuarioCreo;
                        temp.usuarioActualizo = prestamo.usuarioActualizo;
                        temp.fechaCreacion = prestamo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                        temp.fechaActualizacion = prestamo.fechaActualizacion != null ? prestamo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        temp.objetivo = prestamo.objetivo;
                        temp.objetivoEspecifico = prestamo.objetivoEspecifico;
                        temp.ejercicio = prestamo.ejercicio;
                        temp.entidad = prestamo.entidad;

                        long tiempo1 = DateTime.Now.Ticks;
                        Double f1 = (((DateTime.Now.Ticks * 1.0) - tiempo1) - proyecto.fechaInicio.Value.Ticks) / 86400000;
                        Double f2 = (proyecto.fechaFin.Value.Ticks * 1.0 - proyecto.fechaInicio.Value.Ticks) / 86400000;
                        temp.plazoEjecucionPEP = (f1 * 1.0 / f2 * 100);

                        return Ok(new { success = true, prestamo = temp });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("12", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/PrestamoPorPep/prestamoId
        [HttpGet("{prestamoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult PrestamoHistory(int prestamoId)
        {
            try
            {
                Prestamo prestamo = PrestamoDAO.getPrestamoByIdHistory(prestamoId, null);
                stprestamo temp = null;

                if (prestamo != null)
                {
                    temp = new stprestamo();
                    temp.id = prestamo.id;
                    temp.fechaCorte = prestamo.fechaCorte == null ? null : prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                    temp.numeroPrestamo = prestamo.numeroPrestamo;
                    temp.destino = prestamo.destino;
                    temp.sectorEconomico = prestamo.sectorEconomico;
                    temp.fechaFirma = prestamo.fechaFirma == null ? null : prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss");

                    AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));

                    temp.autorizacionTipoid = autorizacionTipo.id;
                    temp.tipoAutorizacionNombre = autorizacionTipo.nombre;
                    temp.numeroAutorizacion = prestamo.numeroAutorizacion;
                    temp.fechaAutorizacion = prestamo.fechaAutorizacion == null ? null : prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                    temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                    temp.fechaFinEjecucion = prestamo.fechaFinEjecucion == null ? null : prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss");
                    temp.peridoEjecucion = prestamo.peridoEjecucion ?? default(int);

                    InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));

                    temp.interesTipoid = interesTipo.id;
                    temp.tipoInteresNombre = interesTipo.nombre;
                    temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                    temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                    prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);
                    temp.tipoMonedaid = prestamo.tipoMonedas.id;
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
                    temp.presupuestoAsignadoFunc = prestamo.presupuestoAsignadoFunc ?? default(decimal);
                    temp.presupuestoAsignadoInv = prestamo.presupuestoAsignadoInv ?? default(decimal);
                    temp.presupuestoModificadoFunc = prestamo.presupuestoModificadoFunc ?? default(decimal);
                    temp.presupuestoModificadoInv = prestamo.presupuestoModificadoInv ?? default(decimal);
                    temp.presupuestoVigenteFunc = prestamo.presupuestoVigenteFunc ?? default(decimal);
                    temp.presupuestoVigenteInv = prestamo.presupuestoVigenteInv ?? default(decimal);
                    temp.presupuestoDevengadoFunc = prestamo.presupuestoDevengadoFunc ?? default(decimal);
                    temp.presupuestoDevengadoInv = prestamo.presupuestoDevengadoInv ?? default(decimal);
                    temp.presupuestoPagadoFunc = prestamo.presupuestoPagadoFunc ?? default(decimal);
                    temp.presupuestoPagadoInv = prestamo.presupuestoPagadoInv ?? default(decimal);
                    temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal);
                    temp.desembolsadoReal = prestamo.desembolsadoReal ?? default(decimal);

                    EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));

                    temp.ejecucionEstadoid = Convert.ToInt32(ejecucionEstado.id);
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
                    temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUsd ?? default(decimal);
                    temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUsd;
                    temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                    temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                    temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal);
                    temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                    prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));

                    temp.cooperantecodigo = prestamo.cooperantes.codigo;
                    temp.cooperantenombre = (prestamo.cooperantes.siglas != null ? prestamo.cooperantes.siglas + " - " : default(string)) + prestamo.cooperantes.nombre;
                    temp.cooperanteejercicio = prestamo.cooperantes.ejercicio;

                    prestamo.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(prestamo.ejercicio, prestamo.entidad, prestamo.ueunidadEjecutora);

                    if (prestamo.unidadEjecutoras != null)
                    {
                        prestamo.unidadEjecutoras.entidads = EntidadDAO.getEntidad(prestamo.entidad, prestamo.ejercicio);
                        temp.ueunidadEjecutora = prestamo.unidadEjecutoras.unidadEjecutora;
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
                    temp.ejercicio = prestamo.ejercicio;
                    temp.entidad = prestamo.entidad;
                }

                return Ok(new { success = true, prestamo = temp });
            }
            catch (Exception e)
            {
                CLogger.write("13", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/congelarDescongelar
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Editar")]
        public IActionResult congelarDescongelar([FromBody]dynamic value)
        {
            try
            {
                bool ret = true;
                String peps = (string)value.peps;
                String nombre = (string)value.nombre;
                int generarLineasBases = (bool)value.lineaBase == true ? 1 : 0;
                String lineaBaseId = (string)value.lineaBaseId;

                JArray pepsArreglo = JArray.Parse(peps);
                foreach (JObject objeto in pepsArreglo.Children<JObject>())
                {
                    foreach(JProperty propiedad in objeto.Properties())
                    {
                        int id = 0;
                        int congelado = 0;
                        int sobreescribir = 0;

                        switch (propiedad.Name)
                        {
                            case "id":
                                id = (int)propiedad.Value;
                                break;
                            case "congelado":
                                congelado = (int)propiedad.Value;
                                break;
                            case "permiso":
                                sobreescribir = (int)propiedad.Value;
                                break;
                        }
                        int tipoLinea = (int)value.tipoLineaBase;
                        Proyecto proyecto = ProyectoDAO.getProyecto(id);

                        LineaBase ultimaLineaBase = LineaBaseDAO.getUltimaLinaBasePorProyecto(proyecto.id, 2);
                        if (ultimaLineaBase != null)
                        {
                            ultimaLineaBase.sobreescribir = sobreescribir;
                            ret = ret && LineaBaseDAO.guardarLineaBase(ultimaLineaBase);
                        }

                        if (!congelado.Equals(proyecto.congelado))
                        {
                            proyecto.congelado = congelado;
                            ret = ret && ProyectoDAO.guardarProyecto(proyecto, false);
                            
                            if (ret && proyecto.congelado.Equals(1) && generarLineasBases.Equals(1))
                            {
                                LineaBase lineaBase = new LineaBase();
                                lineaBase.proyectos = proyecto;
                                lineaBase.proyectoid = proyecto.id;
                                lineaBase.nombre = nombre;
                                lineaBase.usuarioCreo = User.Identity.Name;
                                lineaBase.fechaCreacion = DateTime.Now;
                                lineaBase.tipoLineaBase = tipoLinea;
                                ret = LineaBaseDAO.guardarLineaBase(lineaBase, lineaBaseId);
                            }
                        }
                    }
                }
                return Ok(new { success = ret });
            }
            catch (Exception e)
            {
                CLogger.write("14", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Prestamo/VersionesMatriz/prestamoId
        [HttpGet("{prestamoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult VersionesMatriz(int prestamoId)
        {
            try
            {
                String resultado = PrestamoDAO.getVersionesMatriz(prestamoId);
                return Ok(new { success = true, versiones = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("15", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/HistoriaMatriz
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult HistoriaMatriz([FromBody]dynamic value)
        {
            try
            {
                int prestamoId = (int)value.prestamoId;
                int version = (int)value.prestamoId;
                String codigoPresupuestario = (string)value.codigoPresupuestario;

                int anio_actual = DateTime.Now.Year;
                List <DtmAvanceFisfinanEnp> unidadesEjecutorasSigade = DataSigadeDAO.getUnidadesEjecutoras(codigoPresupuestario, anio_actual);
                List<stunidadejecutora> unidadesEjecutoras = new List<stunidadejecutora>();


                List <DtmAvanceFisfinanCmp> componentesSigade = DataSigadeDAO.getComponentes(codigoPresupuestario);
                List<stcomponentessigade> stcomponentes = new List<stcomponentessigade>();
                if (componentesSigade != null && componentesSigade.Count > 0)
                {
                    for (int i = 0; i < componentesSigade.Count; i++)
                    {
                        stcomponentessigade temp = new stcomponentessigade();
                        temp.nombre = componentesSigade[i].nombreComponente;
                        temp.orden = (int)componentesSigade[i].numeroComponente;
                        unidadesEjecutoras = new List<stunidadejecutora>();
                        if (unidadesEjecutorasSigade != null && unidadesEjecutorasSigade.Count > 0)
                        {
                            for (int j = 0; j < unidadesEjecutorasSigade.Count; j++)
                            {
                                UnidadEjecutora unidad = UnidadEjecutoraDAO.getUnidadEjecutora((int)unidadesEjecutorasSigade[j].ejercicioFiscal, (int)unidadesEjecutorasSigade[j].entidadPresupuestaria,
                                        (int)unidadesEjecutorasSigade[j].unidadEjecutora);
                                stunidadejecutora temp_ = new stunidadejecutora();
                                temp_.id = unidad.unidadEjecutora;
                                temp_.entidad = unidad.entidads.nombre + "";
                                temp_.entidadId = unidad.entidads.entidad;
                                temp_.ejercicio = unidad.ejercicio;
                                temp_.nombre = unidad.nombre;
                                temp_.techo = Convert.ToInt32(temp.techo);
                                temp_.prestamoId = prestamoId;

                                decimal[] techos = PrestamoDAO.getComponenteMatrizHistoria(prestamoId, temp.orden,
                                        temp_.entidadId, temp_.ejercicio, temp_.id, version);

                                if (techos != null)
                                {
                                    temp.techo = techos[0];
                                    temp_.prestamo = techos[1];
                                    temp_.donacion = techos[2];
                                    temp_.nacional = techos[3];
                                    temp.fechaActualizacion = Convert.ToInt64(techos[4]);
                                }
                                unidadesEjecutoras.Add(temp_);
                            }
                        }

                        temp.unidadesEjecutoras = unidadesEjecutoras;
                        stcomponentes.Add(temp);
                    }
                }

                return Ok(new { success = true, unidadesEjecutoras = unidadesEjecutoras, componentes= stcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("16", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }
    }
}