namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.meta")]
    public partial class meta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public meta()
        {
            meta_avance = new HashSet<meta_avance>();
            meta_planificado = new HashSet<meta_planificado>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(4000)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }

        public int meta_unidad_medidaid { get; set; }

        public int dato_tipoid { get; set; }

        public int? objeto_id { get; set; }

        public int? objeto_tipo { get; set; }

        public int? meta_final_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string meta_final_string { get; set; }

        public decimal? meta_final_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? meta_final_fecha { get; set; }

        public virtual dato_tipo dato_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meta_avance> meta_avance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meta_planificado> meta_planificado { get; set; }

        public virtual meta_unidad_medida meta_unidad_medida { get; set; }
    }
}
