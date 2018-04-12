namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.plan_adquisicion")]
    public partial class plan_adquisicion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public plan_adquisicion()
        {
            plan_adquisicion_pago = new HashSet<plan_adquisicion_pago>();
        }

        public int id { get; set; }

        public int tipo_adquisicion { get; set; }

        public int? categoria_adquisicion { get; set; }

        [StringLength(30)]
        public string unidad_medida { get; set; }

        public int? cantidad { get; set; }

        public decimal? total { get; set; }

        public decimal? precio_unitario { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? preparacion_doc_planificado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? preparacion_doc_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? lanzamiento_evento_planificado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? lanzamiento_evento_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? recepcion_ofertas_planificado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? recepcion_ofertas_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? adjudicacion_planificado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? adjudicacion_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? firma_contrato_planificado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? firma_contrato_real { get; set; }

        public int objeto_id { get; set; }

        public int objeto_tipo { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int estado { get; set; }

        public int? bloqueado { get; set; }

        [StringLength(45)]
        public string numero_contrato { get; set; }

        public decimal monto_contrato { get; set; }

        public long? nog { get; set; }

        public int? tipo_revision { get; set; }

        public virtual categoria_adquisicion categoria_adquisicion1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<plan_adquisicion_pago> plan_adquisicion_pago { get; set; }

        public virtual tipo_adquisicion tipo_adquisicion1 { get; set; }
    }
}
