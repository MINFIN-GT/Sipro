namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.recurso_unidad_medida")]
    public partial class recurso_unidad_medida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public recurso_unidad_medida()
        {
            recurso = new HashSet<recurso>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(4000)]
        public string descripcion { get; set; }

        [StringLength(10)]
        public string simbolo { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recurso> recurso { get; set; }
    }
}
