
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
	[Table("prestamo")]
	public partial class prestamo
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string fecha_corte { get; set; }
	    public virtual string codigo_presupuestario { get; set; }
	    public virtual string numero_prestamo { get; set; }
	    public virtual string destino { get; set; }
	    public virtual string sector_economico { get; set; }
	    [ForeignKey("unidad_ejecutora")]
        public virtual string ueunidad_ejecutora { get; set; }
	    public virtual string fecha_firma { get; set; }
	    public virtual string autorizacion_tipoid { get; set; }
	    public virtual string numero_autorizacion { get; set; }
	    public virtual string fecha_autorizacion { get; set; }
	    public virtual string anios_plazo { get; set; }
	    public virtual string anios_gracia { get; set; }
	    public virtual string fecha_fin_ejecucion { get; set; }
	    public virtual string perido_ejecucion { get; set; }
	    public virtual string interes_tipoid { get; set; }
	    public virtual string porcentaje_interes { get; set; }
	    public virtual string porcentaje_comision_compra { get; set; }
	    [ForeignKey("tipo_moneda")]
        public virtual string tipo_monedaid { get; set; }
	    public virtual string monto_contratado { get; set; }
	    public virtual string amortizado { get; set; }
	    public virtual string por_amortizar { get; set; }
	    public virtual string principal_anio { get; set; }
	    public virtual string intereses_anio { get; set; }
	    public virtual string comision_compromiso_anio { get; set; }
	    public virtual string otros_gastos { get; set; }
	    public virtual string principal_acumulado { get; set; }
	    public virtual string intereses_acumulados { get; set; }
	    public virtual string comision_compromiso_acumulado { get; set; }
	    public virtual string otros_cargos_acumulados { get; set; }
	    public virtual string presupuesto_asignado_func { get; set; }
	    public virtual string presupuesto_asignado_inv { get; set; }
	    public virtual string presupuesto_modificado_func { get; set; }
	    public virtual string presupuesto_modificado_inv { get; set; }
	    public virtual string presupuesto_vigente_func { get; set; }
	    public virtual string presupuesto_vigente_inv { get; set; }
	    public virtual string presupuesto_devengado_func { get; set; }
	    public virtual string presupuesto_devengado_inv { get; set; }
	    public virtual string presupuesto_pagado_func { get; set; }
	    public virtual string presupuesto_pagado_inv { get; set; }
	    public virtual string saldo_cuentas { get; set; }
	    public virtual string desembolsado_real { get; set; }
	    public virtual string ejecucion_estadoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string proyecto_programa { get; set; }
	    public virtual string fecha_decreto { get; set; }
	    public virtual string fecha_suscripcion { get; set; }
	    public virtual string fecha_elegibilidad_ue { get; set; }
	    public virtual string fecha_cierre_origianl_ue { get; set; }
	    public virtual string fecha_cierre_actual_ue { get; set; }
	    public virtual string meses_prorroga_ue { get; set; }
	    public virtual string plazo_ejecucion_ue { get; set; }
	    public virtual string monto_asignado_ue { get; set; }
	    public virtual string desembolso_a_fecha_ue { get; set; }
	    public virtual string monto_por_desembolsar_ue { get; set; }
	    public virtual string fecha_vigencia { get; set; }
	    public virtual string monto_contratado_usd { get; set; }
	    public virtual string monto_contratado_qtz { get; set; }
	    public virtual string desembolso_a_fecha_usd { get; set; }
	    public virtual string monto_por_desembolsar_usd { get; set; }
	    public virtual string monto_asignado_ue_usd { get; set; }
	    public virtual string monto_asignado_ue_qtz { get; set; }
	    public virtual string desembolso_a_fecha_ue_usd { get; set; }
	    public virtual string monto_por_desembolsar_ue_usd { get; set; }
	    [ForeignKey("unidad_ejecutora")]
        public virtual string entidad { get; set; }
	    [ForeignKey("unidad_ejecutora")]
        public virtual string ejercicio { get; set; }
	    public virtual string objetivo { get; set; }
	    public virtual string objetivo_especifico { get; set; }
	    public virtual string porcentaje_avance { get; set; }
	    [ForeignKey("cooperante")]
        public virtual string cooperantecodigo { get; set; }
	    [ForeignKey("cooperante")]
        public virtual string cooperanteejercicio { get; set; }
		public virtual unidad_ejecutora unidad_ejecutoras { get; set; }
		public virtual cooperante cooperantes { get; set; }
		public virtual tipo_moneda tipo_monedas { get; set; }
		public virtual IEnumerable<prestamo> prestamoes { get; set; }
	}
}
