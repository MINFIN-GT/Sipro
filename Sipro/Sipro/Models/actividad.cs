namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.actividad")]
    public partial class actividad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public actividad()
        {
            actividad_propiedad_valor = new HashSet<actividad_propiedad_valor>();
            actividad_usuario = new HashSet<actividad_usuario>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(1000)]
        public string descripcion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_inicio { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_fin { get; set; }

        public int porcentaje_avance { get; set; }

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

        public int actividad_tipoid { get; set; }

        public long? snip { get; set; }

        public int? programa { get; set; }

        public int? subprograma { get; set; }

        public int? proyecto { get; set; }

        [Column("actividad")]
        public int? actividad1 { get; set; }

        public int? obra { get; set; }

        public int objeto_id { get; set; }

        public int objeto_tipo { get; set; }

        public int duracion { get; set; }

        [Required]
        [StringLength(1)]
        public string duracion_dimension { get; set; }

        public int? pred_objeto_id { get; set; }

        public int? pred_objeto_tipo { get; set; }

        [StringLength(30)]
        public string latitud { get; set; }

        [StringLength(30)]
        public string longitud { get; set; }

        public decimal? costo { get; set; }

        public int? acumulacion_costo { get; set; }

        public int? renglon { get; set; }

        public int? ubicacion_geografica { get; set; }

        public int? orden { get; set; }

        [StringLength(1000)]
        public string treePath { get; set; }

        public int? nivel { get; set; }

        public int? proyecto_base { get; set; }

        public int? componente_base { get; set; }

        public int? producto_base { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_inicio_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin_real { get; set; }

        public int inversion_nueva { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actividad_propiedad_valor> actividad_propiedad_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actividad_usuario> actividad_usuario { get; set; }

        public virtual actividad_tipo actividad_tipo { get; set; }

        public virtual acumulacion_costo acumulacion_costo1 { get; set; }
    }
}
