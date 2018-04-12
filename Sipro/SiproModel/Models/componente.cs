namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.componente")]
    public partial class componente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public componente()
        {
            componente_usuario = new HashSet<componente_usuario>();
            componente_propiedad_valor = new HashSet<componente_propiedad_valor>();
            producto = new HashSet<producto>();
            subcomponente = new HashSet<subcomponente>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(4000)]
        public string descripcion { get; set; }

        public int proyectoid { get; set; }

        public int componente_tipoid { get; set; }

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

        public int? es_de_sigade { get; set; }

        public decimal? fuente_prestamo { get; set; }

        public decimal? fuente_donacion { get; set; }

        public decimal? fuente_nacional { get; set; }

        public int? componente_sigadeid { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_inicio_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin_real { get; set; }

        public int inversion_nueva { get; set; }

        public virtual acumulacion_costo acumulacion_costo { get; set; }

        public virtual unidad_ejecutora unidad_ejecutora { get; set; }

        public virtual componente_sigade componente_sigade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<componente_usuario> componente_usuario { get; set; }

        public virtual proyecto proyecto1 { get; set; }

        public virtual componente_tipo componente_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<componente_propiedad_valor> componente_propiedad_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto> producto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subcomponente> subcomponente { get; set; }
    }
}
