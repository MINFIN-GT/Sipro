namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.proyecto")]
    public partial class proyecto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proyecto()
        {
            componente = new HashSet<componente>();
            desembolso = new HashSet<desembolso>();
            hito = new HashSet<hito>();
            linea_base = new HashSet<linea_base>();
            programa_proyecto = new HashSet<programa_proyecto>();
            proyecto_rol_colaborador = new HashSet<proyecto_rol_colaborador>();
            proyecto_impacto = new HashSet<proyecto_impacto>();
            proyecto_miembro = new HashSet<proyecto_miembro>();
            proyecto_propiedad_valor = new HashSet<proyecto_propiedad_valor>();
            proyecto_usuario = new HashSet<proyecto_usuario>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(2000)]
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

        public int estado { get; set; }

        public int proyecto_tipoid { get; set; }

        public int? unidad_ejecutoraunidad_ejecutora { get; set; }

        public long? snip { get; set; }

        public int? programa { get; set; }

        public int? subprograma { get; set; }

        [Column("proyecto")]
        public int? proyecto1 { get; set; }

        public int? actividad { get; set; }

        public int? obra { get; set; }

        [StringLength(30)]
        public string latitud { get; set; }

        [StringLength(30)]
        public string longitud { get; set; }

        [StringLength(4000)]
        public string objetivo { get; set; }

        public int? director_proyecto { get; set; }

        [StringLength(4000)]
        public string enunciado_alcance { get; set; }

        public decimal? costo { get; set; }

        public int? acumulacion_costoid { get; set; }

        [StringLength(4000)]
        public string objetivo_especifico { get; set; }

        [StringLength(45)]
        public string vision_general { get; set; }

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

        public int? ejecucion_fisica_real { get; set; }

        public int proyecto_clase { get; set; }

        public int? project_cargado { get; set; }

        public int? prestamoid { get; set; }

        [StringLength(2000)]
        public string observaciones { get; set; }

        public int? coordinador { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_elegibilidad { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_cierre { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_inicio_real { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_fin_real { get; set; }

        public int? congelado { get; set; }

        public virtual acumulacion_costo acumulacion_costo { get; set; }

        public virtual colaborador colaborador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<componente> componente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<desembolso> desembolso { get; set; }

        public virtual etiqueta etiqueta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hito> hito { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<linea_base> linea_base { get; set; }

        public virtual pep_detalle pep_detalle { get; set; }

        public virtual prestamo prestamo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<programa_proyecto> programa_proyecto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_rol_colaborador> proyecto_rol_colaborador { get; set; }

        public virtual unidad_ejecutora unidad_ejecutora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_impacto> proyecto_impacto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_miembro> proyecto_miembro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_propiedad_valor> proyecto_propiedad_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_usuario> proyecto_usuario { get; set; }

        public virtual proyecto_tipo proyecto_tipo { get; set; }
    }
}
