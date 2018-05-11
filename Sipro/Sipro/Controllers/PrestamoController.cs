using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModelCore.Models;
using Sipro.Dao;
using Utilities;

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class PrestamoController : Controller
    {
        public class stunidadejecutora
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
            public decimal techo;
            public int prestamoId;
            public int componenteId;
            public int componenteSigadeId;
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardarPrestamo([FromBody]dynamic value)
        {
            Prestamo prestamo = new Prestamo();
            prestamo.id = (int)value.id;
            prestamo.amortizado = (decimal?)value.amortizado;
            prestamo.aniosGracia = (int?)value.aniosGracia;
            prestamo.aniosPlazo = (int?)value.aniosPlazo;
            prestamo.autorizacionTipoid = (int?)value.autorizacionTipoid;
            prestamo.codigoPresupuestario = (long)value.codigoPresupuestario;
            prestamo.comisionCompromisoAcumulado = (decimal?)value.comisionCompromisoAcumulado;
            prestamo.comisionCompromisoAnio = (decimal?)value.comisionCompromisoAnio;
            prestamo.cooperantecodigo = (int?)value.cooperantecodigo;
            prestamo.cooperanteejercicio = (int?)value.cooperanteejercicio;
            prestamo.desembolsadoReal = (decimal?)value.desembolsadoReal;
            prestamo.desembolsoAFechaUe = (decimal?)value.desembolsoAFechaUe;
            prestamo.desembolsoAFechaUeUsd = (decimal?)value.desembolsoAFechaUeUsd;
            prestamo.desembolsoAFechaUsd = (decimal?)value.desembolsoAFechaUsd;
            prestamo.destino = (string)value.destino;
            prestamo.ejecucionEstadoid = (int?)value.ejecucionEstadoid;
            prestamo.ejercicio = (int)value.ejercicio;
            prestamo.entidad = (int)value.entidad;
            prestamo.estado = 1;
            prestamo.fechaAutorizacion = Utils.getFechaHoraNull((string)value.fechaAutorizacion);
            prestamo.fechaCierreActualUe = Utils.getFechaHora((string)value.fechaCierreActualUe);
            prestamo.fechaCierreOrigianlUe = Utils.getFechaHora((string)value.fechaCierreOrigianlUe);
            prestamo.fechaCorte = Utils.getFechaHoraNull((string)value.fechaCorte);
            prestamo.fechaCreacion = DateTime.Now;
            prestamo.fechaDecreto = Utils.getFechaHora((string)value.fechaDecreto);
            prestamo.fechaElegibilidadUe = Utils.getFechaHora((string)value.fechaElegibilidadUe);
            prestamo.fechaFinEjecucion = Utils.getFechaHoraNull((string)value.fechaFinEjecucion);
            prestamo.fechaFirma = Utils.getFechaHoraNull((string)value.fechaFirma);
            prestamo.fechaSuscripcion = Utils.getFechaHora((string)value.fechaSuscripcion);
            prestamo.fechaVigencia = Utils.getFechaHora((string)value.fechaVigencia);
            prestamo.interesesAcumulados = (decimal?)value.interesesAcumulados;
            prestamo.interesesAnio = (decimal?)value.interesesAnio;
            prestamo.interesTipoid = (int?)value.interesTipoid;
            prestamo.mesesProrrogaUe = (int)value.mesesProrrogaUe;
            prestamo.montoAsignadoUe = (decimal?)value.montoAsignadoUe;
            prestamo.montoAsignadoUeQtz = (decimal?)value.montoAsignadoUeQtz;
            prestamo.montoAsignadoUeUsd = (decimal?)value.montoAsignadoUeUsd;
            prestamo.montoContratado = (decimal)value.montoContratado;
            prestamo.montoContratadoQtz = (decimal)value.montoContratadoQtz;
            prestamo.montoContratadoUsd = (decimal)value.montoContratadoUsd;
            prestamo.montoPorDesembolsarUe = (decimal?)value.montoPorDesembolsarUe;
            prestamo.montoPorDesembolsarUeUsd = (decimal?)value.montoPorDesembolsarUeUsd;
            prestamo.montoPorDesembolsarUsd = (decimal)value.montoPorDesembolsarUsd;
            prestamo.numeroAutorizacion = (string)value.numeroAutorizacion;
            prestamo.numeroPrestamo = (string)value.numeroPrestamo;
            prestamo.objetivo = (string)value.objetivo;
            prestamo.objetivoEspecifico = (string)value.objetivoEspecifico;
            prestamo.otrosCargosAcumulados = (decimal?)value.otrosCargosAcumulados;
            prestamo.otrosGastos = (decimal?)value.otrosGastos;
            prestamo.peridoEjecucion = (int?)value.peridoEjecucion;
            prestamo.plazoEjecucionUe = (int?)value.plazoEjecucionUe;
            prestamo.porAmortizar = (decimal?)value.porAmortizar;
            prestamo.porcentajeAvance = (int)value.porcentajeAvance;
            prestamo.porcentajeComisionCompra = (decimal?)value.porcentajeComisionCompra;
            prestamo.porcentajeInteres = (decimal?)value.porcentajeInteres;
            prestamo.presupuestoAsignadoFunc = (decimal?)value.presupuestoAsignadoFunc;
            prestamo.presupuestoAsignadoInv = (decimal?)value.presupuestoAsignadoInv;
            prestamo.presupuestoDevengadoFunc = (decimal?)value.presupuestoDevengadoFunc;
            prestamo.presupuestoDevengadoInv = (decimal?)value.presupuestoDevengadoInv;
            prestamo.presupuestoModificadoFunc = (decimal?)value.presupuestoModificadoFunc;
            prestamo.presupuestoModificadoInv = (decimal?)value.presupuestoModificadoInv;
            prestamo.presupuestoPagadoFunc = (decimal?)value.presupuestoPagadoFunc;
            prestamo.presupuestoPagadoInv = (decimal?)value.presupuestoPagadoInv;
            prestamo.presupuestoVigenteFunc = (decimal?)value.presupuestoVigenteFunc;
            prestamo.presupuestoVigenteInv = (decimal?)value.presupuestoVigenteInv;
            prestamo.principalAcumulado = (decimal?)value.principalAcumulado;
            prestamo.principalAnio = (decimal?)value.principalAnio;
            prestamo.proyectoPrograma = (string)value.proyectoPrograma;
            prestamo.saldoCuentas = (decimal?)value.saldoCuentas;
            prestamo.sectorEconomico = (string)value.sectorEconomico;
            prestamo.tipoMonedaid = (int)value.tipoMonedaid;
            prestamo.ueunidadEjecutora = (int)value.ueunidadEjecutora;
            prestamo.usuarioCreo = (string)value.usuarioCreo;
            PrestamoDAO.guardarPrestamo(prestamo);
            return Ok(JsonConvert.SerializeObject(prestamo));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamosPagina([FromBody]dynamic value)
        {
            List<Prestamo> lstprestamos = PrestamoDAO.getPrestamosPagina((int)value.pagina, (int)value.elementosPorPagina, (string)value.filtroNombre, (long?)value.filtroCodigoPresupuestario, 
                (string)value.filtroNumeroPrestamo, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion, (string)value.columnaOrdenada, (string)value.ordenDireccion, (string)value.usuario);
            return Ok(JsonConvert.SerializeObject(lstprestamos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTotalPrestamos([FromBody]dynamic value)
        {
            long total = PrestamoDAO.getTotalPrestamos((string)value.filtroNombre, (long)value.filtroCodigoPresupuestario, (string)value.filtroNumeroPrestamo, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion, (string)value.usuario);
            return Ok(JsonConvert.SerializeObject(total));
        }

        // POST api/values
        [HttpPost]
        public IActionResult borrarPrestamo([FromBody]dynamic value)
        {
            Prestamo prestamo = PrestamoDAO.getPrestamoById((int)value.idPrestamo);
            PrestamoDAO.borrarPrestamo(prestamo, (string)value.usuario);
            return Ok(JsonConvert.SerializeObject(prestamo));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamoById([FromBody]dynamic value)
        {
            Prestamo prestamo = PrestamoDAO.getPrestamoById((int)value.idPrestamo);
            return Ok(JsonConvert.SerializeObject(prestamo));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamos([FromBody]dynamic value)
        {
            List<Prestamo> lstprestamos = PrestamoDAO.getPrestamos((string)value.usuario);
            return Ok(JsonConvert.SerializeObject(lstprestamos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamoByIdHistory([FromBody]dynamic value)
        {
            Prestamo lstprestamos = PrestamoDAO.getPrestamoByIdHistory((int)value.idPrestamo, (string)value.lineaBase);
            return Ok(JsonConvert.SerializeObject(lstprestamos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult actualizarMatriz([FromBody]dynamic value)
        {
            List<UnidadEjecutora> unidadesEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutoras((int)value.ejercicio, (int)value.entidad);
            List<stunidadejecutora> lstunidadesEjecutoras = new List<stunidadejecutora>();
            foreach (UnidadEjecutora unidad in unidadesEjecutoras)
            {
                stunidadejecutora stunidad = new stunidadejecutora();
                stunidad.nombre = unidad.nombre;
                stunidad.id = unidad.unidadEjecutora;
                stunidad.entidad = unidad.entidadentidad.ToString();
                stunidad.ejercicio = unidad.ejercicio;
                lstunidadesEjecutoras.Add(stunidad);
            }
            bool actualizado = PrestamoDAO.actualizarMatriz((int)value.idPrestamo, lstunidadesEjecutoras);
            return Ok(JsonConvert.SerializeObject(actualizado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getVersionHistoriaMatriz([FromBody]dynamic value)
        {
            int version = PrestamoDAO.getVersionHistoriaMatriz((int)value.idPrestamo);
            return Ok(JsonConvert.SerializeObject(version));
        }
    }
}