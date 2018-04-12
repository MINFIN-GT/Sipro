namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.producto")]
    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            producto_propiedad_valor = new HashSet<producto_propiedad_valor>();
            producto_usuario = new HashSet<producto_usuario>();
            subproducto = new HashSet<subproducto>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(4000)]
        public string descripcion { get; set; }

        public int? componenteid { get; set; }

        public int? subcomponenteid { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int producto_tipoid { get; set; }

        public int estado { get; set; }

        public int? unidad_ejecutoraunidad_ejecutora { get; set; }

        public long? snip { get; set; }

        public int? programa { get; set; }

        public int? subprograma { get; set; }

        public int? proyecto { get; set; }

        public int? actividad { get; set; }

        public int? obra { get; set; }

        [StringLength(30)]
        public string latitud { get; set; }

        [StringLength(30)]
        public string longitud { get; set; }

        public int? peso { get; set; }

        public decimal? costo { get; set; }

        public int? acumulacion_costoid { get; set; }

        public int? renglon { get; set; }

        public int? ubicacion_geografica { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_inicio { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin { get; set; }

        public int duracion { get; set; }

        [StringLength(1)]
        public string duracion_dimension { get; set; }

        public int? orden { get; set; }

        [StringLength(1000)]
        public string treePath { get; set; }

        public int? nivel { get; set; }

        public int? entidad { get; set; }

        public int? ejercicio { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_inicio_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin_real { get; set; }

        public int inversion_nueva { get; set; }

        public virtual acumulacion_costo acumulacion_costo { get; set; }

        public virtual componente componente { get; set; }

        public virtual subcomponente subcomponente { get; set; }

        public virtual unidad_ejecutora unidad_ejecutora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_propiedad_valor> producto_propiedad_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_usuario> producto_usuario { get; set; }

        public virtual producto_tipo producto_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subproducto> subproducto { get; set; }
    }
}
