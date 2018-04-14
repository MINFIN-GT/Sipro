
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the prestamo table.
    /// </summary>
	[Table("prestamo")]
	public partial class Prestamo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual byte[] fecha_corte { get; set; }
	    public virtual long codigo_presupuestario { get; set; }
	    public virtual string numero_prestamo { get; set; }
	    public virtual string destino { get; set; }
	    public virtual string sector_economico { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? unidad_ejecutoraunidad_ejecutora { get; set; }
	    public virtual byte[] fecha_firma { get; set; }
	    [ForeignKey("autorizaciontipo")]
        public virtual int? autorizacion_tipoid { get; set; }
	    public virtual string numero_autorizacion { get; set; }
	    public virtual byte[] fecha_autorizacion { get; set; }
	    public virtual int? anios_plazo { get; set; }
	    public virtual int? anios_gracia { get; set; }
	    public virtual byte[] fecha_fin_ejecucion { get; set; }
	    public virtual int? perido_ejecucion { get; set; }
	    [ForeignKey("interestipo")]
        public virtual int? interes_tipoid { get; set; }
	    public virtual decimal? porcentaje_interes { get; set; }
	    public virtual decimal? porcentaje_comision_compra { get; set; }
	    [ForeignKey("tipomoneda")]
        public virtual int tipo_monedaid { get; set; }
	    public virtual decimal monto_contratado { get; set; }
	    public virtual decimal? amortizado { get; set; }
	    public virtual decimal? por_amortizar { get; set; }
	    public virtual decimal? principal_anio { get; set; }
	    public virtual decimal? intereses_anio { get; set; }
	    public virtual decimal? comision_compromiso_anio { get; set; }
	    public virtual decimal? otros_gastos { get; set; }
	    public virtual decimal? principal_acumulado { get; set; }
	    public virtual decimal? intereses_acumulados { get; set; }
	    public virtual decimal? comision_compromiso_acumulado { get; set; }
	    public virtual decimal? otros_cargos_acumulados { get; set; }
	    public virtual decimal? presupuesto_asignado_funcionamiento { get; set; }
	    public virtual decimal? prespupuesto_asignado_inversion { get; set; }
	    public virtual decimal? presupuesto_modificado_funcionamiento { get; set; }
	    public virtual decimal? presupuesto_modificado_inversion { get; set; }
	    public virtual decimal? presupuesto_vigente_funcionamiento { get; set; }
	    public virtual decimal? presupuesto_vigente_inversion { get; set; }
	    public virtual decimal? prespupuesto_devengado_funcionamiento { get; set; }
	    public virtual decimal? presupuesto_devengado_inversion { get; set; }
	    public virtual decimal? presupuesto_pagado_funcionamiento { get; set; }
	    public virtual decimal? presupuesto_pagado_inversion { get; set; }
	    public virtual decimal? saldo_cuentas { get; set; }
	    public virtual decimal? desembolsado_real { get; set; }
	    [ForeignKey("ejecucionestado")]
        public virtual int? ejecucion_estadoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string proyecto_programa { get; set; }
	    public virtual byte[] fecha_decreto { get; set; }
	    public virtual byte[] fecha_suscripcion { get; set; }
	    public virtual byte[] fecha_elegibilidad_ue { get; set; }
	    public virtual byte[] fecha_cierre_origianl_ue { get; set; }
	    public virtual byte[] fecha_cierre_actual_ue { get; set; }
	    public virtual int meses_prorroga_ue { get; set; }
	    public virtual int? plazo_ejecucion_ue { get; set; }
	    public virtual decimal? monto_asignado_ue { get; set; }
	    public virtual decimal? desembolso_a_fecha_ue { get; set; }
	    public virtual decimal? monto_por_desembolsar_ue { get; set; }
	    public virtual byte[] fecha_vigencia { get; set; }
	    public virtual decimal monto_contratado_usd { get; set; }
	    public virtual decimal monto_contratado_qtz { get; set; }
	    public virtual decimal? desembolso_a_fecha_usd { get; set; }
	    public virtual decimal monto_por_desembolsar_usd { get; set; }
	    public virtual decimal? monto_asignado_ue_usd { get; set; }
	    public virtual decimal? monto_asignado_ue_qtz { get; set; }
	    public virtual decimal? desembolso_a_fecha_ue_usd { get; set; }
	    public virtual decimal? monto_por_desembolsar_ue_usd { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? entidad { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? ejercicio { get; set; }
	    public virtual string objetivo { get; set; }
	    public virtual string objetivo_especifico { get; set; }
	    public virtual int? porcentaje_avance { get; set; }
	    [ForeignKey("cooperante")]
        public virtual int cooperantecodigo { get; set; }
	    [ForeignKey("cooperante")]
        public virtual int cooperanteejercicio { get; set; }
		public virtual Tipomoneda ttipomoneda { get; set; }
		public virtual Ejecucionestado tejecucionestado { get; set; }
		public virtual Interestipo tinterestipo { get; set; }
		public virtual Autorizaciontipo tautorizaciontipo { get; set; }
		public virtual Unidadejecutora tunidadejecutora { get; set; }
		public virtual Cooperante tcooperante { get; set; }
		public virtual IEnumerable<Prestamo> prestamoes { get; set; }
	}
}
