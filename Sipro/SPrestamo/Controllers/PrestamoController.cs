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

                        InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));
                        if (interesTipo != null)
                        {
                            temp.tipoInteresId = interesTipo.id;
                            temp.tipoInteresNombre = interesTipo.nombre;
                        }

                        temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                        temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                        prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);
                        if (prestamo.tipoMonedas != null)
                        {
                            temp.tipoMonedaId = prestamo.tipoMonedas.id;
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

        // POST api/Prestamos/Prestamo
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
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/Prestamos/Prestamo/5
        [HttpPut("{id}")]
        public IActionResult Prestamo(int id, [FromBody]dynamic value)
        {
            try
            {
                Prestamo prestamo = PrestamoDAO.getPrestamoById(id);
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
                prestamo.fechaActualizacion = DateTime.Now;
                prestamo.fechaAutorizacion = (DateTime)value.fechaAutorizacion;
                prestamo.fechaCierreActualUe = (DateTime)value.fechaCierreActual;
                prestamo.fechaCierreOrigianlUe = (DateTime)value.fechaCierreOriginal;
                prestamo.fechaCorte = (DateTime)value.fechaCorte;
                prestamo.fechaCreacion = prestamo.fechaCreacion;
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
                prestamo.usuarioCreo = prestamo.usuarioCreo;
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
            catch (Exception e)
            {
                CLogger.write("3", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/PrestamoPagina
        [HttpPost]
        public IActionResult PrestamosPagina([FromBody]dynamic value)
        {
            try
            {
                List<Prestamo> lstprestamos = PrestamoDAO.getPrestamosPagina((int)value.pagina, (int)value.elementosPorPagina, (string)value.filtro_nombre, (long)value.filtro_codigo_presupuestario, (string)value.filtro_numero_prestamo,
                        (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion, (string)value.columna_ordenada, (string)value.orden_direccion, User.Identity.Name);

                List<stprestamo> lstprestamo = new List<stprestamo>();
                stprestamo temp = null;
                foreach (Prestamo prestamo in lstprestamos)
                {
                    temp = new stprestamo();
                    temp.id = prestamo.id;
                    temp.fechaCorte = prestamo.fechaCorte != null ? prestamo.fechaCorte.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.codigoPresupuestario = prestamo.codigoPresupuestario;
                    temp.numeroPrestamo = prestamo.numeroPrestamo;
                    temp.destino = prestamo.destino;
                    temp.sectorEconomico = prestamo.sectorEconomico;
                    temp.fechaFirma = prestamo.fechaFirma != null ? prestamo.fechaFirma.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    AutorizacionTipo autorizacionTipo = AutorizacionTipoDAO.getAutorizacionTipoById(prestamo.autorizacionTipoid ?? default(int));
                    if (autorizacionTipo != null)
                    {
                        temp.tipoAutorizacionId = autorizacionTipo.id;
                        temp.tipoAutorizacionNombre = autorizacionTipo.nombre;
                    }

                    temp.numeroAutorizacion = prestamo.numeroAutorizacion;
                    temp.fechaAutorizacion = prestamo.fechaAutorizacion != null ? prestamo.fechaAutorizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.aniosPlazo = prestamo.aniosPlazo ?? default(int);
                    temp.aniosGracia = prestamo.aniosGracia ?? default(int);
                    temp.fechaFinEjecucion = prestamo.fechaFinEjecucion != null ? prestamo.fechaFinEjecucion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.periodoEjecucion = prestamo.peridoEjecucion ?? default(int);

                    InteresTipo interesTipo = InteresTipoDAO.getInteresTipoById(prestamo.interesTipoid ?? default(int));
                    if (interesTipo != null)
                    {
                        temp.tipoInteresId = interesTipo.id;
                        temp.tipoInteresNombre = interesTipo.nombre;
                    }

                    temp.porcentajeInteres = prestamo.porcentajeInteres ?? default(decimal);
                    temp.porcentajeComisionCompra = prestamo.porcentajeComisionCompra ?? default(decimal);

                    prestamo.tipoMonedas = TipoMonedaDAO.getTipoMonedaPorId(prestamo.tipoMonedaid);

                    if (prestamo.tipoMonedas != null)
                    {
                        temp.tipoMonedaId = prestamo.tipoMonedas.id;
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
                    temp.saldoCuentas = prestamo.saldoCuentas ?? default(decimal); ;
                    temp.desembolsoReal = prestamo.desembolsadoReal ?? default(decimal);

                    EjecucionEstado ejecucionEstado = EjecucionEstadoDAO.getEjecucionEstadoById(prestamo.ejecucionEstadoid ?? default(int));
                    if (ejecucionEstado != null)
                    {
                        temp.ejecucionEstadoId = Convert.ToInt32(ejecucionEstado.id);
                        temp.ejecucionEstadoNombre = ejecucionEstado.nombre;
                    }
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
                    temp.desembolsoAFechaUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal); ;
                    temp.montoPorDesembolsarUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);
                    temp.montoAsignadoUeUsd = prestamo.montoAsignadoUeUsd ?? default(decimal);
                    temp.montoAsignadoUeQtz = prestamo.montoAsignadoUeQtz ?? default(decimal);
                    temp.desembolsoAFechaUeUsd = prestamo.desembolsoAFechaUeUsd ?? default(decimal); ;
                    temp.montoPorDesembolsarUeUsd = prestamo.montoPorDesembolsarUeUsd ?? default(decimal);

                    prestamo.cooperantes = CooperanteDAO.getCooperantePorCodigo(prestamo.cooperantecodigo ?? default(int));
                    if (prestamo.cooperantes != null)
                    {
                        temp.cooperanteid = prestamo.cooperantes.codigo;
                        temp.cooperantenombre = (prestamo.cooperantes.siglas != null ?
                                prestamo.cooperantes.siglas + " - " : "") + prestamo.cooperantes.nombre;
                    }

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
            catch (Exception)
            {
                CLogger.write("4", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Prestamo/numeroPrestamos
        [HttpPost]
        public IActionResult NumeroPrestamos([FromBody]dynamic value)
        {
            try
            {
                long total = PrestamoDAO.getTotalPrestamos((string)value.filtro_nombre, (long)value.filtro_codigo_presupuestario, (string)value.filtro_numero_prestamo, (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion, User.Identity.Name);

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
        [HttpGet("{codigo_presupuestario}")]
        public IActionResult ComponentesSigade(string codigoPresupuestario)
        {
            try
            {
                List<DtmAvanceFisfinanCmp> componentesSigade = DataSigadeDAO.getComponentes(codigoPresupuestario);
                List<stcomponentessigade> lstcomponentes = new List<stcomponentessigade>();
                stcomponentessigade temp = null;
                foreach (Object objComponente in componentesSigade)
                {
                    Object[] componente = (Object[])objComponente;
                    temp = new stcomponentessigade();
                    temp.id = (int)componente[1];
                    temp.nombre = (String)componente[2];
                    temp.tipoMoneda = (String)componente[3];
                    temp.techo = (decimal)componente[4];
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
        public IActionResult UnidadesEjecutoras([FromBody]dynamic value)
        {
            try
            {
                int ejercicio = DateTime.Now.Year;
                int prestamoId = (int)value.proyectoId;

                List<DtmAvanceFisfinanEnp> unidadesEjecutoras = DataSigadeDAO.getUnidadesEjecutoras((string)value.codigoPresupuestario, (int)value.ejercicio);
                List<stunidadejecutora> lstunidadesejecutoras = new List<stunidadejecutora>();
                stunidadejecutora temp = null;
                foreach (Object unidadEjecutora in unidadesEjecutoras)
                {
                    temp = new stunidadejecutora();
                    Object[] objEU = (Object[])unidadEjecutora;

                    UnidadEjecutora EU = UnidadEjecutoraDAO.getUnidadEjecutora((int)objEU[1], (int)objEU[2], (int)objEU[3]);
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
        public IActionResult ObtenerMatriz([FromBody]dynamic value)
        {
            try
            {
                string codigoPresupuestario = (string)value.codigoPresupuestario;
                int prestamoId = (int)value.prestamoId;
                bool existenDatos = false;
                int anio_actual = DateTime.Now.Year;
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

                int diferencia = DataSigadeDAO.getDiferenciaMontos(codigoPresupuestario);

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

                JObject parser = new JObject();
                JArray estructuras_ = JArray.Parse(data);
                JArray est_unidadesEjecutoras_ = JArray.Parse(unidadesEjecutoras);

                List<Proyecto> proyectos = new List<Proyecto>();
                List<stunidadejecutora> unidadesEjecutorasMatriz = new List<stunidadejecutora>();
                int k = 0;
                foreach (JObject estructura in jsonData)
                {
                    JObject estructura_ = (JObject)estructuras_[k];
                    k++;
                    parser = new JObject();
                    JObject unidades = (JObject)estructura_["unidadesEjecutoras"];

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
                            if (fuentePrestamo.CompareTo(default(decimal)) > 0 ||
                                    fuenteDonacion.CompareTo(default(decimal)) > 0 ||
                                    fuenteNacional.CompareTo(default(decimal)) > 0)
                            {
                                ret = ret && crearEditarComponente(proyectos.get(proyectos.size() - 1), (String)estructura.get("nombre"), (String)estructura.get("descripcion"),
                                        fuentePrestamo, fuenteDonacion, fuenteNacional, usuario,
                                        prestamo.getCodigoPresupuestario(), ((Double)estructura.get("orden")).intValue());
                            }
                        }
                        else
                        {
                            if (fuentePrestamo.compareTo(BigDecimal.ZERO) > 0 ||
                                    fuenteDonacion.compareTo(BigDecimal.ZERO) > 0 ||
                                    fuenteNacional.compareTo(BigDecimal.ZERO) > 0)
                            {
                                ret = ret && crearEditarComponente(proyectos[posicion], (String)estructura.get("nombre"), (String)estructura.get("descripcion"),
                                        fuentePrestamo, fuenteDonacion, fuenteNacional, User.Identity.Name,
                                        prestamo.codigoPresupuestario, ((Double)estructura.get("orden")).intValue());
                            }
                        }
                        ComponenteSigade componenteSigade = ComponenteSigadeDAO.getComponenteSigadePorCodigoNumero(prestamo.codigoPresupuestario + "", ((Double)estructura.get("orden")).intValue());
                        stunidadejecutora unidadMatriz = new stunidadejecutora();
                        unidadMatriz.componenteSigadeId = componenteSigade.id;
                        unidadMatriz.id = Convert.ToInt32(unidad["id"].getAsString());
                        unidadMatriz.prestamoId = prestamo.id;
                        unidadMatriz.entidadId = Convert.ToInt32(unidad.get("entidad").getAsString());
                        unidadMatriz.ejercicio = Convert.ToInt32(unidad.get("ejercicio").getAsString());
                        unidadMatriz.prestamo = fuentePrestamo;
                        unidadMatriz.donacion = fuenteDonacion;
                        unidadMatriz.nacional = fuenteNacional;
                        unidadMatriz.techo = estructura_.get("techo").getAsBigDecimal();
                        unidadesEjecutorasMatriz.Add(unidadMatriz);
                    }
                }
                PrestamoDAO.actualizarMatriz(prestamo.id, unidadesEjecutorasMatriz);
                int diferencia = DataSigadeDAO.getDiferenciaMontos(prestamo.getCodigoPresupuestario() + "");

                response_text = String.join("", "{\"success\":", ret ? "true" : "false", response_text,
                     ",\"diferencia\":", diferencia + "",
                     "}");
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("9", "PrestamoController.class", e);
                return BadRequest(500);
            }
        }        
    }
}
