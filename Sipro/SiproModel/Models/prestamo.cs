
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PRESTAMO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PRESTAMO")]
	public partial class Prestamo
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    [Column("FECHA_CORTE")]
	    public virtual byte[] fechaCorte { get; set; }
	    [Column("CODIGO_PRESUPUESTARIO")]
	    public virtual Int64 codigoPresupuestario { get; set; }
	    [Column("NUMERO_PRESTAMO")]
	    public virtual string numeroPrestamo { get; set; }
	    public virtual string destino { get; set; }
	    [Column("SECTOR_ECONOMICO")]
	    public virtual string sectorEconomico { get; set; }
	    [Column("UEUNIDAD_EJECUTORA")]
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32 ueunidadEjecutora { get; set; }
	    [Column("FECHA_FIRMA")]
	    public virtual byte[] fechaFirma { get; set; }
	    [Column("AUTORIZACION_TIPOID")]
	    public virtual Int32? autorizacionTipoid { get; set; }
	    [Column("NUMERO_AUTORIZACION")]
	    public virtual string numeroAutorizacion { get; set; }
	    [Column("FECHA_AUTORIZACION")]
	    public virtual byte[] fechaAutorizacion { get; set; }
	    [Column("ANIOS_PLAZO")]
	    public virtual Int32? aniosPlazo { get; set; }
	    [Column("ANIOS_GRACIA")]
	    public virtual Int32? aniosGracia { get; set; }
	    [Column("FECHA_FIN_EJECUCION")]
	    public virtual byte[] fechaFinEjecucion { get; set; }
	    [Column("PERIDO_EJECUCION")]
	    public virtual Int32? peridoEjecucion { get; set; }
	    [Column("INTERES_TIPOID")]
	    public virtual Int32? interesTipoid { get; set; }
	    [Column("PORCENTAJE_INTERES")]
	    public virtual decimal? porcentajeInteres { get; set; }
	    [Column("PORCENTAJE_COMISION_COMPRA")]
	    public virtual decimal? porcentajeComisionCompra { get; set; }
	    [Column("TIPO_MONEDAID")]
	    [ForeignKey("TipoMoneda")]
        public virtual Int32 tipoMonedaid { get; set; }
	    [Column("MONTO_CONTRATADO")]
	    public virtual decimal montoContratado { get; set; }
	    public virtual decimal? amortizado { get; set; }
	    [Column("POR_AMORTIZAR")]
	    public virtual decimal? porAmortizar { get; set; }
	    [Column("PRINCIPAL_ANIO")]
	    public virtual decimal? principalAnio { get; set; }
	    [Column("INTERESES_ANIO")]
	    public virtual decimal? interesesAnio { get; set; }
	    [Column("COMISION_COMPROMISO_ANIO")]
	    public virtual decimal? comisionCompromisoAnio { get; set; }
	    [Column("OTROS_GASTOS")]
	    public virtual decimal? otrosGastos { get; set; }
	    [Column("PRINCIPAL_ACUMULADO")]
	    public virtual decimal? principalAcumulado { get; set; }
	    [Column("INTERESES_ACUMULADOS")]
	    public virtual decimal? interesesAcumulados { get; set; }
	    [Column("COMISION_COMPROMISO_ACUMULADO")]
	    public virtual decimal? comisionCompromisoAcumulado { get; set; }
	    [Column("OTROS_CARGOS_ACUMULADOS")]
	    public virtual decimal? otrosCargosAcumulados { get; set; }
	    [Column("PRESUPUESTO_ASIGNADO_FUNC")]
	    public virtual decimal? presupuestoAsignadoFunc { get; set; }
	    [Column("PRESUPUESTO_ASIGNADO_INV")]
	    public virtual decimal? presupuestoAsignadoInv { get; set; }
	    [Column("PRESUPUESTO_MODIFICADO_FUNC")]
	    public virtual decimal? presupuestoModificadoFunc { get; set; }
	    [Column("PRESUPUESTO_MODIFICADO_INV")]
	    public virtual decimal? presupuestoModificadoInv { get; set; }
	    [Column("PRESUPUESTO_VIGENTE_FUNC")]
	    public virtual decimal? presupuestoVigenteFunc { get; set; }
	    [Column("PRESUPUESTO_VIGENTE_INV")]
	    public virtual decimal? presupuestoVigenteInv { get; set; }
	    [Column("PRESUPUESTO_DEVENGADO_FUNC")]
	    public virtual decimal? presupuestoDevengadoFunc { get; set; }
	    [Column("PRESUPUESTO_DEVENGADO_INV")]
	    public virtual decimal? presupuestoDevengadoInv { get; set; }
	    [Column("PRESUPUESTO_PAGADO_FUNC")]
	    public virtual decimal? presupuestoPagadoFunc { get; set; }
	    [Column("PRESUPUESTO_PAGADO_INV")]
	    public virtual decimal? presupuestoPagadoInv { get; set; }
	    [Column("SALDO_CUENTAS")]
	    public virtual decimal? saldoCuentas { get; set; }
	    [Column("DESEMBOLSADO_REAL")]
	    public virtual decimal? desembolsadoReal { get; set; }
	    [Column("EJECUCION_ESTADOID")]
	    public virtual Int32? ejecucionEstadoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("PROYECTO_PROGRAMA")]
	    public virtual string proyectoPrograma { get; set; }
	    [Column("FECHA_DECRETO")]
	    public virtual byte[] fechaDecreto { get; set; }
	    [Column("FECHA_SUSCRIPCION")]
	    public virtual byte[] fechaSuscripcion { get; set; }
	    [Column("FECHA_ELEGIBILIDAD_UE")]
	    public virtual byte[] fechaElegibilidadUe { get; set; }
	    [Column("FECHA_CIERRE_ORIGIANL_UE")]
	    public virtual byte[] fechaCierreOrigianlUe { get; set; }
	    [Column("FECHA_CIERRE_ACTUAL_UE")]
	    public virtual byte[] fechaCierreActualUe { get; set; }
	    [Column("MESES_PRORROGA_UE")]
	    public virtual Int32 mesesProrrogaUe { get; set; }
	    [Column("PLAZO_EJECUCION_UE")]
	    public virtual Int32? plazoEjecucionUe { get; set; }
	    [Column("MONTO_ASIGNADO_UE")]
	    public virtual decimal? montoAsignadoUe { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_UE")]
	    public virtual decimal? desembolsoAFechaUe { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_UE")]
	    public virtual decimal? montoPorDesembolsarUe { get; set; }
	    [Column("FECHA_VIGENCIA")]
	    public virtual byte[] fechaVigencia { get; set; }
	    [Column("MONTO_CONTRATADO_USD")]
	    public virtual decimal montoContratadoUsd { get; set; }
	    [Column("MONTO_CONTRATADO_QTZ")]
	    public virtual decimal montoContratadoQtz { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_USD")]
	    public virtual decimal? desembolsoAFechaUsd { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_USD")]
	    public virtual decimal montoPorDesembolsarUsd { get; set; }
	    [Column("MONTO_ASIGNADO_UE_USD")]
	    public virtual decimal? montoAsignadoUeUsd { get; set; }
	    [Column("MONTO_ASIGNADO_UE_QTZ")]
	    public virtual decimal? montoAsignadoUeQtz { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_UE_USD")]
	    public virtual decimal? desembolsoAFechaUeUsd { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_UE_USD")]
	    public virtual decimal? montoPorDesembolsarUeUsd { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32 entidad { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32 ejercicio { get; set; }
	    public virtual string objetivo { get; set; }
	    [Column("OBJETIVO_ESPECIFICO")]
	    public virtual string objetivoEspecifico { get; set; }
	    [Column("PORCENTAJE_AVANCE")]
	    public virtual Int32 porcentajeAvance { get; set; }
	    [ForeignKey("Cooperante")]
        public virtual Int32? cooperantecodigo { get; set; }
	    [ForeignKey("Cooperante")]
        public virtual Int32? cooperanteejercicio { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual Cooperante cooperantes { get; set; }
		public virtual TipoMoneda tipoMonedas { get; set; }
		public virtual IEnumerable<Prestamo> prestamoes { get; set; }
	}
}
