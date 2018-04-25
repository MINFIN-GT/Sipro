
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
	    public virtual string id { get; set; }
	    [Column("FECHA_CORTE")]
	    public virtual string fechaCorte { get; set; }
	    [Column("CODIGO_PRESUPUESTARIO")]
	    public virtual string codigoPresupuestario { get; set; }
	    [Column("NUMERO_PRESTAMO")]
	    public virtual string numeroPrestamo { get; set; }
	    public virtual string destino { get; set; }
	    [Column("SECTOR_ECONOMICO")]
	    public virtual string sectorEconomico { get; set; }
	    [Column("UEUNIDAD_EJECUTORA")]
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ueunidadEjecutora { get; set; }
	    [Column("FECHA_FIRMA")]
	    public virtual string fechaFirma { get; set; }
	    [Column("AUTORIZACION_TIPOID")]
	    public virtual string autorizacionTipoid { get; set; }
	    [Column("NUMERO_AUTORIZACION")]
	    public virtual string numeroAutorizacion { get; set; }
	    [Column("FECHA_AUTORIZACION")]
	    public virtual string fechaAutorizacion { get; set; }
	    [Column("ANIOS_PLAZO")]
	    public virtual string aniosPlazo { get; set; }
	    [Column("ANIOS_GRACIA")]
	    public virtual string aniosGracia { get; set; }
	    [Column("FECHA_FIN_EJECUCION")]
	    public virtual string fechaFinEjecucion { get; set; }
	    [Column("PERIDO_EJECUCION")]
	    public virtual string peridoEjecucion { get; set; }
	    [Column("INTERES_TIPOID")]
	    public virtual string interesTipoid { get; set; }
	    [Column("PORCENTAJE_INTERES")]
	    public virtual string porcentajeInteres { get; set; }
	    [Column("PORCENTAJE_COMISION_COMPRA")]
	    public virtual string porcentajeComisionCompra { get; set; }
	    [Column("TIPO_MONEDAID")]
	    [ForeignKey("TipoMoneda")]
        public virtual string tipoMonedaid { get; set; }
	    [Column("MONTO_CONTRATADO")]
	    public virtual string montoContratado { get; set; }
	    public virtual string amortizado { get; set; }
	    [Column("POR_AMORTIZAR")]
	    public virtual string porAmortizar { get; set; }
	    [Column("PRINCIPAL_ANIO")]
	    public virtual string principalAnio { get; set; }
	    [Column("INTERESES_ANIO")]
	    public virtual string interesesAnio { get; set; }
	    [Column("COMISION_COMPROMISO_ANIO")]
	    public virtual string comisionCompromisoAnio { get; set; }
	    [Column("OTROS_GASTOS")]
	    public virtual string otrosGastos { get; set; }
	    [Column("PRINCIPAL_ACUMULADO")]
	    public virtual string principalAcumulado { get; set; }
	    [Column("INTERESES_ACUMULADOS")]
	    public virtual string interesesAcumulados { get; set; }
	    [Column("COMISION_COMPROMISO_ACUMULADO")]
	    public virtual string comisionCompromisoAcumulado { get; set; }
	    [Column("OTROS_CARGOS_ACUMULADOS")]
	    public virtual string otrosCargosAcumulados { get; set; }
	    [Column("PRESUPUESTO_ASIGNADO_FUNC")]
	    public virtual string presupuestoAsignadoFunc { get; set; }
	    [Column("PRESUPUESTO_ASIGNADO_INV")]
	    public virtual string presupuestoAsignadoInv { get; set; }
	    [Column("PRESUPUESTO_MODIFICADO_FUNC")]
	    public virtual string presupuestoModificadoFunc { get; set; }
	    [Column("PRESUPUESTO_MODIFICADO_INV")]
	    public virtual string presupuestoModificadoInv { get; set; }
	    [Column("PRESUPUESTO_VIGENTE_FUNC")]
	    public virtual string presupuestoVigenteFunc { get; set; }
	    [Column("PRESUPUESTO_VIGENTE_INV")]
	    public virtual string presupuestoVigenteInv { get; set; }
	    [Column("PRESUPUESTO_DEVENGADO_FUNC")]
	    public virtual string presupuestoDevengadoFunc { get; set; }
	    [Column("PRESUPUESTO_DEVENGADO_INV")]
	    public virtual string presupuestoDevengadoInv { get; set; }
	    [Column("PRESUPUESTO_PAGADO_FUNC")]
	    public virtual string presupuestoPagadoFunc { get; set; }
	    [Column("PRESUPUESTO_PAGADO_INV")]
	    public virtual string presupuestoPagadoInv { get; set; }
	    [Column("SALDO_CUENTAS")]
	    public virtual string saldoCuentas { get; set; }
	    [Column("DESEMBOLSADO_REAL")]
	    public virtual string desembolsadoReal { get; set; }
	    [Column("EJECUCION_ESTADOID")]
	    public virtual string ejecucionEstadoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    [Column("PROYECTO_PROGRAMA")]
	    public virtual string proyectoPrograma { get; set; }
	    [Column("FECHA_DECRETO")]
	    public virtual string fechaDecreto { get; set; }
	    [Column("FECHA_SUSCRIPCION")]
	    public virtual string fechaSuscripcion { get; set; }
	    [Column("FECHA_ELEGIBILIDAD_UE")]
	    public virtual string fechaElegibilidadUe { get; set; }
	    [Column("FECHA_CIERRE_ORIGIANL_UE")]
	    public virtual string fechaCierreOrigianlUe { get; set; }
	    [Column("FECHA_CIERRE_ACTUAL_UE")]
	    public virtual string fechaCierreActualUe { get; set; }
	    [Column("MESES_PRORROGA_UE")]
	    public virtual string mesesProrrogaUe { get; set; }
	    [Column("PLAZO_EJECUCION_UE")]
	    public virtual string plazoEjecucionUe { get; set; }
	    [Column("MONTO_ASIGNADO_UE")]
	    public virtual string montoAsignadoUe { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_UE")]
	    public virtual string desembolsoAFechaUe { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_UE")]
	    public virtual string montoPorDesembolsarUe { get; set; }
	    [Column("FECHA_VIGENCIA")]
	    public virtual string fechaVigencia { get; set; }
	    [Column("MONTO_CONTRATADO_USD")]
	    public virtual string montoContratadoUsd { get; set; }
	    [Column("MONTO_CONTRATADO_QTZ")]
	    public virtual string montoContratadoQtz { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_USD")]
	    public virtual string desembolsoAFechaUsd { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_USD")]
	    public virtual string montoPorDesembolsarUsd { get; set; }
	    [Column("MONTO_ASIGNADO_UE_USD")]
	    public virtual string montoAsignadoUeUsd { get; set; }
	    [Column("MONTO_ASIGNADO_UE_QTZ")]
	    public virtual string montoAsignadoUeQtz { get; set; }
	    [Column("DESEMBOLSO_A_FECHA_UE_USD")]
	    public virtual string desembolsoAFechaUeUsd { get; set; }
	    [Column("MONTO_POR_DESEMBOLSAR_UE_USD")]
	    public virtual string montoPorDesembolsarUeUsd { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string entidad { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ejercicio { get; set; }
	    public virtual string objetivo { get; set; }
	    [Column("OBJETIVO_ESPECIFICO")]
	    public virtual string objetivoEspecifico { get; set; }
	    [Column("PORCENTAJE_AVANCE")]
	    public virtual string porcentajeAvance { get; set; }
	    [ForeignKey("Cooperante")]
        public virtual string cooperantecodigo { get; set; }
	    [ForeignKey("Cooperante")]
        public virtual string cooperanteejercicio { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual Cooperante cooperantes { get; set; }
		public virtual TipoMoneda tipoMonedas { get; set; }
		public virtual IEnumerable<Prestamo> prestamoes { get; set; }
	}
}
