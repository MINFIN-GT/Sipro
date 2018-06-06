using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelAnalyticCore.Models;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

namespace SDataSigade.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class DataSigadeController : Controller
    {
        private class stcodigopresupuestario
        {
            public String codigopresupuestario;
            public String numeroprestamo;
        }

        private class stprestamo
        {
            public String fechaCorte;
            public String codigoPresupuestario;
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
            public int cooperanteid;
            public String cooperantenombre;
            public String objetivo;
        }

        private class stdesembolsos
        {
            public long ejercicioFiscal;
            public int mesDesembolso;
            public String codigoPresupuestario;
            public int entidadSicoin;
            public int unidadEjecutoraSicoin;
            public String monedaDesembolso;
            public decimal mesDesembolsoMoneda;
            public decimal tipoCambioUSD;
            public decimal mesDesembolsoMonedaUSD;
            public decimal tipoCambioGTQ;
            public decimal mesDesembolsoMonedaGTQ;
        }

        // GET api/DataSigade/Datos/codigoPresupuestario
        [HttpGet("{codigoPresupuestario}")]
        [Authorize("Data Sigade - Visualizar")]
        public IActionResult Datos(string codigoPresupuestario)
        {
            try
            {
                DtmAvanceFisfinanDti inf = DataSigadeDAO.getavanceFisFinanDMS1(codigoPresupuestario);

                stprestamo temp = new stprestamo();
                if (inf != null)
                {
                    temp.codigoPresupuestario = inf.codigoPresupuestario;
                    temp.numeroPrestamo = inf.noPrestamo;
                    temp.proyectoPrograma = inf.nombrePrograma;
                    Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo(inf.codigoOrganismoFinan ?? default(int));
                    if (cooperante != null)
                    {
                        temp.cooperanteid = cooperante.codigo;
                        temp.cooperantenombre = cooperante.nombre;
                    }

                    temp.fechaDecreto = inf.fechaDecreto != null ? inf.fechaDecreto.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaSuscripcion = inf.fechaSuscripcion != null ? inf.fechaSuscripcion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaVigencia = inf.fechaVigencia != null ? inf.fechaVigencia.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    TipoMoneda moneda = TipoMonedaDAO.getTipoMonedaPorSimbolo(inf.monedaPrestamo);
                    temp.tipoMonedaNombre = String.Join("", moneda.nombre, " (" + moneda.simbolo + ")");
                    temp.tipoMonedaId = moneda.id;
                    temp.montoContratado = inf.montoContratado ?? default(decimal);
                    temp.montoContratadoUsd = inf.montoContratadoUsd ?? default(decimal);
                    temp.montoContratadoQtz = inf.montoContratadoGtq ?? default(decimal);
                    temp.desembolsoAFechaUsd = inf.desembolsosUsd ?? default(decimal);
                    temp.montoPorDesembolsarUsd = inf.porDesembolsarUsd ?? default(decimal);
                    temp.objetivo = inf.objetivo;

                    return Ok(new { success = true, prestamo = temp });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("1", "DataSigadeController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/DataSigade/Codigos
        [HttpGet]
        [Authorize("Data Sigade - Visualizar")]
        public IActionResult Codigos()
        {
            try
            {
                List<DtmAvanceFisfinanDti> prestamos = DataSigadeDAO.getCodigos();
                List<stcodigopresupuestario> codigos = new List<stcodigopresupuestario>();
                foreach (DtmAvanceFisfinanDti prestamo in prestamos)
                {
                    stcodigopresupuestario temp = new stcodigopresupuestario();
                    temp.codigopresupuestario = prestamo.codigoPresupuestario;
                    temp.numeroprestamo = prestamo.noPrestamo;
                    codigos.Add(temp);
                }

                return Ok(new { success = true, prestamo = codigos });
            }
            catch (Exception e)
            {
                CLogger.write("2", "DataSigadeController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/DataSigade/DesembolsosUE
        [HttpPost]
        [Authorize("Data Sigade - Visualizar")]
        public IActionResult DesembolsosUE([FromBody]dynamic value)
        {
            try
            {
                string codPrep = value.codPrep != null ? (string)value.codPrep : default(string);
                int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                int entidad = value.entidad != null ? (int)value.entidad : default(int);
                int ue = value.ue != null ? (int)value.ue : default(int);

                List <DtmAvanceFisfinanDetDti> lstDesembolsos = DataSigadeDAO.getInfPorUnidadEjecutora(codPrep, ejercicio, entidad, ue);
                List<stdesembolsos> lstDesembolsosUE = new List<stdesembolsos>();
                foreach (DtmAvanceFisfinanDetDti desembolso in lstDesembolsos)
                {
                    stdesembolsos temp = new stdesembolsos();
                    temp.codigoPresupuestario = desembolso.codigoPresupuestario;
                    temp.ejercicioFiscal = desembolso.ejercicioFiscal;
                    temp.entidadSicoin = desembolso.entidadSicoin ?? default(int);
                    temp.mesDesembolso = Convert.ToInt32(desembolso.mesDesembolso);
                    temp.mesDesembolsoMoneda = desembolso.desembolsosMesMoneda ?? default(decimal);
                    temp.mesDesembolsoMonedaGTQ = desembolso.desembolsosMesGtq ?? default(decimal);
                    temp.mesDesembolsoMonedaUSD = desembolso.desembolsosMesUsd ?? default(decimal);
                    temp.monedaDesembolso = desembolso.monedaDesembolso;
                    temp.tipoCambioUSD = desembolso.tcMonUsd ?? default(decimal);
                    temp.tipoCambioGTQ = desembolso.tcUsdGtq ?? default(decimal);

                    lstDesembolsosUE.Add(temp);
                }

                return Ok(new { success = true, desembolsosUE = lstDesembolsosUE });
            }
            catch (Exception e)
            {
                CLogger.write("3", "DataSigadeController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/DataSigade/MontoDesembolsoUE
        [HttpPost]
        [Authorize("Data Sigade - Visualizar")]
        public IActionResult MontoDesembolsoUE([FromBody]dynamic value)
        {
            try
            {
                string codPrep = value.codPrep != null ? (string)value.codPrep : default(string);
                int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                int entidad = value.entidad != null ? (int)value.entidad : default(int);
                int ue = value.ue != null ? (int)value.ue : default(int);

                List<DtmAvanceFisfinanDetDti> lstDesembolsos = DataSigadeDAO.getInfPorUnidadEjecutora(codPrep, ejercicio, entidad, ue);

                decimal montoDesembolsado = decimal.Zero;

                foreach (DtmAvanceFisfinanDetDti desembolso in lstDesembolsos)
                {
                    montoDesembolsado += desembolso.desembolsosMesUsd ?? default(decimal);
                }

                return Ok(new { success = true, montoDesembolsadoUE = montoDesembolsado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/DataSigade/MontoDesembolsosUEALaFecha
        [HttpPost]
        [Authorize("Data Sigade - Visualizar")]
        public IActionResult MontoDesembolsosUEALaFecha([FromBody]dynamic value)
        {
            try
            {
                string codPrep = value.codPrep != null ? (string)value.codPrep : default(string);
                int entidad = value.entidad != null ? (int)value.entidad : default(int);
                
                List<DtmAvanceFisfinanDetDti> lstDesembolsos = DataSigadeDAO.getInfPorUnidadEjecutoraALaFecha(codPrep, entidad);

                decimal montoDesembolsado = decimal.Zero;

                foreach (DtmAvanceFisfinanDetDti desembolso in lstDesembolsos)
                {
                    montoDesembolsado += desembolso.desembolsosMesUsd ?? default(decimal);
                }

                return Ok(new { success = true, montoDesembolsadoUEALaFecha = montoDesembolsado });
            }
            catch (Exception e)
            {
                CLogger.write("5", "DataSigadeController.class", e);
                return BadRequest(500);
            }
        }
    }
}
