namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.riesgo")]
    public partial class riesgo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public riesgo()
        {
            objeto_riesgo = new HashSet<objeto_riesgo>();
            riesgo_propiedad_valor = new HashSet<riesgo_propiedad_valor>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(4000)]
        public string descripcion { get; set; }

        public int riesgo_tipoid { get; set; }

        public decimal impacto { get; set; }

        public decimal probabilidad { get; set; }

        public decimal? impacto_monto { get; set; }

        public decimal? impacto_tiempo { get; set; }

        [StringLength(1000)]
        public string gatillo { get; set; }

        [StringLength(1000)]
        public string consecuencia { get; set; }

        [StringLength(1000)]
        public string solucion { get; set; }

        [StringLength(1000)]
        public string riesgos_segundarios { get; set; }

        public int ejecutado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_ejecucion { get; set; }

        [StringLength(1000)]
        public string resultado { get; set; }

        [StringLength(1000)]
        public string observaciones { get; set; }

        public int? colaboradorid { get; set; }

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

        public virtual colaborador colaborador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<objeto_riesgo> objeto_riesgo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<riesgo_propiedad_valor> riesgo_propiedad_valor { get; set; }

        public virtual riesgo_tipo riesgo_tipo { get; set; }
    }
}
