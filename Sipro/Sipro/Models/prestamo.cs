namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.prestamo")]
    public partial class prestamo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public prestamo()
        {
            objeto_prestamo = new HashSet<objeto_prestamo>();
            proyecto = new HashSet<proyecto>();
            prestamo_usuario = new HashSet<prestamo_usuario>();
            prestamo_tipo_prestamo = new HashSet<prestamo_tipo_prestamo>();
        }

        public int id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_corte { get; set; }

        public long codigo_presupuestario { get; set; }

        [Required]
        [StringLength(30)]
        public string numero_prestamo { get; set; }

        [StringLength(1000)]
        public string destino { get; set; }

        [StringLength(1000)]
        public string sector_economico { get; set; }

        public int? unidad_ejecutoraunidad_ejecutora { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_firma { get; set; }

        public int? autorizacion_tipoid { get; set; }

        [StringLength(100)]
        public string numero_autorizacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_autorizacion { get; set; }

        public int? anios_plazo { get; set; }

        public int? anios_gracia { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin_ejecucion { get; set; }

        public int? perido_ejecucion { get; set; }

        public int? interes_tipoid { get; set; }

        public decimal? porcentaje_interes { get; set; }

        public decimal? porcentaje_comision_compra { get; set; }

        public int tipo_monedaid { get; set; }

        public decimal monto_contratado { get; set; }

        public decimal? amortizado { get; set; }

        public decimal? por_amortizar { get; set; }

        public decimal? principal_anio { get; set; }

        public decimal? intereses_anio { get; set; }

        public decimal? comision_compromiso_anio { get; set; }

        public decimal? otros_gastos { get; set; }

        public decimal? principal_acumulado { get; set; }

        public decimal? intereses_acumulados { get; set; }

        public decimal? comision_compromiso_acumulado { get; set; }

        public decimal? otros_cargos_acumulados { get; set; }

        public decimal? presupuesto_asignado_funcionamiento { get; set; }

        public decimal? prespupuesto_asignado_inversion { get; set; }

        public decimal? presupuesto_modificado_funcionamiento { get; set; }

        public decimal? presupuesto_modificado_inversion { get; set; }

        public decimal? presupuesto_vigente_funcionamiento { get; set; }

        public decimal? presupuesto_vigente_inversion { get; set; }

        public decimal? prespupuesto_devengado_funcionamiento { get; set; }

        public decimal? presupuesto_devengado_inversion { get; set; }

        public decimal? presupuesto_pagado_funcionamiento { get; set; }

        public decimal? presupuesto_pagado_inversion { get; set; }

        public decimal? saldo_cuentas { get; set; }

        public decimal? desembolsado_real { get; set; }

        public int? ejecucion_estadoid { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(100)]
        public string proyecto_programa { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_decreto { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_suscripcion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_elegibilidad_ue { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_cierre_origianl_ue { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_cierre_actual_ue { get; set; }

        public int meses_prorroga_ue { get; set; }

        public int? plazo_ejecucion_ue { get; set; }

        public decimal? monto_asignado_ue { get; set; }

        public decimal? desembolso_a_fecha_ue { get; set; }

        public decimal? monto_por_desembolsar_ue { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_vigencia { get; set; }

        public decimal monto_contratado_usd { get; set; }

        public decimal monto_contratado_qtz { get; set; }

        public decimal? desembolso_a_fecha_usd { get; set; }

        public decimal monto_por_desembolsar_usd { get; set; }

        public decimal? monto_asignado_ue_usd { get; set; }

        public decimal? monto_asignado_ue_qtz { get; set; }

        public decimal? desembolso_a_fecha_ue_usd { get; set; }

        public decimal? monto_por_desembolsar_ue_usd { get; set; }

        public int? entidad { get; set; }

        public int? ejercicio { get; set; }

        [StringLength(4000)]
        public string objetivo { get; set; }

        [StringLength(4000)]
        public string objetivo_especifico { get; set; }

        public int? porcentaje_avance { get; set; }

        public int cooperantecodigo { get; set; }

        public int cooperanteejercicio { get; set; }

        public virtual autorizacion_tipo autorizacion_tipo { get; set; }

        public virtual cooperante cooperante { get; set; }

        public virtual ejecucion_estado ejecucion_estado { get; set; }

        public virtual interes_tipo interes_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<objeto_prestamo> objeto_prestamo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto> proyecto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestamo_usuario> prestamo_usuario { get; set; }

        public virtual tipo_moneda tipo_moneda { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestamo_tipo_prestamo> prestamo_tipo_prestamo { get; set; }

        public virtual unidad_ejecutora unidad_ejecutora { get; set; }
    }
}
